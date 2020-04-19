using System;
using System.Collections.Generic;
using System.Linq;
using KKSFramework.Management;
using KKSFramework.ResourcesLoad;
using UnityEngine;

namespace KKSFramework.Sound
{
    public enum SoundType
    {
        Bgm,
        Sfx
    }


    /// <summary>
    /// Json 저장용 사운드 데이터 클래스
    /// </summary>
    [Serializable]
    public class AudioClipData
    {
        /// <summary>
        /// Add Data.
        /// </summary>
        public void Add ()
        {
            filePath.Add (string.Empty);
            volumes.Add (1f);
        }

        /// <summary>
        /// Insert List.
        /// </summary>
        public void Insert (int idx)
        {
            filePath.Insert (idx, string.Empty);
            volumes.Insert (idx, 1f);
        }

        /// <summary>
        /// Remove List At.
        /// </summary>
        public void RemoveAt (int idx)
        {
            filePath.RemoveAt (idx);
            volumes.RemoveAt (idx);
        }


        #region Fields & Property

        /// <summary>
        /// Sound Key.
        /// </summary>
        public List<string> filePath = new List<string> ();

        /// <summary>
        /// Volumes.
        /// </summary>
        public List<float> volumes = new List<float> ();

        #endregion
    }

    /// <summary>
    /// 사운드 데이터.
    /// </summary>
    [Serializable]
    public class AudioClipDataBase
    {
        /// <summary>
        /// Audio Clip.
        /// </summary>
        public AudioClip clip;

        /// <summary>
        /// Volume.
        /// </summary>
        public float volume;


        public AudioClipDataBase (AudioClip clip, float volume)
        {
            this.clip = clip;
            this.volume = volume;
        }
    }

    /// <summary>
    /// 사운드 실행 관리 클래스.
    /// </summary>
    public class SoundPlayManager : ManagerBase<SoundPlayManager>
    {
        #region Fields & Property

        /// <summary>
        /// 사운드 컴포넌트 베이스.
        /// </summary>
        private SoundPlayComponent _soundPlayComponent => ComponentBase as SoundPlayComponent;

        /// <summary>
        /// 오디오 데이터 클래스.
        /// </summary>
        private Dictionary<SoundTypeEnum, AudioClipDataBase> _soundClipDataDict =
            new Dictionary<SoundTypeEnum, AudioClipDataBase> ();


        /// <summary>
        /// return volume.
        /// </summary>
        private float GetVolume (SoundType soundType)
        {
            return _soundPlayComponent.AudioSource (soundType).volume;
        }

        #endregion


        #region Methods

        public override void InitManager ()
        {
            var bgmTextAsset = ResourcesLoadHelper.GetResources<TextAsset> (ResourceRoleType._Data, ResourcesType.Json,
                Constants.Framework.SoundJsonFileName);
            var clipData = JsonUtility.FromJson<AudioClipData> (bgmTextAsset.text);

            _soundClipDataDict = clipData.filePath.Zip (clipData.volumes, (path, volume) =>
            {
                var soundTypeEnum = (SoundTypeEnum) Enum.Parse (typeof (SoundTypeEnum), path);
                var clip = ResourcesLoadHelper.GetResources<AudioClip> (ResourceRoleType._Sound, path);
                return (soundTypeEnum, volume, clip);
            }).ToDictionary (tuple => tuple.soundTypeEnum, tuple => new AudioClipDataBase (tuple.clip, tuple.volume));
        }


        /// <summary>
        /// Play Audio Clip.
        /// </summary>
        public void Play (SoundType soundType, SoundTypeEnum soundTypeEnum)
        {
            Play (_soundPlayComponent.AudioSource (soundType), soundTypeEnum);
        }


        /// <summary>
        /// Play Audio Clip on AudioSource.
        /// </summary>
        public void Play (AudioSource audioSource, SoundTypeEnum soundTypeEnum)
        {
            var clipData = _soundClipDataDict[soundTypeEnum];
            audioSource.volume = clipData.volume;
            audioSource.clip = clipData.clip;
        }
        
        
        /// <summary>
        /// Play One Shot Audio Clip.
        /// </summary>
        public void PlayOneShot (SoundType soundType, SoundTypeEnum soundTypeEnum)
        {
            PlayOneShot (_soundPlayComponent.AudioSource (soundType), soundTypeEnum);
        }


        /// <summary>
        /// Play One Shot Audio Clip on AudioSource.
        /// </summary>
        public void PlayOneShot (AudioSource audioSource, SoundTypeEnum soundTypeEnum)
        {
            var clipData = _soundClipDataDict[soundTypeEnum];
            audioSource.PlayOneShot (clipData.clip, clipData.volume);
        }


        /// <summary>
        /// Pause AudioSource.
        /// </summary>
        public void Pause (SoundType soundType)
        {
            Pause (_soundPlayComponent.AudioSource (soundType));
        }


        /// <summary>
        /// Pause AudioSource.
        /// </summary>
        public void Pause (AudioSource audioSource)
        {
            audioSource.Pause ();
        }

        
        /// <summary>
        /// UnPause AudioSource.
        /// </summary>
        public void UnPause (SoundType soundType)
        {
            _soundPlayComponent.AudioSource (soundType).UnPause ();
        }
        
        
        /// <summary>
        /// UnPause AudioSource.
        /// </summary>
        public void UnPause (AudioSource audioSource)
        {
            audioSource.UnPause ();
        }

        
        /// <summary>
        /// Stop AudioSource.
        /// </summary>
        public void Stop (SoundType soundType)
        {
            _soundPlayComponent.AudioSource (soundType).Stop ();
        }
        
        
        /// <summary>
        /// UnPause AudioSource.
        /// </summary>
        public void Stop (AudioSource audioSource)
        {
            audioSource.Stop ();
        }
        

        /// <summary>
        /// Control Volume AudioSource.
        /// </summary>
        public void Volume (SoundType soundType, float volume)
        {
            _soundPlayComponent.AudioSource (soundType).volume = volume;
        }
        
        
        /// <summary>
        /// Control Volume AudioSource.
        /// </summary>
        public void Volume (AudioSource audioSource, float volume)
        {
            audioSource.volume = volume;
        }
        
        
        /// <summary>
        /// Mute AudioSource.
        /// </summary>
        public void Mute (SoundType soundType, bool mute)
        {
            _soundPlayComponent.AudioSource (soundType).mute = mute;
        }
        
        
        /// <summary>
        /// Mute AudioSource.
        /// </summary>
        public void Mute (AudioSource audioSource, bool mute)
        {
            audioSource.mute = mute;
        }

        #endregion
    }
}