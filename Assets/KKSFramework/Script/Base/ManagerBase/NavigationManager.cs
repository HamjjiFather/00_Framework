using System;
using System.Collections.Generic;
using System.Linq;
using KKSFramework.Event;
using KKSFramework.Management;
using KKSFramework.Object;
using KKSFramework.ResourcesLoad;
using UniRx;
using UniRx.Async;
using UnityEngine;
using UnityEngine.Events;
using Zenject;

namespace KKSFramework.Navigation
{
    public enum NavigationTriggerState
    {
        /// <summary>
        /// 일반 팝업 동작.
        /// </summary>
        CloseAndOpen = 0,

        /// <summary>
        /// 최초 페이지로 설정.
        /// </summary>
        First,

        /// <summary>
        /// 백스페이스 트리거 동작을 하지 않음.
        /// </summary>
        NotTrigger
    }

    /// <summary>
    /// 화면 전환 데이터 클래스.
    /// </summary>
    public class NavigationInfo
    {
        public readonly string ViewTypeName;
        public readonly NavigationTriggerState NavigationTriggerState;
        public readonly PageViewBase PageViewBase;

        public NavigationInfo ()
        {
            NavigationTriggerState = NavigationTriggerState.CloseAndOpen;
        }

        public NavigationInfo (string viewTypeName, PageViewBase pageViewBase,
            NavigationTriggerState navigationTriggerState)
        {
            ViewTypeName = viewTypeName;
            PageViewBase = pageViewBase;
            NavigationTriggerState = navigationTriggerState;
        }
    }

    /// <summary>
    /// 화면 전환 관리 클래스.
    /// </summary>
    public class NavigationManager : ManagerBase<NavigationManager>
    {
        #region Constructor

        #endregion


        #region Fields & Property

        /// <summary>
        /// 현재 열려있는 페이지 카운트.
        /// </summary>
        public readonly IntReactiveProperty PageCount = new IntReactiveProperty (0);

        /// <summary>
        /// 현재 열려있는 페이지 타입.
        /// </summary>
        public readonly ReactiveProperty<NavigationViewState> LastPageType =
            new ReactiveProperty<NavigationViewState> (NavigationViewState.EntryPage);

        /// <summary>
        /// 메인 UI 카메라.
        /// </summary>
        public Camera MainCamera => _navigationComponent.mainCamera;

#pragma warning disable CS0649

#pragma warning restore CS0649

        /// <summary>
        /// 네비게이션 컴포넌트.
        /// </summary>
        private NavigationComponent _navigationComponent => ComponentBase as NavigationComponent;

        /// <summary>
        /// 현재 오픈되어 있는 페이지뷰 스택.
        /// </summary>
        private readonly Stack<NavigationInfo> _pageInfoStack = new Stack<NavigationInfo> ();

        /// <summary>
        /// ESC 버튼을 통해 뒤로 갈 수 있는지 여부.
        /// </summary>
        private bool _ableBackPage;

        /// <summary>
        /// 처음 페이지에서 뒤로가기 버튼을 눌렀을 경우 행동할 액션.
        /// </summary>
        private UnityAction _actionOnFirst;

        #endregion


        #region Methods

        /// <summary>
        /// initializing manager class.
        /// </summary>
        public override void InitManager ()
        {
            EscapeEventManager.Instance.AddEscapeEvent (() => GoBackPage ().Forget ());
        }


        /// <summary>
        /// return whether or not the last page information exists.
        /// </summary>
        private bool IsExistLastNavInfo ()
        {
            return _pageInfoStack.Count >= 1;
        }

        /// <summary>
        /// return last 'NavigationInfo' data.
        /// </summary>
        private NavigationInfo GetLastNavInfo ()
        {
            return IsExistLastNavInfo () ? _pageInfoStack.Peek () : new NavigationInfo ();
        }

        /// <summary>
        /// create page view object
        /// </summary>
        private async UniTask<PageViewBase> CreatePage (string viewString)
        {
            // if exist pooled object.
            if (ObjectPoolingManager.Instance.IsExistPooledObject (PoolingObjectType.View, viewString))
            {
                return ObjectPoolingManager.Instance.ReturnLoadResources<PageViewBase> (PoolingObjectType.View,
                    viewString);
            }

            var resObj = await ResourcesLoadHelper.GetResourcesAsync<PageViewBase> (ResourceRoleType._Prefab,
                ResourcesType.Page, viewString);
            var page = ProjectContext.Instance.Container.InstantiatePrefabForComponent<PageViewBase> (resObj);
            page.Created<PageViewBase> (PoolingObjectType.View, viewString);
            return page;
        }


        /// <summary>
        /// create popup view object.
        /// </summary>
        private async UniTask<PopupViewBase> CreatePopup (string viewString)
        {
            // 풀링된 오브젝트가 있을 경우.
            if (ObjectPoolingManager.Instance.IsExistPooledObject (PoolingObjectType.View, viewString))
            {
                return ObjectPoolingManager.Instance.ReturnLoadResources<PopupViewBase> (PoolingObjectType.View,
                    viewString);
            }

            var resObj = await ResourcesLoadHelper.GetResourcesAsync<PopupViewBase> (ResourceRoleType._Prefab,
                ResourcesType.Popup, viewString);
            var popup = ProjectContext.Instance.Container.InstantiatePrefabForComponent<PopupViewBase> (resObj);
            popup.Created<PopupViewBase> (PoolingObjectType.View, viewString);
            return popup;
        }


