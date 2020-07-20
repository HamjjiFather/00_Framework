using UnityEngine;

namespace KKSFramework.GlobalText
{
    /// <summary>
    /// 변환될 텍스트를 가지고 있는 컴포넌트.
    /// </summary>
    public abstract class GlobalTextComponentBase : CachedComponent
    {
        #region Fields & Property

        [Tooltip ("컴포넌트로 사용할 경우 Key값은 무조건 채워져 있어야 한다.")]
        public TranslatedInfo translatedInfo;

#pragma warning disable CS0649
        
#pragma warning restore CS0649

        #endregion


        #region UnityMethods

        private void OnEnable ()
        {
            SetComponent ();
            ChangeText ();
        }

        #endregion


        #region EventMethods

        #endregion


        #region Methods

        /// <summary>
        /// 컴포넌트 세팅.
        /// </summary>
        protected abstract void SetComponent ();

        /// <summary>
        /// 텍스트 변경.
        /// </summary>
        public abstract void ChangeText ();
        
        #endregion
    }
}