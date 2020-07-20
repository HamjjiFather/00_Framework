using KKSFramework.LocalData;
using UnityEditor;
using UnityEngine;

namespace KKSFramework.Editor
{
    /// <summary>
    /// 로컬 데이터 삭제.
    /// </summary>
    public static class DataMenu
    {
        [MenuItem("Data/Open Local Path")]
        public static void OpenLocalPath()
        {
            System.Diagnostics.Process.Start(Application.persistentDataPath);
        }
        
        [MenuItem("Data/Delete Local Data")]
        public static void DeleteLocalData()
        {
            PlayerPrefs.DeleteAll();
            LocalDataHelper.DeleteData ();
        }
    }
}