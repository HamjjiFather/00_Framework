using System.Collections.Generic;
using KKSFramework.Management;
using KKSFramework.Navigation;
using UniRx.Async;
using UnityEngine.SceneManagement;

namespace KKSFramework.SceneLoad
{
    public enum SceneType
    {
        Entry,
        Title,
        Game,
        Bootstrap
    }

    public class SceneLoadManager : ManagerBase<SceneLoadManager>
    {

        #region Fields & Property

        private Dictionary<SceneType, string> _sceneNameDict;

        #endregion

        
        #region Methods
        
        
        public override void InitManager()
        {
            base.InitManager();
            _sceneNameDict = new Dictionary<SceneType, string>
            {
                [SceneType.Entry] = "00Entry",
                [SceneType.Title] = "01Title",
                [SceneType.Game] = "02Game",
                [SceneType.Bootstrap] = "Bootstrap"
            };
        }


        /// <summary>
        /// 씬 로드 비동기 처리.
        /// </summary>
        public async UniTask LoadSceneAsync(SceneType sceneType, bool loadedCheck = false)
        {
            if (loadedCheck)
            {
                var loadedScene = SceneManager.GetSceneByName (_sceneNameDict[sceneType]);
                if (loadedScene.IsValid ())
                {
                    await UniTask.CompletedTask;
                    return;
                }
            }
           
            await SceneManager.LoadSceneAsync(_sceneNameDict[sceneType], LoadSceneMode.Additive);
        }


        /// <summary>
        /// 씬 전환 비동기 처리.
        /// </summary>
        public async UniTask ChangeSceneAsync(SceneType sceneType)
        {
            await NavigationManager.Instance.ShowTransitionViewAsync ();
            NavigationManager.Instance.ChangeTransitionLockState (true);
            await SceneManager.LoadSceneAsync(_sceneNameDict[sceneType], LoadSceneMode.Single);
        }

        #endregion
    }
}