# Prefabs Guide

This guide explains the prefabs included in the FusionWebGL AI package.

## Included Prefabs

### 1. NPC Prefab

Location: `Prefabs/NPC.prefab`

A complete NPC setup with:

- NPCDialogue component
- Collider2D for interaction
- Visual feedback (nameplate)
- Interaction prompt

To use:

1. Drag the prefab into your scene
2. Configure the NPC properties in the Inspector
3. Position the NPC where you want it

### 2. Dialogue UI Prefab

Location: `Prefabs/DialogueUI.prefab`

A complete UI setup with:

- Canvas
- AIDialogueUI component
- All required UI elements
- Modern styling

To use:

1. Drag the prefab into your scene
2. The UI will automatically work with any NPC in the scene

### 3. Services Prefab

Location: `Prefabs/Services.prefab`

Contains all required services:

- ChatGPTService
- LoginManager

To use:

1. Drag the prefab into your scene
2. Configure the API settings in the Inspector

## Customization

### NPC Customization

1. Select the NPC in the scene
2. In the Inspector, modify:
   - NPC Identity (type, faction, name, etc.)
   - Interaction Settings (distance, debug options)
   - Visual Settings (nameplate color, prompt)

### UI Customization

1. Select the DialogueUI in the scene
2. In the Inspector, modify:
   - Colors
   - Fonts
   - Layout
   - Animation settings

### Service Customization

1. Select the Services object in the scene
2. In the Inspector, modify:
   - API URLs
   - Debug settings
   - Authentication options

## Best Practices

1. Always use the prefabs as a starting point
2. Create variants of the NPC prefab for different types
3. Keep the Services prefab in a separate scene
4. Use the UI prefab as a template for custom designs
5. Save your customized prefabs for reuse

## Troubleshooting

### Prefab Not Working

- Check if all components are properly assigned
- Verify the prefab is not broken (blue text in Inspector)
- Make sure all dependencies are installed

### UI Not Showing

- Check Canvas settings
- Verify UI elements are properly assigned
- Check if the UI is behind other elements

### Services Not Working

- Verify API URLs are correct
- Check if all required components are present
- Make sure the prefab is not duplicated
