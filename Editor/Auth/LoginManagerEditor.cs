#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace FusionWebGL.Auth
{
    [CustomEditor(typeof(LoginManager))]
    public class LoginManagerEditor : UnityEditor.Editor
    {
        private SerializedProperty showDebugLogs;

        private void OnEnable()
        {
            showDebugLogs = serializedObject.FindProperty("showDebugLogs");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("Login Manager Settings", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(showDebugLogs);

            serializedObject.ApplyModifiedProperties();

            EditorGUILayout.Space(10);
            EditorGUILayout.HelpBox(
                "The LoginManager is a singleton that handles user authentication.\n" +
                "It automatically creates itself when first accessed and persists across scenes.",
                MessageType.Info
            );

            EditorGUILayout.Space(5);
            if (GUILayout.Button("Test Connection"))
            {
                TestConnection();
            }
        }

        private void TestConnection()
        {
            var loginManager = (LoginManager)target;
            // Here you would implement a connection test
            // For now, we'll just show a message
            EditorUtility.DisplayDialog(
                "Connection Test",
                "Connection test functionality will be implemented in a future version.",
                "OK"
            );
        }
    }
}
#endif