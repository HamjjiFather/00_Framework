using System;
using UniRx.Async;
using UnityEngine;

namespace KKSFramework.PlatformService
{
    public class PlatformService : MonobehaviourSingletonClass<PlatformService>
    {
        public const string KEY_UID = "KEY_UID";
        public const string KEY_GUEST_LOGIN = "KEY_GUEST_LOGIN";
        public const string KEY_PLATFORM_AUTH = "KEY_PLATFORM_AUTH";


        /*
        private FacebookWrapper _facebook;
        public static FacebookWrapper Facebook => Instance._facebook;
        */

#if BF_FIREBASE_ANALITICS
        private FirebaseAlnalyticsWrapper _firebaseAlnalytics;
        public static FirebaseAlnalyticsWrapper FirebaseAlnalytics => Instance._firebaseAlnalytics;
#endif

#if BF_FIREBASE_AUTH
        private FirebaseAuthService _firebaseAuth;
        public FirebaseAuthService FirebaseAuth => _firebaseAuth;
#endif

        private bool _isInit;

        private IGameService _gameService;

        public IGameService Game { get; private set; }

        public IInAppPurchaser Iap { get; private set; }


        public string UID { get; private set; }


        public string DefaultUID { get; private set; }

        protected override void Awake()
        {
            base.Awake();

            //_facebook = gameObject.AddComponent<FacebookWrapper>();
#if BF_FIREBASE_ANALITICS
            _firebaseAlnalytics = gameObject.AddComponent<FirebaseAlnalyticsWrapper>();
#endif
            Game = CreateGameService();
            Iap = CreateInAppPurchaser();

#if BF_FIREBASE_AUTH
            _firebaseAuth = new FirebaseAuthService ();
#endif
        }


        private IGameService CreateGameService()
        {
            IGameService gameService = null;

#if UNITY_EDITOR
            gameService = new UnityEditorService();
#elif UNITY_ANDROID
            gameService = new GooglePlayGameService ();
#elif UNITY_IPHONE
            gameService = new AppleGameCenterService ();
#elif UNITY_FACEBOOK
            gameService = new FacebookGameroomService ();
#endif
            return gameService;
        }


        private IInAppPurchaser CreateInAppPurchaser()
        {
            IInAppPurchaser iap = null;

#if BF_UNITY_IAP
            iap = new UnityInAppPurchaser ();
#endif

            return iap;
        }


        public async UniTask InitializeAsync()
        {
            if (_isInit)
            {
                Log.Verbose(nameof(PlatformService), "이미 초기화 되어 있다");
                return;
            }

            _isInit = true;
            Log.Verbose(nameof(PlatformService), "Initialize");

            var utcs = new UniTaskCompletionSource();
            DeviceUniqueIdentifier.GetUniqueIdentifier(res =>
            {
                DefaultUID = res;
                Log.Verbose(nameof(PlatformService), $"DefaultUID : {DefaultUID}");

                PostInitialize();
                utcs.TrySetResult();
            });

            PostInitialize();

            await utcs.Task;

            void PostInitialize()
            {
                UID = PlayerPrefs.GetString(KEY_UID, DefaultUID);
                if (string.IsNullOrEmpty(UID))
                    UID = DefaultUID;
//                    Log.Verbose (nameof (PlatformService), $"Loaded Uid : {_uid}");

#if UNITY_EDITOR
                var ues = Game as UnityEditorService;
                ues.Initialize();
#elif UNITY_ANDROID
                var gpg = _gameService as GooglePlayGameService;
                gpg.Initialize (isEnableSavedGames: false, isRequestIdToken: false);
#elif UNITY_IOS
                var agc = _gameService as AppleGameCenterService;
                agc.Initialize ();
#endif

#if BF_FIREBASE_AUTH && !UNITY_EDITOR
                _firebaseAuth.InitializeFirebase ();
#endif
            }
        }


        public void SetNewUID(string uid)
        {
            Log.Verbose(nameof(PlatformService), $"SetNewUID = {uid}");
            UID = uid;

            PlayerPrefs.SetString(KEY_UID, UID);
            PlayerPrefs.Save();
        }


        public void DeleteUID()
        {
            PlayerPrefs.SetString(KEY_UID, "");
            PlayerPrefs.Save();

            UID = PlayerPrefs.GetString(KEY_UID, "");
        }


        public static void SetGuestUser(bool isActive)
        {
            PlayerPrefs.SetInt(KEY_GUEST_LOGIN, Convert.ToInt32(isActive));
            PlayerPrefs.Save();
        }


        public static void SetPlatformAuth(bool active)
        {
            PlayerPrefs.SetInt(KEY_PLATFORM_AUTH, Convert.ToInt32(active));
            PlayerPrefs.Save();
        }


        public static bool GetGuestUser()
        {
            return PlayerPrefs.GetInt(KEY_GUEST_LOGIN, 0).Equals(1);
        }


        public static bool GetPlatformAuth()
        {
            return PlayerPrefs.GetInt(KEY_PLATFORM_AUTH, 0).Equals(1);
        }
    }
}