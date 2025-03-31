using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace FusionWebGL.UI
{
   public class AIDialogueUI : MonoBehaviour
   {
      [Header("UI References")]
      [SerializeField] private GameObject dialoguePanel;
      [SerializeField] private TextMeshProUGUI npcNameText;
      [SerializeField] private TextMeshProUGUI messageText;
      [SerializeField] private TMP_InputField inputField;
      [SerializeField] private Button sendButton;
      [SerializeField] private Button closeButton;
      [SerializeField] private ScrollRect messageScrollRect;

      [Header("Settings")]
      [SerializeField] private float messageDisplayDelay = 0.05f;
      [SerializeField] private Color playerMessageColor = Color.blue;
      [SerializeField] private Color npcMessageColor = Color.white;

      private bool isVisible;
      private bool isTyping;
      private string currentMessage;
      private int currentCharIndex;
      private float nextCharTime;
      private List<DialogueMessage> messageHistory = new List<DialogueMessage>();

      private void Start()
      {
         if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

         if (sendButton != null)
            sendButton.onClick.AddListener(SendMessage);

         if (closeButton != null)
            closeButton.onClick.AddListener(Hide);

         if (inputField != null)
         {
            inputField.onSubmit.AddListener(_ => SendMessage());
            inputField.onValueChanged.AddListener(_ => UpdateSendButtonState());
         }
      }

      private void Update()
      {
         if (isTyping)
         {
            if (Time.time >= nextCharTime)
            {
               DisplayNextCharacter();
            }
         }

         if (Keyboard.current.enterKey.wasPressedThisFrame && !isTyping)
         {
            SendMessage();
         }
      }

      public void Show()
      {
         isVisible = true;
         if (dialoguePanel != null)
            dialoguePanel.SetActive(true);

         if (inputField != null)
         {
            inputField.Select();
            inputField.ActivateInputField();
         }

         UpdateSendButtonState();
      }

      public void Hide()
      {
         isVisible = false;
         if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

         if (inputField != null)
         {
            inputField.text = "";
            inputField.DeactivateInputField();
         }
      }

      public void SetNPCName(string name)
      {
         if (npcNameText != null)
            npcNameText.text = name;
      }

      public void AddMessage(string message, bool isPlayer)
      {
         var dialogueMessage = new DialogueMessage(message, isPlayer);
         messageHistory.Add(dialogueMessage);
         currentMessage = message;
         currentCharIndex = 0;
         nextCharTime = Time.time;
         isTyping = true;

         UpdateMessageDisplay();
      }

      private void DisplayNextCharacter()
      {
         if (currentCharIndex < currentMessage.Length)
         {
            if (messageText != null)
            {
               messageText.text += currentMessage[currentCharIndex];
               currentCharIndex++;
               nextCharTime = Time.time + messageDisplayDelay;
            }
         }
         else
         {
            isTyping = false;
         }
      }

      private void UpdateMessageDisplay()
      {
         if (messageText != null)
         {
            messageText.text = "";
            foreach (var message in messageHistory)
            {
               messageText.text += $"<color=#{ColorUtility.ToHtmlStringRGBA(message.IsPlayer ? playerMessageColor : npcMessageColor)}>";
               messageText.text += message.IsPlayer ? "You: " : "NPC: ";
               messageText.text += message.Text;
               messageText.text += "</color>\n\n";
            }
         }

         if (messageScrollRect != null)
         {
            Canvas.ForceUpdateCanvases();
            messageScrollRect.verticalNormalizedPosition = 0f;
         }
      }

      private void SendMessage()
      {
         if (inputField == null || string.IsNullOrWhiteSpace(inputField.text))
            return;

         string message = inputField.text;
         inputField.text = "";
         UpdateSendButtonState();

         // Notify the NPCDialogue component
         var npcDialogue = FindObjectOfType<FusionWebGL.AI.NPCDialogue>();
         if (npcDialogue != null)
         {
            npcDialogue.SendMessage(message);
         }
      }

      private void UpdateSendButtonState()
      {
         if (sendButton != null)
            sendButton.interactable = !string.IsNullOrWhiteSpace(inputField?.text);
      }
   }

   public class DialogueMessage
   {
      public string Text { get; }
      public bool IsPlayer { get; }

      public DialogueMessage(string text, bool isPlayer)
      {
         Text = text;
         IsPlayer = isPlayer;
      }
   }
}