//using UnityEngine.Events;
//using UnityEngine.Advertisements;

///// <summary>
///// 유니티 광고 관리 클래스.
///// </summary>
//public class UnityAdsManager : SingletoneMonoClass<UnityAdsManager>
//{
//    /// <summary>
//    /// Ads ID.
//    /// </summary>
//    public const string ads_id = "ads_id";

//    void Awake()
//    {
//        if (Advertisement.isSupported)
//            Advertisement.Initialize(ads_id);
//    }

//    /// <summary>
//    /// 콜백 등록.
//    /// </summary>
//    /// <param name="p_uaction"></param>
//    public void RegistCallBack(UnityAction p_uaction)
//    {
//        uevent_callback.AddListener(p_uaction);
//    }

//    /// <summary>
//    /// 동영상 광고 출력.
//    /// </summary>
//    public void PlayAds()
//    {
//        if(Advertisement.IsReady())
//        {
//            Advertisement.Show(null, new ShowOptions
//            {
//                resultCallback = result =>
//                {
//                    switch(result)
//                    {
//                        case ShowResult.Failed:
//                            break;
//                        case ShowResult.Finished:
//                            break;
//                        case ShowResult.Skipped:
//                            break;
//                    }
//                }
//            });
//        }
//    }
//}

