using System;
using UniRx.Async;
using UnityEngine;

namespace KKSFramework.PlatformService
{
    public class PlatformServiceManager : Singleton<PlatformServiceManager>
    {
        public const string KeyUid = "KEY_UID";
        public const string KeyGuestLogin = "KEY_GUEST_LOGIN";
        public const string KeyPlatformAuth = "KEY_PLATFORM_AUTH";

        private bool _isInit;

        private IGameService _gameService;

        public IGameService Game { get; private set; }

        public string UID { get; private set; }

        public string DefaultUID { get; private set; }


        private IGameService CreateGameService ()
        {
            IGameService gameService = null;

#if UNITY_EDITOR
            gameService = new UnityEditorService ();
#elif UNITY_ANDROID
            gameService = new GooglePlayGameService ();
#elif UNITY_IPHONE
            gameService = new AppleGameCenterService ();
#endif
            return gameService;
        }


        public async UniTask InitializeAsync ()
        {
            if (_isInit)
            {
                return;
            }

            _isInit = true;

            var utcs = new UniTaskCompletionSource ();
            DeviceUniqueIdentifier.GetUniqueIdentifier (res =>
            {
                DefaultUID = res;

                PostInitialize ();
                utcs.TrySetResult ();
            });

            PostInitialize ();

            await utcs.Task;

            void PostInitialize ()
            {
                UID = PlayerPrefs.GetString (KeyUid, DefaultUID);
                if (string.IsNullOrEmpty (UID))
                    UID = DefaultUID;

#if UNITY_EDITOR
                var ues = Game as UnityEditorService;
                ues?.Initialize ();
#elif UNITY_ANDROID
                var gpg = _gameService as GooglePlayGameService;
                gpg.Initialize (isEnableSavedGames: false, isRequestIdToken: false);
#elif UNITY_IOS
                var agc = _gameService as AppleGameCenterService;
                agc.Initialize ();
#endif
            }
        }


        public void SetNewUID (string uid)
        {
            UID = uid;

            PlayerPrefs.SetString (KeyUid, UID);
            PlayerPrefs.Save ();
        }


        public void DeleteUID ()
        {
            PlayerPrefs.SetString (KeyUid, "");
            PlayerPrefs.Save ();

            UID = PlayerPrefs.GetString (KeyUid, "");
        }


        public static void SetGuestUser (bool isActive)
        {
            PlayerPrefs.SetInt (KeyGuestLogin, Convert.ToInt32 (isActive));
            PlayerPrefs.Save ();
        }


        public static void SetPlatformAuth (bool active)
        {
            PlayerPrefs.SetInt (KeyPlatformAuth, Convert.ToInt32 (active));
            PlayerPrefs.Save ();
        }


        public static bool GetGuestUser ()
        {
            return PlayerPrefs.GetInt (KeyGuestLogin, 0).Equals (1);
        }


        public static bool GetPlatformAuth ()
        {
            return PlayerPrefs.GetInt (KeyPlatformAuth, 0).Equals (1);
        }
    }
}