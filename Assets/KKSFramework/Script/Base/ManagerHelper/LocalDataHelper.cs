using System;

namespace KKSFramework.LocalData
{
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

        /// <summary>
        /// 게임 데이터 클래스.
        /// </summary>
        [Serializable]
        public class LocalData
        {
            
        }
    }
}