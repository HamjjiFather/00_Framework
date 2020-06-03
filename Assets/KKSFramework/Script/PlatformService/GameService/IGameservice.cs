using System;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace KKSFramework.PlatformService
{
    public interface IGameService
    {
        string PlayerId { get; }

        string PlayerName { get; }

        Texture2D PlayerImage { get; }

        string PlayerImageUrl { get; }

        /// <summary>
        /// 인증이 되어 있는지(플랫폼 or 게스트)
        /// </summary>
        bool IsAuthenticated { get; }

        /// <summary>
        /// 이전 실행때 플랫폼 인증을 선택했는지
        /// </summary>
        bool IsPlatformAuth { get; }

        string PlatformTag { get; }

        bool IsGuestUser { get; }


        #region Friend

        void LoadFriends(Action<bool, IUserProfile[]> callback);

        #endregion

        #region Login / Logout

        void LoginGuest(string uid, Action<bool> callback);
        void Login(Action<bool> callback);
        void Login(Action<bool, string> callback);
        void SetGuestUser(bool active);
        void Logout();

        #endregion


        #region Achievement

        void ShowAchievementsUi();
        void UnlockAchievement(string achievementId, Action<bool> callback = null);

        #endregion


        #region Leaderboad

        void SetDefaultLeaderBoard(string leaderBoardId);
        void PostScore(long score, Action<bool> callback = null, string leaderBoardId = null);
        void ShowLeaderBoardUi();
        void ShowLeaderBoardUi(string leaderBoardId);

        #endregion


        #region Cloud Save / Load

        void SavedGameData(byte[] data, Action<bool> callback);
        void LoadGameData(Action<bool, byte[]> callback);
        void DeleteGameData(Action<bool> callback = null);

        #endregion
    }
}