#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace FusionWebGL.AI
{
    [CustomEditor(typeof(NPCDialogue))]
    public class NPCDialogueEditor : UnityEditor.Editor
    {
        private SerializedProperty npcType;
        private SerializedProperty npcFaction;
        private SerializedProperty npcName;
        private SerializedProperty npcPersonality;
        private SerializedProperty npcBackground;
        private SerializedProperty nameplateColor;
        private SerializedProperty isInteractive;
        private SerializedProperty interactionDistance;
        private SerializedProperty showDebugLogs;
        private SerializedProperty showGizmos;
        private SerializedProperty dialogueUI;
        private SerializedProperty chatService;
        private SerializedProperty interactionCenter;
        private SerializedProperty interactionPrompt;
        private SerializedProperty playerCharacter;

        private void OnEnable()
        {
            npcType = serializedObject.FindProperty("npcType");
            npcFaction = serializedObject.FindProperty("npcFaction");
            npcName = serializedObject.FindProperty("npcName");
            npcPersonality = serializedObject.FindProperty("npcPersonality");
            npcBackground = serializedObject.FindProperty("npcBackground");
            nameplateColor = serializedObject.FindProperty("nameplateColor");
            isInteractive = serializedObject.FindProperty("isInteractive");
            interactionDistance = serializedObject.FindProperty("interactionDistance");
            showDebugLogs = serializedObject.FindProperty("showDebugLogs");
            showGizmos = serializedObject.FindProperty("showGizmos");
            dialogueUI = serializedObject.FindProperty("dialogueUI");
            chatService = serializedObject.FindProperty("chatService");
            interactionCenter = serializedObject.FindProperty("interactionCenter");
            interactionPrompt = serializedObject.FindProperty("interactionPrompt");
            playerCharacter = serializedObject.FindProperty("playerCharacter");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("NPC Identity", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(npcType);
            EditorGUILayout.PropertyField(npcFaction);
            EditorGUILayout.PropertyField(npcName);
            EditorGUILayout.PropertyField(npcPersonality);
            EditorGUILayout.PropertyField(npcBackground);
            EditorGUILayout.PropertyField(nameplateColor);
            EditorGUILayout.PropertyField(isInteractive);

            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("Interaction Settings", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(interactionDistance);
            EditorGUILayout.PropertyField(showDebugLogs);
            EditorGUILayout.PropertyField(showGizmos);

            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("References", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(dialogueUI);
            EditorGUILayout.PropertyField(chatService);
            EditorGUILayout.PropertyField(interactionCenter);
            EditorGUILayout.PropertyField(interactionPrompt);
            EditorGUILayout.PropertyField(playerCharacter);

            serializedObject.ApplyModifiedProperties();

            EditorGUILayout.Space(10);
            if (GUILayout.Button("Find Required Components"))
            {
                FindRequiredComponents();
            }
        }

        private void FindRequiredComponents()
        {
            var npcDialogue = (NPCDialogue)target;

            // Find AIDialogueUI
            if (npcDialogue.GetComponent<AIDialogueUI>() == null)
            {
                var dialogueUI = FindObjectOfType<AIDialogueUI>();
                if (dialogueUI != null)
                {
                    serializedObject.FindProperty("dialogueUI").objectReferenceValue = dialogueUI;
                }
            }

            // Find ChatGPTService
            if (npcDialogue.GetComponent<ChatGPTService>() == null)
            {
                var chatService = FindObjectOfType<ChatGPTService>();
                if (chatService != null)
                {
                    serializedObject.FindProperty("chatService").objectReferenceValue = chatService;
                }
            }

            // Set interaction center if not set
            if (serializedObject.FindProperty("interactionCenter").objectReferenceValue == null)
            {
                serializedObject.FindProperty("interactionCenter").objectReferenceValue = npcDialogue.transform;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif