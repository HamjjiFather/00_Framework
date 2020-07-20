using UniRx.Async;
using UnityEngine;
using UnityEngine.UI;

namespace KKSFramework.Navigation
{
    /// <summary>
    /// 페이지에서 규칙적으로 호출되는 ViewLayout들을 호출하기 위한 컴포넌트.
    /// </summary>
    public class ViewLayoutLoader : MonoBehaviour
    {
        #region Fields & Property
        
        public ViewLayoutBase[] viewLayoutObjs;
        
        public Button[] layoutViewButton;

        public bool initOnAwake = true;

#pragma warning disable CS0649

#pragma warning restore CS0649

        private int _nowLayout;

        #endregion


        #region UnityMethods
        
        private void Awake ()
        {
            layoutViewButton.Foreach ((button, index) =>
            {
                button.onClick.AddListener (() => ClickLayoutViewButton(index));
            });

            if (!initOnAwake) return;
            Initialize ();
        }

        #endregion


        #region Methods

        public void Initialize ()
        {
            viewLayoutObjs.Foreach (x =>
            {
                x.Initialize ();
            });
        }
        
        
        public void SetSubView (int index)
        {
            viewLayoutObjs[_nowLayout].DisableLayout ().Forget();
            _nowLayout = index;
            viewLayoutObjs[_nowLayout].ActiveLayout ().Forget();
        }

        #endregion


        #region EventMethods
        
        private void ClickLayoutViewButton (int index)
        {
            SetSubView (index);
        }

        #endregion
    }
}