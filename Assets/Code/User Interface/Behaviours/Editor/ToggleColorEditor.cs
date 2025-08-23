using UnityEditor;
using UnityEditor.UI;

namespace UnityEngine.UI
{
    [CustomEditor(typeof(ToggleColor)), CanEditMultipleObjects]
    public class ToggleColorEditor : ToggleEditor
    {
        private SerializedProperty _highlightColor;

        protected override void OnEnable()
        {
            base.OnEnable();
            _highlightColor = serializedObject.FindProperty("_highlightColor");
        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.LabelField("HighLight Color Toggle", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(_highlightColor);

            serializedObject.ApplyModifiedProperties();

            EditorGUILayout.Space();
            base.OnInspectorGUI();
        }
    }
}