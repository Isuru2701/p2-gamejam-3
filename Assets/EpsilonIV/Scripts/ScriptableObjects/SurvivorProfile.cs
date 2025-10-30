using UnityEngine;

namespace EpsilonIV
{
    /// <summary>
    /// Defines a survivor NPC that the player must locate and rescue.
    /// Contains all personality, knowledge, and configuration needed for AI conversations.
    /// </summary>
    [CreateAssetMenu(fileName = "NewSurvivor", menuName = "Survivors/Survivor Profile")]
    public class SurvivorProfile : ScriptableObject
    {
        [Header("Identity")]
        [Tooltip("Unique identifier for this survivor (used internally)")]
        public string survivorID = "survivor_id";

        [Tooltip("Display name shown to player")]
        public string displayName = "Survivor Name";

        [Header("Player2 API Configuration")]
        [Tooltip("First-person character description - how the NPC describes themselves")]
        [TextArea(3, 10)]
        public string characterDescription =
            "I am [name], [role]. [Brief personality description].";

        [Tooltip("Third-person system prompt - personality, behavior, how they respond")]
        [TextArea(5, 15)]
        public string systemPrompt =
            "You are [name], [description of personality and behavior]. " +
            "[How they communicate]. [Their current mental/emotional state].";

        [Tooltip("Player2 TTS voice ID for this survivor")]
        public string voiceId = "01955d76-ed5b-7451-92d6-5ef579d3ed28";

        [Header("Survivor's Knowledge Base")]
        [Tooltip("All factual information this survivor knows - sent as game_state_info with each message")]
        [TextArea(15, 50)]
        public string knowledgeBase =
            @"SURVIVOR'S LOCATION & MEMORY:
- Where they are hiding
- How they got there
- What they remember about their location

AREA LAYOUT:
- Description of the location where they're hiding
- Important landmarks or features
- Connection to other areas

NAVIGATION INFORMATION:
- How to reach this location from other parts of the station
- Door codes they know
- Routes they can describe

ENVIRONMENTAL DETAILS:
- What they can see, hear, smell
- Temperature, lighting conditions
- Any distinctive features";

        [Header("Physical Location Setup")]
        [Tooltip("World position where this survivor is hiding (for placing trigger collider)")]
        public Vector3 hidingSpotPosition;

        [Tooltip("Unique identifier for the trigger zone that detects when player finds this survivor")]
        public string triggerID = "location_id";

        [Header("Mission Status")]
        [Tooltip("Current state of this survivor in the rescue mission")]
        public RescueState rescueState = RescueState.Undiscovered;
    }

    /// <summary>
    /// Tracks the current state of a survivor in the rescue mission
    /// </summary>
    public enum RescueState
    {
        Undiscovered,   // Player hasn't opened communication yet
        Active,         // Radio channel open, player can talk to them
        Located,        // Player physically found them (entered trigger)
        Rescued         // Successfully led back to ship
    }
}
