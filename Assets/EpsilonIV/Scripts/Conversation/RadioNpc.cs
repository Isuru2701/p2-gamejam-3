using UnityEngine;
using UnityEngine.Events;
using player2_sdk;
using TMPro;
using System.Reflection;

namespace EpsilonIV
{
    /// <summary>
    /// Extension of Player2Npc that provides public API for sending messages.
    /// Designed for radio-style communication where one UI talks to multiple NPCs.
    /// </summary>
    public class RadioNpc : Player2Npc
    {
        [Header("Radio NPC Events")]
        [Tooltip("Fired when this NPC sends a response")]
        public UnityEvent<string, string> OnRadioResponse = new UnityEvent<string, string>();

        private string lastOutputMessage = "";

        void Start()
        {
            // Ensure outputMessage is assigned (SDK requires it)
            if (outputMessage == null)
            {
                Debug.Log($"RadioNpc: Creating hidden outputMessage for {gameObject.name}");
                GameObject hiddenOutput = new GameObject("HiddenOutputMessage");
                hiddenOutput.transform.SetParent(transform);

                Canvas canvas = hiddenOutput.AddComponent<Canvas>();
                canvas.enabled = false; // Keep it invisible

                outputMessage = hiddenOutput.AddComponent<TextMeshProUGUI>();
            }
        }

        void Update()
        {
            // Watch for changes to outputMessage (where SDK writes responses)
            if (outputMessage != null)
            {
                string currentMessage = outputMessage.text;

                if (!string.IsNullOrEmpty(currentMessage) && currentMessage != lastOutputMessage)
                {
                    lastOutputMessage = currentMessage;
                    string npcName = gameObject.name;

                    Debug.Log($"RadioNpc: Response received from {npcName}: '{currentMessage}'");
                    OnRadioResponse.Invoke(npcName, currentMessage);
                }
            }
        }

        /// <summary>
        /// Public method to send a message to this NPC.
        /// Uses reflection to call the private SendChatMessageAsync method.
        /// </summary>
        public void SendMessage(string message, string context = "")
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                Debug.LogWarning($"RadioNpc: Attempted to send empty message to {gameObject.name}");
                return;
            }

            Debug.Log($"RadioNpc: Sending message to {gameObject.name}: '{message}'");

            // Call the private SendChatMessageAsync method using reflection
            var method = typeof(Player2Npc).GetMethod("SendChatMessageAsync",
                BindingFlags.NonPublic | BindingFlags.Instance);

            if (method != null)
            {
                // The private method signature is: SendChatMessageAsync(string message)
                method.Invoke(this, new object[] { message });
            }
            else
            {
                Debug.LogError("RadioNpc: Could not find SendChatMessageAsync method via reflection!");
            }

            // TODO: Phase 7 - Add context parameter support
            // The SDK's SendChatMessageAsync eventually calls SendChatRequestAsync
            // which has a ChatRequest with game_state_info field for context
        }
    }
}
