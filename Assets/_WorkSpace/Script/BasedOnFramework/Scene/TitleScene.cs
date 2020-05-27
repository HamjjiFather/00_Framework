using UniRx.Async;

namespace KKSFramework.InGame
{
    public class TitleScene : SceneController
    {
        public TitlePageView titlePageView;


        protected override UniTask InitializeAsync ()
        {
            ProjectInstall.InstallViewmodel ();
            titlePageView.Push ().Forget ();
            return base.InitializeAsync ();
        }
    }
}