#if UNITY_IOS

using System;
using UnityEngine;
using UnityEngine.SocialPlatforms;

#if UNITY_IPHONE
using UnityEngine.SocialPlatforms.GameCenter;
#endif


namespace KKSFramework.PlatformService
{
    public class AppleGameCenterService : GameServiceBase
    {
        #region Initialize


        public override void Initialize (object param)
        {
            if (IsInited)
            {
                return;
            }

            IsInited = true;
            
#if UNITY_IPHONE
            GameCenterPlatform.ShowDefaultAchievementCompletionBanner (true);
#endif
        }

        #endregion

    }
}

#endif