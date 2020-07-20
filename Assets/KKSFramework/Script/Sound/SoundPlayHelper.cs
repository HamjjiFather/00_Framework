namespace KKSFramework.Sound
{
    public static class SoundPlayHelper
    {
        #region Methods

        /// <summary>
        /// play bgm sound.
        /// </summary>
        public static void PlayBgm (SoundTypeEnum soundTypeEnum)
        {
            SoundPlayManager.Instance.Play (SoundType.Bgm, soundTypeEnum);
        }


        /// <summary>
        /// play Sfx sound.
        /// </summary>
        public static void PlaySfx (SoundTypeEnum soundTypeEnum)
        {
            SoundPlayManager.Instance.PlayOneShot (SoundType.Sfx, soundTypeEnum);
        }


        /// <summary>
        /// Pause AudioSource.
        /// </summary>
        public static void Pause (SoundType soundType)
        {
            SoundPlayManager.Instance.Pause (soundType);
        }


        /// <summary>
        /// UnPause AudioSource.
        /// </summary>
        public static void UnPause (SoundType soundType)
        {
            SoundPlayManager.Instance.UnPause (soundType);
        }


        /// <summary>
        /// Stop AudioSource.
        /// </summary>
        public static void Stop (SoundType soundType)
        {
            SoundPlayManager.Instance.Stop (soundType);
        }


        /// <summary>
        /// Control Volume AudioSource.
        /// </summary>
        public static void Volume (SoundType soundType, float volume)
        {
            SoundPlayManager.Instance.Volume (soundType, volume);
        }


        /// <summary>
        /// Mute AudioSource.
        /// </summary>
        public static void Mute (SoundType soundType, bool mute)
        {
            SoundPlayManager.Instance.Mute (soundType, mute);
        }

        #endregion


        #region EventMethods

        #endregion
    }
}