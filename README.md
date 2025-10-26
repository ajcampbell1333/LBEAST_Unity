# LBEAST for Unity

**Location-Based Entertainment Activation Standard (LBEAST)** - A comprehensive SDK for developing VR/AR Location-Based Entertainment experiences in Unity with support for AI facial animation, large hydraulic haptics, and embedded systems integration.

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

- [Three-Tier Architecture](#-three-tier-architecture)
- [Hardware-Agnostic Input System](#-hardware-agnostic-input-system)
- [Features](#-features)
- [Experience Templates](#experience-templates-pre-configured-solutions)
- [Low-Level APIs](#low-level-apis-technical-modules)
- [Installation](#-installation)
- [Examples](#-examples)
- [Roadmap](#-roadmap)
- [License](#-license)

---

## 🏗️ Three-Tier Architecture

LBEAST is structured in three layers, allowing developers to choose their level of control:

### Tier 1: Low-Level APIs (Technical Modules)
Foundation modules providing core functionality:
- `LBEASTCore` - VR/XR tracking abstraction, networking
- `AIFacemask` - Facial animation control
- `LargeHaptics` - Platform/gyroscope control
- `EmbeddedSystems` - Microcontroller integration

**Use these when:** Building custom experiences from scratch with full control.

### Tier 2: Experience Templates (Pre-Configured MonoBehaviours)
Ready-to-use complete experiences combining multiple APIs:
- `AIFacemaskExperience` - Live actor-driven multiplayer VR
- `MovingPlatformExperience` - Standing hydraulic platform
- `GunshipExperience` - 4-player seated platform
- `CarSimExperience` - Racing/driving simulator
- `FlightSimExperience` - Flight sim with HOTAS

**Use these when:** Rapid deployment of standard LBE configurations.

### Tier 3: Your Custom Game Logic
Build your specific experience on top of templates or APIs.

### When to Use What?

| Scenario | Use This | Why |
|----------|----------|-----|
| Building a gunship VR arcade game | `GunshipExperience` | Pre-configured for 4 players, all hardware setup included |
| Building a racing game | `CarSimExperience` | Simplified driving API, optimized motion profiles |
| Building a space combat game | `FlightSimExperience` | HOTAS integration ready, continuous rotation supported |
| Custom 3-player standing platform | Low-Level APIs | Need custom configuration not covered by templates |
| Live actor-driven escape room | `AIFacemaskExperience` | Live actor support, multiplayer, and embedded systems ready |
| Unique hardware configuration | Low-Level APIs | Full control over all actuators and systems |

**Rule of thumb:** Start with templates, drop to APIs only when you need customization beyond what templates offer.

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

---

## 📦 Installation

### Option 1: Manual Installation

1. Clone or download this repository
2. Copy the `Assets/LBEAST` folder into your Unity project's `Assets` directory
3. Install dependencies via Package Manager:
   - **Unity NetCode for GameObjects** (for multiplayer)
   - **Input System** (for HOTAS and VR controllers)
   - **XR Plugin Management** (for VR support)

### Option 2: Unity Package Manager (Git URL)

1. Open Unity Package Manager
2. Click **+ → Add package from git URL**
3. Enter: `https://github.com/ajcampbell1333/LBEAST_Unity.git`

### Dependencies

Add these packages via **Window → Package Manager**:

```
com.unity.netcode.gameobjects (Latest)
com.unity.inputsystem (Latest)
com.unity.xr.management (Latest)
```

For OpenXR support:
```
com.unity.xr.openxr (Latest)
```

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

## 🗺️ Roadmap

### ✅ Completed (v1.0)
- [x] Core VR tracking abstraction
- [x] 5DOF hydraulic platform support (4 & 6 actuators)
- [x] 2DOF gyroscope support
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

