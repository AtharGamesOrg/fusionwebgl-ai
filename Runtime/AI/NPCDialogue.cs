using System;
using System.Threading.Tasks;
using FusionWebGL.UI;
using GameCreator.Runtime.Characters;
using GameCreator.Runtime.Common;
using NinjutsuGames.FusionNetwork.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

namespace FusionWebGL.AI
{
   public class NPCDialogue : MonoBehaviour
   {
      [Header("NPC Identity")]
      [SerializeField] private string npcType = "merchant";
      [SerializeField] private string npcFaction = "Merchants";
      [SerializeField] private string npcName = "Merchant";
      [SerializeField] private string npcPersonality = "";
      [SerializeField] private string npcBackground = "";
      [SerializeField] private Color nameplateColor = Color.white;
      [SerializeField] private bool isInteractive = true;

      [Header("Interaction Settings")]
      [SerializeField] private float interactionDistance = 2.11f;
      [SerializeField] private bool showDebugLogs = true;
      [SerializeField] private bool showGizmos = true;

      [Header("References")]
      [SerializeField] private AIDialogueUI dialogueUI;
      [SerializeField] private ChatGPTService chatService;
      [SerializeField] private Transform interactionCenter;
      [SerializeField] private GameObject interactionPrompt;
      [SerializeField] private PropertyGetGameObject playerCharacter = GetGameObjectPlayer.Create();

      [Header("Detection Settings")]
      [SerializeField] private float playerCheckInterval = 1f;
      [SerializeField] private int maxPlayerCheckAttempts = 10;

      // Events
      public event Action OnDialogueEndEvent;

      private NPCActionSystem actionSystem;
      private bool isPlayerInRange;
      private bool isDialogueActive;
      private Character localPlayerCharacter;
      private Collider2D npcCollider;
      private float nextPlayerCheckTime;
      private int playerCheckAttempts;
      private bool isInitialized;

      public bool IsDialogueActive => isDialogueActive;

      public string NPCName
      {
         get => npcName;
         set => npcName = value;
      }

      private void Awake()
      {
         // Get required components
         npcCollider = GetComponent<Collider2D>();
         if (npcCollider == null)
         {
            if (showDebugLogs) Debug.Log($"[NPCDialogue] Adding Collider2D to {gameObject.name}");
            npcCollider = gameObject.AddComponent<CircleCollider2D>();
            ((CircleCollider2D)npcCollider).radius = interactionDistance;
            npcCollider.isTrigger = true;
         }

         if (dialogueUI == null)
         {
            dialogueUI = UnityEngine.Object.FindAnyObjectByType<AIDialogueUI>();
            if (dialogueUI == null)
            {
               Debug.LogError("[NPCDialogue] Could not find AIDialogueUI in scene!");
               enabled = false;
               return;
            }
         }

         if (chatService == null)
         {
            chatService = UnityEngine.Object.FindAnyObjectByType<ChatGPTService>();
            if (chatService == null)
            {
               Debug.LogError("[NPCDialogue] Could not find ChatGPTService in scene!");
               enabled = false;
               return;
            }
         }

         if (interactionCenter == null)
         {
            interactionCenter = transform;
         }

         actionSystem = NPCActionSystem.Instance;
         isInitialized = false;
         playerCheckAttempts = 0;
         nextPlayerCheckTime = Time.time + 1f;
      }

      private void Start()
      {
         if (interactionPrompt != null) interactionPrompt.SetActive(false);
         isPlayerInRange = false;
         isDialogueActive = false;
      }

