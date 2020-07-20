using System;

namespace KKSFramework.Animation
{
    /// <summary>
    /// 수동 작동 애니메이션 데이터.
    /// </summary>
    [Serializable]
    public class ManuallySpriteAnimationData : SpriteAnimationData
    {
        /// <summary>
        /// OnEnable에서 실행 여부.
        /// </summary>
        public bool is_played_on_enable;
    }

    /// <summary>
    /// 수동 작동 애니메이션 클래스.
    /// </summary>
    public class ManuallySpriteAnimationComponent : SpriteAnimationComponent
    {
        
        //[Header("[ManuallySpriteAnimation]"), Space(5)]

        #region Fields & Property

        /// <summary>
        /// 애니메이션 데이터.
        /// </summary>
        public ManuallySpriteAnimationData com_ManuallySpriteAnimationData;

        #endregion

        #region Methods

        private void OnEnable()
        {
            if (com_ManuallySpriteAnimationData.is_played_on_enable)
            {
                if (IsSetAnimation(StatusDirection.Default + StatusBehaviour.Default.ToString()) == false)
                    SetSprites(StatusDirection.Default, StatusBehaviour.Default, com_ManuallySpriteAnimationData);

                Play(com_ManuallySpriteAnimationData);
            }
        }

        #endregion
    }
}