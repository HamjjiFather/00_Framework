using KKSFramework.Navigation;
using UniRx.Async;
using Zenject;

namespace KKSFramework
{
    public abstract class SceneController : MonoInstaller
    {
        private static bool _isLoadedBootstrap;

        public override void InstallBindings()
        {
            InitializeAsync ().Forget();
        }


        protected virtual async UniTask InitializeAsync()
        {
            NavigationManager.Instance.ChangeTransitionLockState (false);
            await NavigationManager.Instance.HideTransitionViewAsync ();
            await UniTask.CompletedTask;
        }
        

        public virtual void Finalized()
        {
        }
    }
}