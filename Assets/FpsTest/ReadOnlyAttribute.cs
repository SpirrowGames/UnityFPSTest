using UnityEditor;
using UnityEngine;

namespace FpsTest
{
    // 読み取り専用のプロパティをインスペクターに表示する
    // https://discussions.unity.com/t/how-to-make-a-readonly-property-in-inspector/75448
    public class ReadOnlyAttribute : PropertyAttribute
    {
    }
    
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUI.PropertyField(position, property, label);
            EditorGUI.EndDisabledGroup();
        }
    }
#endif
}