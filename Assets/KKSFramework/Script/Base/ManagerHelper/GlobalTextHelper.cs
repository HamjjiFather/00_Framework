using UnityEngine.UI;

namespace KKSFramework.GameSystem.GlobalText
{
    public static class GlobalTextHelper
    {
        /// <summary>
        /// Change Language.
        /// </summary>
        public static void ChangeLanguage (LanguageType languageType)
        {
            GlobalTextManager.Instance.ChangeLanguage ((int)languageType);
        }
        

        /// <summary>
        /// Translate Text Component. 
        /// </summary>
        public static void GetTranslatedString (this Text textComp, string key, params object[] args)
        {
            GlobalTextManager.Instance.RegistTranslate (TargetGlobalTextCompType.UIText, key, textComp, args);
        }
        
        // If use TMP, Should disabled this method summary.
        // public static void GetTranslatedString (this TextMeshPro textComp, string key, params object[] args)
        // {
        //     GlobalTextManager.Instance.RegistTranslate (TargetGlobalTextCompType.TMP, key, textComp, args);
        // }
    }
}