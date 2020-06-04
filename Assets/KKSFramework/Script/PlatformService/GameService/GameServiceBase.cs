using System;
using GooglePlayGames;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace KKSFramework.PlatformService
{
    public abstract class GameServiceBase : IGameServiceCore, IGameServiceAchievement, IGameServiceLeaderboard, IGameServiceFriend
    {
        #region Fields & Property

        public bool IsInited { get; protected set; }

        public string PlayerId => Social.localUser.id;

        public string PlayerName => Social.localUser.userName;

        public Texture2D PlayerImage => Social.localUser.image;

        public bool IsAuthenticated { get; protected set; }

        public bool IsGuestUser { get; private set; }

#pragma warning disable CS0649

#pragma warning restore CS0649

        #endregion


        #region Methods

        public abstract void Initialize (object param);


        #region Authenticate

        public virtual void Login (Action<bool, string> callback)
        {
            Social.localUser.Authenticate ((isAuthenticated, res2) =>
            {
                if (isAuthenticated)
                {
                    IsAuthenticated = true;
                    IsGuestUser = false;
                }

                callback?.Invoke (isAuthenticated, res2);
            });
        }

        public virtual void Logout ()
        {
            IsAuthenticated = false;
            IsGuestUser = false;
        }

        #endregion


        #region Achievement

        public void UnlockAchievementRequest (string achievementId, Action<bool> callback = null)
        {
            // 0.0f의 진행은 업적을 나타냄을 의미하고 100.0f의 진행은 업적 달성을 의미합니다.
            // 따라서 잠금 해제하지 않고 이전에 숨겨진 업적을 표시하려면 Social.ReportProgress를 0.0f의 진행률로 호출하기 만하면 됩니다.
            Social.ReportProgress (achievementId, 100.0f, callback);
        }


        public void IncrementAchievementRequest (string achievementId, int steps, Action<bool> callback = null)
        {
            PlayGamesPlatform.Instance.IncrementAchievement (achievementId, steps, callback);
        }


        public void ShowAchievementsUiRequest ()
        {
            Social.ShowAchievementsUI ();
        }

        #endregion


        #region Leaderboad

        public void ReportScore (long score, Action<bool> callback = null, string leaderBoardId = null)
        {
            Social.ReportScore (score, leaderBoardId, callback);
        }


        public void ShowLeaderBoardUi ()
        {
            Social.ShowLeaderboardUI ();
        }


        public void ShowLeaderBoardUi (string leaderBoardId)
        {
            PlayGamesPlatform.Instance.ShowLeaderboardUI (leaderBoardId);
        }

        #endregion


        #region Friend

        public void LoadFriends (Action<bool, IUserProfile[]> callback)
        {
            Social.localUser.LoadFriends (success => { callback.CallSafe (success, Social.localUser.friends); });
        }

        #endregion

        #endregion
    }
}