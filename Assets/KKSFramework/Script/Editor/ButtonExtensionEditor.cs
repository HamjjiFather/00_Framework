using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using KKSFramework.UI;
using UnityEditor;
using UnityEditor.UI;

namespace KKSFramework.Editor
{
    /// <summary>
    /// ButtonExtension 클래스 커스텀 에디터.
    /// </summary>
    [CustomEditor(typeof(ButtonExtension), true)]
    [CanEditMultipleObjects]
    public class ButtonExtensionEditor : ButtonEditor
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
            is_inheritance = !target.GetType().Name.Equals(typeof(ButtonExtension).Name);
            if (is_inheritance)
                list_property_infos = target.GetType().GetFields().ToList().FindAll(x =>
                    !x.DeclaringType.Name.Equals(typeof(ButtonExtension).Name) &&
                    x.DeclaringType.Name.Equals(target.GetType().Name));
        }

        public override void OnInspectorGUI()
        {
            if (is_inheritance)
                if (list_property_infos.Count != 0)
                    for (var i = 0; i < list_property_infos.Count; i++)
                    {
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(list_property_infos[i].Name));
                        serializedObject.ApplyModifiedProperties();
                    }

            // 색상 변경 옵션.
            EditorGUILayout.PropertyField(serializedObject.FindProperty("soundTypeEnum"));
            serializedObject.ApplyModifiedProperties();

            // 색상 변경 옵션.
            EditorGUILayout.PropertyField(serializedObject.FindProperty("buttonText"));
            serializedObject.ApplyModifiedProperties();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("[Button]", EditorStyles.boldLabel);

            base.OnInspectorGUI();
        }

        #endregion

        #region Methods

        #endregion

        #region EventMethods

        #endregion
    }
}