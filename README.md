# FusionWebGL AI Package

A powerful AI-powered NPC dialogue system for Unity that integrates seamlessly with Fusion and GameCreator.

## Features

- 🤖 AI-powered NPC dialogue system
- 🌐 Seamless integration with Fusion networking
- 🎮 Full compatibility with GameCreator
- 🔐 Built-in authentication system
- 🎯 Customizable NPC behaviors
- 💬 Natural language processing
- 🎨 Modern UI system
- 📱 Mobile-friendly design

## Installation

1. Open the Unity Package Manager (Window > Package Manager)
2. Click the + button in the top-left corner
3. Select "Add package from git URL"
4. Enter: `https://github.com/yourusername/fusionwebgl-ai.git`
5. Click Add

## Requirements

- Unity 2022.3 or later
- Fusion 1.1.0 or later
- GameCreator Core 1.0.0 or later
- Input System package
- Newtonsoft.Json package

## Quick Start

1. Create a new scene
2. Drag the Services prefab from the Samples folder
3. Drag the DialogueUI prefab
4. Create an NPC using the NPC prefab
5. Configure the API settings in the ChatGPTService component
6. Press Play to test

## Package Structure

```
com.fusionwebgl.ai/
├── Runtime/
│   ├── AI/
│   │   ├── ChatGPTService.cs
│   │   ├── NPCDialogue.cs
│   │   └── NPCContext.cs
│   ├── Auth/
│   │   └── LoginManager.cs
│   └── UI/
│       └── AIDialogueUI.cs
├── Editor/
│   ├── AI/
│   │   └── NPCDialogueEditor.cs
│   └── Auth/
│       └── LoginManagerEditor.cs
└── Samples/
    ├── BasicDialogue/
    └── AdvancedNPC/
```

## Configuration

### API Settings

1. Select the Services GameObject
2. In the ChatGPTService component:
   - Set your API base URL
   - Configure the chat endpoint
   - Set up authentication if needed

### NPC Configuration

1. Select your NPC GameObject
2. In the NPCDialogue component:
   - Set NPC type and faction
   - Configure personality and background
   - Adjust interaction settings
   - Set up visual feedback

### UI Customization

1. Select the DialogueUI GameObject
2. In the AIDialogueUI component:
   - Customize colors and fonts
   - Adjust layout and animations
   - Configure message display settings

## Samples

### Basic Dialogue

A simple scene demonstrating:

- Basic NPC setup
- Dialogue UI configuration
- Service integration
- Player interaction

### Advanced NPC

A more complex example showing:

- Multiple NPC types
- Custom behaviors
- Quest integration
- Inventory system

## Documentation

For detailed documentation:

- [Getting Started Guide](Documentation/GettingStarted.md)
- [API Reference](Documentation/API.md)
- [Best Practices](Documentation/BestPractices.md)
- [Troubleshooting](Documentation/Troubleshooting.md)

## Support

- [Documentation](https://docs.fusionwebgl.ai)
- [Forum](https://forum.fusionwebgl.ai)
- [Discord](https://discord.gg/fusionwebgl)
- [Email](support@fusionwebgl.ai)

## License

This package is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

## Credits

- Built with [Fusion](https://www.photonengine.com/fusion)
- Powered by [GameCreator](https://gamecreator.io)
- AI powered by [OpenAI](https://openai.com)
