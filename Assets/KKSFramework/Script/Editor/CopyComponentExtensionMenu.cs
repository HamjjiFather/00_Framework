using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace KKSFramework.Editor
{
    /// <summary>
    /// Extension Copy Component Editor Class.
    /// </summary>
    public static class CopyComponentExtensionMenu
    {
        #region Fields & Property

        private static Type _pastedType;

        #endregion


        #region Static Methods

        [MenuItem ("CONTEXT/Component/Copy Component Extension", validate = false)]
        public static void AddPrefabComponent (MenuCommand command)
        {
            _pastedType = command.context.GetType ();
        }

        [MenuItem ("CONTEXT/Component/Paste Component Extension", validate = false)]
        public static void PastedPrefabComponent (MenuCommand command)
        {
            var properties = _pastedType.GetProperties ().ToList ();

            foreach (var type in command.context.GetType ().Assembly.GetTypes ()
                .Where (myType => myType == _pastedType))
            {
                Debug.Log (type.Name + "//" + properties.Count);
                foreach (var t1 in properties)
                {
                    var targetPropertyInfo = type.GetProperty (t1.Name);
                    try
                    {
                        var propertyValue = t1.GetValue (type, null);
                        targetPropertyInfo?.SetValue (targetPropertyInfo.GetType (), propertyValue, null);
                    }
                    catch
                    {
                        // ignored
                    }
                }
            }
        }

        [MenuItem ("CONTEXT/Component/Paste Component Extension", validate = true)]
        public static bool IsAblePasteComponent (MenuCommand command)
        {
            if (_pastedType == null) return false;
            var isSameType = false;
            foreach (var t in command.context.GetType ().Assembly.GetTypes ().Where (myType => myType == _pastedType))
            {
                isSameType = _pastedType.ToString ().Equals (t.ToString ());
                Debug.Log (isSameType);
            }

            return isSameType;

        }

        #endregion
    }
}