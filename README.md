# Matcha Latte

A Unity VR locomotion system integrated with EMG (Electromyography) signals from the Cyton biosensing device. This project enables hands-free VR movement control using muscle signals, combined with traditional keyboard input and haptic feedback.

## Overview

Matcha Latte is an experimental VR application that demonstrates EMG-based locomotion in virtual reality. Users can navigate VR environments using muscle signals captured from a Cyton EMG device, which are transmitted to Unity via UDP. The project also includes VR painting capabilities with haptic feedback integration.

## Features

- **EMG-Based Locomotion**: Control VR movement using muscle signals from Cyton EMG device
- **UDP Communication**: Real-time data streaming from Cyton device to Unity
- **Hybrid Input System**: Combines EMG signals with keyboard input
- **VR Support**: Built for Oculus VR headsets (OVRCameraRig)
- **Haptic Feedback**: Integration with Afference haptic SDK for tactile responses
- **Surface Painting**: Paint on VR surfaces with collision-based haptic triggers
- **Auto Height Adjustment**: Automatically adjusts character controller height based on headset position
- **Smooth Movement**: Configurable smoothing and deadzone for EMG input
- **Procedural Maze Generation**: Blender-based maze generation assets

## Requirements

### Hardware
- Oculus VR headset (Quest, Rift, etc.)
- Cyton EMG device (optional, for EMG-based control)
- Afference haptic device (optional, for haptic feedback)

### Software
- Unity 2020.x or later
- Python 3.x (for Cyton UDP bridge)
- Blender (for maze generation/modification)

### Dependencies
- Oculus Integration SDK
- Afference Unity SDK (included in `UnityProject/Assets/AfferenceUnitySDK/`)
- Newtonsoft JSON (included)

## Setup

### 1. Clone the Repository
```bash
git clone https://github.com/thomazli/matcha-latte.git
cd matcha-latte
```

### 2. Unity Project Setup
1. Open Unity Hub
2. Add the project from `UnityProject/` directory
3. Open the project (Unity will import all assets)
4. Ensure Oculus Integration is properly configured
5. Connect your Oculus headset

### 3. Cyton EMG Setup (Optional)
If using EMG control:

1. Configure your Cyton device hardware settings:
   - Edit `Cyton/CytonHardwardSettings` for channel configuration
   - Edit `Cyton/CytonUserSettings.json` for user preferences

2. Start the UDP bridge:
   ```bash
   cd Cyton
   python CytonUDPExample.py
   ```
   The script will listen on `127.0.0.1:12345` and forward EMG data to Unity.

### 4. Unity Scene Configuration

1. Locate the main scene in Unity
2. Find the character controller GameObject with `UnifiedVrLocomotion` script
3. Configure the component:
   - Assign the `UDPReceiver` component if using EMG
   - Adjust movement speed, smoothing, and deadzone values
   - Enable/disable keyboard input as needed

## Usage

### Running the Application

1. **With EMG Control**:
   - Start the Cyton UDP bridge: `python Cyton/CytonUDPExample.py`
   - Press Play in Unity Editor or build and run
   - EMG signals from channels 0 and 1 control forward/backward and left/right movement

2. **Keyboard Control Only**:
   - Press Play in Unity
   - Use WASD keys for movement
   - Use Q/E keys for rotation

### Input Configuration

The `UnifiedVrLocomotion` component provides several configurable options:

- **Movement Settings**:
  - `moveSpeed`: Base movement speed (default: 2.5)
  - `deadzone`: Minimum input threshold to ignore noise (default: 0.08)
  - `udpSmoothing`: Low-pass filter amount (0 = raw, 1 = heavy smoothing)

- **Keyboard Input**:
  - `enableKeyboard`: Toggle keyboard input on/off
  - `keyboardSpeed`: Keyboard movement multiplier

- **Turning**:
  - `enableTurn`: Enable/disable rotation controls
  - `turnSpeed`: Rotation speed in degrees per second

- **Height Adjustment**:
  - `autoAdjustHeight`: Automatically adjust character height
  - `minHeight` / `maxHeight`: Height bounds

### EMG Data Format

The UDP receiver expects JSON packets in the following format:
```json
{
  "type": "emg",
  "data": [x_value, z_value]
}
```
- `data[0]`: Left/right movement (-1 to 1)
- `data[1]`: Forward/backward movement (-1 to 1)

## Project Structure

```
matcha-latte/
├── Blender/
│   └── mazeGeneration.blend      # Blender file for procedural maze generation
├── Cyton/
│   ├── CytonHardwardSettings     # Cyton device configuration
│   ├── CytonUserSettings.json    # User-specific settings
│   └── CytonUDPExample.py        # Python UDP bridge for EMG data
├── Resources/
│   ├── referencecube.fbx         # Reference 3D models
│   └── test_maze.fbx
├── UnityProject/
│   └── Assets/
│       ├── matcha-latte/
│       │   ├── Scripts/
│       │   │   ├── CollisionTrigger.cs      # Haptic feedback on collision
│       │   │   ├── KeyboardInput.cs         # Keyboard input handler
│       │   │   ├── MovementInput.cs         # Movement input abstraction
│       │   │   ├── SurfacePainter.cs        # VR surface painting
│       │   │   ├── UDPReceiver.cs           # UDP EMG data receiver
│       │   │   └── UnifiedVrLocomotion.cs   # Main locomotion controller
│       │   └── Materials/                    # Project materials
│       └── AfferenceUnitySDK/               # Haptic feedback SDK
└── README.md
```

## Components

### UnifiedVrLocomotion
Main locomotion controller that:
- Processes EMG input from UDP
- Handles keyboard input
- Applies head-relative movement
- Manages character height adjustment
- Supports smooth rotation

### UDPReceiver
Receives and parses EMG data packets:
- Binds to UDP port 12345
- Parses JSON EMG packets
- Exposes `valueX` and `valueZ` for movement

### SurfacePainter
Enables painting on VR surfaces:
- Runtime texture modification
- UV-based painting
- Configurable brush size and color

### CollisionTrigger
Triggers haptic feedback on collision:
- Works with tagged surfaces ("HapticSurface")
- Rate-limited pulse generation
- Optional material swapping

## Development

### Building for Oculus Quest
1. File → Build Settings
2. Select Android platform
3. Configure Oculus Quest settings
4. Build and deploy to device

### Customizing Movement
Edit `UnifiedVrLocomotion.cs` to customize:
- Movement smoothing algorithms
- Input processing
- Deadzone behavior
- Head-relative direction calculation

### Adding New EMG Channels
Modify `UDPReceiver.cs` to handle additional data channels in the `EmgPacket` class.

## Troubleshooting

**UDP Connection Issues:**
- Verify the Python script is running and listening on port 12345
- Check firewall settings
- Ensure Unity's `UDPReceiver` port matches the Python script

**Movement Not Working:**
- Check that `UDPReceiver` component is assigned in `UnifiedVrLocomotion`
- Verify EMG data is being received (enable `logUdp` option)
- Adjust deadzone if values are too small

**VR Headset Not Detected:**
- Ensure Oculus Integration is properly installed
- Check that the headset is connected and Oculus software is running
- Verify the scene uses OVRCameraRig

## License

MIT License - see [LICENSE](LICENSE) file for details.

Copyright (c) 2025 Thomas Li

## Acknowledgments

- Afference Unity SDK for haptic feedback integration
- OpenBCI/Cyton for EMG biosensing hardware
- Oculus for VR SDK

## Contributing

Contributions are welcome! Please feel free to submit issues or pull requests.

## Contact

For questions or feedback, please open an issue on the GitHub repository.
