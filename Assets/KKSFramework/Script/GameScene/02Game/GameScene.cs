using KKSFramework.Navigation;
using UniRx.Async;

namespace KKSFramework
{
    public class GameScene : SceneController
    {
        protected override async UniTask InitializeAsync()
        {
            CreateCommonView ();
            // await NavigationHelper.OpenPage (NavigationViewState.HomePage, NavigationTriggerState.First, actionOnFirst:OpenQuitPopup);
            base.InitializeAsync ().Forget();

            void CreateCommonView ()
            {
                
            }
            
            void OpenQuitPopup ()
            {
                NavigationHelper.OpenPopup (NavigationViewState.QuitPopup).Forget();
            }
        }
    }
}