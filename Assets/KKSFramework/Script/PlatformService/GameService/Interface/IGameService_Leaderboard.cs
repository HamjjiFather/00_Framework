using System;

namespace KKSFramework.PlatformService
{
    public interface IGameServiceLeaderboard
    {
        void ReportScore(long score, Action<bool> callback = null, string leaderBoardId = null);
        
        void ShowLeaderBoardUi();
        
        void ShowLeaderBoardUi(string leaderBoardId);
    }
}