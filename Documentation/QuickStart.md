# Quick Start Guide

This guide will help you get started with the FusionWebGL AI package, even if you're not a programmer.

## Step 1: Install Required Packages

1. Open Unity Hub
2. Create a new Unity project (Unity 2022.3 or later)
3. Open the Package Manager (Window > Package Manager)
4. Install these packages:
   - Input System
   - TextMeshPro
   - Newtonsoft.Json
   - Fusion
   - GameCreator

## Step 2: Install FusionWebGL AI

1. In the Package Manager, click the "+" button
2. Select "Add package from git URL"
3. Enter: `https://github.com/yourusername/fusionwebgl-ai.git`

## Step 3: Create Your First NPC

1. Create a new empty GameObject in your scene
2. Name it "NPC"
3. Add the `NPCDialogue` component
4. In the Inspector, set:
   - NPC Type: "merchant"
   - Faction: "Merchants"
   - Name: "Merchant"
   - Personality: "Friendly and helpful"
   - Background: "A local merchant who has been trading for years"

## Step 4: Set Up the UI

1. Create a UI Canvas (right-click in Hierarchy > UI > Canvas)
2. Add the `AIDialogueUI` component to the Canvas
3. Create these UI elements:
   - Panel (name it "DialoguePanel")
   - TextMeshPro Text (name it "NPCName")
   - TextMeshPro Text (name it "MessageText")
   - TMP Input Field (name it "InputField")
   - Button (name it "SendButton")
   - Button (name it "CloseButton")
4. Drag these elements to the corresponding fields in the AIDialogueUI component

## Step 5: Configure Fusion

1. Open Fusion Hub (Tools > Fusion > Fusion Hub)
2. Enter your Fusion App ID
3. Save settings

## Step 6: Test Your Setup

1. Enter Play mode
2. Approach the NPC
3. Press E to start dialogue
4. Type a message and press Enter
5. Press ESC to end dialogue

## Common Issues and Solutions

### NPC Not Responding

- Check if the ChatGPTService is in the scene
- Verify the API URL is correct
- Make sure you're logged in

### UI Not Showing

- Check if all UI elements are assigned in the AIDialogueUI component
- Verify the Canvas is set to "Screen Space - Overlay"

### Can't Interact with NPC

- Check if the NPC has a Collider2D
- Verify the interaction distance is appropriate
- Make sure the player has a Rigidbody2D

## Tips for Better NPCs

1. Give your NPCs unique personalities
2. Write detailed backgrounds
3. Use appropriate factions
4. Set reasonable interaction distances
5. Add visual feedback (like nameplates)

## Next Steps

1. Create more NPCs with different types
2. Customize the UI appearance
3. Add animations to your NPCs
4. Create quests and dialogue branches
5. Add inventory systems

## Need Help?

1. Check the [documentation](https://github.com/yourusername/fusionwebgl-ai/wiki)
2. Join our [Discord community](https://discord.gg/your-server)
3. Contact support at <support@yourdomain.com>
