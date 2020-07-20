using System;

namespace KKSFramework.LocalData
{
    // [Serializable]
    // public class TemplateBundle : Bundle
    // {
    //     
    // }
    
    
    public static class LocalDataHelper
    {
        private static readonly LocalData LocalDataClass = new LocalData();

        #region Load

        /// <summary>
        /// 게임 데이터 로드.
        /// </summary>
        public static void LoadAllGameData()
        {
        }

        #endregion


        #region Save

        /// <summary>
        /// 게임 데이터 저장.
        /// </summary>
        public static void SaveAllGameData()
        {
        }


        #endregion
        
        
        public static void DeleteData ()
        {
            LocalDataManager.Instance.DeleteData ();
        }
        

        [Serializable]
        public class LocalData
        {
            // public TemplateBundle TemplateBundle;
            // TODO - Add a class that inherits a Bundle type.
        }
    }
}