      private void InitializePlayerReference()
      {
         if (isInitialized || playerCheckAttempts >= maxPlayerCheckAttempts) return;

         try
         {
            // First try to get the local player directly
            var networkPlayers = UnityEngine.Object.FindObjectsByType<NetworkPlayer>(FindObjectsSortMode.None);
            foreach (var networkPlayer in networkPlayers)
            {
               if (networkPlayer.HasInputAuthority)
               {
                  var character = networkPlayer.GetComponent<Character>();
                  if (character != null && character.IsPlayer)
                  {
                     localPlayerCharacter = character;
                     isInitialized = true;
                     if (showDebugLogs) Debug.Log($"[NPCDialogue] Successfully initialized local player reference: {networkPlayer.name}");
                     return;
                  }
               }
            }

            // Fallback to Game Creator's player reference if local player not found
            GameObject player = this.playerCharacter.Get(gameObject);
            if (player != null)
            {
               Character character = player.GetComponent<Character>();
               if (character != null && character.IsPlayer)
               {
                  var networkPlayer = player.GetComponent<NetworkPlayer>();
                  if (networkPlayer != null && networkPlayer.HasInputAuthority)
                  {
                     localPlayerCharacter = character;
                     isInitialized = true;
                     if (showDebugLogs) Debug.Log($"[NPCDialogue] Successfully initialized player reference via Game Creator: {player.name}");
                     return;
                  }
               }
            }

            playerCheckAttempts++;
            if (playerCheckAttempts >= maxPlayerCheckAttempts && showDebugLogs)
            {
               Debug.LogWarning($"[NPCDialogue] Failed to find player after {maxPlayerCheckAttempts} attempts");
            }
         }
         catch (System.Exception e)
         {
            Debug.LogError($"[NPCDialogue] Error initializing player reference: {e.Message}");
            isInitialized = false;
         }
      }

      private async void Update()
      {
         if (!enabled || isInitialized) return;

         if (Time.time >= nextPlayerCheckTime)
         {
            InitializePlayerReference();
            nextPlayerCheckTime = Time.time + playerCheckInterval;
         }

         if (isPlayerInRange && !isDialogueActive && Keyboard.current.eKey.wasPressedThisFrame)
         {
            await StartDialogue();
         }
         else if (isDialogueActive && Keyboard.current.escapeKey.wasPressedThisFrame)
         {
            await EndDialogue();
         }
      }

      private async void OnTriggerExit2D(Collider2D other)
      {
         if (!enabled) return;

         var networkPlayer = other.GetComponent<NetworkPlayer>();
         if (networkPlayer != null && networkPlayer.HasInputAuthority)
         {
            isPlayerInRange = false;
            if (showDebugLogs) Debug.Log($"[NPCDialogue] Local player left interaction range of {npcName}");
            HideInteractionPrompt();
            if (isDialogueActive) await EndDialogue();
            localPlayerCharacter = null;
            return;
         }

         GameObject player = this.playerCharacter.Get(gameObject);
         if (player != null && other.gameObject == player)
         {
            networkPlayer = player.GetComponent<NetworkPlayer>();
            if (networkPlayer != null && networkPlayer.HasInputAuthority)
            {
               isPlayerInRange = false;
               if (showDebugLogs) Debug.Log($"[NPCDialogue] Player left interaction range of {npcName}");
               HideInteractionPrompt();
               if (isDialogueActive) await EndDialogue();
               localPlayerCharacter = null;
            }
         }
      }

      public void ShowInteractionPrompt()
      {
         if (interactionPrompt != null)
         {
            interactionPrompt.SetActive(true);
         }
      }

      public void HideInteractionPrompt()
      {
         if (interactionPrompt != null)
         {
            interactionPrompt.SetActive(false);
         }
      }

