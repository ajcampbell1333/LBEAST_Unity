# LBEAST for Unity

<img src="Packages/com.ajcampbell.lbeast/Runtime/images/lbeast-logo.png" alt="LBEAST Logo" width="100%">

**Location-Based Entertainment Activation Standard Toolkit (LBEAST)** - A comprehensive SDK for developing VR/AR Location-Based Entertainment experiences in Unity with support for AI facial animation, large hydraulic haptics, and embedded systems integration.

**Author Disclaimer**
This is a brand new plugin as of November 2025. Parts of it are not fully fleshed out. I built a lot of LBE for Fortune 10 brands in the 20-teens. This is the dream game-engine toolchain I wish we had back then, but I'm 100% certain it's full of unforeseen bugs in its current form. If you're seeing this message, it's because I have yet to deploy this plugin on a single professional project. I'm sure I will have lots of fixes to push after I've deployed it a few times in-situ. Use at your own risk in the meantime.

[![Unity](https://img.shields.io/badge/Unity-6%20LTS-black?logo=unity)](https://unity.com)
[![License](https://img.shields.io/badge/License-MIT-blue.svg)](https://opensource.org/licenses/MIT)
[![Platform](https://img.shields.io/badge/Platform-Windows-lightgrey.svg)](https://microsoft.com/windows)

---

## 🎯 Quick Start

```csharp
using LBEAST.ExperienceTemplates;

// Create a moving platform experience
var platform = gameObject.AddComponent<MovingPlatformExperience>();
platform.InitializeExperience();

// Send normalized motion (hardware-agnostic)
platform.SendPlatformTilt(0.5f, -0.3f, 0f, 1.5f);  // TiltX, TiltY, Vertical, Duration
```

---

## 📚 Table of Contents

- [Prerequisites & Package Dependencies](#-prerequisites--package-dependencies)
- [Three-Tier Architecture](#-three-tier-architecture)
- [Hardware-Agnostic Input System](#-hardware-agnostic-input-system)
- [Features](#-features)
- [Experience Templates](#experience-templates-pre-configured-solutions)
- [Low-Level APIs](#low-level-apis-technical-modules)
- [Installation](#-installation)
- [Examples](#-examples)
- [Dedicated Server & Server Manager](#-dedicated-server--server-manager)
- [Roadmap](#-roadmap)
- [License](#-license)

---

## 📦 Prerequisites & Package Dependencies

> **📦 LBEAST is a Unity Package** - See the [Installation](#-installation) section below for detailed setup instructions.

### **Unity Version Requirements**

- **Unity 6 LTS (Recommended)** or Unity 2022.3 LTS (Minimum)
- **Windows 10/11** (Primary platform)
- **Linux** (Experimental support)

### **LBEAST Package Dependencies**

LBEAST requires several Unity packages. These are **automatically installed** when you install LBEAST via Package Manager or Git URL. If installing manually, install these via **Window > Package Manager**:

#### **Core Dependencies (Required)**

| Package | Version | Purpose | Installation |
|---------|---------|---------|--------------|
| **XR Plugin Management** | 4.4.0+ | VR/AR runtime management | `com.unity.xr.management` |
| **OpenXR Plugin** | 1.9.0+ | Cross-platform VR support | `com.unity.xr.openxr` |
| **Input System** | 1.7.0+ | Modern input handling | `com.unity.inputsystem` |
| **TextMeshPro** | 3.0.6+ | UI text rendering | `com.unity.textmeshpro` |

#### **Multiplayer Dependencies (Required for AIFacemask)**

| Package | Version | Purpose | Installation |
|---------|---------|---------|--------------|
| **NetCode for GameObjects** | 1.8.0+ | Multiplayer networking | `com.unity.netcode.gameobjects` |
| **Unity Transport** | 2.2.0+ | Network transport layer | `com.unity.transport` |

#### **Optional Packages (Platform-Specific)**

| Package | Version | Purpose | When Needed |
|---------|---------|---------|-------------|
| **SteamVR Plugin** | 2.7.3+ | SteamVR integration | If using Valve Index, HTC Vive |
| **Oculus Integration** | Latest | Meta Quest integration | If targeting Quest 2/3/Pro |
| **XR Interaction Toolkit** | 2.5.0+ | VR interaction helpers | For advanced VR interactions |

### **Quick Setup (If You Cloned This Repo)**

If you've cloned the `LBEAST_Unity` repository, everything is already configured:

1. **Open the project in Unity**
2. **LBEAST package** is already in `Packages/com.ajcampbell.lbeast/`
3. **Dependencies** will auto-install from `Packages/manifest.json`
4. **That's it!** See the [Installation](#-installation) section for more details.

> **💡 Note:** If you cloned the repo, LBEAST is already set up as a local package. All dependencies will automatically install when you first open the project.

For installation in other projects, see the [Installation](#-installation) section below.

### **Project Settings Configuration**

After installing packages, configure:

1. **XR Plugin Management**
   - **Edit > Project Settings > XR Plug-in Management**
   - Enable **OpenXR** for your target platform
   - Configure **OpenXR Feature Sets** (hand tracking, controllers, etc.)

2. **Input System**
   - **Edit > Project Settings > Player > Active Input Handling**
   - Select **"Both"** (supports old and new input systems)

3. **Physics**
   - **Edit > Project Settings > Physics**
   - Verify collision layers for VR interactions

### **Verification**

To verify all dependencies are installed:

1. Open **Window > Package Manager**
2. Switch to **"In Project"** view
3. Confirm all required packages are listed

If you see compilation errors about missing namespaces:
- `Unity.Netcode` → Install NetCode for GameObjects
- `Unity.XR.OpenXR` → Install OpenXR Plugin
- `UnityEngine.InputSystem` → Install Input System

### **Common Issues & Troubleshooting**

#### **"The type or namespace name 'NetworkBehaviour' could not be found"**
**Solution:** Install `com.unity.netcode.gameobjects` via Package Manager.

#### **"The type or namespace name 'InputSystem' could not be found"**
**Solution:** 
1. Install `com.unity.inputsystem`
2. Go to **Edit > Project Settings > Player**
3. Change **Active Input Handling** to **"Both"** or **"Input System Package (New)"**

#### **"Assembly has reference to non-existent assembly 'Unity.XR.OpenXR'"**
**Solution:** Install `com.unity.xr.openxr` and enable OpenXR in **Edit > Project Settings > XR Plug-in Management**.

#### **Package installation fails or gets stuck**
**Solution:**
1. Close Unity Editor
2. Delete `Library/` folder in project root
3. Delete `Packages/packages-lock.json`
4. Reopen Unity Editor (will reimport all packages)

#### **"Could not load file or assembly 'Unity.Netcode.Runtime'"**
**Solution:** 
1. Restart Unity Editor
2. If that fails, reimport NetCode package: **Package Manager > NetCode for GameObjects > Right-click > Reimport**

---

## 🏗️ Three-Tier Architecture

LBEAST is structured in three layers, allowing developers to choose their level of control:

### Tier 1: Low-Level APIs (Technical Modules)
Foundation modules providing core functionality:
- `LBEASTCore` - VR/XR tracking abstraction, networking
- `AIFacemask` - Facial animation control
- `LargeHaptics` - Platform/gyroscope control
- `EmbeddedSystems` - Microcontroller integration
- `ProAudio` - Professional audio console control via OSC
- `ProLighting` - DMX lighting control (Art-Net, USB DMX)
- `VOIP` - Low-latency voice communication with 3D HRTF spatialization

**Use these when:** Building custom experiences from scratch with full control.

### Tier 2: Experience Templates (Pre-Configured MonoBehaviours)
Ready-to-use complete experiences combining multiple APIs:
- `AIFacemaskExperience` - Live actor-driven multiplayer VR
- `MovingPlatformExperience` - Standing hydraulic platform
- `GunshipExperience` - 4-player seated platform
- `CarSimExperience` - Racing/driving simulator
- `FlightSimExperience` - Flight sim with HOTAS
- `EscapeRoomExperience` - Puzzle-based escape room with embedded door locks and prop sensors

**Use these when:** Rapid deployment of standard LBE configurations.

### Tier 3: Your Custom Game Logic
Build your specific experience (Tier 3) on top of templates (Tier 2) or APIs (Tier 1).

### When to Use What?

| Scenario | Use This | Why |
|----------|----------|-----|
| Building a gunship VR arcade game | `GunshipExperience` | Pre-configured for 4 players, all hardware setup included |
| Building a racing game | `CarSimExperience` | Simplified driving API, optimized motion profiles |
| Building a space combat game | `FlightSimExperience` | HOTAS integration ready, continuous rotation supported |
| Custom 3-player standing platform | Low-Level APIs | Need custom configuration not covered by templates |
| Live actor-driven escape room | `AIFacemaskExperience` | Live actor support, multiplayer, and embedded systems ready |
| Puzzle-based escape room | `EscapeRoomExperience` | Narrative state machine, door locks, prop sensors, embedded systems |
| Unique hardware configuration | Low-Level APIs | Full control over all actuators and systems |

**Rule of thumb:** Start with templates, drop to APIs only when you need customization beyond what templates offer.

### When to Use What Configuration?

| Scenario | Recommended Configuration | Why |
|----------|---------------------------|-----|
| Basic single-player experience | **Local Command Console** (same PC as server) | Simple setup, no need for separate machines. Command Console launches and manages server locally. |
| Basic multiplayer with RPCs but no heavy data transferring wirelessly | **Local Command Console** (same PC as server) | Network traffic is lightweight (player positions, events). Local Command Console can manage server on same machine efficiently. |
| Lots of heavy graphics processing you want to offload from VR HMD(s) | **Dedicated Server + Remote Command Console** (separate PCs) | Offload GPU-intensive rendering and AI processing to dedicated server PC. Remote Command Console monitors and controls from separate machine. Better performance isolation and HMD battery life. |
| Need to monitor the experience in real-time from off-site? | **Dedicated Server + Remote Command Console** (separate PCs) ⚠️ | Remote Command Console can connect over network to monitor server status, player count, experience state, and logs from a separate location. **⚠️ Recommended for debugging/testing only. For general public operation, full internet isolation is recommended for security.** Requires authentication enabled in Command Protocol settings. |

**Configuration Options:**
- **Local Command Console:** Command Console (UI Panel) and Server Manager (dedicated server) run on the same PC. Simple setup, one machine.
- **Remote Command Console:** Command Console runs on separate PC from Server Manager. Networked via UDP (port 7779). Better for heavy processing workloads.

---

## 🎮 Hardware-Agnostic Input System

### Normalized Tilt Control (-1 to +1)

LBEAST uses a **joystick-style normalized input system** for all 5DOF hydraulic platforms. This means you write your game code once, and it works on any hardware configuration:

**Why Normalized Inputs?**
- ✅ **Hardware Independence:** Same game code works on platforms with 5° tilt or 15° tilt
- ✅ **Venue Flexibility:** Operators can upgrade/downgrade hardware without code changes
- ✅ **Intuitive API:** Think like a joystick: -1.0 (full left/back), 0.0 (center), +1.0 (full right/forward)
- ✅ **Automatic Scaling:** SDK maps your inputs to actual hardware capabilities

**Example:**
```csharp
// Your game sends: "tilt 50% right, 80% forward"
platform.SendPlatformTilt(0.5f, 0.8f, 0f, 1f);

// On 5° max platform: Translates to Roll=2.5°, Pitch=4.0°
// On 15° max platform: Translates to Roll=7.5°, Pitch=12.0°
// Same code, automatically scaled!
```

**Axis Mapping:**
- **TiltX:** Left/Right roll (-1.0 = full left, +1.0 = full right)
- **TiltY:** Forward/Backward pitch (-1.0 = full backward, +1.0 = full forward)
- **VerticalOffset:** Up/Down translation (-1.0 = full down, +1.0 = full up)

**Advanced Users:** If you need precise control and know your hardware specs, angle-based APIs are available.

---

## ✨ Features

### Experience Templates (Drag-and-Drop Solutions)

Experience Templates are complete, pre-configured MonoBehaviours that you can add to your scene and use immediately. Each combines multiple low-level APIs into a cohesive, tested solution.

#### 🎭 AI Facemask Experience

**Class:** `AIFacemaskExperience`

Deploy LAN multiplayer VR experiences where immersive theater live actors drive avatars with **autonomous AI-generated facial expressions**. The AI face operates independently using NVIDIA Audio2Face (Neural Face via Omniverse server), while live actors control the experience flow through wrist-mounted buttons.

**Architecture:**
- **AI Face**: Fully autonomous, driven by NVIDIA Audio2Face for natural conversation
- **Live Actor Role**: Experience director, NOT puppeteer
- **Wrist Controls**: 4 buttons (2 left, 2 right) to advance/retreat through experience states
- **Experience Loop**: Built-in state machine (Intro → Tutorial → Act1 → Act2 → Finale → Credits)

**Includes:**
- Pre-configured `FacialAnimationController` (autonomous facial animation bridge)
- Pre-configured `SerialDeviceController` (wrist buttons)
- Pre-configured `ExperienceStateMachine` (story progression)
- LAN multiplayer support (configurable live actor/player counts)

**Button Layout:**
- **Left Wrist**: Button 0 (Forward), Button 1 (Backward)
- **Right Wrist**: Button 2 (Forward), Button 3 (Backward)

**Quick Start:**
```csharp
var experience = gameObject.AddComponent<AIFacemaskExperience>();
experience.InitializeExperience();

// React to experience state changes
string currentState = experience.GetCurrentExperienceState();

// Manually trigger state changes (usually handled by wrist buttons automatically)
experience.AdvanceExperience();
experience.RetreatExperience();
```

**Custom State Machine:**
```csharp
// Define custom experience states
var customStates = new List<ExperienceState>
{
    new ExperienceState("Intro", "Welcome sequence"),
    new ExperienceState("Challenge1", "First puzzle"),
    new ExperienceState("Boss", "Final boss fight")
};

// Initialize with custom states
var loop = GetComponent<ExperienceStateMachine>();
loop.Initialize(customStates);
loop.StartExperience();
```

**Handle State Changes:**
Override `OnExperienceStateChanged` in your derived class:
```csharp
protected override void OnExperienceStateChanged(string oldState, string newState, int newStateIndex)
{
    base.OnExperienceStateChanged(oldState, newState, newStateIndex);
    
    if (newState == "Boss")
    {
        SpawnBossEnemy();
        PlayDramaticMusic();
    }
}
```

#### 🎢 Moving Platform Experience

**Class:** `MovingPlatformExperience`

Single-player standing VR experience on an unstable hydraulic platform with safety harness. Provides pitch, roll, and Y/Z translation for immersive motion.

**Includes:**
- Pre-configured 5DOF hydraulic platform (4 actuators + scissor lift)
- 10° pitch and roll capability
- Vertical translation for rumble/earthquake effects
- Emergency stop and return-to-neutral functions

**Quick Start:**
```csharp
var platform = gameObject.AddComponent<MovingPlatformExperience>();
platform.InitializeExperience();

// Send normalized tilt (RECOMMENDED - hardware-agnostic)
platform.SendPlatformTilt(0.3f, -0.5f, 0f, 2f);  // TiltX, TiltY, Vertical, Duration

// Advanced: Use absolute angles if needed
platform.SendPlatformMotion(5f, -3f, 20f, 2f);  // pitch, roll, vertical, duration
```

#### 🚁 Gunship Experience

**Class:** `GunshipExperience`

Four-player seated VR experience on a hydraulic platform. Perfect for multiplayer gunship, helicopter, spaceship, or multi-crew vehicle simulations.

**Includes:**
- Pre-configured 5DOF hydraulic platform (6 actuators + scissor lift)
- 4 pre-defined seat positions
- LAN multiplayer support (4 players)
- Synchronized motion for all passengers

**Quick Start:**
```csharp
var gunship = gameObject.AddComponent<GunshipExperience>();
gunship.InitializeExperience();

// Send normalized motion (RECOMMENDED)
gunship.SendGunshipTilt(0.5f, 0.8f, 0.2f, 1.5f);

// Advanced: Use absolute angles
gunship.SendGunshipMotion(8f, 5f, 10f, 15f, 1.5f);
```

#### 🏎️ Car Sim Experience

**Class:** `CarSimExperience`

Single-player seated racing/driving simulator on a hydraulic platform. Perfect for arcade racing games and driving experiences.

**Includes:**
- Pre-configured 5DOF hydraulic platform optimized for driving
- Motion profiles for cornering, acceleration, and bumps
- Compatible with racing wheels and pedals (via Unity Input System)

**Quick Start:**
```csharp
var carSim = gameObject.AddComponent<CarSimExperience>();
carSim.InitializeExperience();

// Use normalized driving API (RECOMMENDED)
carSim.SimulateCornerNormalized(-0.8f, 0.5f);      // Left turn (-1 to +1)
carSim.SimulateAccelerationNormalized(0.5f, 0.5f); // Accelerate (-1 to +1)
carSim.SimulateBump(0.8f, 0.2f);                   // Bump (intensity 0-1)

// Advanced: Use absolute angles
carSim.SimulateCorner(-8f, 0.5f);         // degrees
carSim.SimulateAcceleration(5f, 0.5f);    // degrees
```

#### ✈️ Flight Sim Experience

**Class:** `FlightSimExperience`

Single-player flight simulator using a two-axis gyroscope for continuous rotation beyond 360 degrees. Perfect for realistic flight arcade games and space combat.

**Includes:**
- Pre-configured 2DOF gyroscope system (continuous pitch/roll)
- **HOTAS controller integration:**
  - Logitech G X56 support
  - Thrustmaster T.Flight support
  - Joystick, throttle, and pedal controls
  - Configurable sensitivity and axis inversion
- Continuous rotation (720°, 1080°, unlimited)
- Unity Input System integration

**Quick Start:**
```csharp
var flightSim = gameObject.AddComponent<FlightSimExperience>();
flightSim.InitializeExperience();

// Apply normalized flight input
flightSim.ApplyFlightInput(-0.5f, 0.3f);  // pitch, roll (-1 to +1)

// Send direct gyroscope rotation (can exceed 360°)
flightSim.SendGyroscopeRotation(720f, 1080f, 2f);  // 2 full barrel rolls!

// Read HOTAS inputs
Vector2 joystick = flightSim.GetJoystickInput();
float throttle = flightSim.GetThrottleInput();
float pedals = flightSim.GetPedalInput();
```

#### 🚪 Escape Room Experience

**Class:** `EscapeRoomExperience`

Puzzle-based escape room experience with narrative state machine, embedded door locks, and prop sensors. Perfect for interactive puzzle experiences with physical hardware integration.

**Includes:**
- Pre-configured narrative state machine (puzzle progression)
- Embedded door lock control (unlock/lock doors via microcontroller)
- Prop sensor integration (read sensor values from embedded devices)
- Automatic door unlocking based on puzzle state
- Door state callbacks (confirm when doors actually unlock)

**Quick Start:**
```csharp
var escapeRoom = gameObject.AddComponent<EscapeRoomExperience>();
escapeRoom.InitializeExperience();

// Unlock a specific door (by index)
escapeRoom.UnlockDoor(0);  // Unlock door 0

// Lock a door
escapeRoom.LockDoor(0);

// Check if door is unlocked
bool isUnlocked = escapeRoom.IsDoorUnlocked(0);

// Trigger a prop action (e.g., activate a sensor)
escapeRoom.TriggerPropAction(0, 1.0f);  // Prop 0, value 1.0

// Read prop sensor value
float sensorValue = escapeRoom.ReadPropSensor(0);

// Get current puzzle state
string currentState = escapeRoom.GetCurrentPuzzleState();
```

**Handle State Changes:**
Override `OnExperienceStateChanged` in your derived class:
```csharp
protected override void OnExperienceStateChanged(string oldState, string newState, int newStateIndex)
{
    base.OnExperienceStateChanged(oldState, newState, newStateIndex);
    
    if (newState == "Puzzle1_Complete")
    {
        // Unlock next door, play sound, etc.
    }
}
```

---

## 🔧 Low-Level APIs (Technical Modules)

When you need full control or custom hardware configurations, use the low-level API modules:

### Core Module (`LBEAST.Core`)

**Classes:**
- `LBEASTTrackingSystem` - VR/XR tracking abstraction (OpenXR, Meta, SteamVR)
- `LBEASTNetworkManager` - LAN multiplayer with Unity NetCode for GameObjects
- `LBEASTExperienceBase` - Base class for custom experiences

**Example:**
```csharp
using LBEAST.Core;

var tracking = gameObject.AddComponent<LBEASTTrackingSystem>();
tracking.InitializeTracking();

Vector3 hmdPos = tracking.GetHMDPosition();
Quaternion hmdRot = tracking.GetHMDRotation();
bool triggerPressed = tracking.IsTriggerPressed(XRNode.RightHand);
```

### Large Haptics Module (`LBEAST.LargeHaptics`)

**Classes:**
- `HapticPlatformController` - Control 5DOF platforms (4 or 6 actuators) or 2DOF gyroscopes
- `HapticPlatformConfig` - Platform configuration
- `PlatformMotionCommand` - Motion commands
- `GyroscopeConfig` - Gyroscope and HOTAS configuration

**Example:**
```csharp
using LBEAST.LargeHaptics;

var controller = gameObject.AddComponent<HapticPlatformController>();

HapticPlatformConfig config = new HapticPlatformConfig
{
    platformType = PlatformType.MovingPlatform_SinglePlayer,
    maxPitchDegrees = 10f,
    maxRollDegrees = 10f,
    maxTranslationZ = 100f
};

controller.InitializePlatform(config);

// Send normalized motion (recommended)
controller.SendNormalizedMotion(0.5f, -0.3f, 0f, 1.5f);

// Or send absolute motion command (advanced)
PlatformMotionCommand cmd = new PlatformMotionCommand
{
    pitch = 5f,
    roll = -3f,
    translationZ = 20f,
    duration = 1.5f
};
controller.SendMotionCommand(cmd);
```

### AI Facemask Module (`LBEAST.AIFacemask`)

**Classes:**
- `FacialAnimationController` - Control facial blend shapes
- `FacialAnimationMode` - Live, Prerecorded, or Procedural
- `FacialAnimationFrame` - Animation data structure

**Example:**
```csharp
using LBEAST.AIFacemask;

var facial = GetComponentInChildren<FacialAnimationController>();
facial.Initialize();

// Set animation mode
facial.SetAnimationMode(FacialAnimationMode.Live);
facial.StartAnimation();

// Manual control
facial.SetBlendShapeWeight("smile", 80f);
facial.SetBlendShapeWeight("eyebrow_left_raise", 50f);

// Record animation
facial.StartRecording();
// ... perform animation ...
var recording = facial.StopRecording();
```

### Embedded Systems Module (`LBEAST.EmbeddedSystems`)

**Classes:**
- `SerialDeviceController` - Communicate with Arduino, ESP32, STM32, Raspberry Pi, Jetson Nano
- `MicrocontrollerType` - Device type enumeration
- `ButtonState` - Button state tracking

**Example:**
```csharp
using LBEAST.EmbeddedSystems;
using System.IO.Ports;

var device = gameObject.AddComponent<SerialDeviceController>();
device.ConnectToDevice();

// Read buttons
if (device.IsButtonPressed(0))
{
    Debug.Log("Button 0 is pressed");
}

if (device.GetButtonDown(1))
{
    Debug.Log("Button 1 was just pressed");
}

// Send haptics
device.SendHapticPulse(255, 500);  // intensity, duration(ms)

// Control LEDs
device.SetLEDColor(0, 255, 0, 0);  // Red LED

// Send custom commands
device.SendCustomCommand("PLAY:SOUND:1");
```

### Pro Audio Module (`LBEAST.ProAudio`)

**Classes:**
- `ProAudioController` - Hardware-agnostic professional audio console control via OSC
- `LBEASTOSCClient` - Lightweight OSC client for sending commands
- `LBEASTOSCServer` - Lightweight OSC server for receiving updates (bidirectional sync)
- `ProAudioConsole` - Supported console types (Behringer, Yamaha, Allen & Heath, Other, Custom)
- `ProAudioConfig` - Configuration (console type, IP, ports, channel offset, custom patterns)

**Basic Example:**
```csharp
using LBEAST.ProAudio;

var audioController = gameObject.AddComponent<ProAudioController>();

// Configure for Behringer X32
audioController.Config.ConsoleType = ProAudioConsole.BehringerX32;
audioController.Config.BoardIPAddress = "192.168.1.100";
audioController.Config.OSCPort = 10023;  // X32 default OSC port
audioController.Config.EnableReceive = true;  // Enable bidirectional sync
audioController.Config.ReceivePort = 8000;

// Initialize connection
audioController.InitializeConsole(audioController.Config);

// Control channel fader (0.0 = -inf, 1.0 = 0dB)
audioController.SetChannelFader(1, 0.75f);  // Virtual channel 1 to 75%

// Mute/unmute channel
audioController.SetChannelMute(2, true);   // Mute virtual channel 2
audioController.SetChannelMute(2, false);  // Unmute virtual channel 2

// Set bus send (e.g., reverb send)
audioController.SetChannelBusSend(1, 1, 0.5f);  // Virtual channel 1 → Bus 1 at 50%

// Control master fader
audioController.SetMasterFader(0.9f);  // Master to 90%
```

**Virtual-to-Physical Channel Mapping (For UI Toolkit Widgets):**
```csharp
// Register virtual channel 1 to map to physical hardware channel 5
bool success = audioController.RegisterChannelForSync(
    virtualChannel: 1,      // UI Toolkit widget channel number
    physicalChannel: 5     // Physical hardware channel number
);

// Now SetChannelFader(1, level) will send OSC to physical channel 5

// Unregister when removing channel from UI
audioController.UnregisterChannelForSync(1);

// Get max channels for current console type
int maxChannels = audioController.GetMaxChannelsForConsole();  // 32 for X32, 64 for Other, etc.
```

**Bidirectional Sync with Actions (For UI Toolkit Window):**
```csharp
// Subscribe to physical board updates
audioController.OnChannelFaderChanged += (virtualChannel, level) =>
{
    Debug.Log($"Physical board changed channel {virtualChannel} to {level}");
    // Update UI Toolkit slider for this channel
};

audioController.OnChannelMuteChanged += (virtualChannel, mute) =>
{
    Debug.Log($"Physical board muted channel {virtualChannel}: {mute}");
    // Update UI Toolkit toggle for this channel
};

audioController.OnMasterFaderChanged += (level) =>
{
    Debug.Log($"Physical board changed master to {level}");
    // Update UI Toolkit master slider
};
```

**Supported Consoles:**
- ✅ Behringer X32, M32, Wing
- ✅ Yamaha QL, CL, TF, DM7
- ✅ Allen & Heath SQ, dLive
- ✅ Soundcraft Si
- ✅ PreSonus StudioLive
- ✅ Other (generic 64-channel support)
- ✅ Custom (user-defined OSC path patterns with XX/YY placeholders)

**Features:**
- ✅ **Virtual-to-Physical Mapping** - Map UI channels to any hardware channel
- ✅ **Bidirectional Sync** - Physical board changes update UI, UI changes update board
- ✅ **Channel Validation** - Validates channel numbers against console-specific limits
- ✅ **Channel Offset** - Support for 0-based vs 1-based channel indexing
- ✅ **Custom OSC Patterns** - Define custom OSC paths for unsupported hardware
- ✅ **No Max for Live** - Direct OSC to console (no intermediate software)
- ✅ **Cross-Manufacturer** - Same API works with all supported boards
- ✅ **Lightweight** - Custom OSC implementation (no Asset Store dependencies)
- ✅ **UDP-Based** - Consistent with LBEAST architecture (uses existing UDP infrastructure)

**Next Steps:** See `ProAudioNextSteps.md` for UI Toolkit template implementation guide.

### Pro Lighting Module (`LBEAST.ProLighting`)

**Classes:**
- `ProLightingController` - DMX lighting control via Art-Net or USB DMX
- `ProLightingConfig` - Configuration (transport type, IP, port, universe)
- `DMXFixture` - Virtual fixture definition
- `ArtNetNode` - Discovered Art-Net node metadata

**Example:**
```csharp
using LBEAST.ProLighting;

var lightingController = gameObject.AddComponent<ProLightingController>();

// Configure for Art-Net
lightingController.Config.TransportType = DMXTransportType.ArtNet;
lightingController.Config.ArtNetIPAddress = "192.168.1.200";
lightingController.Config.ArtNetPort = 6454;  // Art-Net default port
lightingController.Config.ArtNetUniverse = 0;

lightingController.InitializeLighting(lightingController.Config);

// Register a fixture
DMXFixture fixture = new DMXFixture
{
    fixtureType = DMXFixtureType.RGBW,
    dmxAddress = 1,
    universe = 0
};
int fixtureId = lightingController.RegisterFixture(fixture);

// Control fixture intensity (0.0 to 1.0)
lightingController.SetFixtureIntensity(fixtureId, 0.75f);

// Set RGBW color
lightingController.SetFixtureColorRGBW(fixtureId, 1.0f, 0.5f, 0.0f, 0.0f);  // Orange

// Start a fade
lightingController.StartFixtureFade(fixtureId, 0.0f, 1.0f, 2.0f);  // Fade from 0 to 1 over 2 seconds
```

**Supported Transports:**
- ✅ Art-Net (UDP) - Full support with auto-discovery
- ✅ USB DMX - Stubbed (coming soon)

**Features:**
- ✅ **Fixture Registry** - Virtual fixture management by ID
- ✅ **Fade Engine** - Time-based intensity fades
- ✅ **RDM Discovery** - Automatic fixture discovery (stubbed)
- ✅ **Art-Net Discovery** - Auto-detect Art-Net nodes on network
- ✅ **Multiple Fixture Types** - Dimmable, RGB, RGBW, Moving Head, Custom

### VOIP Module (`LBEAST.VOIP`)

**Classes:**
- `VOIPManager` - Main component for managing VOIP connections
- `MumbleClient` - Mumble protocol wrapper
- `SteamAudioSourceComponent` - Per-user spatial audio source
- `VOIPConnectionState` - Connection state enumeration

**Example:**
```csharp
using LBEAST.VOIP;

var voipManager = gameObject.AddComponent<VOIPManager>();
voipManager.serverIP = "192.168.1.100";
voipManager.serverPort = 64738;  // Mumble default port
voipManager.autoConnect = true;
voipManager.playerName = "Player_1";

// Connect to Mumble server
voipManager.Connect();

// Mute/unmute microphone
voipManager.SetMicrophoneMuted(false);

// Set output volume (0.0 to 1.0)
voipManager.SetOutputVolume(0.8f);

// Listen to connection events
voipManager.OnConnectionStateChanged.AddListener((state) =>
{
    Debug.Log($"VOIP connection state: {state}");
});
```

**Features:**
- ✅ **Mumble Protocol** - Low-latency VOIP (< 50ms on LAN)
- ✅ **Steam Audio** - 3D HRTF spatialization for positional audio
- ✅ **Per-User Audio Sources** - Automatic spatialization for each remote player
- ✅ **HMD-Agnostic** - Works with any HMD's microphone and headphones
- ✅ **Unity-Friendly** - Easy integration via MonoBehaviour component

**Prerequisites:**
- Murmur server running on LAN
- Steam Audio plugin (git submodule)
- MumbleLink plugin (git submodule)

---

## 📦 Installation

LBEAST is distributed as a Unity Package, making it easy to integrate into your projects. Choose the installation method that best fits your workflow.

### **Installation Methods**

#### **Option 1: Local Package (If You Cloned This Repo)**

If you've cloned the `LBEAST_Unity` repository, the package is already set up:

1. **Open your Unity project**
2. The package will automatically appear in **Package Manager**
3. **Window > Package Manager**
4. Switch to **"In Project"** tab
5. Look for **"LBEAST - Location-Based Entertainment Activation Standard"**

> **✅ Already Done!** If you cloned the repo, LBEAST is already configured as a local package in `Packages/com.ajcampbell.lbeast/`

#### **Option 2: Git URL Installation (Recommended for Distribution)**

Install LBEAST directly from a Git repository:

1. **Open Unity Editor**
2. **Window > Package Manager**
3. Click **"+"** button (top-left)
4. Select **"Add package from git URL..."**
5. Enter the Git URL:
   ```
   https://github.com/ajcampbell1333/LBEAST_Unity.git?path=Packages/com.ajcampbell.lbeast
   ```
6. Click **"Add"**
7. Unity will download and install the package

> **📌 Note:** LBEAST is currently in **early alpha (version 0.1.0)**. Version tags are not yet available, so the Git URL above will pull the latest commit from the `main` branch. Once version tags are added, you can pin to specific versions using `#v0.1.0` syntax. LBEAST uses **Semantic Versioning** (SemVer): MAJOR.MINOR.PATCH (e.g., 0.1.0 = minor changes, 1.0.0 = major/breaking changes).

**For a specific branch:**
```
https://github.com/ajcampbell1333/LBEAST_Unity.git?path=Packages/com.ajcampbell.lbeast#main
```

**For a specific version tag (when available):**
```
https://github.com/ajcampbell1333/LBEAST_Unity.git?path=Packages/com.ajcampbell.lbeast#v0.1.0
```

**Using SSH:**
```
git@github.com:ajcampbell1333/LBEAST_Unity.git?path=Packages/com.ajcampbell.lbeast
```

#### **Option 3: Manual Package Installation**

If you have the package folder locally (e.g., downloaded ZIP or different location):

1. **Open Unity Editor**
2. **Window > Package Manager**
3. Click **"+"** button (top-left)
4. Select **"Add package from disk..."**
5. Navigate to the `package.json` file in the LBEAST package folder:
   ```
   Packages/com.ajcampbell.lbeast/package.json
   ```
6. Select `package.json`
7. Click **"Open"**

#### **Option 4: Copy Package to Your Project**

If you want to embed LBEAST directly in your project:

1. **Copy the package folder** to your project:
   ```
   Copy: Packages/com.ajcampbell.lbeast/
   To: YourProject/Packages/com.ajcampbell.lbeast/
   ```
2. **Open Unity Editor**
3. Unity will automatically detect and import the package
4. Verify in **Window > Package Manager > "In Project"**

### **Verifying Installation**

After installation, verify LBEAST is properly installed:

1. **Window > Package Manager**
2. Switch to **"In Project"** tab
3. Look for **"LBEAST - Location-Based Entertainment Activation Standard"** (version 0.1.0)
4. **Create a test script** to verify namespaces:
   ```csharp
   using LBEAST.Core;
   using LBEAST.ExperienceTemplates;
   
   // This should compile without errors
   public class LBEASTTest : MonoBehaviour
   {
       void Start()
       {
           Debug.Log("LBEAST package loaded successfully!");
       }
   }
   ```

### **Package Dependencies**

LBEAST requires several Unity packages. These will be **automatically installed** when you install LBEAST via Git URL or Package Manager. If installing manually, ensure these are installed:

| Package | Version | Purpose |
|---------|---------|---------|
| `com.unity.inputsystem` | 1.14.2+ | Modern input handling |
| `com.unity.netcode.gameobjects` | 2.6.0+ | Multiplayer networking |
| `com.unity.xr.management` | 4.4.0+ | VR/AR runtime management |
| `com.unity.xr.openxr` | 1.13.1+ | Cross-platform VR support |
| `com.unity.modules.uielements` | 1.0.0 | UI Toolkit for Command Console |

**To install dependencies manually:**
1. **Window > Package Manager**
2. Click **"+"** → **"Add package by name..."**
3. Enter package name (e.g., `com.unity.inputsystem`)
4. Click **"Add"**

### **Troubleshooting Installation**

#### **Package doesn't appear in Package Manager**
- **Solution:** Close and reopen Unity Editor
- **Solution:** Check that `package.json` exists in `Packages/com.ajcampbell.lbeast/`
- **Solution:** Verify `Packages/manifest.json` includes the package (if using Git URL)

#### **"Could not resolve package" (Git URL installation)**
- **Solution:** Verify the Git repository URL is correct and publicly accessible
- **Solution:** Check your internet connection
- **Solution:** Try using HTTPS instead of SSH or vice versa

#### **"Assembly 'LBEAST.Runtime' could not be found"**
- **Solution:** Ensure `Runtime/LBEAST.Runtime.asmdef` exists in the package
- **Solution:** Close and reopen Unity Editor to force reimport
- **Solution:** Check for compilation errors in the Console

#### **Dependencies not installing automatically**
- **Solution:** Install dependencies manually (see table above)
- **Solution:** Check `package.json` in the LBEAST package for the `dependencies` section

---

## 💻 Examples

### Example 1: Simple Moving Platform

```csharp
using UnityEngine;
using LBEAST.ExperienceTemplates;

public class SimplePlatformDemo : MonoBehaviour
{
    private MovingPlatformExperience platform;

    void Start()
    {
        platform = gameObject.AddComponent<MovingPlatformExperience>();
        platform.InitializeExperience();
    }

    void Update()
    {
        // Simulate ocean waves
        float time = Time.time;
        float tiltX = Mathf.Sin(time * 0.5f) * 0.3f;
        float tiltY = Mathf.Cos(time * 0.7f) * 0.2f;
        
        platform.SendPlatformTilt(tiltX, tiltY, 0f, 0.1f);
    }
}
```

### Example 2: Car Racing with Input

```csharp
using UnityEngine;
using LBEAST.ExperienceTemplates;

public class RacingGameController : MonoBehaviour
{
    private CarSimExperience carSim;

    void Start()
    {
        carSim = gameObject.AddComponent<CarSimExperience>();
        carSim.InitializeExperience();
    }

    void Update()
    {
        // Get steering and throttle from Input System
        float steering = Input.GetAxis("Horizontal");  // -1 to +1
        float throttle = Input.GetAxis("Vertical");    // -1 to +1

        // Send to platform
        carSim.SimulateCornerNormalized(steering, 0.2f);
        carSim.SimulateAccelerationNormalized(throttle, 0.2f);

        // Simulate road bumps randomly
        if (Random.value < 0.01f)
        {
            carSim.SimulateBump(0.5f, 0.2f);
        }
    }
}
```

### Example 3: Multiplayer Gunship

```csharp
using UnityEngine;
using LBEAST.ExperienceTemplates;

public class GunshipGameController : MonoBehaviour
{
    private GunshipExperience gunship;

    void Start()
    {
        gunship = gameObject.AddComponent<GunshipExperience>();
        gunship.InitializeExperience();
        
        // Start as host for multiplayer
        gunship.StartAsHost();
    }

    void Update()
    {
        // Simulate flight turbulence
        float turbulence = Mathf.PerlinNoise(Time.time * 0.5f, 0f) * 0.2f - 0.1f;
        gunship.SendGunshipTilt(turbulence, 0f, 0f, 0.1f);
    }

    public void OnHit()
    {
        // Shake platform when taking damage
        gunship.SendGunshipTilt(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f), 0f, 0.1f);
    }
}
```

---

## 🖥️ Dedicated Server & Server Manager

> **Terminology Note:** 
> - **"Command Console"** (operations terminology) = The UI Panel (admin interface) used by Operations Technicians to monitor and control the experience
> - **"Server Manager"** (developer terminology) = The dedicated server backend that handles network traffic, decision-making, graphics processing offloaded from VR harnesses, and other computational tasks
>
> These are **separate components** that **may** run on the same CPU/PC, or may be networked on separate machines in close proximity.

The AIFacemask experience (and optionally other multiplayer experiences) uses a **dedicated server architecture** to offload AI processing and enable robust multi-player experiences.

### **Architecture Overview**

```
┌─────────────────────────────────────────────────┐
│  LBEAST Server Manager PC (Dedicated Server)    │
│  ─────────────────────────────────────────────  │
│  • Handles all network traffic                  │
│  • Decision-making & game state logic           │
│  • Graphics processing offloaded from VR        │
│  • AI workflow (Speech → NLU → Emotion →        │
│    Audio2Face)                                  │
│  • Streams facial animation to HMDs             │
└─────────────────────────────────────────────────┘
                    │
                    ├─ UDP Broadcast ──→ LAN (auto-discovery)
                    │
        ┌───────────┴───────────┐
        │                       │
        ▼                       ▼
   ┌─────────┐            ┌─────────┐
   │  HMD 1  │            │  HMD 2  │
   │ (Client)│            │ (Client)│
   └─────────┘            └─────────┘

┌─────────────────────────────────────────────────┐
│  Command Console PC (Optional - May be same)    │
│  ─────────────────────────────────────────────  │
│  • Server Manager GUI (UI Toolkit)              │
│  • Admin Panel for Ops Tech monitoring          │
│  • Experience control interface                 │
└─────────────────────────────────────────────────┘
         │ (May share same CPU/PC as Server Manager)
         │ OR networked separately
```

### **Building the Dedicated Server**

1. **Open Unity Hub** and load the LBEAST_Unity project
2. **File > Build Settings**
3. **Select "Dedicated Server" platform** (added in Unity 2021.2+)
4. **Click "Build"** and save to `Builds/Server/`

### **Option 1: Command-Line Launch (Quick)**

Use the provided launch scripts:

**Windows:**
```batch
LaunchDedicatedServer.bat -experience AIFacemask -port 7777 -maxplayers 4
```

**Linux:**
```bash
./LaunchDedicatedServer.sh -experience AIFacemask -port 7777 -maxplayers 4
```

### **Option 2: Server Manager GUI (Recommended)**

The **Command Console** (the admin UI Panel) is a UI Toolkit-based application for managing dedicated servers with a graphical interface. This provides the operations interface to monitor and control the **Server Manager** (dedicated server backend) which handles network traffic, decision-making, and graphics processing.

#### **Setting Up the Server Manager**

1. **Create a new scene** called `ServerManager`
2. **Create an empty GameObject** named `ServerManager`
3. **Add components:**
   - `ServerManagerController`
   - `ServerManagerUI`
   - `UIDocument` (assign the ServerManager UXML file)
4. **Configure UIDocument:**
   - Create a UXML file with the UI layout (see example below)
   - Assign to UIDocument component
5. **Play** the scene

#### **Server Manager Interface**

The Unity version mirrors the Unreal UMG interface:

```
┌────────────────────────────────────────┐
│  LBEAST Server Manager                 │
├────────────────────────────────────────┤
│  Configuration:                        │
│  Experience: [AIFacemask     ▼]        │
│  Server Name: [LBEAST Server]          │
│  Max Players: [4]                      │
│  Port: [7777]                          │
│  Scene: [LBEASTScene]                  │
│                                        │
│  [Start Server]  [Stop Server]         │
├────────────────────────────────────────┤
│  Status:                               │
│  ● Running                             │
│  Players: 2/4                          │
│  State: Act1                           │
│  Uptime: 00:15:32                      │
│  PID: 12345                            │
├────────────────────────────────────────┤
│  Omniverse Audio2Face:                 │
│  Status: ○ Not Connected               │
│  Streams: 0 active                     │
│  [Configure Omniverse]                 │
├────────────────────────────────────────┤
│  Server Logs:                          │
│  [12:30:15] Server started...          │
│  [12:30:22] State changed to: Lobby    │
└────────────────────────────────────────┘
```

#### **Key Features**

- **Start/Stop Servers** - Launch and manage dedicated server processes
- **Real-Time Monitoring** - Player count, experience state, uptime
- **Omniverse Integration** - Configure Audio2Face streaming (coming soon)
- **Live Logs** - View server output in real-time
- **Multi-Experience Support** - Switch between different experience types
- **Process Management** - Automatic detection of crashed servers

### **Automatic Server Discovery**

Clients automatically discover and connect to dedicated servers on the LAN using UDP broadcast:

- **Server** broadcasts presence every 2 seconds on port `7778`
- **Clients** listen for broadcasts and auto-connect
- **No manual IP entry required** for venue deployments

See the Unreal documentation for details on the server beacon protocol.

### **Omniverse Audio2Face Integration**

The dedicated server PC should also run NVIDIA Omniverse with Audio2Face for real-time facial animation:

1. **Install** [NVIDIA Omniverse](https://www.nvidia.com/en-us/omniverse/)
2. **Install** Audio2Face from Omniverse Launcher
3. **Configure** Audio2Face to stream to your HMDs
4. **Connect** via the Server Manager Omniverse panel

> **Note:** Omniverse integration is in development. Current implementation provides the architecture and UI hooks.

### **Remote Monitoring & Off-Site Access**

The Command Console supports remote connection to Server Managers over the network, enabling off-site monitoring and control.

#### **Local Network (LAN)**

- ✅ **Auto-Discovery:** Server Beacon automatically discovers servers on the local network (UDP broadcast on port 7778)
- ✅ **Command Protocol:** Direct UDP connection on port 7779 for remote control
- ✅ **Real-Time Status:** Status updates via Server Beacon broadcasts

#### **Internet/Off-Site Access**

The Command Protocol (UDP port 7779) **can work over the internet** with proper network configuration:

**What Works:**
- ✅ Command Protocol connects directly via UDP (not broadcast)
- ✅ Can send Start/Stop commands to remote servers
- ✅ Can request status via `RequestStatus` command
- ✅ Manual IP entry supported for known server addresses

**What Doesn't Work:**
- ❌ **Auto-Discovery:** Server Beacon (UDP broadcast) is LAN-only - routers don't forward broadcasts
- ❌ **Real-Time Status:** Server Beacon status updates won't work over internet
- ⚠️ **Workaround:** Use `RequestStatus` command for periodic status polling

**Security Considerations:**
- ⚠️ **Authentication:** Enable authentication for off-site connections (shared secret in Command Console settings)
- ⚠️ **Firewall:** Must open UDP port 7779 inbound on server firewall
- ⚠️ **Production:** For public operation, use VPN or full internet isolation
- ⚠️ **Debugging Only:** Off-site monitoring recommended for debugging/testing only

**Recommended Setup for Off-Site:**
1. **VPN Connection:** Connect via VPN between Command Console and Server Manager
2. **Manual IP Entry:** Enter server IP address manually (no auto-discovery over internet)
3. **Enable Authentication:** Configure shared secret in both Command Console and Server Manager
4. **Status Polling:** Use `RequestStatus` command for periodic status updates
5. **For Production:** Use full internet isolation - off-site monitoring is for debugging only

**Network Requirements:**
- Server must have public IP or port-forwarded private IP
- Firewall must allow UDP port 7779 inbound
- NAT traversal may require port forwarding or UPnP configuration

---

## 🗺️ Roadmap

### ✅ Completed (v1.0)
- [x] Core VR tracking abstraction
- [x] 5DOF hydraulic platform support (4 & 6 actuators)
- [x] 2DOF gyroscope support
- [x] **Dedicated Server** architecture
- [x] **Server Manager GUI** (UI Toolkit-based)
- [x] **Automatic Server Discovery** (UDP broadcast)
- [x] Normalized input system (-1 to +1)
- [x] HOTAS integration framework
- [x] AI facial animation control
- [x] Embedded systems (Arduino, ESP32, STM32)
- [x] LAN multiplayer (Unity NetCode)
- [x] 5 experience templates

### 🔄 In Progress (v1.1)
- [ ] Meta Quest 3 native integration
- [ ] Unity Input System HOTAS profiles
- [ ] Sample Arduino/ESP32 firmware
- [ ] WebSocket alternative for live actor streaming

### 🎯 Planned (v2.0)
- [ ] Apple Vision Pro support
- [ ] Advanced inverse kinematics for custom actuator configs
- [ ] Visual scripting (Bolt/Visual Scripting) support
- [ ] Cloud multiplayer (Photon/Mirror)
- [ ] Prefab packages (ready-to-use scene templates)

---

## 📄 License

This project is licensed under the **MIT License** - see the [LICENSE](../LICENSE) file for details.

**TL;DR:** Free to use, modify, and distribute for personal or commercial projects. Attribution appreciated but not required.

---

## 🙏 Acknowledgments

- Built for VR arcade operators, LBE venues, and immersive experience developers
- Supports standard hardware configurations used across the industry
- Community-driven and open-source

---

## 📞 Support

- **GitHub Issues:** [https://github.com/ajcampbell1333/LBEAST_Unity/issues](https://github.com/ajcampbell1333/LBEAST_Unity/issues)
- **Documentation:** [https://github.com/ajcampbell1333/LBEAST_Unity/wiki](https://github.com/ajcampbell1333/LBEAST_Unity/wiki)

---

**Built with ❤️ for the VR arcade community**

