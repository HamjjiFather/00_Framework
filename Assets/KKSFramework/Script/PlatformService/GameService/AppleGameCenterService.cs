#if GAME_PLATFORM

using System;
using UnityEngine;
using UnityEngine.SocialPlatforms;

#if UNITY_IPHONE
using UnityEngine.SocialPlatforms.GameCenter;
#endif


namespace KKSFramework.PlatformService
{
    public class AppleGameCenterService : IGameService
    {
        private string _defaultLeaderBoardId;
        private bool _isInited;

        public string PlayerId => IsGuestUser ? "" : Social.localUser.id;

        public string PlayerName => IsGuestUser ? "" : Social.localUser.userName;

        /// <summary>
        /// 로그인 완료후 이미지를 다운로드 하기 때문에 null 이 아닐때까지 기달렸다가 사용해야 한다.
        /// </summary>
        public Texture2D PlayerImage => IsGuestUser ? null : Social.localUser.image;

        public string PlayerImageUrl => "";

        public bool IsAuthenticated => IsGuestUser || IsPlatformAuth;

        public bool IsPlatformAuth { get; private set; }

        public string PlatformTag => IsGuestUser ? "" : "a";

        public bool IsGuestUser { get; private set; }


        #region Friend

        public void LoadFriends(Action<bool, IUserProfile[]> callback)
        {
            Social.localUser.LoadFriends(success =>
            {
                callback.CallSafe(success, Social.localUser.friends);
            });
        }

        #endregion


        #region Initialize

        public void Initialize()
        {
            if (_isInited)
            {
                return;
            }

            _isInited = true;

#if UNITY_IPHONE
            GameCenterPlatform.ShowDefaultAchievementCompletionBanner (true);
#endif
            IsGuestUser = PlatformService.GetGuestUser();
            IsPlatformAuth = PlatformService.GetPlatformAuth();
        }


        public void SetDefaultLeaderBoard(string leaderBoardId)
        {
            _defaultLeaderBoardId = leaderBoardId;
        }

        #endregion


        #region Login / Logout

        public void LoginGuest(string uid, Action<bool> callback)
        {
            SetGuestUser(true);
            SetPlatformAuth(false);
            PlatformService.Instance.SetNewUID(uid);

            callback?.Invoke(true);
        }


        public void Login(Action<bool> callback)
        {
            Social.localUser.Authenticate(res =>
            {
                if (res)
                {
                    PlatformService.Instance.SetNewUID(PlatformService.Instance.DefaultUID);
                    SetGuestUser(false);
                    SetPlatformAuth(true);
                }

                callback?.Invoke(res);
            });
        }


        public void Login(Action<bool, string> callback)
        {
            Social.localUser.Authenticate((res1, res2) =>
            {
                if (res1)
                {
                    PlatformService.Instance.SetNewUID(PlatformService.Instance.DefaultUID);
                    SetGuestUser(false);
                    SetPlatformAuth(true);
                }

                callback?.Invoke(res1, res2);
            });
        }


        public void SetGuestUser(bool active)
        {
            IsGuestUser = active;
            PlatformService.SetGuestUser(IsGuestUser);
        }


        private void SetPlatformAuth(bool active)
        {
            IsPlatformAuth = active;
            PlatformService.SetPlatformAuth(IsPlatformAuth);
        }


        public void Logout()
        {
            PlatformService.Instance.DeleteUID();

            SetGuestUser(false);
            SetPlatformAuth(false);
        }

        #endregion


        #region Achievement

        public void ShowAchievementsUi()
        {
            Social.ShowAchievementsUI();
        }


        public void UnlockAchievement(string achievementId, Action<bool> callback = null)
        {
            // 0.0f의 진행은 업적을 나타냄을 의미하고 100.0f의 진행은 업적 달성을 의미합니다.
            // 따라서 잠금 해제하지 않고 이전에 숨겨진 업적을 표시하려면 Social.ReportProgress를 0.0f의 진행률로 호출하기 만하면 됩니다.
            Social.ReportProgress(achievementId, 100.0f, callback);
        }

        #endregion


        #region Leaderboard

        public void PostScore(long score, Action<bool> callback = null, string leaderBoardId = null)
        {
            leaderBoardId = leaderBoardId ?? _defaultLeaderBoardId;
            Social.ReportScore(score, leaderBoardId, callback);
        }


        public void ShowLeaderBoardUi()
        {
            Social.ShowLeaderboardUI();
        }


        public void ShowLeaderBoardUi(string leaderBoardId)
        {
            throw new NotImplementedException();
        }

        #endregion


        #region Cloud Save / Load

        public void SavedGameData(byte[] data, Action<bool> callback)
        {
            throw new NotImplementedException();
        }


        public void LoadGameData(Action<bool, byte[]> callback)
        {
            throw new NotImplementedException();
        }


        public void DeleteGameData(Action<bool> callback = null)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}

#endif