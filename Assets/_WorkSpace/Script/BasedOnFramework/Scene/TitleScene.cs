using Cysharp.Threading.Tasks;
using KKSFramework.LocalData;
using KKSFramework.Navigation;
using KKSFramework.SceneLoad;


namespace KKSFramework.InGame
{
    public class TitleScene : SceneController
    {
        protected override async UniTask InitializeAsync ()
        {
            LocalDataHelper.LoadAllGameData ();
            LocalDataManager.Instance.SetSaveAction (LocalDataHelper.SaveAllGameData);
            await NavigationHelper.OpenPage (NavigationViewType.TitlePage, NavigationTriggerState.First);
            await base.InitializeAsync ();
        }
    }
}