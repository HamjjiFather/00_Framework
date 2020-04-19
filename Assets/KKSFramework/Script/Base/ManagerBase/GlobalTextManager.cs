using System;
using System.Collections.Generic;
using System.Linq;
using KKSFramework.Management;
using KKSFramework.ResourcesLoad;
using TMPro;
using UniRx.Async;
using UnityEngine;
using UnityEngine.UI;

namespace KKSFramework.GameSystem.GlobalText
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

    public enum LanguageType
    {
        Kor,
        Eng,
        Jpn,
        Chn,
        Twn,
        Ger,
        FRA
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
        private LanguageType _languageType;

        /// <summary>
        /// 글로벌 텍스트 언어 넘버.
        /// </summary>
        public int SelectedLanguage { get; private set; }

        /// <summary>
        /// 테이블에서 가져온 글로벌 텍스트.
        /// </summary>
        private readonly Dictionary<string, string[]> _globalTexts = new Dictionary<string, string[]> ();

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
        /// TSV파일을 분석해 글로벌 텍스트 정보를 로드함.
        /// </summary>
        public async UniTask LoadGlobalText ()
        {
            var text = await ResourcesLoadHelper.GetResourcesAsync<TextAsset> (ResourceRoleType._Data,
                ResourcesType.GlobalTextTSV, "GlobalText");
            var readText = text.text.Split ('\n').GetEnumerator ();

            readText.MoveNext ();
            while (readText.MoveNext ())
            {
                var datas = readText.Current?.ToString ();
                if (string.IsNullOrEmpty (datas)) continue;
                var tempListValues = datas.Trim ('\r').Split ('\t').ToList ();
                _globalTexts.Add (tempListValues.First (), tempListValues.Skip (1).ToArray ());
            }
        }


        /// <summary>
        /// 언어가 변경됨.
        /// </summary>
        public void ChangeLanguage (int p_num)
        {
            SelectedLanguage = p_num;
            _languageType = (LanguageType) SelectedLanguage;
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
                x.Value.text = string.Format (_globalTexts[x.Value.Key][(int) _languageType], x.Value.ToObjectArgs);
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

            if (!_globalTexts.ContainsKey (key)) return;
            if (!_translatedInfos.ContainsKey (textComp))
            {
                _translatedInfos.Add (textComp, new TranslatedInfo ());
            }

            var translatedInfo = _translatedInfos[textComp];
            translatedInfo.targetGlobalTextCompType = targetCompType;
            translatedInfo.Key = key;
            translatedInfo.Args = Array.ConvertAll (args, x => x.ToString ());
            translatedInfo.SetTextComp (textComp);
            translatedInfo.text = string.Format (_globalTexts[translatedInfo.Key][(int) _languageType],
                translatedInfo.ToObjectArgs);
        }

        public void UnregistTranslatedTextComp (Graphic textComp)
        {
            if (!_translatedInfos.ContainsKey (textComp))
                return;

            _translatedInfos.Remove (textComp);
        }

        #endregion


        #region EventMethods

        #endregion
    }
}