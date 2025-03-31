using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace FusionWebGL.Auth
{
   [Serializable]
   public class PlayerData
   {
      public string username;
      public string created_at;
      public string last_login;
      public Dictionary<string, object> metadata;
   }

   [Serializable]
   public class LoginResponse
   {
      public bool success;
      public string token;
      public PlayerData player;
      public string error;
   }

   public class LoginManager : MonoBehaviour
   {
      private const string API_BASE_URL = "http://localhost:3001";
      private const string LOGIN_ENDPOINT = "/api/auth/login";
      private const string REGISTER_ENDPOINT = "/api/auth/register";
      private const string VALIDATE_ENDPOINT = "/api/auth/validate";

      private static LoginManager instance;
      public static LoginManager Instance
      {
         get
         {
            if (instance == null)
            {
               var go = new GameObject("LoginManager");
               instance = go.AddComponent<LoginManager>();
               DontDestroyOnLoad(go);
            }
            return instance;
         }
      }

      public string AuthToken { get; set; }
      public PlayerData CurrentPlayer { get; private set; }

      private void Awake()
      {
         if (instance == null)
         {
            instance = this;
         }
         else if (instance != this)
         {
            Destroy(gameObject);
         }

         // Try to restore token from PlayerPrefs
         AuthToken = PlayerPrefs.GetString("AuthToken", "");
         if (!string.IsNullOrEmpty(AuthToken))
         {
            ValidateToken();
         }
      }

      public async Task<bool> Login(string username, string password)
      {
         try
         {
            var loginData = new Dictionary<string, string>
                {
                    { "username", username },
                    { "password", password }
                };

            string json = JsonConvert.SerializeObject(loginData);
            using (UnityWebRequest www = new UnityWebRequest($"{API_BASE_URL}{LOGIN_ENDPOINT}", "POST"))
            {
               byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
               www.uploadHandler = new UploadHandlerRaw(jsonToSend);
               www.downloadHandler = new DownloadHandlerBuffer();
               www.SetRequestHeader("Content-Type", "application/json");

               await www.SendWebRequest();

               if (www.result != UnityWebRequest.Result.Success)
               {
                  Debug.LogError($"Login failed: {www.error}");
                  return false;
               }

               var response = JsonConvert.DeserializeObject<LoginResponse>(www.downloadHandler.text);
               if (response.success)
               {
                  AuthToken = response.token;
                  CurrentPlayer = response.player;
                  PlayerPrefs.SetString("AuthToken", AuthToken);
                  return true;
               }
               else
               {
                  Debug.LogError($"Login failed: {response.error}");
                  return false;
               }
            }
         }
         catch (Exception e)
         {
            Debug.LogError($"Login error: {e.Message}");
            return false;
         }
      }

      public async Task<bool> Register(string username, string password, string email)
      {
         try
         {
            var registerData = new Dictionary<string, string>
                {
                    { "username", username },
                    { "password", password },
                    { "email", email }
                };

            string json = JsonConvert.SerializeObject(registerData);
            using (UnityWebRequest www = new UnityWebRequest($"{API_BASE_URL}{REGISTER_ENDPOINT}", "POST"))
            {
               byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
               www.uploadHandler = new UploadHandlerRaw(jsonToSend);
               www.downloadHandler = new DownloadHandlerBuffer();
               www.SetRequestHeader("Content-Type", "application/json");

               await www.SendWebRequest();

               if (www.result != UnityWebRequest.Result.Success)
               {
                  Debug.LogError($"Registration failed: {www.error}");
                  return false;
               }

               var response = JsonConvert.DeserializeObject<LoginResponse>(www.downloadHandler.text);
               if (response.success)
               {
                  AuthToken = response.token;
                  CurrentPlayer = response.player;
                  PlayerPrefs.SetString("AuthToken", AuthToken);
                  return true;
               }
               else
               {
                  Debug.LogError($"Registration failed: {response.error}");
                  return false;
               }
            }
         }
         catch (Exception e)
         {
            Debug.LogError($"Registration error: {e.Message}");
            return false;
         }
      }

      public async Task<bool> ValidateToken()
      {
         if (string.IsNullOrEmpty(AuthToken))
         {
            return false;
         }

         try
         {
            using (UnityWebRequest www = new UnityWebRequest($"{API_BASE_URL}{VALIDATE_ENDPOINT}", "GET"))
            {
               www.downloadHandler = new DownloadHandlerBuffer();
               www.SetRequestHeader("Authorization", $"Bearer {AuthToken}");

               await www.SendWebRequest();

               if (www.result != UnityWebRequest.Result.Success)
               {
                  Debug.LogError($"Token validation failed: {www.error}");
                  return false;
               }

               var response = JsonConvert.DeserializeObject<LoginResponse>(www.downloadHandler.text);
               if (response.success)
               {
                  CurrentPlayer = response.player;
                  return true;
               }
               else
               {
                  Debug.LogError($"Token validation failed: {response.error}");
                  return false;
               }
            }
         }
         catch (Exception e)
         {
            Debug.LogError($"Token validation error: {e.Message}");
            return false;
         }
      }

      public void Logout()
      {
         AuthToken = null;
         CurrentPlayer = null;
         PlayerPrefs.DeleteKey("AuthToken");
      }
   }
}