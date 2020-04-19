using KKSFramework.Object;
using UniRx.Async;
using UnityEngine.Events;

namespace KKSFramework.Navigation
{
    public enum NavigationViewState
    {
        EntryPage,
        TitlePage,
        HomePage = 1000,
        QuitPopup,
    }


    /// <summary>
    /// Navigation은 게임에 따라 변형 될 수 있으므로 Helper를 두어 접근한다.
    /// </summary>
    public static class NavigationHelper
    {
        #region Methods

        /// <summary>
        /// 페이지 오픈.
        /// </summary>
        public static async UniTask OpenPage(NavigationViewState navigationViewState, 
            NavigationTriggerState triggerState = NavigationTriggerState.CloseAndOpen,
            object pushValue = null, UnityAction actionOnFirst = null)
        {
            await NavigationManager.Instance.OpenPage(navigationViewState.ToString(), triggerState, pushValue,
                actionOnFirst);
        }

        /// <summary>
        /// 팝업 오픈.
        /// </summary>
        public static async UniTask OpenPopup(NavigationViewState navigationViewState, object pushValue = null)
        {
            await NavigationManager.Instance.OpenPopup(navigationViewState.ToString(), pushValue);
        }


        /// <summary>
        /// CommonView 오픈.
        /// </summary>
        public static async UniTask OpenCommonView<T> (string viewName) where T : PooledObjectComponent
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