using System.Collections.Generic;
using KKSFramework.Event;
using KKSFramework.GameSystem.GlobalText;
using KKSFramework.LocalData;
using KKSFramework.Management;
using KKSFramework.Navigation;
using KKSFramework.Object;
using KKSFramework.ResourcesLoad;
using KKSFramework.SceneLoad;
using KKSFramework.Sound;
using UnityEngine;

namespace KKSFramework.Installer
{
    /// <summary>
    /// 모든 기능 관리 클래스의 인스톨 클래스.
    /// </summary>
    public class ManagerInstaller : MonoBehaviour
    {
        private readonly List<IManagerBase> _managerBases = new List<IManagerBase>();

        #region UnityMethods

        private void Awake()
        {
            LocalDataLoad();
            InstallManagerClass();
        }

        #endregion

        #region Methods

        /// <summary>
        /// 로컬 데이터 로드.
        /// </summary>
        private void LocalDataLoad()
        {
            LocalDataManager.Instance.AddComponentBase(FindObjectOfType<LocalDataComponent>());
            LocalDataHelper.LoadAllGameData();
        }

        /// <summary>
        /// 매니저 클래스 초기화 작업.
        /// </summary>
        private void InstallManagerClass()
        {
            RegistManagerClass();
            InitializeManagerClass();
            CreateComponentBase();
        }

        /// <summary>
        /// 매니저 클래스 등록.
        /// </summary>
        private void RegistManagerClass()
        {
            _managerBases.Add(NavigationManager.Instance);
            _managerBases.Add(ObjectPoolingManager.Instance);
            _managerBases.Add(ResourcesLoadManager.Instance);
            _managerBases.Add(SceneLoadManager.Instance);
            _managerBases.Add(SoundPlayManager.Instance);
            _managerBases.Add(GlobalTextManager.Instance);
            _managerBases.Add(EscapeEventManager.Instance);
        }

        /// <summary>
        /// 매니저 클래스 초기화.
        /// </summary>
        private void InitializeManagerClass()
        {
            _managerBases.ForEach(x => x.InitManager());
        }

        /// <summary>
        /// 매니저 클래스에 컴포넌트 등록.
        /// </summary>
        private void CreateComponentBase()
        {
            NavigationManager.Instance.AddComponentBase(FindObjectOfType<NavigationComponent>());
            ObjectPoolingManager.Instance.AddComponentBase(FindObjectOfType<ObjectPoolingComponent>());
            SoundPlayManager.Instance.AddComponentBase(FindObjectOfType<SoundPlayComponent>());
        }

        #endregion
    }
}