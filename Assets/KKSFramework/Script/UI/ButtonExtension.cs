using KKSFramework.Sound;
using UnityEngine;
using UnityEngine.UI;

namespace KKSFramework.UI
{
    /// <summary>
    /// 버튼 베이스 클래스.
    /// </summary>
    public class ButtonExtension : Button
    {
        #region EventMethods

        /// <summary>
        /// 클릭 이벤트.
        /// </summary>
        protected virtual void OnClick ()
        {
            if (soundTypeEnum != SoundTypeEnum.None) SoundPlayHelper.PlaySfx (soundTypeEnum);
        }

        #endregion


        #region Fields & Property

        /// <summary>
        /// 사운드 타입.
        /// </summary>
        [Header ("[ButtonExtension]")]
        [Space (5)]
        public SoundTypeEnum soundTypeEnum;

        /// <summary>
        /// 버튼 텍스트.
        /// </summary>
        public Text buttonText;

        #endregion


        #region UnityMethods

        protected new void Reset ()
        {
            targetGraphic = GetComponentInChildren<Graphic> ();
        }

        protected override void Start ()
        {
            onClick.AddListener (OnClick);
        }

        #endregion


        #region Methods

        /// <summary>
        /// 베이스 버튼에서 컴포넌트를 변경함.
        /// </summary>
        public void ReplaceComponent (Button targetButton)
        {
            interactable = targetButton.interactable;
            transition = targetButton.transition;
            colors = targetButton.colors;
            onClick = targetButton.onClick;
        }

        /// <summary>
        /// 버튼 텍스트 변경.
        /// </summary>
        /// <param name="text"></param>
        public void SetText (string text)
        {
            if (buttonText != null)
                buttonText.text = text;
        }

        #endregion
    }
}