using UniRx.Async;
using UnityEngine;

namespace KKSFramework.Navigation
{
    [RequireComponent(typeof(PopupOption))]
    public class PopupViewBase : ViewBase
    {
        #region Fields & Property

        private PopupOption popupOption => GetCachedComponent<PopupOption>();

        #endregion


        #region EventMethods

        protected override UniTask OnPush(object pushValue = null)
        {
            popupOption.InitializePopupOption(ClickClose);
            return base.OnPush(pushValue);
        }

        protected override async UniTask OnShow()
        {
            await popupOption.ShowAsync(CancellationToken);
            await base.OnShow();
            Showed();
        }

        protected override async UniTask OnHide()
        {
            await popupOption.HideAsync(CancellationToken);
            await base.OnHide();
            Hid();
        }


        protected virtual void ClickClose()
        {
            NavigationHelper.GoBackPage();
        }

        #endregion
    }
}