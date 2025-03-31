# Basic Dialogue Sample

This sample demonstrates how to set up a basic NPC dialogue system using the FusionWebGL AI package.

## Scene Setup

The sample scene includes:

1. Services GameObject
   - LoginManager component
   - ChatGPTService component
   - Configured with default settings

2. NPC GameObject
   - NPCDialogue component
   - BoxCollider2D for interaction
   - Sample merchant configuration
   - Interaction prompt

3. DialogueUI GameObject
   - Canvas with proper settings
   - AIDialogueUI component
   - Message display area
   - Input field for player messages
   - Send button
   - Typing indicator

## How to Use

1. Open the scene in Unity
2. Configure the API settings in the ChatGPTService component
3. Set up your player character reference in the NPCDialogue component
4. Press Play to test the dialogue system

## Customization

### NPC Settings

- Change the NPC type, faction, name, and personality
- Adjust interaction distance and visual feedback
- Modify the interaction prompt appearance

### UI Settings

- Customize colors and fonts
- Adjust typing speed and animations
- Change the layout and positioning
- Modify the maximum number of messages

### Service Settings

- Update API endpoints
- Configure debug logging
- Set up authentication

## Testing

1. Move your player character near the NPC
2. Press the interaction key (default: E)
3. Type messages in the input field
4. Press Enter or click Send to communicate
5. Watch the NPC respond with AI-generated dialogue

## Common Issues

### NPC Not Responding

- Check if the ChatGPTService is properly configured
- Verify the API URL and endpoints
- Ensure the player character reference is set

### UI Not Showing

- Check Canvas settings
- Verify UI elements are properly assigned
- Make sure the UI is not behind other elements

### Interaction Not Working

- Verify the BoxCollider2D is properly sized
- Check interaction distance settings
- Ensure the player character has the required components

## Next Steps

1. Create your own NPC variants
2. Customize the dialogue UI
3. Add more complex NPC behaviors
4. Implement quest systems
5. Add inventory and trading

## Support

For more information and support:

- Check the main package documentation
- Visit our support forum
- Contact us at <support@fusionwebgl.ai>
