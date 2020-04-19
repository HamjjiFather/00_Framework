using System.Collections.Generic;
using System.IO;
using System.Linq;
using KKSFramework.ResourcesLoad;
using KKSFramework.Sound;
using UnityEditor;
using UnityEngine;

namespace KKSFramework.Editor
{
    /// <summary>
    /// 사운드 클립 로드 에디터 클래스.
    /// </summary>
    public class AddSoundClipEditorWindow : EditorWindow
    {
        #region Static Fields & Property

        /// <summary>
        /// 에디터 윈도우.
        /// </summary>
        private static EditorWindow _nowOpenedWindow;

        /// <summary>
        /// 현재 로드된 XML파일.
        /// </summary>
        private static AudioClipData _openedClipData;

        /// <summary>
        /// 현재 로드된 클립.
        /// </summary>
        private static List<AudioClip> _loadedClips = new List<AudioClip> ();

        /// <summary>
        /// 데이터 로드 여부.
        /// </summary>
        private static bool _isLoaded;

        /// <summary>
        /// Json파일.
        /// </summary>
        private static TextAsset _textAsset;

        private static TextAsset DataFileTextAsset ()
        {
            _textAsset = ResourcesLoadHelper.GetResources<TextAsset> (ResourceRoleType._Data, ResourcesType.Json,
                Constants.Framework.SoundJsonFileName);
            return _textAsset;
        }

        #endregion


        #region UnityMethods

        private void OnGUI ()
        {
            CheckDataStatus ();
            if (!_isLoaded || !_nowOpenedWindow || _openedClipData == null) return;

            GUILayout.BeginVertical ();
            for (var i = 0; i < _openedClipData.filePath.Count; i++)
            {
                GUILayout.BeginHorizontal ();
                EditorGUILayout.LabelField ($"Element {i}", GUILayout.MaxWidth (75));

                _loadedClips[i] = EditorGUILayout.ObjectField (_loadedClips[i], typeof (AudioClip), false,
                    GUILayout.MinWidth (125), GUILayout.MaxWidth (200)) as AudioClip;

                if (_loadedClips[i])
                {
                    _openedClipData.filePath[i] = _loadedClips[i].name;
                    _openedClipData.volumes[i] =
                        EditorGUILayout.Slider (_openedClipData.volumes[i], 0, 1, GUILayout.MinWidth (125));
                }

                if (GUILayout.Button ("+", GUILayout.MaxWidth (25)))
                {
                    _openedClipData.Insert (i);
                    _loadedClips.Insert (i, null);
                }

                GUI.color = Color.red;
                if (GUILayout.Button ("x", GUILayout.MaxWidth (25)))
                {
                    _openedClipData.RemoveAt (i);
                    _loadedClips.RemoveAt (i);
                }

                GUI.color = Color.white;
                GUILayout.EndHorizontal ();
            }

            if (GUILayout.Button ("데이터 추가"))
            {
                _openedClipData.Add ();
                _loadedClips.Add (null);
            }

            if (IsAbleToSave ())
            {
                GUI.color = Color.green;
                if (GUILayout.Button ("데이터 저장"))
                {
                    // Json파일 수정.
                    using (var jsonFile =
                        new StreamWriter (Application.dataPath + Constants.Framework.SoundJsonFileAssetPath))
                    {
                        jsonFile.WriteLine (JsonUtility.ToJson (_openedClipData));
                        jsonFile.Close ();
                    }

                    // 스크립트 텍스트 수정.
                    using (var classFile =
                        new StreamWriter (Application.dataPath + Constants.Framework.SoundTypeFileAssetPath))
                    {
                        var enumData = _loadedClips.Aggregate (string.Empty,
                            (current, t) => current + $"\t{t.name},\n");

                        classFile.WriteLine (
                            string.Format (Constants.Framework.CsFileSummary, name) +
                            Constants.Framework.SoundTypeEnumClassDeclare + "\n{\n\t" + "None," + "\n" + enumData +
                            "}");
                        classFile.Close ();
                    }

                    AssetDatabase.Refresh ();
                    _nowOpenedWindow.Close ();
                }

                GUI.color = Color.white;
            }

            GUILayout.EndVertical ();
        }

        #endregion


        #region Static Methods

        /// <summary>
        /// SFX 사운드 클립 윈도우 호출.
        /// </summary>
        [MenuItem ("Sound/Sound Clip Setting", priority = 11)]
        public static void SoundClipWindow ()
        {
            OpenSoundClipWindow ();
        }

        /// <summary>
        /// 사운드 클립 윈도우 호출.
        /// </summary>
        private static void OpenSoundClipWindow ()
        {
            if (!_nowOpenedWindow)
            {
                _nowOpenedWindow = CreateInstance<AddSoundClipEditorWindow> ();
                _nowOpenedWindow.titleContent = new GUIContent ("Sound Clip");
                _nowOpenedWindow.minSize = new Vector2 (400, 400);
                _nowOpenedWindow.Show ();

                _isLoaded = true;
                _openedClipData = LoadSoundClip ();
                _loadedClips = LoadAudioClip (_openedClipData);
            }
            else
            {
                _nowOpenedWindow.Focus ();
                CheckDataStatus ();
            }
        }

        /// <summary>
        /// 데이터 현황 체크.
        /// </summary>
        private static void CheckDataStatus ()
        {
            if (_isLoaded) return;
            _isLoaded = true;

            if (_nowOpenedWindow == null)
                _nowOpenedWindow = GetWindow (typeof (AddSoundClipEditorWindow), true);

            if (_openedClipData == null)
                _openedClipData = LoadSoundClip ();

            if (_loadedClips == null)
                _loadedClips = LoadAudioClip (_openedClipData);
        }

        /// <summary>
        /// XML 데이터 파일의 경로 데이터를 받아옴.
        /// </summary>
        /// <returns></returns>
        private static AudioClipData LoadSoundClip ()
        {
            var textAssetString = DataFileTextAsset ().text;
            return JsonUtility.FromJson<AudioClipData> (textAssetString);
        }

        /// <summary>
        /// XML 데이터의 파일 경로를 토대로 받아온 TextAsset 데이터를 받아옴.
        /// </summary>
        private static List<AudioClip> LoadAudioClip (AudioClipData clipData)
        {
            return clipData.filePath
                .Select (path => ResourcesLoadHelper.GetResources<AudioClip> (ResourceRoleType._Sound, path)).ToList ();
        }

        /// <summary>
        /// 현재 파일 리스트들을 저장할 수 있는지 여부.
        /// </summary>
        /// <returns></returns>
        private static bool IsAbleToSave ()
        {
            var isAvail = true;

            using (var tempEnumerator = _loadedClips.GetEnumerator ())
            {
                while (tempEnumerator.MoveNext ())
                {
                    if (tempEnumerator.Current) continue;
                    isAvail = false;
                    break;
                }
            }

            return isAvail;
        }

        #endregion
    }
}