        /// <summary>
        /// open page.
        /// </summary>
        public async UniTask OpenPage (string viewString, NavigationTriggerState triggerState,
            object pushValue = null, UnityAction actionOnFirst = null)
        {
            await ShowTransitionViewAsync ();
            var page = await CreatePage (viewString);

            if (triggerState == NavigationTriggerState.First)
            {
                _pageInfoStack.Foreach (x => x.PageViewBase.Pop ().Forget ());
                _pageInfoStack.Clear ();
                _pageInfoStack.Push (new NavigationInfo (viewString, page, triggerState));
                InitPage ();
                await page.Push (pushValue);
                _actionOnFirst = actionOnFirst;
            }
            else
            {
                var lastPage = _pageInfoStack.Peek ();
                _pageInfoStack.Push (new NavigationInfo (viewString, page, triggerState));
                InitPage ();
                await page.Push (pushValue);
                await lastPage.PageViewBase.ToBackground ();
                lastPage.PageViewBase.PoolingObject ();
            }

            PageCount.Value = _pageInfoStack.Count;
            LastPageType.Value = (NavigationViewState) Enum.Parse (typeof (NavigationViewState),
                _pageInfoStack.First ().ViewTypeName);

            await HideTransitionViewAsync ();

            void InitPage ()
            {
                var rectT = page.GetComponent<RectTransform> ();
                rectT.SetParent (_navigationComponent.PageParents);
                rectT.SetInstantiateTransform ();
            }
        }

        /// <summary>
        /// open popup.
        /// </summary>
        public async UniTask OpenPopup (string viewString, object pushValue = null)
        {
            var popup = await CreatePopup (viewString);
            var lastPage = _pageInfoStack.Peek ();
            await popup.Push (pushValue);
            lastPage.PageViewBase.RegistPopup (popup);
        }

        /// <summary>
        /// create common view.
        /// </summary>
        public async UniTask OpenCommonView<T> (string viewName) where T : PooledObjectComponent
        {
            var resObj =
                await ResourcesLoadHelper.GetResourcesAsync<T> (ResourceRoleType._Prefab, ResourcesType.CommonView,
                    viewName);
            var commonView = ProjectContext.Instance.Container.InstantiatePrefabForComponent<T> (resObj);
            commonView.Created<T> (PoolingObjectType.Prefab, viewName);

            var rectT = commonView.GetComponent<RectTransform> ();
            rectT.SetParent (_navigationComponent.CommonViewParents);
            rectT.SetInstantiateTransform ();
        }

        /// <summary>
        /// back to previous page.
        /// order.
        /// 1. fade-in transition animation effect.
        /// 2. check opened popup in opened page and close the popup.
        /// 3. invoke (Pop -> Hide -> PoolingObject) method opened page.
        /// 4. invoke (ToForeground -> Show) method previous page.
        /// 5. invoke unPooled method on previous page.
        /// 6. fade-out transition animation effect.
        /// </summary>
        public async UniTask GoBackPage ()
        {
            if (_pageInfoStack.Count == 0) return;

            var navigationInfo = _pageInfoStack.Peek ();

            switch (navigationInfo.NavigationTriggerState)
            {
                case NavigationTriggerState.CloseAndOpen:
                    if (!IsExistLastNavInfo ())
                        break;

                    await ShowTransitionViewAsync ();

                    if (navigationInfo.PageViewBase.CloseLastPopup ().Result == false)
                    {
                        await navigationInfo.PageViewBase.Pop ();
                        _pageInfoStack.Pop ();

                        var nextNavInfo = _pageInfoStack.Peek ();
                        nextNavInfo.PageViewBase.transform.SetParent (_navigationComponent.PageParents);
                        nextNavInfo.PageViewBase.ToForeground ().Forget ();
                        nextNavInfo.PageViewBase.Unpooled ();
                    }

                    PageCount.Value = _pageInfoStack.Count;
                    LastPageType.Value = (NavigationViewState) Enum.Parse (typeof (NavigationViewState),
                        navigationInfo.ViewTypeName);

                    await HideTransitionViewAsync();

                    break;

                case NavigationTriggerState.First:
                    var existPopup = navigationInfo.PageViewBase.ExistPopup;
                    await navigationInfo.PageViewBase.CloseLastPopup ();

                    if (!existPopup)
                        _actionOnFirst?.Invoke ();

                    break;
            }
        }
        
        
        /// <summary>
        /// task show async transition view.
        /// </summary>
        public async UniTask ShowTransitionViewAsync ()
        {
            await _navigationComponent.transitionEffector.ShowAsync ();
        }


        /// <summary>
        /// task hide async transition view. 
        /// </summary>
        public async UniTask HideTransitionViewAsync ()
        {
            await _navigationComponent.transitionEffector.HideAsync ();
        }


        /// <summary>
        /// change effector playable state.
        /// </summary>
        public void ChangeTransitionLockState (bool isLockState)
        {
            _navigationComponent.transitionEffector.SetLockState (isLockState);
        }

        #endregion
    }
}