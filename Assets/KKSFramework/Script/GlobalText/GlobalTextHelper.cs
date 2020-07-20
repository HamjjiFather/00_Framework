using UnityEngine.UI;

namespace KKSFramework.GlobalText
{
    public static class GlobalTextHelper
    {
        /// <summary>
        /// Change Language.
        /// </summary>
        public static void ChangeLanguage (GlobalLanguageType languageType)
        {
            GlobalTextManager.Instance.ChangeLanguage ((int)languageType);
        }


        public static string GetTranslatedString (string key)
        {
            return GlobalTextManager.Instance.GetTranslatedString (key, GlobalTextManager.Instance.LanguageType);
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