      public Task StartDialogue(bool handlePlayerControl = true)
      {
         if (showDebugLogs) Debug.Log($"[NPCDialogue] StartDialogue called on {gameObject.name}");

         if (localPlayerCharacter == null)
         {
            InitializePlayerReference();
         }

         if (localPlayerCharacter == null)
         {
            var networkPlayers = UnityEngine.Object.FindObjectsByType<NetworkPlayer>(FindObjectsSortMode.None);
            foreach (var networkPlayer in networkPlayers)
            {
               if (networkPlayer.HasInputAuthority)
               {
                  var character = networkPlayer.GetComponent<Character>();
                  if (character != null && character.IsPlayer)
                  {
                     localPlayerCharacter = character;
                     if (Vector2.Distance(transform.position, character.transform.position) <= interactionDistance)
                     {
                        isPlayerInRange = true;
                     }
                     break;
                  }
               }
            }
         }

         if (!isPlayerInRange || localPlayerCharacter == null)
         {
            Debug.LogWarning($"[NPCDialogue] Cannot start dialogue - Player in range: {isPlayerInRange}, Local player: {(localPlayerCharacter != null ? localPlayerCharacter.name : "null")}");
            return Task.CompletedTask;
         }

         if (dialogueUI == null)
         {
            Debug.LogError("[NPCDialogue] DialogueUI reference is missing!");
            return Task.CompletedTask;
         }

         if (chatService == null)
         {
            Debug.LogError("[NPCDialogue] ChatService reference is missing!");
            return Task.CompletedTask;
         }

         isDialogueActive = true;
         dialogueUI.gameObject.SetActive(true);
         dialogueUI.Show();
         HideInteractionPrompt();

         if (handlePlayerControl && localPlayerCharacter != null)
         {
            if (showDebugLogs) Debug.Log($"[NPCDialogue] Disabling player controls for {localPlayerCharacter.name}");
            localPlayerCharacter.IsPlayer = false;
         }

         chatService.InitializeDialogue(new NPCContext(npcType, npcFaction, npcName));
         if (showDebugLogs) Debug.Log($"[NPCDialogue] Dialogue started successfully with {npcName}");
         return Task.CompletedTask;
      }

      public Task EndDialogue()
      {
         if (showDebugLogs) Debug.Log($"[NPCDialogue] Ending dialogue with {npcName}");

         isDialogueActive = false;
         if (dialogueUI != null)
         {
            dialogueUI.Hide();
         }

         if (isPlayerInRange)
         {
            ShowInteractionPrompt();
         }

         if (localPlayerCharacter != null)
         {
            if (showDebugLogs) Debug.Log($"[NPCDialogue] Restoring player controls for {localPlayerCharacter.name}");
            localPlayerCharacter.IsPlayer = true;
         }

         OnDialogueEndEvent?.Invoke();
         if (showDebugLogs) Debug.Log($"[NPCDialogue] Dialogue ended with {npcName}");
         return Task.CompletedTask;
      }

      private void OnDrawGizmos()
      {
         if (!showGizmos || interactionCenter == null) return;

         Gizmos.color = isPlayerInRange ? Color.green : Color.yellow;
         Gizmos.DrawWireSphere(interactionCenter.position, interactionDistance);
      }

      public void SetPlayerCharacter(Character character)
      {
         if (character != null && character.IsPlayer)
         {
            localPlayerCharacter = character;
            if (showDebugLogs) Debug.Log($"[NPCDialogue] Player character set manually: {character.name}");
         }
      }

      public string GetNPCName() => npcName;
      public string GetNPCFaction() => npcFaction;
      public string GetNPCType() => npcType;

      public NPCData GetNPCData()
      {
         return new NPCData(
             npcName,
             npcType,
             npcPersonality,
             npcBackground,
             nameplateColor,
             isInteractive
         );
      }

      public void UpdateFromData(NPCData data)
      {
         if (data == null) return;

         npcName = data.Name;
         npcType = data.Type;
         npcPersonality = data.Personality;
         npcBackground = data.Background;
         nameplateColor = data.NameplateColor;
         isInteractive = data.IsInteractive;
      }

      public void ForceSetPlayerInRange(Character character)
      {
         if (character != null && character.IsPlayer)
         {
            var networkPlayer = character.GetComponent<NetworkPlayer>();
            if (networkPlayer != null && networkPlayer.HasInputAuthority)
            {
               localPlayerCharacter = character;
               isPlayerInRange = true;
               if (showDebugLogs) Debug.Log($"[NPCDialogue] Force set player in range: {character.name}");
            }
         }
      }
   }
}