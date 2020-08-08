using Cysharp.Threading.Tasks;
using KKSFramework.Localization;
using KKSFramework.SceneLoad;


namespace KKSFramework.InGame
{
    public class EntryScene : SceneController
    {
        public EntryPageView entryPageView;


        protected override async UniTask InitializeAsync ()
        {
            await LocalizationTextManager.Instance.LoadGlobalTextData ();
            SceneLoadManager.Instance.InitManager ();
            entryPageView.Push ().Forget ();
            await UniTask.CompletedTask;
        }
    }
}