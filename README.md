# VR but Dark
Modern VR games are almost entirely built on game controller inputs, button/vibration feedback, and are addicted to conveying all information visually. What if we subverted all of that?

## The Vision
Accidentally slide down the rabbit hole.. Direct muscle control of movement! Tactile feedback on fingers from the world! We create a controller-free world where your hands are completely free, with full freedom of movement along the world using a mix of EMG muscle state and head rotation. In a world of complete darkness, you must rely on haptic feedback and sound in order to traverse your way to the exit, with users building up a map of their worlds as they progress through the environment. 

## Tech Stack
### Hardware
- Meta Quest 3S
- OpenBCI Cyton board and EMG kit
- Afference Ring
### Software
- Unity 6000.0.26f1 (Main game simulation)
- OpenBCI GUI (Stream EMG data)
- Afference SDK (Connect to ring via BLE)
- Blender (Generating scenes and assets)
