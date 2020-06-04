#if UNITY_ANDROID

using System;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using UnityEngine;

namespace KKSFramework.PlatformService
{
    public class GooglePlayGameService : GameServiceBase, IGameServiceCloudData
    {
        public string SaveDataName { get; private set; }

        public string IdToken => PlayGamesPlatform.Instance.GetIdToken();

        public string AccessCode => PlayGamesPlatform.Instance.GetServerAuthCode();

        public string PlayerImageUrl => PlayGamesPlatform.Instance.GetUserImageUrl();


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
                }
            });
        }

        #endregion


        #region Initialize

        public override void Initialize(object param)
        {
            if (IsInited)
            {
                return;
            }

            IsInited = true;

            if (param is GooglePlayInitializeModel initializeModel)
            {
                var config = new PlayGamesClientConfiguration.Builder ();

                // 구글 계정에 게임 저장 가능.
                if (initializeModel.IsEnableSavedGames) config = config.EnableSavedGames ();

                // 게임이 실행되고 있지 않을 때 받은 게임 초대장을 처리하기 위해 Callback 등록.
                if (initializeModel.InvitationReceivedCallback != null)
                    config = config.WithInvitationDelegate (initializeModel.InvitationReceivedCallback);

                // 플레이어의 email 주소를 요청. (동의 여부를 묻는 메시지가 나타난다.)
                if (initializeModel.IsRequestEmail) config = config.RequestEmail ();

                // 게임이 실행되고 있지 않을 때 받은 턴기반 매치 알림에 대한 Callback 등록.
                if (initializeModel.MatchCallback != null) config = config.WithMatchDelegate (initializeModel.MatchCallback);

                // 서버 인증 코드를 생성하여 관련된 백엔드 서버 응용 프로그램에 전달하고 OAuth 토큰과 교환 할수 있도록 요청한다.
                if (initializeModel.IsRequestServerAuthCode) config = config.RequestServerAuthCode (false);

                // ID 토큰을 생성하도록 요청한다. 이 OAuth 토큰을 사용하여 플레이어를 Firebase와 같은 다른 서비스로 식별 할수 있다.
                if (initializeModel.IsRequestIdToken) config = config.RequestIdToken ();

                PlayGamesPlatform.InitializeInstance (config.Build ());
            }
            
            PlayGamesPlatform.DebugLogEnabled = true;
            PlayGamesPlatform.Activate();
            SaveDataName = Application.identifier + "_cloud_savegame";
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
            savedGame.OpenWithAutomaticConflictResolution(SaveDataName, DataSource.ReadCacheOrNetwork,
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
                        callback.CallSafe(false);
                    }
                });
        }


        public void LoadGameData(Action<bool, byte[]> callback)
        {
            var savedGame = PlayGamesPlatform.Instance.SavedGame;
            savedGame.OpenWithAutomaticConflictResolution(SaveDataName, DataSource.ReadCacheOrNetwork,
                ConflictResolutionStrategy.UseLongestPlaytime, (openStatus, game) =>
                {
                    if (openStatus == SavedGameRequestStatus.Success)
                    {
                        savedGame.ReadBinaryData(game,
                            (status, data) => { callback.CallSafe(status == SavedGameRequestStatus.Success, data); });
                    }
                    else
                    {
                        callback.CallSafe(false, null);
                    }
                });
        }


        public void DeleteGameData(Action<bool> callback = null)
        {
            var savedGame = PlayGamesPlatform.Instance.SavedGame;
            savedGame.OpenWithAutomaticConflictResolution(SaveDataName, DataSource.ReadCacheOrNetwork,
                ConflictResolutionStrategy.UseLongestPlaytime, (status, game) =>
                {
                    if (status == SavedGameRequestStatus.Success)
                    {
                        savedGame.Delete(game);
                        callback.CallSafe(status == SavedGameRequestStatus.Success);
                    }
                    else
                    {
                        callback.CallSafe(false);
                    }
                });
        }

        #endregion
    }
}
#endif