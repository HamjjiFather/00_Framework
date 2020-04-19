using System;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace KKSFramework.PlatformService
{
    public class UnityEditorService : IGameService
    {
        private bool _isInited;

        public string PlayerId => "";

        public string PlayerName { get; private set; }

        public Texture2D PlayerImage { get; private set; }

        public string PlayerImageUrl { get; private set; }
        public bool IsAuthenticated => IsGuestUser;
        public bool IsPlatformAuth => true;

        public string PlatformTag => "n";

        public bool IsGuestUser { get; private set; }


        public void LoginGuest(string uid, Action<bool> callback)
        {
            SetGuestUser(true);
            PlatformService.Instance.SetNewUID(uid);
            callback?.Invoke(true);
        }


        public void Login(Action<bool> callback)
        {
            SetGuestUser(true);

            callback?.Invoke(true);
        }


        public void Login(Action<bool, string> callback)
        {
            SetGuestUser(true);

            callback?.Invoke(true, "");
        }


        public void SetGuestUser(bool active)
        {
            IsGuestUser = active;
            PlatformService.SetGuestUser(IsGuestUser);
        }


        public void Logout()
        {
            PlatformService.Instance.DeleteUID();
            SetGuestUser(false);
        }


        public void ShowAchievementsUi()
        {
            // Pass
        }


        public void UnlockAchievement(string achievementId, Action<bool> callback = null)
        {
            // Pass
        }


        public void SetDefaultLeaderBoard(string leaderBoardId)
        {
            // Pass
        }


        public void PostScore(long score, Action<bool> callback = null, string leaderBoardId = null)
        {
            // Pass
        }


        public void ShowLeaderBoardUi()
        {
            // Pass
        }


        public void ShowLeaderBoardUi(string leaderBoardId)
        {
            // Pass
        }


        public void SavedGameData(byte[] data, Action<bool> callback)
        {
            // Pass
        }


        public void LoadGameData(Action<bool, byte[]> callback)
        {
            // Pass
        }


        public void DeleteGameData(Action<bool> callback = null)
        {
            // Pass
        }


        public void LoadFriends(Action<bool, IUserProfile[]> callback)
        {
            // Pass
        }


        public void Initialize()
        {
            if (_isInited)
            {
                Log.Warning(nameof(UnityEditorService), "이미 초기화 되어 있다");
                return;
            }

            Log.Verbose(nameof(UnityEditorService), "Initialize");
            _isInited = true;

            IsGuestUser = PlatformService.GetGuestUser();
        }
    }
}