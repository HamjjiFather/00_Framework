using System;
using System.IO;
using KKSFramework.Management;
using UnityEngine;

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
    
    
    public static class LocalDataExtension
    {
        /// <summary>
        /// Bundle 클래스 Json파일로 저장.
        /// </summary>
        public static Bundle FromJsonData(this Bundle bundle)
        {
            var filePath = $"{Application.persistentDataPath}/{bundle.GetType().Name}.json";
            if (!File.Exists (filePath)) return bundle;
            var dataString = File.ReadAllText(filePath);
            JsonUtility.FromJsonOverwrite(dataString, bundle);

            return bundle;
        }

        /// <summary>
        /// Json파일 Bundle클래스로 저장.
        /// </summary>
        public static void ToJsonData<T>(this Bundle bundle) where T : Bundle
        {
            var filePath = $"{Application.persistentDataPath}/{typeof(T).Name}.json";
            File.WriteAllText(filePath, JsonUtility.ToJson(bundle));
        }
    }
}