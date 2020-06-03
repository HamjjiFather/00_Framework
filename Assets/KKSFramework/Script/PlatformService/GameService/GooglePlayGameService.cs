#if (UNITY_ANDROID || (UNITY_IPHONE && !NO_GPGS)) && GAME_PLATFORM

using System;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.Multiplayer;
using GooglePlayGames.BasicApi.SavedGame;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace KKSFramework.PlatformService
{
    public class GooglePlayGameService : IGameService
    {
        private string _defaultLeaderBoardId;
        private bool _isInited;

        private string _savedDataFilename;

        public string IdToken => PlayGamesPlatform.Instance.GetIdToken();

        public string AccessCode => PlayGamesPlatform.Instance.GetServerAuthCode();

        public string PlayerId => Social.localUser.id;

        public string PlayerName => Social.localUser.userName;

        public string PlatformTag => IsGuestUser ? "" : "g";


        public bool IsGuestUser { get; private set; }

        /// <summary>
        /// 로그인 완료후 이미지를 다운로드 하기 때문에 null 이 아닐때까지 기달렸다가 사용해야 한다.
        /// </summary>
        public Texture2D PlayerImage => Social.localUser.image;

        public string PlayerImageUrl => PlayGamesPlatform.Instance.GetUserImageUrl();

        public bool IsAuthenticated => IsGuestUser || IsPlatformAuth;

        public bool IsPlatformAuth { get; private set; }


        #region Friend

        public void LoadFriends(Action<bool, IUserProfile[]> callback)
        {
            Social.localUser.LoadFriends(success =>
            {
                Log.Verbose(nameof(GooglePlayGameService), "Friends loaded OK : " + success);
                callback.CallSafe(success, Social.localUser.friends);
            });
        }

        #endregion


        #region Player Stats

        /// <summary>
        /// 플레이어 통계 API를 사용하면 플레이어의 특정 세그먼트 및 플레이어 라이프 사이클의 여러 단계에 맞게 게임 경험을 조정할 수 있습니다.
        /// 플레이어의 진행 방식, 지출 및 참여 방식을 기반으로 각 플레이어 부문별로 맞춤식 환경을 구축 할 수 있습니다.
        /// 참고 https://developers.google.com/android/reference/com/google/android/gms/common/api/CommonStatusCodes
        /// 참고 https://developers.google.com/games/services/android/stats
        /// </summary>
        public void RequestPlayerStats(Action<PlayerStats> callback)
        {
            ((PlayGamesLocalUser) Social.localUser).GetStats((rc, stats) =>
            {
                // -1은 캐시된 통계를 의미하고 0은 성공을 의미 합니다.
                // 모든 값에 대해 CommonStatusCodes를 참조하십시오. 
                if (rc <= 0 && stats.HasDaysSinceLastPlayed())
                {
                    callback.CallSafe(stats);
                    Log.Verbose(nameof(GooglePlayGameService), "It has been " + stats.DaysSinceLastPlayed + " days");
                }
            });
        }

        #endregion


        #region Initialize

        public void Initialize(bool isEnableSavedGames = true, bool isRequestEmail = false,
            bool isRequestServerAuthCode = false, bool isRequestIdToken = false,
            InvitationReceivedDelegate invitationReceivedCallback = null,
            MatchDelegate matchCallback = null)
        {
            if (_isInited)
            {
                Log.Warning(nameof(GooglePlayGameService), "플랫폼은 이미 초기화 되어 있다");
                return;
            }

            _isInited = true;

            Log.Verbose(nameof(GooglePlayGameService), "Initialize");

            var config = new PlayGamesClientConfiguration.Builder();

            // 구글 계정에 게임 저장 가능.
            if (isEnableSavedGames) config = config.EnableSavedGames();

            // 게임이 실행되고 있지 않을 때 받은 게임 초대장을 처리하기 위해 Callback 등록.
            if (invitationReceivedCallback != null) config = config.WithInvitationDelegate(invitationReceivedCallback);

            // 플레이어의 email 주소를 요청. (동의 여부를 묻는 메시지가 나타난다.)
            if (isRequestEmail) config = config.RequestEmail();

            // 게임이 실행되고 있지 않을 때 받은 턴기반 매치 알림에 대한 Callback 등록.
            if (matchCallback != null) config = config.WithMatchDelegate(matchCallback);

            // 서버 인증 코드를 생성하여 관련된 백엔드 서버 응용 프로그램에 전달하고 OAuth 토큰과 교환 할수 있도록 요청한다.
            if (isRequestServerAuthCode) config = config.RequestServerAuthCode(false);

            // ID 토큰을 생성하도록 요청한다. 이 OAuth 토큰을 사용하여 플레이어를 Firebase와 같은 다른 서비스로 식별 할수 있다.
            if (isRequestIdToken) config = config.RequestIdToken();

            PlayGamesPlatform.InitializeInstance(config.Build());
#if BF_DEBUG
            PlayGamesPlatform.DebugLogEnabled = true;
#endif
            // Google Play Games platform 활성화
            PlayGamesPlatform.Activate();
            _savedDataFilename = Application.identifier + "_cloud_savegame";
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


        /// <summary>
        /// 인증에 필요한 동의 대화 상자가 표시된다.
        /// 과거에 이미 로그인한 경우 이 프로세스는 자동으로 수행되며 사용자는 대화 상자와 상호 작용할 필요가 없다.
        /// </summary>
        /// <param name="callback">성공 또는 실패 처리</param>
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
            PlayGamesPlatform.Instance.SignOut();
        }

        #endregion


        #region Achievement

        public void UnlockAchievement(string achievementId, Action<bool> callback = null)
        {
            // 0.0f의 진행은 업적을 나타냄을 의미하고 100.0f의 진행은 업적 달성을 의미합니다.
            // 따라서 잠금 해제하지 않고 이전에 숨겨진 업적을 표시하려면 Social.ReportProgress를 0.0f의 진행률로 호출하기 만하면 됩니다.
            Social.ReportProgress(achievementId, 100.0f, callback);
        }


        public void IncrementAchievement(string achievementId, int steps, Action<bool> callback = null)
        {
            PlayGamesPlatform.Instance.IncrementAchievement(achievementId, steps, callback);
        }


        public void ShowAchievementsUi()
        {
            Social.ShowAchievementsUI();
        }

        #endregion


        #region Leaderboad

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
            PlayGamesPlatform.Instance.ShowLeaderboardUI(leaderBoardId);
        }

        #endregion


        #region Accessing Leaderboard data

        public void LoadScores(Action<LeaderboardScoreData> callback,
            LeaderboardStart start = LeaderboardStart.PlayerCentered,
            LeaderboardCollection collection = LeaderboardCollection.Public,
            LeaderboardTimeSpan timeSpan = LeaderboardTimeSpan.AllTime,
            int rowCount = 100,
            string leaderboardId = null)
        {
            leaderboardId = leaderboardId ?? _defaultLeaderBoardId;
            PlayGamesPlatform.Instance.LoadScores(leaderboardId, start, rowCount, collection, timeSpan, callback);
        }


        public void GetNextScorePage(ScorePageToken nextPageToken, Action<LeaderboardScoreData> callback,
            int rowCount = 10)
        {
            PlayGamesPlatform.Instance.LoadMoreScores(nextPageToken, rowCount, callback);
        }

        #endregion


        #region Cloud Save / Load

        public void SavedGameData(byte[] data, Action<bool> callback)
        {
            var savedGame = PlayGamesPlatform.Instance.SavedGame;
            savedGame.OpenWithAutomaticConflictResolution(_savedDataFilename, DataSource.ReadCacheOrNetwork,
                ConflictResolutionStrategy.UseLongestPlaytime, (openStatus, game) =>
                {
                    if (openStatus == SavedGameRequestStatus.Success)
                    {
                        var builder = new SavedGameMetadataUpdate.Builder();
                        var updatedMetadata = builder.Build();
                        savedGame.CommitUpdate(game, updatedMetadata, data,
                            (commitStatus, _) =>
                            {
                                callback.CallSafe(commitStatus == SavedGameRequestStatus.Success);
                            });
                    }
                    else
                    {
                        Log.Warning(nameof(GooglePlayGameService),
                            "PlayGamesPlatform.Instance.SavedGame.OpenWithAutomaticConflictResolution 실패");
                        callback.CallSafe(false);
                    }
                });
        }


        public void LoadGameData(Action<bool, byte[]> callback)
        {
            var savedGame = PlayGamesPlatform.Instance.SavedGame;
            savedGame.OpenWithAutomaticConflictResolution(_savedDataFilename, DataSource.ReadCacheOrNetwork,
                ConflictResolutionStrategy.UseLongestPlaytime, (openStatus, game) =>
                {
                    if (openStatus == SavedGameRequestStatus.Success)
                    {
                        savedGame.ReadBinaryData(game,
                            (status, data) => { callback.CallSafe(status == SavedGameRequestStatus.Success, data); });
                    }
                    else
                    {
                        Log.Warning(nameof(GooglePlayGameService),
                            "PlayGamesPlatform.Instance.SavedGame.OpenWithAutomaticConflictResolution 실패");
                        callback.CallSafe(false, null);
                    }
                });
        }


        public void DeleteGameData(Action<bool> callback = null)
        {
            var savedGame = PlayGamesPlatform.Instance.SavedGame;
            savedGame.OpenWithAutomaticConflictResolution(_savedDataFilename, DataSource.ReadCacheOrNetwork,
                ConflictResolutionStrategy.UseLongestPlaytime, (status, game) =>
                {
                    if (status == SavedGameRequestStatus.Success)
                    {
                        savedGame.Delete(game);
                        callback.CallSafe(status == SavedGameRequestStatus.Success);
                    }
                    else
                    {
                        Log.Warning(nameof(GooglePlayGameService),
                            "PlayGamesPlatform.Instance.SavedGame.OpenWithAutomaticConflictResolution 실패");
                        callback.CallSafe(false);
                    }
                });
        }

        #endregion
    }
}
#endif