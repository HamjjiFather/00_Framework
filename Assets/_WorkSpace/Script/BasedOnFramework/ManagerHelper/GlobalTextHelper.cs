using UnityEngine.UI;

namespace KKSFramework.Localization
{
    public static class GlobalTextHelper
    {
        /// <summary>
        /// Change Language.
        /// </summary>
        public static void ChangeLanguage (GlobalLanguageType languageType)
        {
            LocalizationTextManager.Instance.ChangeLanguage ((int) languageType);
        }


        public static string GetTranslatedString (string key)
        {
            return string.Empty;
            // LocalizationTextManager.Instance.
            // return LocalizationTextManager.Instance.GetTranslatedString (key, LocalizationTextManager.Instance.LanguageType);
        }
        

        // If use TMP, Should disabled this method summary.
        // public static void GetTranslatedString (this TextMeshPro textComp, string key, params object[] args)
        // {
        //     LocalizationTextManager.Instance.RegistTranslate (TargetGlobalTextCompType.TMP, key, textComp, args);
        // }
    }
}