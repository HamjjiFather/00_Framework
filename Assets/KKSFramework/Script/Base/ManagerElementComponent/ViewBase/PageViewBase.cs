using System.Collections.Generic;
using System.Linq;
using UniRx.Async;
using UnityEngine;

namespace KKSFramework.Navigation
{
    [RequireComponent(typeof(PageOption))]
    public class PageViewBase : ViewBase
    {
        #region Fields & Property

        private PageOption PageOption => GetCachedComponent<PageOption>();

        private readonly Stack<PopupViewBase> _registedPopupStack = new Stack<PopupViewBase>();

        public bool ExistPopup => _registedPopupStack.Count != 0;

        #endregion

        #region UnityMethods

        #endregion

        #region Methods

        /// <summary>
        /// 페이지 팝업 등록.
        /// </summary>
        public void RegistPopup(PopupViewBase popupViewBase)
        {
            popupViewBase.transform.SetParent(PageOption.popupParents);
            popupViewBase.rectTransform.SetInstantiateTransform ();
            _registedPopupStack.Push(popupViewBase);
        }

        /// <summary>
        /// 페이지에 오픈된 팝업을 닫음.
        /// </summary>
        /// <returns></returns>
        public async UniTask<bool> CloseLastPopup()
        {
            if (ExistPopup)
            {
                var last = _registedPopupStack.Pop();
                await last.Pop();
                return true;
            }

            return false;
        }

        #endregion

        #region EventMethods

        /// <summary>
        /// 뷰 오픈.
        /// </summary>
        protected override async UniTask OnPush(object pushValue = null)
        {
            await UniTask.WhenAll(_registedPopupStack.Select(x => x.Push(pushValue)));
            await base.OnPush(pushValue);
        }

        protected override async UniTask Popped()
        {
            await UniTask.Run(() => _registedPopupStack.Select(x => x.Pop()));
            await base.Popped();
        }

        protected override async UniTask OnForeground()
        {
            await UniTask.Run(() => _registedPopupStack.Select(x => x.ToForeground()));
            await base.OnForeground();
        }

        protected override async UniTask OnBackground()
        {
            await UniTask.Run(() => _registedPopupStack.Select(x => x.ToBackground()));
            await base.OnBackground();
        }

        #endregion
    }
}