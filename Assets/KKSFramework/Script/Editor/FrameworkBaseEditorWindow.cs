using System.IO;
using UnityEditor;
using UnityEngine;

namespace KKSFramework.Editor
{
    /// <summary>
    /// Summary
    /// </summary>
    public class FrameworkBaseEditorWindow : EditorWindow
    {
        #region UnityMethods

        #region Static Fields & Property

        /// <summary>
        /// 에디터 윈도우.
        /// </summary>
        private static EditorWindow _openedWindow;

        /// <summary>
        /// 작성중인 프레임워크 베이스 데이터.
        /// </summary>
        private static FrameworkBase.FrameworkBaseData _nowChangedData;

        /// <summary>
        /// 데이터 로드 여부.
        /// </summary>
        private static bool _isLoaded;

        #endregion
        

        private void OnGUI()
        {
            CheckDataStatus();

            if (!_isLoaded || !_openedWindow) return;
            
            GUILayout.BeginVertical();
            _nowChangedData.keysoPassword =
                EditorGUILayout.TextField("Keystore Password", _nowChangedData.keysoPassword);
            _nowChangedData.aliasPassword =
                EditorGUILayout.TextField("Alias Password", _nowChangedData.aliasPassword);

            GUI.color = Color.green;
            if (GUILayout.Button("데이터 저장"))
            {
                using (var temp_sw_json =
                    new StreamWriter(Application.dataPath + Constants.Framework.FrameworkDataJsonFileAssetPath))
                {
                    temp_sw_json.WriteLine(JsonUtility.ToJson(_nowChangedData));
                    temp_sw_json.Close();
                }

                FrameworkBase.Instance.UpdateFrameWorkData(_nowChangedData);
                InputKeystore();

                AssetDatabase.Refresh();
                _openedWindow.Close();
            }

            GUI.color = Color.white;
            GUILayout.EndVertical();
        }
        
        
        /// <summary>
        /// Input keystore password automatically.
        /// </summary>
        private static void InputKeystore()
        {
            if (EditorUserBuildSettings.activeBuildTarget != BuildTarget.Android) return;
            if (PlayerSettings.Android.keyaliasName == string.Empty) return;
            
            PlayerSettings.Android.keystorePass =
                FrameworkBase.Instance.GetFrameworkBaseData.keysoPassword;
            PlayerSettings.Android.keyaliasPass = FrameworkBase.Instance.GetFrameworkBaseData.aliasPassword;
        }

        #endregion

        
        #region Static Methods

        [MenuItem("Framework/Framework Data", priority = 12)]
        public static void AddDataKey()
        {
            if (!_openedWindow)
            {
                _openedWindow = CreateInstance<FrameworkBaseEditorWindow>();
                _openedWindow.titleContent = new GUIContent("Framework Data");
                _openedWindow.Show();

                _nowChangedData = FrameworkBase.Instance.GetFrameworkBaseData;
                Debug.Log(_nowChangedData.keysoPassword);
                _isLoaded = true;
            }
            else
            {
                _openedWindow.Focus();
                CheckDataStatus();
            }
        }

        /// <summary>
        /// 데이터 현황 체크.
        /// </summary>
        private static void CheckDataStatus()
        {
            if (_isLoaded) return;
            _isLoaded = true;

            if (_openedWindow == null) _openedWindow = GetWindow(typeof(FrameworkBaseEditorWindow), true);

            if (_nowChangedData == null) _nowChangedData = FrameworkBase.Instance.GetFrameworkBaseData;
        }
        
        #endregion
    }
}