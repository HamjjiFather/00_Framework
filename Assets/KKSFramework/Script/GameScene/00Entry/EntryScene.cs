using KKSFramework.SceneLoad;
using UniRx.Async;

namespace KKSFramework
{
    public class EntryScene : SceneController
    {
        public EntryPageView entryPageView;


        protected override UniTask InitializeAsync ()
        {
            SceneLoadManager.Instance.InitManager ();
            entryPageView.Push ().Forget ();
            return UniTask.CompletedTask;
        }
    }
}