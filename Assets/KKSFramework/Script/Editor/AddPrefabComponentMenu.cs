using KKSFramework.Object;
using UnityEditor;
using UnityEngine;

namespace KKSFramework.Editor
{
    /// <summary>
    /// Add PrefabComponent class on Transform Component.
    /// </summary>
    public static class AddPrefabComponentMenu
    {
        #region Static Methods

        [MenuItem("CONTEXT/Transform/Add Prefab Component", validate = false)]
        public static void AddPrefabComponent(MenuCommand command)
        {
            ((Transform) command.context).gameObject.AddComponent<PrefabComponent>();
        }

        [MenuItem("CONTEXT/Transform/Add Prefab Component", validate = true)]
        public static bool IsAlreadyHasPrefabComponent(MenuCommand command)
        {
            try
            {
                return !(PrefabComponent) command.context;
            }
            catch
            {
                return true;
            }
        }

        #endregion
    }
}