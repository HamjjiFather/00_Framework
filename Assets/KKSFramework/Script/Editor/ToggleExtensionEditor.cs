using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using KKSFramework.UI;
using UnityEditor;
using UnityEditor.UI;

namespace KKSFramework.Editor
{
    /// <summary>
    /// ToggleBase 클래스 커스텀 에디터.
    /// </summary>
    [CustomEditor(typeof(ToggleExtension), true)]
    [CanEditMultipleObjects]
    public class ToggleExtensionEditor : ToggleEditor
    {
        // 모든 베이스 클래스, 베이스 클래스를 상속한 클래스에서 사용.
        //[Header("[Editor_UIToggleBase]"), Space(5)]

        #region Fields & Property

        /// <summary>
        /// 필드 인포.
        /// </summary>
        private List<FieldInfo> list_property_infos;

        /// <summary>
        /// 상속 여부.
        /// </summary>
        private bool is_inheritance;

        #endregion

        #region UnityMethods

        protected override void OnEnable()
        {
            base.OnEnable();
            is_inheritance = !target.GetType().Name.Equals(typeof(ToggleExtension).Name);
            if (is_inheritance)
                list_property_infos = target.GetType().GetFields().ToList().FindAll(x =>
                    x.DeclaringType != null && !x.DeclaringType.Name.Equals(typeof(ToggleExtension).Name) &&
                    x.DeclaringType.Name.Equals(target.GetType().Name));
        }

        public override void OnInspectorGUI()
        {
            if (is_inheritance)
                if (list_property_infos.Count != 0)
                    foreach (var value in list_property_infos)
                    {
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(value.Name));
                        serializedObject.ApplyModifiedProperties();
                    }

            // 색상 변경 옵션.
            EditorGUILayout.PropertyField(serializedObject.FindProperty("clickSoundClip"));
            serializedObject.ApplyModifiedProperties();

            // 색상 변경 옵션.
            EditorGUILayout.PropertyField(serializedObject.FindProperty("toggleText"));
            serializedObject.ApplyModifiedProperties();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("[Toggle]", EditorStyles.boldLabel);

            base.OnInspectorGUI();

            // 클릭 이벤트 등록.
            EditorGUILayout.PropertyField(serializedObject.FindProperty("onClick"));
            serializedObject.ApplyModifiedProperties();
        }

        #endregion

        #region Methods

        #endregion

        #region EventMethods

        #endregion
    }
}