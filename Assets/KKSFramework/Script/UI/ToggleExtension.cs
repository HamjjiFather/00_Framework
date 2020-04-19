using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using KKSFramework.Sound;

namespace KKSFramework.UI
{
    /// <summary>
    /// 토글 베이스 클래스.
    /// </summary>
    public class ToggleExtension : Toggle
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


        // 모든 베이스 클래스, 베이스 클래스를 상속한 클래스에서 사용.


        #region Fields & Property

        /// <summary>
        /// 사운드 타입.
        /// </summary>
        [Header ("[ButtonExtension]")]
        [Space (5)]
        public SoundTypeEnum soundTypeEnum;

        /// <summary>
        /// 토글 텍스트.
        /// </summary>
        public Text toggleText;

        /// <summary>
        /// 클릭 이벤트.
        /// </summary>
        public UnityEvent onClick = new UnityEvent ();

        #endregion


        #region UnityMethods

        private new void Reset ()
        {
            targetGraphic = GetComponentInChildren<Graphic> ();
            if (targetGraphic && targetGraphic.transform.childCount != 0)
                graphic = targetGraphic.transform.GetChild (0).GetComponentInChildren<Graphic> ();
        }

        protected override void Start ()
        {
            onClick.AddListener (OnClick);
        }

        #endregion


        #region Methods

        /// <summary>
        /// 베이스 토글에서 컴포넌트를 변경함.
        /// </summary>
        public void ReplaceComponent (Toggle targetToggle)
        {
            transition = targetToggle.transition;
            targetGraphic = targetToggle.targetGraphic;
            colors = targetToggle.colors;
            isOn = targetToggle.isOn;
            graphic = targetToggle.graphic;
            group = targetToggle.group;
            onValueChanged = targetToggle.onValueChanged;
        }


        /// <summary>
        /// 토글 내 텍스트 변경.
        /// </summary>
        public void SetText (string text)
        {
            if (toggleText != null)
                toggleText.text = text;
        }

        #endregion
    }
}