using System;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;

namespace KKSFramework.PlatformService
{
    public static class PlatformServiceHelper
    {
        private static bool _isInit;

        
        private static GameServiceBase _gameServiceBase;
        public static GameServiceBase GameServiceBase => _gameServiceBase;


        /// <summary>
        /// 초기화.
        /// </summary>
        public static void Initialize ()
        {
            if (_isInit)
            {
                return;
            }

            _isInit = true;

            CreateGameService ();
            PostInitialize ();

            void CreateGameService ()
            {
#if UNITY_EDITOR
                _gameServiceBase = new UnityEditorService ();
#elif UNITY_ANDROID
                _gameServiceBase = new GooglePlayGameService ();
#elif UNITY_IPHONE
                _gameServiceBase = new AppleGameCenterService ();
#endif
            }

            void PostInitialize ()
            {
#if UNITY_ANDROID
                var initModel = new GooglePlayInitializeModel ();
                _gameServiceBase.Initialize (initModel);
#else
                _gameServiceBase.Initialize (null);
#endif
            }
        }

        
        #region Authenticate
        
        public static void Login (Action<bool, string> callback)
        {
            _gameServiceBase?.Login (callback);
        }

        public static void LogOut ()
        {
            _gameServiceBase?.Logout ();
        }
        
        #endregion
        
        
        #region Achievement

        public static void UnlockAchievementRequest (string achievementId, Action<bool> callback = null)
        {
            _gameServiceBase?.UnlockAchievementRequest (achievementId, callback);
        }


        public static void IncrementAchievementRequest (string achievementId, int steps, Action<bool> callback = null)
        {
            _gameServiceBase?.IncrementAchievementRequest (achievementId, steps, callback);
        }


        public static void ShowAchievementsUi ()
        {
            _gameServiceBase?.ShowAchievementsUiRequest ();
        }

        #endregion
        
        
        #region Leaderboad

        public static void ReportScore (long score, Action<bool> callback = null, string leaderBoardId = null)
        {
            _gameServiceBase?.ReportScore (score, callback, leaderBoardId);
        }


        public static void ShowLeaderBoardUi ()
        {
            _gameServiceBase?.ShowLeaderBoardUi ();
        }


        public static void ShowLeaderBoardUi (string leaderBoardId)
        {
            _gameServiceBase?.ShowLeaderBoardUi (leaderBoardId);
        }

        #endregion
        
        
        #region Friend

        public static void LoadFriends(Action<bool, IUserProfile[]> callback)
        {
            _gameServiceBase?.LoadFriends (callback);
        }

        #endregion
        
        
        #if UNITY_ANDROID
        
        
        #region Accessing Leaderboard data

        public static void LoadScores(Action<LeaderboardScoreData> callback,
            LeaderboardStart start = LeaderboardStart.PlayerCentered,
            LeaderboardCollection collection = LeaderboardCollection.Public,
            LeaderboardTimeSpan timeSpan = LeaderboardTimeSpan.AllTime,
            int rowCount = 100,
            string leaderboardId = null)
        {
            (_gameServiceBase as GooglePlayGameService)?.LoadScores (callback, start, collection, timeSpan, rowCount, leaderboardId);
        }


        public static void GetNextScorePage(ScorePageToken nextPageToken, Action<LeaderboardScoreData> callback,
            int rowCount = 10)
        {
            (_gameServiceBase as GooglePlayGameService)?.GetNextScorePage (nextPageToken, callback, rowCount);
        }

        #endregion
        
        
        #region Cloud Save / Load

        public static void SavedGameData(byte[] data, Action<bool> callback)
        {
            (_gameServiceBase as GooglePlayGameService)?.SavedGameData (data, callback);
        }


        public static void LoadGameData(Action<bool, byte[]> callback)
        {
            (_gameServiceBase as GooglePlayGameService)?.LoadGameData (callback);
        }


        public static void DeleteGameData(Action<bool> callback = null)
        {
            (_gameServiceBase as GooglePlayGameService)?.DeleteGameData (callback);
        }

        #endregion
        
        #endif
    }
}