using System;

namespace KKSFramework.PlatformService
{
    public interface IGameServiceAchievement
    {
        #region Achievement

        void ShowAchievementsUiRequest();
        
        void UnlockAchievementRequest(string achievementId, Action<bool> callback = null);

        #endregion
    }
}