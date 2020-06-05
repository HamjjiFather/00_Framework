#if UNITY_ANDROID || UNITY_IOS

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Advertisements;

namespace KKSFramework.UnityAds
{
    /// <summary>
    /// 광고 타입.
    /// </summary>
    public enum UnityAdsType
    {
        Video,
        Interstitial,
        Banner,
    }

    /// <summary>
    /// 배너 광고 모델.
    /// </summary>
    public class BannerAdsModel : UnityAdsModel
    {
        public BannerPosition BannerPosition;

        public BannerAdsModel (UnityAdsType unityAdsType, BannerPosition bannerPosition) : base (unityAdsType)
        {
            BannerPosition = bannerPosition;
        }
    }
    
    
    /// <summary>
    /// 광고 모델.
    /// </summary>
    public class UnityAdsModel
    {
        public UnityAdsType UnityAdsType;

        public UnityAdsModel (UnityAdsType unityAdsType)
        {
            UnityAdsType = unityAdsType;
        }
    }
    
    /// <summary>
    /// 유니티 광고 관리 클래스.
    /// </summary>
    public static class UnityAdsManager
    {
        private static readonly UnityAdsSettingModel UnityAdsSettingModel = new UnityAdsSettingModel ();
        
        private static readonly UnityAdsListener UnityAdsListener = new UnityAdsListener ();

        /// <summary>
        /// Initialize.
        /// </summary>
        public static void Initialize ()
        {
            if (Advertisement.isSupported)
            {
#if UNITY_ANDROID
                Advertisement.Initialize (UnityAdsSettingModel.GoogleAdsId, UnityAdsSettingModel.IsTestMode);
#elif UNITY_IOS
                Advertisement.Initialize (UnityAdsSettingModel.AppleAdsId, UnityAdsSettingModel.IsTestMode);
#endif
                Advertisement.AddListener (UnityAdsListener);
            }
            
            UnityAdsSettingModel.UnityAdsModels.Foreach (model =>
            {
                Advertisement.Load (model.Key.ToString());
            });
            
            UnityAdsSettingModel.BannerAdsModels.Foreach (model =>
            {
                Advertisement.Banner.Load (model.Key.ToString());
            });
        }


        /// <summary>
        /// 배너 광고 출력.
        /// </summary>
        public static void PlayBannerAds (AdsPlacementNames key)
        {
            if (!UnityAdsSettingModel.BannerAdsModels.ContainsKey (key))
                return;
            
            Advertisement.Banner.SetPosition (UnityAdsSettingModel.BannerAdsModels[key].BannerPosition);
            Advertisement.Banner.Show (key.ToString());
        }


        /// <summary>
        /// 전면 광고 출력.
        /// </summary>
        public static void PlayInterstitialAds (AdsPlacementNames key)
        {
            if (!UnityAdsSettingModel.UnityAdsModels.ContainsKey (key))
                return;
            
            Advertisement.Show (key.ToString());
        }


        /// <summary>
        /// 동영상 광고 출력.
        /// </summary>
        public static void PlayVideoAds (AdsPlacementNames key)
        {
            if (!UnityAdsSettingModel.UnityAdsModels.ContainsKey (key))
                return;
            
            if (Advertisement.IsReady ())
            {
                Advertisement.Show (key.ToString());
            }
        }

        
        public static void AddReadyCallback (UnityAction<string> callback)
        {
            UnityAdsListener.AdsReadyCallback = callback;
        }
        
        public static void AdsDidStartCallback (UnityAction<string> callback)
        {
            UnityAdsListener.AdsDidStartCallback = callback;
        }
        
        public static void AdsDidErrorCallback (UnityAction<string> callback)
        {
            UnityAdsListener.AdsDidErrorCallback = callback;
        }
        
        public static void AdsDidFinishCallback (UnityAction<string, ShowResult> callback)
        {
            UnityAdsListener.AdsDidFinishCallback = callback;
        }
    }
}

#endif