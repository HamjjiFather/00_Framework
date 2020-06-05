#if UNITY_ANDROID || UNITY_IOS

using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.UI;

namespace KKSFramework.UnityAds
{
    public class UnityAdsSample : MonoBehaviour
    {
        public Button playAdsButton;
        
        public Button playInterstitialButton;
        
        public Button playBannerAdsButton;

        private void Start ()
        {
            UnityAdsManager.Initialize ();
            UnityAdsManager.AdsDidFinishCallback (FinishCallback);
            
            playAdsButton.onClick.AddListener (() =>
            {
                UnityAdsManager.PlayVideoAds (AdsPlacementNames.video);
            });
            
            playInterstitialButton.onClick.AddListener (() =>
            {
                UnityAdsManager.PlayInterstitialAds(AdsPlacementNames.interstitial);
            });
            
            playBannerAdsButton.onClick.AddListener (() =>
            {
                UnityAdsManager.PlayBannerAds(AdsPlacementNames.banner);
            });
        }

        private void FinishCallback (string placementId, ShowResult showResult)
        {
            Debug.Log (showResult);
        }
    }
}

#endif