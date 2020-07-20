using System;
using System.Collections.Generic;
using System.Linq;
using KKSFramework.Management;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KKSFramework.GlobalText
{
    public enum TargetGlobalTextCompType
    {
        /// <summary>
        /// 글로벌 텍스트를 표시하려는 컴포넌트가 UGUI Text임.
        /// </summary>
        UIText,

        /// <summary>
        /// 글로벌 텍스트를 표시하려는 컴포넌트가 TextMeshPro임.
        /// </summary>
        TMP,
    }

    [Serializable]
    public class TranslatedInfo
    {
        public TargetGlobalTextCompType targetGlobalTextCompType;

        private Graphic _targetText;

        public string Key;

        public string[] Args = new string[0];
        public object[] ToObjectArgs => Array.ConvertAll (Args, x => x.ToString ()).ToArray ();

        public string text
        {
            set
            {
                switch (targetGlobalTextCompType)
                {
                    case TargetGlobalTextCompType.TMP:
                        ((TextMeshPro) _targetText).text = value;
                        break;


                    case TargetGlobalTextCompType.UIText:
                        ((Text) _targetText).text = value;
                        break;

                    default:
                        break;
                }
            }
        }

        public void SetTextComp (Graphic textComp)
        {
            if (textComp is null)
            {
                Debug.Log ("textComp is null");
                return;
            }

            if (!(textComp is Text) && !(textComp is TextMeshPro))
            {
                Debug.Log ($"textComp is not TextTypeComponent: {textComp.GetType ().Name}");
                return;
            }

            _targetText = textComp;
        }
    }


    /// <summary>
    /// 글로벌 텍스트 관리 클래스.
    /// </summary>
    public class GlobalTextManager : ManagerBase<GlobalTextManager>
    {
        #region Fields & Property

        /// <summary>
        /// 글로벌 텍스트 언어 타입.
        /// </summary>
        private GlobalLanguageType _languageType;
        public GlobalLanguageType LanguageType => _languageType;

        /// <summary>
        /// 글로벌 텍스트 언어 넘버.
        /// </summary>
        public int SelectedLanguage { get; private set; }

        /// <summary>
        /// 글로벌 텍스트 번역을 사용하고 있는 텍스트 컴포넌트.
        /// </summary>
        private readonly Dictionary<MonoBehaviour, TranslatedInfo> _translatedInfos =
            new Dictionary<MonoBehaviour, TranslatedInfo> ();

        /// <summary>
        /// 글로벌 텍스트 컴포넌트 클래스 리스트.
        /// </summary>
        private readonly List<GlobalTextComponentBase> _globalTextComps = new List<GlobalTextComponentBase> ();

        #endregion


        #region UnityMethods

        #endregion


        #region Methods

        /// <summary>
        /// 언어가 변경됨.
        /// </summary>
        public void ChangeLanguage (int p_num)
        {
            SelectedLanguage = p_num;
            _languageType = (GlobalLanguageType) SelectedLanguage;
            ChangeGlobalText ();
        }


        /// <summary>
        /// 글로벌 텍스트 변경.
        /// </summary>
        private void ChangeGlobalText ()
        {
            _globalTextComps.ForEach (x => x.ChangeText ());
            _translatedInfos.Foreach (x =>
            {
                x.Value.text =
                    string.Format (
                        TableDataManager.Instance.GlobalTextDict[x.Value.Key].GlobalTexts[(int) _languageType],
                        x.Value.ToObjectArgs);
            });
        }


        /// <summary>
        /// 글로벌 텍스트 컴포넌트를 추가.
        /// </summary>
        public void RegistGlobalText (GlobalTextComponentBase pGlobalTextComponent)
        {
            if (_globalTextComps.Contains (pGlobalTextComponent) == false) _globalTextComps.Add (pGlobalTextComponent);
        }


        /// <summary>
        /// 글로벌 텍스트 등록. 
        /// </summary>
        public void RegistTranslate (TargetGlobalTextCompType targetCompType, string key, Graphic textComp,
            params object[] args)
        {
            if (textComp is null)
            {
                Debug.Log ("textComp is null");
                return;
            }

            if (!(textComp is Text) && !(textComp is TextMeshPro))
            {
                Debug.Log ($"textComp is not TextTypeComponent: {textComp.GetType ().Name}");
                return;
            }

            if (!TableDataManager.Instance.GlobalTextDict.ContainsKey (key)) return;
            if (!_translatedInfos.ContainsKey (textComp))
            {
                _translatedInfos.Add (textComp, new TranslatedInfo ());
            }

            var translatedInfo = _translatedInfos[textComp];
            translatedInfo.targetGlobalTextCompType = targetCompType;
            translatedInfo.Key = key;
            translatedInfo.Args = Array.ConvertAll (args, x => x.ToString ());
            translatedInfo.SetTextComp (textComp);
            translatedInfo.text = string.Format (
                TableDataManager.Instance.GlobalTextDict[translatedInfo.Key].GlobalTexts[(int) _languageType],
                translatedInfo.ToObjectArgs);
        }

        public void UnregistTranslatedTextComp (Graphic textComp)
        {
            if (!_translatedInfos.ContainsKey (textComp))
                return;

            _translatedInfos.Remove (textComp);
        }


        public string GetTranslatedString (string key, GlobalLanguageType globalLanguageType)
        {
            return TableDataManager.Instance.GlobalTextDict.ContainsKey (key)
                ? TableDataManager.Instance.GlobalTextDict[key].GlobalTexts[(int) globalLanguageType]
                : string.Empty;
        }

        #endregion


        #region EventMethods

        #endregion
    }
}