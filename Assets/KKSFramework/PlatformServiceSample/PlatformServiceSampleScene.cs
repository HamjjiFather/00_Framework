using UnityEngine;
using UnityEngine.UI;

namespace KKSFramework.PlatformService
{
    public class PlatformServiceSampleScene : MonoBehaviour
    {
        public Text debugText;
        
        public Button loginButton;
        
        public Button logoutButton;

        public Button showAchievementButton;
        
        public Button unlockAchievementButton;
        
        public Button showLeaderBoardButton;
        
        public Button reportScoreButton;


        private void Awake ()
        {
            SetLoginState (false);
            PlatformServiceHelper.Initialize ();

            debugText.text = "Initialized";

            loginButton.onClick.AddListener (() =>
            {
                PlatformServiceHelper.Login ((b, s) =>
                {
                    SetLoginState (b);
                    debugText.text = $"Login State {b}, {s}";
                });
            });
            
            logoutButton.onClick.AddListener (() =>
            {
                PlatformServiceHelper.LogOut ();
                SetLoginState (false);
            });
            
            showAchievementButton.onClick.AddListener (PlatformServiceHelper.ShowAchievementsUi);
            showLeaderBoardButton.onClick.AddListener (PlatformServiceHelper.ShowLeaderBoardUi);
            unlockAchievementButton.onClick.AddListener (() =>
            {
                PlatformServiceHelper.UnlockAchievementRequest (GPGSIds.achievement_achievement1);
            });
            
            reportScoreButton.onClick.AddListener (() =>
            {
                PlatformServiceHelper.ReportScore (100, leaderBoardId: GPGSIds.leaderboard_leaderboard);
            });

            void SetLoginState (bool loginState)
            {
                showAchievementButton.gameObject.SetActive (loginState);
                showLeaderBoardButton.gameObject.SetActive (loginState);
                unlockAchievementButton.gameObject.SetActive (loginState);
                reportScoreButton.gameObject.SetActive (loginState);
            }
        }
    }
}