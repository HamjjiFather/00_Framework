using Cysharp.Threading.Tasks;
using KKSFramework.Navigation;
using KKSFramework.SceneLoad;
using UnityEngine;
using UnityEngine.UI;

namespace KKSFramework.InGame
{
    public class TitlePageView : PageViewBase
    {
        #region Fields & Property

        public Button titleButton;

#pragma warning disable CS0649

#pragma warning restore CS0649

        private bool _isLoaded; 

        #endregion


        private void Awake ()
        {
            titleButton.onClick.AddListener (ClickTitle);
        }

        
        protected override void Showed ()
        {
            base.Showed ();
            _isLoaded = true;
        }


        private void ClickTitle ()
        {
            if (!_isLoaded) return;
            _isLoaded = false;
            SceneLoadManager.Instance.ChangeSceneAsync (SceneType.Game).Forget ();
        }
    }
}