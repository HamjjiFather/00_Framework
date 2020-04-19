using System;
using KKSFramework.Management;

namespace KKSFramework.LocalData
{
    [Serializable]
    public class Bundle
    {
    }

    /// <summary>
    /// 보존이 필요한 게임 로컬 데이터 관리 클래스.
    /// </summary>
    public class LocalDataManager : ManagerBase<LocalDataManager>
    {
        #region Fields & Property

        private LocalDataComponent LocalDataComponent => ComponentBase as LocalDataComponent;

        #endregion

        public override void AddComponentBase(ComponentBase componentBase)
        {
            base.AddComponentBase(componentBase);
            LocalDataComponent.SetSaveAction(LocalDataHelper.SaveAllGameData);
        }

        #region UnityMethods

        #endregion

        #region Methods

        /// <summary>
        /// 게임 데이터 로드.
        /// </summary>
        public Bundle LoadGameData(Bundle bundle)
        {
            return bundle.FromJsonData();
        }

        /// <summary>
        /// 게임 데이터 저장.
        /// </summary>
        /// .
        public void SaveGameData(Bundle bundle)
        {
            bundle.ToJsonData<Bundle>();
        }

        #endregion
    }
}