#if UNITY_ANDROID || UNITY_IOS

using System.Collections.Generic;
using UnityEngine.Advertisements;

namespace KKSFramework.UnityAds
{
    /// <summary>
    /// Ads placement name type.
    /// Must be changed by games. 
    /// </summary>
    public enum AdsPlacementNames
    {
        video,
        rewardedVideo,
        interstitial,
        banner,
    }

    public class UnityAdsSettingModel
    {
        /// <summary>
        /// Apple App Store Unity Ads ID.
        /// 7 number digit. 
        /// </summary>
        public const string AppleAdsId = "3638559";

        /// <summary>
        /// Google Play Store Unity Ads ID.
        /// 7 number digit. 
        /// </summary>
        public const string GoogleAdsId = "3638558";

        /// <summary>
        /// TestMode Flag.
        /// </summary>
        public const bool IsTestMode = true;

        /// <summary>
        /// Ads model.
        /// </summary>
        public readonly Dictionary<AdsPlacementNames, UnityAdsModel> UnityAdsModels =
            new Dictionary<AdsPlacementNames, UnityAdsModel> ();

        /// <summary>
        /// Banner ads model.
        /// </summary>
        public readonly Dictionary<AdsPlacementNames, BannerAdsModel> BannerAdsModels =
            new Dictionary<AdsPlacementNames, BannerAdsModel> ();


        /// <summary>
        /// Set Ads model.
        /// Must be changed by games. 
        /// </summary>
        public UnityAdsSettingModel ()
        {
            UnityAdsModels.Add (AdsPlacementNames.video, new UnityAdsModel (UnityAdsType.Video));
            UnityAdsModels.Add (AdsPlacementNames.rewardedVideo, new UnityAdsModel (UnityAdsType.Video));
            UnityAdsModels.Add (AdsPlacementNames.interstitial, new UnityAdsModel (UnityAdsType.Interstitial));

            BannerAdsModels.Add (AdsPlacementNames.banner,
                new BannerAdsModel (UnityAdsType.Banner, BannerPosition.BOTTOM_CENTER));
        }
    }
}

#endif