using System;
using System.Threading.Tasks;
using FusionWebGL.Auth;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace FusionWebGL.AI
{
   [Serializable]
   public class ChatRequest
   {
      public string message;
      public string sessionId;
      public string npcType;
      public string playerContext;
   }

   [Serializable]
   public class ChatResponse
   {
      public bool success;
      public string message;
      public string error;
   }

   public class ChatGPTService : MonoBehaviour
   {
      private const string API_BASE_URL = "http://localhost:3001";
      private const string CHAT_ENDPOINT = "/api/chat/send";

      [Header("Debug Settings")]
      [SerializeField] private bool showDebugLogs = true;

      private string sessionId;
      private NPCContext currentContext;

      private void Start()
      {
         sessionId = Guid.NewGuid().ToString();
         if (showDebugLogs) Debug.Log($"[ChatGPTService] Initialized with session ID: {sessionId}");
      }

      public void InitializeDialogue(NPCContext context)
      {
         currentContext = context;
         if (showDebugLogs) Debug.Log($"[ChatGPTService] Initialized dialogue with NPC: {context.Name} ({context.Type})");
      }

      public void SetNPCType(string type)
      {
         if (currentContext != null)
         {
            currentContext.Type = type;
            if (showDebugLogs) Debug.Log($"[ChatGPTService] Updated NPC type to: {type}");
         }
         else
         {
            Debug.LogWarning("[ChatGPTService] Cannot set NPC type: no active context");
         }
      }

      public new async Task<string> SendMessage(string message)
      {
         string authToken = null;

         // First try to get token from LoginManager
         var loginManager = LoginManager.Instance;
         if (loginManager != null)
         {
            authToken = loginManager.AuthToken;
            if (string.IsNullOrEmpty(authToken))
            {
               Debug.Log("[ChatGPTService] No token in LoginManager, trying PlayerPrefs...");
               // Try to get token from PlayerPrefs
               authToken = PlayerPrefs.GetString("AuthToken", "");

               // If found in PlayerPrefs, update LoginManager
               if (!string.IsNullOrEmpty(authToken))
               {
                  loginManager.AuthToken = authToken;
                  Debug.Log("[ChatGPTService] Restored auth token from PlayerPrefs to LoginManager");
               }
            }
         }

         if (string.IsNullOrEmpty(authToken))
         {
            Debug.LogError("[ChatGPTService] Cannot send message: No authentication token available");
            return "I apologize, but I'm not properly configured to communicate at the moment. Please ensure you are logged in.";
         }

         try
         {
            var requestData = new ChatRequest
            {
               message = message,
               sessionId = sessionId,
               npcType = currentContext?.Type ?? "merchant",
               playerContext = GetPlayerContext()
            };

            string json = JsonConvert.SerializeObject(requestData);
            Debug.Log($"[ChatGPTService] Sending request with session ID: {sessionId}");
            Debug.Log($"[ChatGPTService] Using auth token: {new string('*', authToken.Length)}");

            using (UnityWebRequest www = new UnityWebRequest($"{API_BASE_URL}{CHAT_ENDPOINT}", "POST"))
            {
               byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
               www.uploadHandler = new UploadHandlerRaw(jsonToSend);
               www.downloadHandler = new DownloadHandlerBuffer();
               www.SetRequestHeader("Content-Type", "application/json");
               www.SetRequestHeader("Authorization", $"Bearer {authToken}");

               await www.SendWebRequest();

               if (www.result != UnityWebRequest.Result.Success)
               {
                  Debug.LogError($"[ChatGPTService] Error: {www.error}");
                  Debug.LogError($"[ChatGPTService] Response Code: {www.responseCode}");
                  Debug.LogError($"[ChatGPTService] Response: {www.downloadHandler.text}");
                  return "I apologize, but I'm having trouble communicating at the moment.";
               }

               var response = JsonConvert.DeserializeObject<ChatResponse>(www.downloadHandler.text);
               Debug.Log($"[ChatGPTService] Received response: {www.downloadHandler.text}");
               return response.message;
            }
         }
         catch (Exception e)
         {
            Debug.LogError($"[ChatGPTService] Exception: {e.Message}");
            Debug.LogError($"[ChatGPTService] Stack trace: {e.StackTrace}");
            return "I apologize, but I'm having trouble communicating at the moment.";
         }
      }

      private string GetPlayerContext()
      {
         var playerData = LoginManager.Instance?.CurrentPlayer;
         if (playerData == null) return "A new adventurer";

         return $"Background: A new adventurer\nPlayer Info:\n- Username: {playerData.username}\n- Created: {playerData.created_at}\n- Last Login: {playerData.last_login}";
      }
   }
}