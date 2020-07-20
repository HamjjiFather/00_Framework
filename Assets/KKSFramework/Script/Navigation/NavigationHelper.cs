using KKSFramework.ResourcesLoad;
using UniRx.Async;
using UnityEngine.Events;

namespace KKSFramework.Navigation
{
    /// <summary>
    /// Navigation은 게임에 따라 변형 될 수 있으므로 Helper를 두어 접근한다.
    /// </summary>
    public static class NavigationHelper
    {
        #region Methods

        /// <summary>
        /// 페이지 오픈.
        /// </summary>
        public static async UniTask OpenPage(NavigationViewType navigationViewType, 
            NavigationTriggerState triggerState = NavigationTriggerState.CloseAndOpen,
            object pushValue = null, UnityAction actionOnFirst = null)
        {
            await NavigationManager.Instance.OpenPage(navigationViewType.ToString(), triggerState, pushValue,
                actionOnFirst);
        }

        /// <summary>
        /// 팝업 오픈.
        /// </summary>
        public static async UniTask OpenPopup(NavigationViewType navigationViewType, object pushValue = null)
        {
            await NavigationManager.Instance.OpenPopup(navigationViewType.ToString(), pushValue);
        }


        /// <summary>
        /// CommonView 오픈.
        /// </summary>
        public static async UniTask OpenCommonView<T> (string viewName) where T : PooingComponent
        {
            await NavigationManager.Instance.OpenCommonView<T> (viewName);
        }
        

        /// <summary>
        /// 뒷 페이지로 이동.
        /// </summary>
        public static void GoBackPage()
        {
            NavigationManager.Instance.GoBackPage().Forget();
        }

        #endregion
    }
}