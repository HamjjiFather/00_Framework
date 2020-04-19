using UnityEditor;
using UnityEngine;

namespace KKSFramework.Editor
{
    /// <summary>
    /// 로컬 데이터 삭제.
    /// </summary>
    public static class DeleteDataMenu
    {
        [MenuItem("Data/Delete Local Data")]
        public static void DeleteLocalData()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}