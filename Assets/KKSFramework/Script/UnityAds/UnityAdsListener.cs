using UnityEngine.Advertisements;
using UnityEngine.Events;

namespace KKSFramework.UnityAds
{
    /// <summary>
    /// Ads State Listener.
    /// </summary>
    public class UnityAdsListener : IUnityAdsListener
    {
        /// <summary>
        /// Ready state callback.
        /// </summary>
        public UnityAction<string> AdsReadyCallback;
        
        /// <summary>
        /// Error state callback.
        /// </summary>
        public UnityAction<string> AdsDidErrorCallback;
        
        /// <summary>
        /// Start state callback.
        /// </summary>
        public UnityAction<string> AdsDidStartCallback;
        
        /// <summary>
        /// Finish state callback.
        /// </summary>
        public UnityAction<string, ShowResult> AdsDidFinishCallback;
        
        public void OnUnityAdsReady (string placementId)
        {
            AdsReadyCallback?.Invoke (placementId);
            AdsReadyCallback = null;
        }

        public void OnUnityAdsDidError (string message)
        {
            AdsDidErrorCallback?.Invoke (message);
            AdsDidErrorCallback = null;
        }

        public void OnUnityAdsDidStart (string placementId)
        {
            AdsDidStartCallback?.Invoke (placementId);
            AdsDidStartCallback = null;
        }

        public void OnUnityAdsDidFinish (string placementId, ShowResult showResult)
        {
            AdsDidFinishCallback?.Invoke (placementId, showResult);
            AdsDidFinishCallback = null;
        }
    }
}