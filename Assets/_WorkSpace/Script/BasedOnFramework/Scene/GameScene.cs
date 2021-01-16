using Cysharp.Threading.Tasks;
using KKSFramework.Navigation;
using KKSFramework.SceneLoad;


namespace KKSFramework.InGame
{
    public class GameScene : SceneController
    {
        protected override async UniTask InitializeAsync ()
        {
            ProjectInstall.InitViewmodel ();
            await TableDataManager.Instance.LoadTableDatas ();
            ProjectInstall.InitLocalDataViewmodel ();
            ProjectInstall.InitTableDataViewmodel ();

            CreateCommonView ();
            await NavigationHelper.OpenPage (NavigationViewType.HomePage, NavigationTriggerState.First,
                actionOnFirst: OpenQuitPopup);
            base.InitializeAsync ().Forget ();

            void CreateCommonView ()
            {
            }

            void OpenQuitPopup ()
            {
                // NavigationHelper.OpenPopup (NavigationViewType.QuitPopup).Forget ();
            }
        }
    }
}