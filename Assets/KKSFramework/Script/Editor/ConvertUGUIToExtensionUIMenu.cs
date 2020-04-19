using UnityEditor;
using UnityEngine.UI;
using KKSFramework.UI;

namespace KKSFramework.Editor
{
    public static class ConvertUGUIToExtensionUIMenu
    {
        #region Static Methods

        [MenuItem ("CONTEXT/Button/Button Component Change", validate = false)]
        public static void ChangeButtonComponent (MenuCommand command)
        {
            var btnComp = (Button) command.context;
            var btnObj = btnComp.gameObject;
            Undo.DestroyObjectImmediate ((Button) command.context);
            var buttonExtension = Undo.AddComponent<ButtonExtension> (btnObj);
            buttonExtension.ReplaceComponent (btnComp);
        }

        [MenuItem ("CONTEXT/Button/Button Component Change", validate = true)]
        public static bool IsBaseButton (MenuCommand command)
        {
            try
            {
                return !(ButtonExtension) command.context;
            }
            catch
            {
                return true;
            }
        }

        [MenuItem ("CONTEXT/Toggle/Toggle Component Change", validate = false)]
        public static void ChangeToggleComponent (MenuCommand p_command)
        {
            var toggleComp = (Toggle) p_command.context;
            var toggleObj = toggleComp.gameObject;
            Undo.DestroyObjectImmediate ((Toggle) p_command.context);
            var toggleExtension = Undo.AddComponent<ToggleExtension> (toggleObj);
            toggleExtension.ReplaceComponent (toggleComp);
        }

        [MenuItem ("CONTEXT/Toggle/Toggle Component Change", validate = true)]
        public static bool IsBaseToggle (MenuCommand command)
        {
            try
            {
                return !(ToggleExtension) command.context;
            }
            catch
            {
                return true;
            }
        }

        #endregion
    }
}