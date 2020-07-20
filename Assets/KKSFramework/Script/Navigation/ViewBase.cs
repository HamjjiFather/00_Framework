using System.Threading;
using KKSFramework.ResourcesLoad;
using UniRx.Async;
using UnityEngine;

namespace KKSFramework.Navigation
{
    public class ViewBase : PooingComponent
    {
        #region Fields & Property

        /// <summary>
        /// RectTransform.
        /// </summary>
        public RectTransform rectTransform => GetCachedComponent<RectTransform> ();
        
        
#pragma warning disable CS0649

#pragma warning restore CS0649

        #endregion

        private ViewOption ViewOption => GetCachedComponent<ViewOption>();

        private CancellationTokenSource _cancellationTokenSource;

        protected CancellationToken CancellationToken => _cancellationTokenSource.Token;

        #region Constructor

        #endregion

        #region Methods

        /// <summary>
        /// 뷰 오픈.
        /// </summary>
        public async UniTask Push(object pushValue = null)
        {
            _cancellationTokenSource = new CancellationTokenSource();
            Unpooled();
            await OnPush(pushValue);
            await Show();
        }

        /// <summary>
        /// 뷰 클로즈.
        /// </summary>
        public async UniTask Pop()
        {
            await Hide();
            await Popped();
            PoolingObject();
        }

        public async UniTask Show(object pushValue = null)
        {
            await OnShow();
        }

        public async UniTask Hide()
        {
            await OnHide();
        }

        public virtual async UniTask ToForeground()
        {
            await OnForeground();
            await Show();
        }

        public virtual async UniTask ToBackground()
        {
            await OnBackground();
            await Hide();
        }

        #endregion

        #region EventMethods

        /// <summary>
        /// 팝업 열림 이벤트 함수.
        /// </summary>
        protected virtual async UniTask OnPush(object pushValue = null)
        {
            await UniTask.CompletedTask;
            Pushed (pushValue);
        }

        /// <summary>
        /// 팝업 열림 마무리 이벤트 함수.
        /// </summary>
        protected virtual void Pushed(object pushValue = null)
        {
        }

        /// <summary>
        /// 페이지, 팝업이 감춰져있다가 맨앞으로 나옴.
        /// 전환시 처리 대기를 하면 된다.
        /// </summary>
        protected virtual UniTask OnForeground()
        {
            return UniTask.CompletedTask;
        }

        protected virtual UniTask OnShow()
        {
            Showed();
            return UniTask.CompletedTask;
        }


        protected virtual void Showed()
        {
        }


        protected virtual UniTask OnHide()
        {
            Hid();
            gameObject.SetActive (false);
            return UniTask.CompletedTask;
        }


        protected virtual void Hid()
        {
        }


        /// <summary>
        /// 팝업 닫힘 이벤트 함수.
        /// 풀링 이후 실행.
        /// </summary>
        protected virtual UniTask Popped()
        {
            _cancellationTokenSource?.Cancel();
            _cancellationTokenSource?.Dispose ();
            _cancellationTokenSource = null;
            return UniTask.CompletedTask;
        }

        /// <summary>
        /// </summary>
        protected virtual UniTask OnBackground()
        {
            return UniTask.CompletedTask;
        }

        #endregion
    }
}