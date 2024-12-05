using UnityEditor;
using UnityEngine;

namespace FpsTest
{
    [System.Serializable]
    [CustomEditor(typeof(CharacterMovement))]
    public class CharacterMovementEditor : Editor
    {
        private string[] tabs =
        {
            "References", "Movement", "Camera", "Jumping"
        };

        private int currentTab = 0;

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            var myScript = target as CharacterMovement;

            EditorGUILayout.BeginVertical();
            currentTab = GUILayout.SelectionGrid(currentTab, tabs, 6);
            EditorGUILayout.Space(10f);
            EditorGUILayout.EndVertical();

            #region variables

            if (currentTab >= 0 || currentTab < tabs.Length)
            {
                switch (tabs[currentTab])
                {
                    case "References":
                        EditorGUILayout.LabelField("REFERENCES", EditorStyles.boldLabel);
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(CharacterMovement.characterCamera)));
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(CharacterMovement.orientation)));
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(CharacterMovement.groundCheck)));

                        break;
                    case "Movement":
                        EditorGUILayout.LabelField("MOVEMENT", EditorStyles.boldLabel);
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(CharacterMovement.speed)));
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(CharacterMovement.groundMask)));
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(CharacterMovement.groundDistance)));
                        
                        break;
                    case "Camera":
                        EditorGUILayout.LabelField("CAMERA", EditorStyles.boldLabel);
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(CharacterMovement.mouseSensitivity)));
                        break;
                    case "Jumping":
                        EditorGUILayout.LabelField("JUMPING", EditorStyles.boldLabel);
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(CharacterMovement.gravity)));
                        EditorGUILayout.PropertyField(serializedObject.FindProperty(nameof(CharacterMovement.jumpHeight)));
                        
                        break;
                }
            }

            #endregion

            EditorGUILayout.Space(10f);
            serializedObject.ApplyModifiedProperties();
        }
    }
}