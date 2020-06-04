#if UNITY_ANDROID

using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.Multiplayer;

namespace KKSFramework.PlatformService
{
    /// <summary>
    /// GooglePlay  
    /// </summary>
    public class GooglePlayInitializeModel
    {
        public bool IsEnableSavedGames = false;

        public bool IsRequestEmail = false;

        public bool IsRequestServerAuthCode = false;

        public bool IsRequestIdToken = false;

        public InvitationReceivedDelegate InvitationReceivedCallback = null;

        public MatchDelegate MatchCallback = null;
    }
}

#endif