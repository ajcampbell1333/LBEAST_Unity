# LBEAST for Unity

<img src="Packages/com.ajcampbell.lbeast/Runtime/images/lbeast-logo.png" alt="LBEAST Logo" width="100%">

**Location-Based Entertainment Activation Standard Toolkit (LBEAST)** - A comprehensive SDK for developing VR/AR Location-Based Entertainment experiences in Unity with support for AI facial animation, large hydraulic haptics, and embedded systems integration.

**Author Disclaimer**
This is a brand new plugin as of November 2025. Parts of it are not fully fleshed out. I built a lot of LBE for Fortune 10 brands in the 20-teens. This is the dream game-engine toolchain I wish we had back then, but I'm 100% certain it's full of unforeseen bugs in its current form. If you're seeing this message, it's because I have yet to deploy this plugin on a single professional project. I'm sure I will have lots of fixes to push after I've deployed it a few times in-situ. Use at your own risk in the meantime.

[![Unity](https://img.shields.io/badge/Unity-6%20LTS-black?logo=unity)](https://unity.com)
[![License](https://img.shields.io/badge/License-MIT-blue.svg)](https://opensource.org/licenses/MIT)
[![Platform](https://img.shields.io/badge/Platform-Windows-lightgrey.svg)](https://microsoft.com/windows)

> **🔗 Unreal Version:** Also available at [github.com/ajcampbell1333/lbeast_unreal](https://github.com/ajcampbell1333/lbeast_unreal)

## Philosophy

<details>
<summary><strong>Why LBEAST?</strong></summary>

<div style="margin-left: 20px;">

If a Hollywood studio invested millions in a new project, and then only showed in one theater? They'd never turn profit. We need hundreds of VR venues ready to deploy the same experience at once, and hundreds of developer teams need a way to deploy to the specs of those venues reliably so the venues and their customers can count on a continuous variety of new content. Enter LBEAST.

</div>

</details>

<details>
<summary><strong>Who is LBEAST for?</strong></summary>

<div style="margin-left: 20px;">

LBEAST is for professional teams building commercial Location-Based Entertainment installations. It's a professional-grade toolchain designed for teams of programmers and technical artists.

**Target audiences:**
- **Theme park attraction designers/engineers/production staff** - Building immersive attractions with motion platforms, embedded systems, and live actor integration
- **VR Arcade venue designers/engineers/production staff** - Deploying multiplayer VR experiences with synchronized motion and professional audio/lighting
- **Brands at trade shows** interested in wowing audiences with VR
- **3rd-party VR developers** who want to deploy new content rapidly to theme parks and VR Arcades
- **VR educators** who want to expose students to professional toolchains used in commercial LBE production

The SDK provides:
- ✅ **C# programmers** with robust APIs and extensible architecture
- ✅ **Unity developers** with drag-and-drop components and visual scripting
- ✅ **Content teams** with rapid deployment capabilities
- ✅ **Commercial projects** with free-to-use, MIT-licensed code

</div>

</details>

<details>
<summary><strong>Who is LBEAST not for?</strong></summary>

<div style="margin-left: 20px;">

Developers with little or no experience with C# may struggle to put LBEAST to its fullest use. It is meant for a scale of production that would be challenging for lone developers. However, it can be a great learning tool for educators to prepare students to work on professional team projects.

**Important notes:**
- LBEAST is **not** a no-code solution. It requires programming knowledge (C# or Unity scripting) to customize experiences beyond the provided templates.
- LBEAST is designed for **team-based production** with multiple developers, technical artists, and production staff.
- LBEAST provides prefabs, but it assumes tech art team members have access to C# programmers on the team to back them up for customization.

</div>

</details>

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
- [Experience Genre Templates](#experience-genre-templates-pre-configured-solutions)
- [Low-Level APIs](#low-level-apis-technical-modules)
- [Installation](#-installation)
- [Examples](#-examples)
- [Dedicated Server & Server Manager](#-dedicated-server--server-manager)
- [Roadmap](#-roadmap)
- [License](#-license)

---

## 📦 Prerequisites & Package Dependencies

> **📦 LBEAST is a Unity Package** - See the [Installation](#-installation) section below for detailed setup instructions.

<details>
<summary><strong>Unity Version Requirements</strong></summary>

<div style="margin-left: 20px;">

- **Unity 6 LTS (Recommended)** or Unity 2022.3 LTS (Minimum)
- **Windows 10/11** (Primary platform)
- **Linux** (Experimental support)

</div>

</details>

<details>
<summary><strong>LBEAST Package Dependencies</strong></summary>

<div style="margin-left: 20px;">

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

</div>

</details>

<details>
<summary><strong>Quick Setup (If You Cloned This Repo)</strong></summary>

<div style="margin-left: 20px;">

If you've cloned the `LBEAST_Unity` repository, everything is already configured:

1. **Open the project in Unity**
2. **LBEAST package** is already in `Packages/com.ajcampbell.lbeast/`
3. **Dependencies** will auto-install from `Packages/manifest.json`
4. **That's it!** See the [Installation](#-installation) section for more details.

> **💡 Note:** If you cloned the repo, LBEAST is already set up as a local package. All dependencies will automatically install when you first open the project.

For installation in other projects, see the [Installation](#-installation) section below.

</div>

</details>

<details>
<summary><strong>Project Settings Configuration</strong></summary>

<div style="margin-left: 20px;">

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

</div>

</details>

<details>
<summary><strong>Verification</strong></summary>

<div style="margin-left: 20px;">

To verify all dependencies are installed:

1. Open **Window > Package Manager**
2. Switch to **"In Project"** view
3. Confirm all required packages are listed

If you see compilation errors about missing namespaces:
- `Unity.Netcode` → Install NetCode for GameObjects
- `Unity.XR.OpenXR` → Install OpenXR Plugin
- `UnityEngine.InputSystem` → Install Input System

</div>

</details>

<details>
<summary><strong>Common Issues & Troubleshooting</strong></summary>

<div style="margin-left: 20px;">

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

</div>

</details>

---

## 🏗️ Three-Tier Architecture

LBEAST uses a modular three-tier architecture for code organization and server/client deployment.

### Code and Class Structure

<details>
<summary><strong>Tier 1: Low-Level APIs (Technical Modules)</strong></summary>

<div style="margin-left: 20px;">

Foundation modules providing core functionality:
- `LBEASTCore` - VR/XR tracking abstraction, networking
- `AIFacemask` - Facial animation control
- `LargeHaptics` - Platform/gyroscope control
- `EmbeddedSystems` - Microcontroller integration
- `ProAudio` - Professional audio console control via OSC
- `ProLighting` - DMX lighting control (Art-Net, USB DMX)
- `Retail` - Cashless tap card payment interface for VR tap-to-play
- `VOIP` - Low-latency voice communication with 3D HRTF spatialization

**Use these when:** Building custom experiences from scratch with full control.

</div>

</details>

<details>
<summary><strong>Tier 2: Experience Genre Templates (Pre-Configured MonoBehaviours)</strong></summary>

<div style="margin-left: 20px;">

Ready-to-use complete experiences combining multiple APIs:
- `AAIFacemaskExperience` - Live actor-driven multiplayer VR with wireless trigger buttons controlling automated AI facemask performances
- `AMovingPlatformExperience` - A 4-gang hydraulic platform on which a single VR player stands while hooked to a suspended cable harness to prevent falling
- `AGunshipExperience` - 4-player seated platform with 4DOF hydraulic motion driven by a 4-gang actuator platform with a player strapped in at each corner, all fixed to a hydraulic lift that can dangle players a few feet in the air
- `ACarSimExperience` - A racing/driving simulator where 1-4 player seats are bolted on top of a 4-gang hydraulic platform
- `AFlightSimExperience` - A single player flight sim with HOTAS controls in a 2-axis gyroscopic cockpit built with servo motors for pitch and roll. **⚠️ Requires outside-in tracking with cockpit-mounted trackers for Space Reset feature (see FlightSimExperience/README.md)** 
- `AEscapeRoomExperience` - Puzzle-based escape room with embedded door lock/prop latch solenoids, sensors, and pro AV integration for light/sound and live improv actors

**Use these when:** Rapid deployment of standard LBE genres.


</div>

</details>

<details>
<summary><strong>Tier 3: Your Custom Game Logic</strong></summary>

<div style="margin-left: 20px;">

Build your specific experience (Tier 3) on top of templates (Tier 2) or APIs (Tier 1).

</div>

</details>

<details>
<summary><strong>When to Use What?</strong></summary>

<div style="margin-left: 20px;">

| Scenario | Use This | Why |
|----------|----------|-----|
| Building a gunship VR arcade game | `GunshipExperience` | Pre-configured for 4 players, all hardware setup included |
| Building a racing game | `CarSimExperience` | Simplified driving API, optimized motion profiles |
| Building a space combat game | `FlightSimExperience` | HOTAS integration ready, continuous rotation supported |
| Custom 3-player standing platform | Low-Level APIs | Need custom configuration not covered by templates |
| Live actor-driven escape room | `AIFacemaskExperience` | Wireless trigger buttons in costume control narrative state machine, automated AI facemask performances |
| Puzzle-based escape room | `EscapeRoomExperience` | Narrative state machine, door locks, prop sensors, embedded systems |
| Unique hardware configuration | Low-Level APIs | Full control over all actuators and systems |

**Rule of thumb:** Start with templates, drop to APIs only when you need customization beyond what templates offer.

</div>

</details>

### LAN Server/Client Configuration

<details>
<summary><strong>Local Command Console</strong></summary>

<div style="margin-left: 20px;">

```
┌─────────────────────────────────────────────┐
│  Single PC (Command Console + Server)       │
│  ────────────────────────────────────────   │
│  • Command Console UI (monitoring)          │
│  • Server Manager (dedicated server)        │
│  • NVIDIA ACE Pipeline (if AIFacemask)       │
└───────────────────┬─────────────────────────┘
                    │
                    │ UDP Broadcast (port 7778)
                    │
        ┌───────────┴───────────┐
        │                       │
        ▼                       ▼
   ┌─────────┐            ┌─────────┐
   │  HMD 1  │            │  HMD 2  │
   │(Client) │            │(Client) │
   └─────────┘            └─────────┘
   ... (Player 1...N, Live Actor 1...N)
```

**Use when:** Simple setup, single machine, lightweight network traffic.

</div>

</details>

<details>
<summary><strong>Dedicated Server + Separate Local Command Console</strong></summary>

<div style="margin-left: 20px;">

```
┌─────────────────────────────────────────────┐
│  Server PC (Dedicated Server)               │
│  ────────────────────────────────────────   │
│  • Server Manager (dedicated server)        │
│  • NVIDIA ACE Pipeline (if AIFacemask)      │
└───────────────────┬─────────────────────────┘
                    │
                    │ UDP Broadcast (port 7778)
                    │
        ┌───────────┴───────────┐
        │                       │
        ▼                       ▼
   ┌─────────┐            ┌─────────┐
   │  HMD 1  │            │  HMD 2  │
   │(Client) │            │(Client) │
   └─────────┘            └─────────┘
   ... (Player 1...N, Live Actor 1...N)

┌─────────────────────────────────────────────┐
│  Console PC (Command Console)               │
│  ────────────────────────────────────────   │
│  • Command Console UI (monitoring)           │
│  • Connected via UDP (port 7779)             │
└─────────────────────────────────────────────┘
```

**Use when:** Heavy processing workloads, better performance isolation, HMD battery life optimization.

</div>

</details>

<details>
<summary><strong>Dedicated Server + Remote Command Console</strong></summary>

<div style="margin-left: 20px;">

```
┌─────────────────────────────────────────────┐
│  Server PC (Dedicated Server)               │
│  ────────────────────────────────────────   │
│  • Server Manager (dedicated server)        │
│  • NVIDIA ACE Pipeline (if AIFacemask)    │
└───────────────────┬─────────────────────────┘
                    │
                    │ UDP Broadcast (port 7778)
                    │
        ┌───────────┴───────────┐
        │                       │
        ▼                       ▼
   ┌─────────┐            ┌─────────┐
   │  HMD 1  │            │  HMD 2  │
   │(Client) │            │(Client) │
   └─────────┘            └─────────┘
   ... (Player 1...N, Live Actor 1...N)

                    │
                    │ Internet Node
                    │
                    ▼
┌─────────────────────────────────────────────┐
│  Remote Console PC (Command Console)       │
│  ────────────────────────────────────────   │
│  • Command Console UI (monitoring)          │
│  • Connected via UDP (port 7779) over       │
│    internet (VPN recommended)               │
└─────────────────────────────────────────────┘
```

**Use when:** Off-site monitoring (debugging/testing only - use VPN and authentication for security).

</div>

</details>

<details>
<summary><strong>When to Use What Configuration?</strong></summary>

<div style="margin-left: 20px;">

| Scenario | Recommended Configuration | Why |
|----------|---------------------------|-----|
| Basic single-player experience | **Local Command Console** (same PC as server) | Simple setup, no need for separate machines. Command Console launches and manages server locally. |
| Basic multiplayer with RPCs but no heavy data transferring wirelessly | **Local Command Console** (same PC as server) | Network traffic is lightweight (player positions, events). Local Command Console can manage server on same machine efficiently. |
| Lots of heavy graphics processing you want to offload from VR HMD(s) | **Dedicated + Separate Local** or **Dedicated + Remote** | Offload GPU-intensive rendering and AI processing to dedicated server PC. Better performance isolation and HMD battery life. |
| Need to monitor the experience in real-time from off-site? | **Dedicated + Remote** ⚠️ | Remote Command Console can connect over network to monitor server status, player count, experience state, and logs from a separate location. **⚠️ Recommended for debugging/testing only. For general public operation, full internet isolation is recommended for security.** Requires authentication enabled in Command Protocol settings. |

**Configuration Options:**
- **Local Command Console:** Command Console (UI Panel) and Server Manager (dedicated server) run on the same PC. Simple setup, one machine.
- **Dedicated + Separate Local:** Server Manager runs on dedicated PC, Command Console runs on separate local PC (same LAN). Networked via UDP (port 7779). Better for heavy processing workloads.
- **Dedicated + Remote:** Server Manager runs on dedicated PC, Command Console runs on remote PC (over internet). Networked via UDP (port 7779). VPN and authentication recommended.

</div>

</details>

---

## Unity Terminology

<details>
<summary><strong>Unity MonoBehaviour vs. Live Actor</strong></summary>

<div style="margin-left: 20px;">

### Unity MonoBehaviour (Game Object Component)

A **Unity MonoBehaviour** refers to a C# class that inherits from `MonoBehaviour` and exists as a component on a GameObject in the Unity scene.

```csharp
// This is a Unity MonoBehaviour
public class AIFacemaskExperience : LBEASTExperienceBase
{
    // Component code...
}
```

### Live Actor (Physical Performer)

A **Live Actor** refers to a **physical human performer** wearing VR equipment and/or costumes in the LBE installation. They drive in-game avatars with real-time facial animation and interact with players.

```csharp
// This configures support for 2 physical performers
experience.numberOfLiveActors = 2;  // Human performers wearing facemasks
experience.numberOfPlayers = 4;     // VR players
```

### Quick Reference
- **"Unity MonoBehaviour"** = C# class that exists as a component on a GameObject
- **"Live Actor"** = Physical person performing in the experience
- **"Avatar"** = The in-game character controlled by a live actor
- **"Player"** = VR participant (not performing, just experiencing)

**In this documentation:**
- When we say "MonoBehaviour" or "Component" in code context, we mean the Unity class
- When we say "Live Actor" or "live actors", we mean physical human performers
- Context should make it clear, but this distinction is important for the AI Facemask system

</div>

</details>

---


---

## 🏗️ Standard Pop-up Layout

> **Note:** The Standard Pop-up Layout is **recommended but not required**. LBEAST can be deployed in any configuration that meets your needs. This standard format is optimized for rapid pop-up deployments in public venues.

LBEAST is applicable to a large variety of venues, but it is designed in particular to enable rapid deployment of pop-up VR LBE. The SDK pairs well with a standard physical layout which, when used, gives everyone in the ecosystem confidence of rapid deployment and content refresh.

<img src="Packages/com.ajcampbell.lbeast/Runtime/images/standard-layout.png" alt="Standard Pop-up Layout" width="100%">

<details>
<summary><strong>Overview</strong></summary>

<div style="margin-left: 20px;">

LBEAST is designed for **1-to-4 player co-located VR multiplayer experiences** in publicly accessible venues such as:
- Trade shows
- Civic centers
- Shopping malls
- Theme parks
- Corporate events
- Brand activations

</div>

</details>

<details>
<summary><strong>Space Recommendations</strong></summary>
<div style="margin-left: 20px;">

- **Play Area:** 100+ square feet of open play space
- **Ceiling Height:** Sufficient clearance for players swinging long padded props (minimum 10+ feet recommended)
- **Minimum Total Space:** 50% of total space may be allocated for retail, ingress, and egress infrastructure
- **Flexible Boundaries:** Play space can be cordoned off with temporary trade-show walls or dividers around the 50% play area

</div>

</details>

<details>
<summary><strong>Minimum Square Footage</strong></summary>

<div style="margin-left: 20px;">

**Standard pop-up installation minimum square footage recommendation: ~40' × ~40'**

This includes:
- **Dual ingress/egress stations** (~12' × ~12' each) equipped with up to 4 VR HMDs apiece
- **Dual battery charging stations** ready for hot-swap after each playthrough
- **Charger stalls in staging area** near Ops Tech monitor console/server (~12' × ~12')
- **Play space** with enough room for ingress/egress door swing (~18' × ~40')
- **Lobby/Greeting area** with dual ingress/egress entry/exit (~10' × ~40')

</div>

</details>

<details>
<summary><strong>Modular Wall System</strong></summary>

<div style="margin-left: 20px;">

The standard installation uses a **modular wall facade system** for rapid setup and teardown:

#### Wall Components
- **Panel Size:** 4' × 8' lightweight composite panels (e.g. ACM)
- **Frame Height:** 10' total (8' panel + 2' footer)
- **Frame Material:** Steel framing on pairs of swivel caster legs
- **Exterior Surface:** Light-weight composite material with vinyl graphics capability
- **Connections:** QuickConnect interfaces on all sides for rapid assembly
- **Bracket Support:** Rivnuts offset from parallel QuickConnect for 90-degree bracket attachments
- **Optional detachable 2-caster side-mounts:** Consider letting footer sit on ground with rivnuts on inner footer ready to mate with a caster pair for each end to facilitate rapid redeploy of reusable parts to other stations at the same location

#### Footer System
- **Height:** 2' tall swivel caster footers
- **Exterior:** Composite surface flush with walls above and floor on exterior side
- **Interior:** Standard grid pattern enabling 80/20 aluminum furniture attachments and snap-on facia tiling

#### Facade Configuration
- **Standard Height:** 10' tall facade behind lobby desk
- **Quick Assembly:** Modular panels connect rapidly via QuickConnect system
- **Graphics Ready:** Vinyl exterior graphics can be applied to panels

</div>

</details>

<details>
<summary><strong>Ingress/Egress Rooms</strong></summary>

<div style="margin-left: 20px;">

The standard layout includes **two mirror-image ingress/egress stations**:

#### Dimensions & Layout
- **Room Size:** 12' × 12' each (3 panels wide)
- **Separation:** Two rooms separated by 4' with two parallel panels forming a closet space between them
- **Open-Air Console:** The rear of the two parallel panels may be left out to provide visibility into the play space for the Ops Tech to run the console from the closet space during playthrough.
- **AR Experience Monitoring:** If the experience is AR, the second panel may be one-way glass or a solid wall with camera monitors supporting the Ops Tech at the console.
- **Command Console:** The Ops Tech may drive the experience from a networked console usually running an Admin Panel built with either UI Toolkit in Unity or UMG in Unreal.
  > **Note:** The **"Command Console"** is the UI Panel (admin interface) used by Operations Technicians. It provides the graphical interface for monitoring and controlling the experience. The **"Server Manager"** is the dedicated server backend that handles all network traffic, decision-making, graphics processing offloaded from VR harnesses, and other heavy computational tasks. The Command Console (UI) may run on the same CPU/PC as the Server Manager (dedicated server), or they may be separate but networked in close proximity.
- **Flow:** Front-of-House (FOH) Retail clerk directs up to four players in alternating fashion to the next available ingress station (left or right)

#### Features per Room
- **Swing Wall:** One panel-footer pair may include a built-in hinge to enable the entire rear wall to swing open, revealing the play area after players don VR headsets
- **Harness Storage:** Wall with four hooks to stow VR harnesses between uses
- **Charging Cabinet:** 80/20 aluminum framing cabinet for rapid battery recharge cycling
- **Capacity:** Up to four VR harnesses per room (eight total across both rooms)
- **Chargers:** Four chargers per room (eight total)

</div>

</details>

<details>
<summary><strong>Staffing Requirements</strong></summary>

<div style="margin-left: 20px;">

**Minimum Staff:** Two employees during operation hours

1. **Front-of-House (FOH) Retail Clerk**
   - Operates lobby desk
   - Point-of-sale station (tablet or computer)
   - Directs players to ingress stations
   - Handles transactions and customer service

2. **Operations Technician (Ops Tech)**
   - Assists with player ingress/egress
   - Manages VR harness distribution and collection
   - Performs battery swaps
   - Monitors experience operations

**Optional Staff:**
- **Immersive Actors:** Join players in the experience to enhance immersion
- Additional support staff as needed for high-traffic venues

</div>

</details>

<details>
<summary><strong>VR Harness & Power Specifications</strong></summary>

<div style="margin-left: 20px;">

#### Battery System
- **Type:** Hot-swap LiFePO4 6S5P 21700 battery packs
- **Drain Rate:** ~5% per playthrough
- **Swap Protocol:** Ops Tech swaps batteries after each playthrough to ensure harnesses are always near 100% State of Charge (SOC)
- **Total Harnesses:** 8 harnesses (4 per ingress/egress room)

#### Power Requirements
- **Continuous Draw:** 250W-500W per harness
- **Drain-to-Charge Ratio:** 1:4 (always reaching near 100% SOC before reuse)
- **Charging Specifications:**
  - **250W Harnesses:** 5A chargers
  - **500W Harnesses:** 10A chargers

#### Power Management
- All batteries reach near 100% SOC before reuse
- Continuous operation enabled by hot-swap system
- No reserve battery mode needed due to swap protocol

</div>

</details>

<details>
<summary><strong>Lobby & Retail Area</strong></summary>

<div style="margin-left: 20px;">

- **Lobby Desk:** Point-of-sale station with tablet or computer
- **Facade:** 10' tall modular wall facade behind lobby desk
- **Graphics:** Vinyl exterior graphics on facade panels
- **Flow:** Customers enter lobby → FOH directs to ingress → Ops Tech assists with setup → Play → Egress → Return to lobby

</div>

</details>

<details>
<summary><strong>Rapid Deployment Benefits</strong></summary>

<div style="margin-left: 20px;">

This standard format enables:
- **Fast Setup:** Modular components assemble quickly via QuickConnect system
- **Easy Teardown:** Disassembles rapidly for venue transitions
- **Consistent Operations:** Standardized layout and procedures across venues
- **Professional Appearance:** Clean, branded facade with custom graphics
- **Operational Efficiency:** Streamlined player flow and battery management

</div>

</details>

<details>
<summary><strong>LBEAST-Ready Venue Configuration</strong></summary>

<div style="margin-left: 20px;">

To be considered **LBEAST-ready**, a venue would aim to have at least a handful of 40' × 40' stations:

- **100' × 100' play space** subdivided into 4 play stations is perfect for variety
- **One play space each** dedicated to each unique hardware genre:
  - One gunship space
  - One AI narrative space
  - One escape room space
  - One car and flight sim arcade

**The Theater Analogy:**
Just like movie theaters where multiple screens offer variety, VR play spaces can function similarly. Variety creates demand:
- **Customer** arrives knowing a variety of new content choice is always on-site
- **Developer** knows their experience is supported by on-site hardware
- **Venue** knows many developers are in-progress on new content
- **Result:** A healthy, thriving market

</div>

</details>

<details>
<summary><strong>Safety Considerations</strong></summary>

<div style="margin-left: 20px;">

- **QTY2 Up-to-code Fire Emergency Fire Extinguishers:** One at the Ops Tech Console and another near any hydraulic equipment.
- **Movable stairs:** Any system that causes players to be lifted into the air must have a physical means of egress in an e-stop emergency.
- **Hydraulically-actuated equipment should have multiple manual and auto e-stops** located at console and on device.
- **Theme park safety regulations vary by state** - take steps to abide by the same rules that apply to carnival equipment in your state.
- **The author of LBEAST disclaims any liability resulting in the use of this free software.**

</div>

</details>

<details>
<summary><strong>Recommended HMD Hardware Example</strong></summary>

<div style="margin-left: 20px;">

For standard LBEAST installations, the following hardware configuration provides optimal performance and reliability:

#### VR Headset
- **Model:** Meta Quest 3 (512GB, standalone VR/MR)
- **Price Range:** $450–$500 per unit (2025 pricing)
- **Features:** Standalone VR/MR capability, OpenXR-compatible, includes controllers
- **Note:** Supports both standalone and PC-connected modes for maximum flexibility

#### Backpack PC (VR Harness Compute Unit)
- **Model:** ASUS ROG Zephyrus G16 GU605 (2025 edition)
- **CPU:** Intel Core Ultra 9
- **GPU:** NVIDIA RTX 5080 (or RTX 5070 Ti for cost optimization)
- **RAM:** 32GB
- **Storage:** 2TB SSD
- **Price Range:** $2,800–$3,200 per unit
- **Form Factor:** Gaming laptop (backpack-compatible)
- **Use Case:** Powers VR headset for high-end rendering, offloads graphics processing from HMD battery

#### Safety Harness
- **Model:** Petzl EasyFit harness (full-body fall arrest, size 1–2)
- **Price Range:** $300–$350 per unit
- **Features:** Newton EasyFit model; padded, quick-donning for adventure/ride use
- **Use Case:** Full-body fall arrest protection for players on motion platforms and elevated play spaces
- **Availability:** REI/Amazon pricing

#### Integration & Assembly
- **System Integration:** The backpack PC, HMD, and EasyFit harness are all connected together as an integrated VR harness system
- **Connection Method:** Custom straps and 3D-printed interfaces secure all components together
- **Assembly:** Backpack PC mounts to harness via 3D-printed brackets; HMD connects to backpack via cable; harness provides structural support and safety attachment points
- **Result:** Single unified system that players don and doff as one unit, streamlining ingress/egress operations
- **Ingress/Egress Support:** Each ingress/egress station contains four carabiner hooks mounted to the wall, allowing the entire integrated rig to be suspended during donning/doffing. This enables players to unstrap and egress rapidly without dropping or damaging equipment, while keeping the rig ready for the next player

**Why This Configuration?**
- **High Performance:** RTX 5080/5070 Ti provides sufficient power for complex VR experiences
- **Battery Efficiency:** Offloading graphics processing extends HMD battery life
- **Flexibility:** Laptop form factor enables backpack mounting or stationary use
- **Future-Proof:** High-end specs support demanding experiences and future content updates

**Alternative Configurations:**
- For lighter experiences: RTX 5070 Ti configuration (~$2,800) provides cost savings
- For maximum performance: RTX 5080 configuration (~$3,200) enables highest-quality rendering
- Bulk purchasing (10+ units) typically provides ~5% discount

</div>

</details>

---

## ✨ Features

### Experience Genre Templates (Drag-and-Drop Solutions)

Experience Genre Templates are complete, pre-configured MonoBehaviours that you can add to your scene and use immediately. Each combines multiple low-level APIs into a cohesive, tested solution.

<details>
<summary><strong>🎭 AI Facemask Experience</strong></summary>

<div style="margin-left: 20px;">

**Class:** `AIFacemaskExperience`

Deploy LAN multiplayer VR experiences where immersive theater live actors drive avatars with **fully automated AI-generated facial expressions**. The AI face is controlled entirely by NVIDIA ACE pipeline - no manual animation, rigging, or blend shape tools required.

**⚠️ DEDICATED SERVER REQUIRED ⚠️**

This template **enforces** dedicated server mode. You **must** run a separate local PC as a headless dedicated server. This is **not optional** - the experience will fail to initialize if ServerMode is changed to Listen Server.

**Network Architecture:**
```
┌─────────────────────────────────────┐
│   Dedicated Server PC (Headless)    │
│                                     │
│  ┌───────────────────────────────┐  │
│  │  Unity Dedicated Server       │  │ ← Multiplayer networking
│  │  (No HMD, no rendering)       │  │
│  └───────────────────────────────┘  │
│                                     │
│  ┌───────────────────────────────┐  │
│  │  NVIDIA ACE Pipeline           │  │ ← AI Workflow:
│  │  - Speech Recognition         │  │   Audio → NLU → Emotion
│  │  - NLU (Natural Language)     │  │              ↓
│  │  - Emotion Detection          │  │   Facial Animation
│  │  - Facial Animation Gen       │  │   (Textures + Blend Shapes)
│  └───────────────────────────────┘  │              ↓
└─────────────────────────────────────┘   Stream to HMDs
               │
        LAN Network (UDP/TCP)
               │
        ┌──────┴────────┐
        │               │
   VR HMD #1      VR HMD #2      (Live Actors)
   VR HMD #3      VR HMD #4      (Players)
```

**AI Facial Animation (Fully Automated):**
- **NVIDIA ACE Pipeline**: Generates facial textures and blend shapes automatically
- **No Manual Control**: Live actors never manually animate facial expressions
- **No Rigging Required**: NVIDIA ACE handles all facial animation generation
- **Real-Time Application**: AIFaceController receives NVIDIA ACE output and applies to mesh
- **Mask-Like Tracking**: AIFace mesh is tracked on top of live actor's face in HMD
- **Context-Aware**: Facial expressions determined by audio, NLU, emotion, and narrative state machine
- **Automated Performances**: Each narrative state triggers fully automated AI facemask performances

**Live Actor Control (High-Level Flow Only):**
- **Wireless Trigger Buttons**: Embedded in live actor's costume/clothes (ESP32, WiFi-connected)
- **Narrative State Control**: Buttons advance/retreat the narrative state machine (Intro → Act1 → Act2 → Finale)
- **Automated Performance Triggers**: State changes trigger automated AI facemask performances - live actor controls when, not how
- **Experience Direction**: Live actor guides players through story beats by controlling narrative flow

**Why Dedicated Server?**
- **Performance**: Offloads heavy AI processing from VR HMDs
- **Parallelization**: Supports multiple live actors simultaneously
- **Reliability**: Isolated AI workflow prevents HMD performance degradation
- **Scalability**: Easy to add more live actors or players

**Automatic Server Discovery:**

LBEAST includes a **zero-configuration UDP broadcast system** for automatic server discovery:
- **Server**: Broadcasts presence every 2 seconds on port `7778`
- **Clients**: Automatically discover and connect to available servers
- **No Manual IP Entry**: Perfect for LBE installations where tech setup should be invisible
- **Multi-Experience Support**: Discover multiple concurrent experiences on the same LAN
- **Server Metadata**: Includes experience type, player count, version, current state

When a client HMD boots up, it automatically finds the dedicated server and connects - zero configuration required!

**Complete System Flow:**

The AI Facemask system supports two workflows: **pre-baked scripts** (narrative-driven) and **real-time improv** (player interaction-driven).

**Pre-Baked Script Flow (Narrative-Driven):**
```
Live Actor presses wireless trigger button (embedded in costume)
    ↓
Narrative State Machine advances/retreats (Intro → Act1 → Act2 → Finale)
    ↓
ACE Script Manager triggers pre-baked script for new state
    ↓
NVIDIA ACE Server streams pre-baked facial animation (from cached TTS + Audio2Face)
    ↓
AIFaceController receives facial animation data (blend shapes + textures)
    ↓
Facial animation displayed on live actor's HMD-mounted mesh
```

**Real-Time Improv Flow (Player Interaction-Driven):**
```
Player speaks into HMD microphone
    ↓
VOIPManager captures audio → Sends to Mumble server
    ↓
Dedicated Server receives audio via Mumble
    ↓
ACE ASR Manager (visitor pattern) receives audio → Converts speech to text (NVIDIA Riva ASR)
    ↓
ACE Improv Manager receives text → Local LLM (with LoRA) generates improvised response
    ↓
Local TTS (NVIDIA Riva) converts text → audio
    ↓
Local Audio2Face (NVIDIA NIM) converts audio → facial animation
    ↓
Facial animation streamed to AIFaceController
    ↓
Facial animation displayed on live actor's HMD-mounted mesh
```

**Component Architecture:**
```
┌─────────────────────────────────────────────────────────────────┐
│  PLAYER HMD (Client)                                            │
│  ────────────────────────────────────────────────────────────   │
│  1. Player speaks into HMD microphone                           │
│  2. VOIPManager captures audio                                  │
│  3. Audio sent to Mumble server (Opus encoded)                  │
└───────────────────────┬─────────────────────────────────────────┘
                        │
                        ▼
┌─────────────────────────────────────────────────────────────────┐
│  MUMBLE SERVER (LAN)                                            │
│  ────────────────────────────────────────────────────────────   │
│  Routes audio to dedicated server                               │
└───────────────────────┬─────────────────────────────────────────┘
                        │
                        ▼
┌─────────────────────────────────────────────────────────────────┐
│  DEDICATED SERVER PC (Unity Server)                             │
│  ────────────────────────────────────────────────────────────   │
│                                                                 │
│  ┌──────────────────────────────────────────────────────────┐   │
│  │  ACE ASR Manager (VOIP-to-AIFacemask Visitor Pattern)    │   │
│  │  - Receives audio from Mumble                            │   │
│  │  - Buffers audio (voice activity detection)              │   │
│  │  - Converts speech → text (NVIDIA Riva ASR)              │   │
│  │  - Triggers Improv Manager with text                     │   │
│  └───────────────────────┬──────────────────────────────────┘   │
│                          │                                      │
│                          ▼                                      │
│  ┌──────────────────────────────────────────────────────────┐   │
│  │  ACE Improv Manager                                      │   │
│  │  - Receives text from ASR Manager                        │   │
│  │  - Local LLM (Ollama/vLLM/NIM + LoRA) → Improvised text  │   │
│  │  - Local TTS (NVIDIA Riva) → Audio file                  │   │
│  │  - Local Audio2Face (NVIDIA NIM) → Facial animation      │   │
│  └───────────────────────┬──────────────────────────────────┘   │
│                          │                                      │
│                          ▼                                      │
│  ┌──────────────────────────────────────────────────────────┐   │
│  │  ACE Script Manager                                      │   │
│  │  - Manages pre-baked scripts                             │   │
│  │  - Triggers scripts on narrative state changes           │   │
│  │  - Pre-bakes scripts (TTS + Audio2Face) on ACE server    │   │
│  └──────────────────────────────────────────────────────────┘   │
└───────────────────────┬─────────────────────────────────────────┘
                        │
                        │ Facial Animation Data (Blend Shapes + Textures)
                        │
                        ▼
┌─────────────────────────────────────────────────────────────────┐
│  LIVE ACTOR HMD (Client)                                        │
│  ────────────────────────────────────────────────────────────   │
│  ┌──────────────────────────────────────────────────────────┐   │
│  │  AIFaceController                                        │   │
│  │  - Receives facial animation data from server            │   │
│  │  - Applies blend shapes/textures to mesh                 │   │
│  │  - Real-time facial animation display                    │   │
│  └──────────────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────────────┘
```

**Architecture:**
- **AI Face**: Fully autonomous, driven by NVIDIA ACE pipeline (Audio → NLU → Emotion → Facial Animation)
- **Live Actor Role**: High-level experience director via wireless trigger buttons, NOT facial puppeteer
- **Wireless Controls**: Embedded trigger buttons in live actor's costume/clothes (4 buttons total)
- **Narrative State Machine**: Live actor advances/retreats through story beats (Intro → Tutorial → Act1 → Act2 → Finale → Credits)
- **Automated Performances**: AI facemask performances are fully automated - live actor controls flow, not expressions
- **Server Mode**: **ENFORCED** to Dedicated Server (attempting to change will fail initialization)

**Live Actor Control System:**
- **Wireless Trigger Buttons**: Embedded in live actor's costume/clothes (ESP32-based, WiFi-connected)
- **High-Level Flow Control**: Buttons advance/retreat the narrative state machine, which triggers automated AI facemask performances
- **No Facial Control**: Live actor never manually controls facial expressions - NVIDIA ACE handles all facial animation
- **Experience Direction**: Live actor guides players through story beats by advancing/retreating narrative states

**Includes:**
- Pre-configured `AIFaceController` (receives NVIDIA ACE output, applies to mesh)
- Pre-configured `SerialDeviceController` (wireless trigger buttons embedded in costume)
- Pre-configured `ExperienceStateMachine` (narrative story progression)
- Pre-configured `AIFacemaskACEScriptManager` (pre-baked script collections)
- Pre-configured `AIFacemaskACEImprovManager` (real-time improvised responses)
- Pre-configured `AIFacemaskASRManager` (speech-to-text for player voice)
- LAN multiplayer support (configurable live actor/player counts)
- Passthrough mode for live actors to help players

**Button Layout (Embedded in Costume):**
- **Left Wrist/Clothing**: Button 0 (Advance narrative), Button 1 (Retreat narrative)
- **Right Wrist/Clothing**: Button 2 (Advance narrative), Button 3 (Retreat narrative)

**Quick Start:**
```csharp
// In your scene
var experience = gameObject.AddComponent<AIFacemaskExperience>();
experience.numberOfLiveActors = 1;
experience.numberOfPlayers = 4;
experience.liveActorMesh = myCharacterMesh;

// ServerMode is already set to DedicatedServer by default
// DO NOT CHANGE IT - initialization will fail if you do

experience.InitializeExperience();  // Will validate server mode

// Live actor controls high-level flow via wireless trigger buttons embedded in costume
// Buttons advance/retreat narrative state machine, which triggers automated AI facemask performances
// Facial expressions are fully automated by NVIDIA ACE - no manual control needed

// React to experience state changes (triggered by live actor's buttons)
string currentState = experience.GetCurrentExperienceState();

// Programmatically trigger state changes (usually handled by wireless buttons automatically)
experience.AdvanceExperience();  // Advance narrative state
experience.RetreatExperience();  // Retreat narrative state
```

**❌ What Happens If You Try to Use Listen Server:**
```
========================================
⚠️  SERVER MODE CONFIGURATION ERROR ⚠️
========================================
This experience REQUIRES ServerMode to be set to 'DedicatedServer'
Current ServerMode is set to 'ListenServer'

Please change ServerMode in the Inspector to 'DedicatedServer'
========================================
```

**Handle State Changes:**
Override `OnNarrativeStateChanged` to trigger game events when live actor advances/retreats narrative state via wireless trigger buttons:
```csharp
protected override void OnNarrativeStateChanged(string oldState, string newState, int newStateIndex)
{
    // State changes are triggered by live actor's wireless trigger buttons
    // Each state change triggers automated AI facemask performances
    if (newState == "Act1")
    {
        // Spawn enemies, trigger cutscene, etc.
        // NVIDIA ACE will automatically generate facial expressions for this state
    }
}
```

</div>

</details>

<details>
<summary><strong>🎢 Moving Platform Experience</strong></summary>

<div style="margin-left: 20px;">

**Class:** `MovingPlatformExperience`

Single-player standing VR experience on an unstable hydraulic platform with safety harness. Provides pitch, roll, and Y/Z translation for immersive motion.

**Includes:**
- Pre-configured 4DOF hydraulic platform (4 actuators + scissor lift)
- 10° pitch and roll capability
- Vertical translation for rumble/earthquake effects
- Emergency stop and return-to-neutral functions
- Blueprint-friendly motion commands

**Quick Start:**
```csharp
var platform = gameObject.AddComponent<MovingPlatformExperience>();
platform.maxPitch = 10.0f;
platform.maxRoll = 10.0f;
platform.InitializeExperience();

// Send normalized tilt (RECOMMENDED - hardware-agnostic)
// -1.0 to +1.0 automatically scales to hardware capabilities
platform.SendPlatformTilt(0.3f, -0.5f, 0.0f, 2.0f);  // TiltX (right), TiltY (backward), Vertical, Duration

// Advanced: Use absolute angles if you need precise control
platform.SendPlatformMotion(5.0f, -3.0f, 20.0f, 2.0f);  // pitch, roll, vertical, duration
```

</div>

</details>

<details>
<summary><strong>🚁 Gunship Experience</strong></summary>

<div style="margin-left: 20px;">

**Class:** `GunshipExperience`

Four-player VR experience where each player is strapped to the corner of a hydraulic platform capable of 4DOF motion (pitch/roll/forward/reverse/lift-up/liftdown). Perfect for multiplayer gunship, helicopter, spaceship, or multi-crew vehicle simulations.

**Includes:**
- Pre-configured 4DOF hydraulic platform (6 actuators + scissor lift)
- 4 pre-defined seat positions
- LAN multiplayer support (4 players)
- Synchronized motion for all passengers
- Emergency stop and safety functions

**Quick Start:**
```csharp
var gunship = gameObject.AddComponent<GunshipExperience>();
gunship.InitializeExperience();

// Send normalized motion (RECOMMENDED - hardware-agnostic)
// Values from -1.0 to +1.0 automatically scale to hardware capabilities
gunship.SendGunshipTilt(0.5f, 0.8f, 0.2f, 0.1f, 1.5f);  // TiltX (roll), TiltY (pitch), ForwardOffset, VerticalOffset, Duration

// Advanced: Use absolute angles if you need precise control
gunship.SendGunshipMotion(8.0f, 5.0f, 10.0f, 15.0f, 1.5f);  // pitch, roll, forwardOffset (cm), verticalOffset (cm), duration
```

</div>

</details>

<details>
<summary><strong>🏎️ Car Sim Experience</strong></summary>

<div style="margin-left: 20px;">

**Class:** `CarSimExperience`

Single-player seated racing/driving simulator on a hydraulic platform. Perfect for arcade racing games and driving experiences.

**Includes:**
- Pre-configured 4DOF hydraulic platform optimized for driving
- Motion profiles for cornering, acceleration, and bumps
- Compatible with racing wheels and pedals (via Unity Input System)
- Simplified API for driving simulation

**Quick Start:**
```csharp
var carSim = gameObject.AddComponent<CarSimExperience>();
carSim.InitializeExperience();

// Use normalized driving API (RECOMMENDED - hardware-agnostic)
carSim.SimulateCornerNormalized(-0.8f, 0.5f);      // Left turn (normalized -1 to +1)
carSim.SimulateAccelerationNormalized(0.5f, 0.5f); // Accelerate (normalized -1 to +1)
carSim.SimulateBump(0.8f, 0.2f);                   // Road bump (intensity 0-1)

// Advanced: Use absolute angles if you need precise control
carSim.SimulateCorner(-8.0f, 0.5f);         // Left turn (degrees)
carSim.SimulateAcceleration(5.0f, 0.5f);    // Accelerate (degrees)
```

</div>

</details>

<details>
<summary><strong>✈️ Flight Sim Experience</strong></summary>

<div style="margin-left: 20px;">

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
flightSim.hotasType = HOTASType.LogitechX56;
flightSim.enableJoystick = true;
flightSim.enableThrottle = true;
flightSim.InitializeExperience();

// Read HOTAS input in Update
Vector2 joystick = flightSim.GetJoystickInput();  // X=roll, Y=pitch
float throttle = flightSim.GetThrottleInput();

// Send continuous rotation command (can exceed 360°)
flightSim.SendContinuousRotation(720.0f, 360.0f, 4.0f);  // Two barrel rolls!
```

</div>

</details>

<details>
<summary><strong>🚪 Escape Room Experience</strong></summary>

<div style="margin-left: 20px;">

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
Override `OnNarrativeStateChanged` in your derived class:
```csharp
protected override void OnNarrativeStateChanged(string oldState, string newState, int newStateIndex)
{
    if (newState == "Puzzle1_Complete")
    {
        // Unlock next door, play sound, etc.
    }
}
```

</div>

</details>

---

## 🔧 Low-Level APIs (Technical Modules)

When you need full control or custom hardware configurations, use the low-level API modules:

<details>
<summary><strong>Core Module (`LBEAST.Core`)</strong></summary>

<div style="margin-left: 20px;">

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

</div>

</details>

<details>
<summary><strong>🎛️ LargeHaptics API</strong></summary>

<div style="margin-left: 20px;">

**Module:** `LBEAST.LargeHaptics`

Manual control of individual hydraulic actuators, gyroscopes, and scissor lift translation.

<details>
<summary><strong>🎮 Hardware-Agnostic Input System - Normalized Tilt Control (-1 to +1)</strong></summary>

<div style="margin-left: 20px;">

LBEAST uses a **joystick-style normalized input system** for all 4DOF hydraulic platforms. This means you write your game code once, and it works on any hardware configuration:

**Why Normalized Inputs?**
- ✅ **Hardware Independence:** Same game code works on platforms with 5° tilt or 15° tilt
- ✅ **Venue Flexibility:** Operators can upgrade/downgrade hardware without code changes
- ✅ **Intuitive API:** Think like a joystick: -1.0 (full left/back), 0.0 (center), +1.0 (full right/forward)
- ✅ **Automatic Scaling:** SDK maps your inputs to actual hardware capabilities

**Example:**
```csharp
// Your game sends: "tilt 50% right, 80% forward"
platform.SendPlatformTilt(0.5f, 0.8f, 0.0f, 1.0f);

// On 5° max platform: Translates to Roll=2.5°, Pitch=4.0°
// On 15° max platform: Translates to Roll=7.5°, Pitch=12.0°
// Same code, automatically scaled!
```

**Axis Mapping:**
- **TiltX:** Left/Right roll (-1.0 = full left, +1.0 = full right)
- **TiltY:** Forward/Backward pitch (-1.0 = full backward, +1.0 = full forward)
- **ForwardOffset:** Scissor lift forward/reverse (-1.0 = full reverse, +1.0 = full forward, 0.0 = neutral)
- **VerticalOffset:** Scissor lift up/down (-1.0 = full down, +1.0 = full up, 0.0 = neutral)

**Advanced Users:** If you need precise control and know your hardware specs, angle-based APIs are available in the `Advanced` category.

</div>

</details>

**4DOF Platform Example:**
```csharp
using LBEAST.LargeHaptics;

var controller = gameObject.AddComponent<PlatformController4DOF>();

HapticPlatformConfig config = new HapticPlatformConfig
{
    platformType = PlatformType.MovingPlatform_SinglePlayer,
    maxPitchDegrees = 10f,
    maxRollDegrees = 10f,
    maxTranslationY = 100f,  // Scissor lift forward/reverse
    maxTranslationZ = 100f   // Scissor lift up/down
};

controller.InitializePlatform(config);

// Send normalized motion (recommended)
controller.SendNormalizedMotion(0.5f, -0.3f, 0.2f, 1.5f);  // TiltX, TiltY, VerticalOffset, Duration

// Or send absolute motion command (advanced)
PlatformMotionCommand cmd = new PlatformMotionCommand
{
    pitch = 5f,
    roll = -3f,
    translationY = 20f,  // Scissor lift forward/reverse (cm)
    translationZ = 15f,  // Scissor lift up/down (cm)
    duration = 1.5f
};
controller.SendMotionCommand(cmd);
```

**2DOF Flight Sim with HOTAS Example:**
```csharp
var flightSimController = gameObject.AddComponent<GyroscopeController2DOF>();

// Configure gyroscope settings
flightSimController.SetMaxRotationSpeed(90.0f);  // degrees per second
flightSimController.SetJoystickSensitivity(1.5f);
flightSimController.SetEnableHOTAS(true);

// Initialize connection to ECU
flightSimController.Initialize();

// Read HOTAS input
Vector2 joystickInput = flightSimController.GetHOTASJoystickInput();  // X = roll, Y = pitch
float throttleInput = flightSimController.GetHOTASThrottleInput();
float pedalInput = flightSimController.GetHOTASPedalInput();

// Send gyroscope state (automatically sent in Update() based on HOTAS input)
// Or send custom gyro state:
GyroState gyroState = new GyroState
{
    pitch = 720.0f,  // Two full rotations
    roll = 360.0f    // One full roll
};
flightSimController.SendGyroStruct(gyroState, 102);
```

</div>

</details>

<details>
<summary><strong>🤖 AIFace API</strong></summary>

<div style="margin-left: 20px;">

**Module:** `LBEAST.AIFacemask`

Receive and apply NVIDIA ACE facial animation output to a live actor's HMD-mounted mesh.

**Important:** This is a receiver/display system - facial animation is fully automated by NVIDIA ACE. No manual control, keyframe animation, or rigging required.

```csharp
using LBEAST.AIFacemask;

var faceController = gameObject.AddComponent<AIFaceController>();

AIFaceConfig config = new AIFaceConfig
{
    targetMesh = liveActorMesh,  // Mesh attached to live actor's HMD/head
    nvidiaACEEndpointURL = "http://localhost:8080/ace",  // NVIDIA ACE endpoint
    updateRate = 30.0f  // Receive updates at 30 Hz
};

faceController.InitializeAIFace(config);

// NVIDIA ACE will automatically stream facial animation data
// Component receives and applies it via ReceiveFacialAnimationData()
```

</div>

</details>

<details>
<summary><strong>🔌 Embedded Systems Module (`LBEAST.EmbeddedSystems`)</strong></summary>

<div style="margin-left: 20px;">

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

</div>

</details>

<details>
<summary><strong>🎚️ Pro Audio Module (`LBEAST.ProAudio`)</strong></summary>

<div style="margin-left: 20px;">

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

</div>

</details>

<details>
<summary><strong>💡 Pro Lighting Module (`LBEAST.ProLighting`)</strong></summary>

<div style="margin-left: 20px;">

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

</div>

</details>

<details>
<summary><strong>💳 Retail API</strong></summary>

<div style="margin-left: 20px;">

**Module:** `LBEAST.Retail`

Cashless tap card payment interface for VR tap-to-play capability. Supports multiple payment providers and provides in-process HTTP webhook server for receiving payment confirmations.

**Use Case:** Setting up self-assist VR play stations with tap-card or tap-wristband token payment provider kiosks? LBEAST provides integration with five different tap-card providers.

**Supported Providers:**
- ✅ Embed
- ✅ Nayax
- ✅ Intercard
- ✅ Core Cashless
- ✅ Cantaloupe

**Example:**
```csharp
using LBEAST.Retail;

var paymentManager = gameObject.AddComponent<ArcadePaymentManager>();

// Configure payment provider
paymentManager.config.provider = PaymentProvider.Embed;
paymentManager.config.apiKey = "your-api-key";
paymentManager.config.baseUrl = "https://api.embed.com";
paymentManager.config.cardId = "player-card-id";

// Check card balance (async coroutine)
StartCoroutine(paymentManager.CheckBalance(paymentManager.config.cardId, (balance) =>
{
    Debug.Log($"Card balance: ${balance:F2}");
}));

// Allocate tokens for gameplay (async coroutine)
StartCoroutine(paymentManager.AllocateTokens("station-1", 10.0f, (success) =>
{
    if (success)
    {
        Debug.Log("Tokens allocated successfully");
    }
}));
```

**Webhook Server:**
The payment manager automatically starts an in-process HTTP webhook server on port 8080 (configurable) to receive payment confirmations from the payment provider. When a player taps their card, the provider sends a POST request to the webhook endpoint, which triggers `OnPaymentConfirmed()` automatically and can be wired to start a VR session.

**Features:**
- ✅ **In-Process Webhook Server** - Runs in the same executable as the VR HMD using `HttpListener` (no separate server process)
- ✅ **Multi-Provider Support** - Provider-specific API endpoints and webhook paths
- ✅ **Async Coroutine API** - Balance checking and token allocation with callback support
- ✅ **Automatic Payment Processing** - Webhook triggers payment confirmation callback on successful payment
- ✅ **Unity-Friendly** - Easy integration via MonoBehaviour component

</div>

</details>

<details>
<summary><strong>🎤 VOIP API</strong></summary>

<div style="margin-left: 20px;">

**Module:** `LBEAST.VOIP`

Low-latency voice communication with 3D HRTF spatialization using Mumble protocol and Steam Audio.

**Basic Example:**
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

**Custom Audio Processing (Visitor Pattern):**

If your experience genre template needs to process player voice (speech recognition, voice commands, audio analysis, etc.), use the **visitor interface pattern** to subscribe to audio events without coupling your module to VOIP:

```csharp
// 1. Create a component that implements IVOIPAudioVisitor
public class MyAudioProcessor : MonoBehaviour, IVOIPAudioVisitor
{
    public void OnPlayerAudioReceived(int playerId, float[] audioData, int sampleRate, Vector3 position)
    {
        // Process audio for your custom use case
        // AudioData is PCM (decoded from Opus), SampleRate is typically 48000
        ProcessVoiceCommand(audioData, sampleRate);
    }
    
    private void ProcessVoiceCommand(float[] audioData, int sampleRate)
    {
        // Your custom audio processing logic
    }
}

// 2. In your experience's InitializeExperienceImpl(), register as visitor:
protected override bool InitializeExperienceImpl()
{
    // ... other initialization ...
    
    if (VOIPManager voipManager = GetComponent<VOIPManager>())
    {
        if (MyAudioProcessor audioProcessor = GetComponent<MyAudioProcessor>())
        {
            voipManager.RegisterAudioVisitor(audioProcessor);
        }
    }
}
```

**Why Use the Visitor Pattern?**

- ✅ **Decoupled Architecture** - VOIP module doesn't know about your experience
- ✅ **Multiple Visitors** - Multiple components can subscribe to the same audio stream
- ✅ **Clean Separation** - Your experience code stays in your experience module
- ✅ **Reusable Pattern** - Same approach works for any experience genre template

**Real-World Example:**

`AIFacemaskExperience` uses this pattern for speech recognition:
- `AIFacemaskASRManager` implements `IVOIPAudioVisitor`
- Receives player audio → Converts to text → Triggers AI improv responses
- All AIFacemask code stays in the AIFacemask module, VOIP module remains decoupled

**Features:**
- ✅ **Mumble Protocol** - Low-latency VOIP (< 50ms on LAN)
- ✅ **Steam Audio** - 3D HRTF spatialization for positional audio
- ✅ **Per-User Audio Sources** - Automatic spatialization for each remote player
- ✅ **HMD-Agnostic** - Works with any HMD's microphone and headphones
- ✅ **Unity-Friendly** - Easy integration via MonoBehaviour component
- ✅ **Visitor Pattern** - Subscribe to audio events without module coupling

**Prerequisites:**
- Murmur server running on LAN
- Steam Audio plugin (git submodule)
- MumbleLink plugin (git submodule)

</div>

</details>

---

## 📦 Installation

LBEAST is distributed as a Unity Package, making it easy to integrate into your projects. Choose the installation method that best fits your workflow.

### **Installation Methods**

> **✅ Simple Installation:** The LBEAST repository root **is** the package folder. You can clone directly into your project's `Packages/` directory - no need to extract subfolders or copy files manually.

<details>
<summary><strong>Option 1: Git Clone (Recommended - Simple One-Command Install)</strong></summary>

<div style="margin-left: 20px;">

The LBEAST repository root **is** the package folder. Simply clone directly into your project's `Packages/` directory:

```bash
# From your Unity project root
cd Packages
git clone https://github.com/ajcampbell1333/lbeast_unity.git com.ajcampbell.lbeast
```

**Windows PowerShell:**
```powershell
# From your Unity project root
cd Packages
git clone https://github.com/ajcampbell1333/lbeast_unity.git com.ajcampbell.lbeast
```

That's it! The package is ready to use.

**Next steps:**
1. Open Unity Editor
2. Unity will automatically detect and import the package
3. Verify in **Window > Package Manager > "In Project"**

**To update the package later:**
```bash
cd Packages/com.ajcampbell.lbeast
git pull
```

</div>

</details>

<details>
<summary><strong>Option 2: Git Submodule (Recommended for Git-based Projects)</strong></summary>

<div style="margin-left: 20px;">

If your project uses Git, add LBEAST as a submodule:

```bash
# From your Unity project root
cd Packages
git submodule add https://github.com/ajcampbell1333/lbeast_unity.git com.ajcampbell.lbeast
```

**Windows PowerShell:**
```powershell
# From your Unity project root
cd Packages
git submodule add https://github.com/ajcampbell1333/lbeast_unity.git com.ajcampbell.lbeast
```

Then:
1. Open Unity Editor
2. Unity will automatically detect and import the package
3. Verify in **Window > Package Manager > "In Project"**

**To update the package later:**
```bash
cd Packages/com.ajcampbell.lbeast
git submodule update --remote
```

**To clone a project that uses LBEAST as a submodule:**
```bash
git clone --recursive https://github.com/yourusername/yourproject.git
```

</div>

</details>

<details>
<summary><strong>Option 3: Git URL Installation (Package Manager)</strong></summary>

<div style="margin-left: 20px;">

Install LBEAST directly from a Git repository via Unity Package Manager:

1. **Open Unity Editor**
2. **Window > Package Manager**
3. Click **"+"** button (top-left)
4. Select **"Add package from git URL..."**
5. Enter the Git URL:
   ```
   https://github.com/ajcampbell1333/lbeast_unity.git?path=Packages/com.ajcampbell.lbeast
   ```
6. Click **"Add"**
7. Unity will download and install the package

> **📌 Note:** LBEAST is currently **version 0.1.0**. Version tags are not yet available, so the Git URL above will pull the latest commit from the `main` branch. Once version tags are added, you can pin to specific versions using `#v0.1.0` syntax. LBEAST uses **Semantic Versioning** (SemVer): MAJOR.MINOR.PATCH (e.g., 0.1.0 = minor changes, 1.0.0 = major/breaking changes).

**For a specific branch:**
```
https://github.com/ajcampbell1333/lbeast_unity.git?path=Packages/com.ajcampbell.lbeast#main
```

**For a specific version tag (when available):**
```
https://github.com/ajcampbell1333/lbeast_unity.git?path=Packages/com.ajcampbell.lbeast#v0.1.0
```

**Using SSH:**
```
git@github.com:ajcampbell1333/lbeast_unity.git?path=Packages/com.ajcampbell.lbeast
```

</div>

</details>

<details>
<summary><strong>Option 4: Download ZIP (Manual Installation)</strong></summary>

<div style="margin-left: 20px;">

1. **Download** the repository as a ZIP from GitHub
2. **Extract** the ZIP to a temporary location
3. **Copy the entire extracted folder** to your project's `Packages/` directory and rename it to `com.ajcampbell.lbeast`:
   ```
   YourProject/
   └── Packages/
       └── com.ajcampbell.lbeast/          ← Copy entire extracted folder here
           ├── package.json
           ├── Runtime/
           ├── Editor/
           └── ...
   ```
4. **Open Unity Editor**
5. Unity will automatically detect and import the package
6. Verify in **Window > Package Manager > "In Project"**

</div>

</details>

<details>
<summary><strong>Verifying Installation</strong></summary>

<div style="margin-left: 20px;">

### **Verifying Installation**

After installation, verify LBEAST is properly installed:

1. **Window > Package Manager**
2. Switch to **"In Project"** tab
3. Look for **"LBEAST - Location-Based Entertainment Activation Standard"** (v0.1.0)
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

</div>

</details>

<details>
<summary><strong>Package Dependencies</strong></summary>

<div style="margin-left: 20px;">

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

</div>

</details>

<details>
<summary><strong>Troubleshooting Installation</strong></summary>

<div style="margin-left: 20px;">

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

</div>

</details>

---

## 💻 Examples

<details>
<summary><strong>Example 1: Simple Moving Platform</strong></summary>

<div style="margin-left: 20px;">

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

</div>

</details>

<details>
<summary><strong>Example 2: Car Racing with Input</strong></summary>

<div style="margin-left: 20px;">

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
            carSim.SimulateBump(0.5f, 0.2f);
    }
}
```

</div>

</details>

<details>
<summary><strong>Example 3: Multiplayer Gunship</strong></summary>

<div style="margin-left: 20px;">

### Example 3: Multiplayer Gunship

```csharp
using UnityEngine;
using LBEAST.ExperienceTemplates;

public class GunshipGameController : MonoBehaviour
{
    private GunshipExperience gunship;

    // Simulate flight turbulence
    private float turbulence => Mathf.PerlinNoise(Time.time * 0.5f, 0f) * 0.2f - 0.1f;
    
    // Random tilt for damage shake
    private float randomTilt => Random.Range(-0.3f, 0.3f);

    void Start()
    {
        gunship = gameObject.AddComponent<GunshipExperience>();
        gunship.InitializeExperience();
        gunship.StartAsHost();
    }

    void Update() => gunship.SendGunshipTilt(turbulence, 0f, 0f, 0.1f);
    public void OnHit() => gunship.SendGunshipTilt(randomTilt, randomTilt, 0f, 0.1f);
}
```

</div>

</details>

---

## Architecture

<details>
<summary><strong>Module Structure</strong></summary>

<div style="margin-left: 20px;">

## Module Structure

```
LBEAST/
├── LBEAST.Core          # Core systems, VR/XR tracking abstraction, networking
├── LBEAST.AIFacemask    # AI facial animation API
├── LBEAST.LargeHaptics  # Hydraulic platform & gyroscope control API
├── LBEAST.EmbeddedSystems # Microcontroller integration API
├── LBEAST.ProAudio      # Professional audio console control via OSC
├── LBEAST.ProLighting   # DMX lighting control (Art-Net, USB DMX)
├── LBEAST.Retail        # Cashless tap card payment interface for VR tap-to-play
├── LBEAST.VOIP          # Low-latency voice communication with 3D HRTF
└── LBEAST.ExperienceTemplates # Pre-configured experience genre templates
    ├── AIFacemaskExperience
    ├── MovingPlatformExperience
    ├── GunshipExperience
    ├── CarSimExperience
    ├── FlightSimExperience
    └── EscapeRoomExperience
```

</div>

</details>

<details>
<summary><strong>Networking</strong></summary>

<div style="margin-left: 20px;">

## Networking

LBEAST v0.1.0 focuses on **local LAN multiplayer** using Unity NetCode for GameObjects:

- **Listen Server** (one player acts as host) or **Dedicated Server** (headless PC for monitoring)
- No web-hosting or online matchmaking in v0.1.0
- Future versions may add cloud deployment

</div>

</details>

<details>
<summary><strong>Hardware Integration</strong></summary>

<div style="margin-left: 20px;">

## Hardware Integration

All hardware communication is **abstracted** through interfaces:

- **HMD Interface** → OpenXR, SteamVR, Meta
- **Tracking Interface** → SteamVR Trackers (future: UWB, optical, ultrasonic)
- **Platform Controller** → UDP/TCP to hydraulic controller
- **Embedded Devices** → Serial, WiFi, Bluetooth, Ethernet

This allows you to:
1. Develop with simulated hardware
2. Integrate real hardware without changing game code
3. Swap hardware systems in configuration

</div>

</details>

<details>
<summary><strong>Use Cases</strong></summary>

<div style="margin-left: 20px;">

## Use Cases

LBEAST is designed for **commercial LBE installations** including:

- 🎬 **Movie/TV Promotional Activations** (Comic-Con, CES, Sundance, Tribeca)
- 🎮 **VR Arcades** with motion platforms
- 🎪 **Theme Park Attractions** with custom haptics
- 🎭 **Immersive Theater** with live actor-driven avatars
- 🏢 **Corporate Events** and brand experiences
- 🚀 **Research Labs** and academic projects

</div>

</details>

<details>
<summary><strong>Dedicated Server & Server Manager</strong></summary>

<div style="margin-left: 20px;">

## Dedicated Server & Server Manager

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

</div>

</details>

---

## Network Configuration

LBEAST requires reliable network communication between game engine servers, ECUs (embedded control units), VR clients, and console interfaces. This section details network setup options and recommended configurations.

> **⚠️ Network Isolation & Safety:** Each LBEAST deployment operates as its own isolated network—a micro-intranet completely offline from any other systems on-site. This isolation is a critical safety requirement: LBEAST networks must not connect to corporate networks, guest WiFi, or any other infrastructure. The OpsTech personnel operating the LBEAST console serve as the system administrators (SysAdmins) for this isolated micro-intranet, responsible for network configuration, device management, and troubleshooting. While most network management tasks can be automated through LBEAST's router API integration (when using professional routers with REST API support), the OpsTech team must understand and maintain this isolated network environment.

<details>
<summary><strong>Overview</strong></summary>

<div style="margin-left: 20px;">

### Overview

**Network Architecture:**
- **Local Network (LAN)**: All LBEAST devices operate on the same local network
- **Static DHCP (Recommended)**: Devices use DHCP but receive reserved IP addresses
- **Centralized Management**: Console interface for IP and port assignment
- **Security Rotation**: Optional scheduled IP address rotation for security

**Key Concepts:**
- **DHCP Reservations**: Router assigns same IP to device based on MAC address (static DHCP mode)
- **UDP Port Assignment**: Each device type uses specific UDP ports (configurable)
- **Connection Verification**: System verifies all device connections at session start
- **NAT Punchthrough**: Optional fallback for remote connections through firewalls

</div>

</details>

<details>
<summary><strong>Consumer Router Setup (Manual Configuration)</strong></summary>

<div style="margin-left: 20px;">

### Consumer Router Setup (Manual Configuration)

**Recommended for:** Small deployments, home labs, budget-conscious setups

**Router Requirements:**
- Router with DHCP reservation capability (most modern routers support this)
- Web-based admin interface (standard on consumer routers)
- No API access required (manual configuration)

**Setup Walkthrough:**

1. **Identify Device MAC Addresses**
   - Each ECU (Gunship ECU, Gun ECUs) has a unique MAC address
   - Document MAC addresses for all LBEAST devices
   - Methods to obtain MAC:
     - Serial monitor output during device boot
     - Router's connected devices list
     - Device firmware can report MAC address

2. **Configure DHCP Reservations in Router**
   - Access router admin panel (typically `192.168.1.1` or `192.168.0.1`)
   - Navigate to DHCP settings → DHCP Reservations (or "Static DHCP")
   - For each LBEAST device:
     - Enter MAC address
     - Assign desired static IP address
     - Ensure IPs are outside DHCP dynamic range (e.g., if DHCP pool is 192.168.1.100-200, use 192.168.1.10-99)
   - Save configuration

3. **Configure LBEAST Console IP Settings**
   - Open LBEAST Command Console
   - Navigate to Network Configuration
   - Manually enter IP address and UDP port for each device:
     - Gunship ECU: IP address, port 8888
     - Gun ECU Station 0: IP address, port 8888
     - Gun ECU Station 1: IP address, port 8889
     - Gun ECU Station 2: IP address, port 8890
     - Gun ECU Station 3: IP address, port 8891
     - Game Engine Server: IP address, port 8888
   - Save configuration

4. **Verify Connections**
   - Console will verify all device connections at session start
   - If any device fails to connect, check:
     - IP address matches router reservation
     - Device is powered on and connected to network
     - Firewall allows UDP traffic on assigned ports

5. **IP Rotation (Manual)**
   - To rotate IPs for security:
     - Access router admin panel
     - Update DHCP reservations with new IP addresses
     - Update LBEAST Console IP settings to match
     - Restart devices to acquire new IPs
   - **Note:** Manual process - must be done outside operating hours

**Limitations:**
- ❌ No automated bulk IP rotation
- ❌ Manual entry required in console for each device
- ❌ IP changes require manual router + console updates
- ✅ Works with any consumer router
- ✅ No special router features required

</div>

</details>

<details>
<summary><strong>Professional Router Setup (API Integration)</strong></summary>

<div style="margin-left: 20px;">

### Professional Router Setup (API Integration)

**Recommended for:** Production deployments, enterprise installations, advanced users

**Router Requirements:**
- Enterprise-grade router with API access (SNMP, REST, or vendor-specific API)
- Examples: Ubiquiti UniFi, Cisco, pfSense, MikroTik RouterOS
- Router API credentials for programmatic access

**Setup Walkthrough:**

1. **Configure Router API Access**
   - Enable API access in router admin panel
   - Generate API credentials (API key, username/password, or certificate)
   - Document API endpoint URL and authentication method
   - Test API connectivity from game engine server

2. **Initial DHCP Reservation Setup**
   - Option A: Use router web UI (same as consumer setup)
   - Option B: Use router API to create reservations programmatically
   - Configure all LBEAST device MAC addresses with reserved IPs
   - Ensure IPs are outside DHCP dynamic range

3. **Configure LBEAST Console**
   - Open LBEAST Command Console
   - Navigate to Network Configuration → Router Integration
   - Enter router API credentials:
     - Router IP address
     - API endpoint URL
     - Authentication credentials
   - Test router API connection
   - Console will automatically discover and populate device IPs from router

4. **Automatic Device Discovery**
   - Console queries router API for all DHCP reservations
   - Filters for LBEAST devices (by MAC address prefix or device naming)
   - Automatically populates IP addresses and ports
   - No manual entry required

5. **Connection Verification**
   - Console verifies all device connections at session start
   - Confirms IP addresses match router reservations
   - Automatic reconnection via NAT punchthrough if needed

6. **Scheduled IP Rotation (Automated)**
   - Configure rotation schedule in console (morning/evening, before/after hours)
   - Console triggers router API to update all DHCP reservations simultaneously
   - Console automatically updates its own IP configuration
   - Devices acquire new IPs on next DHCP renewal
   - **Note:** Rotation only occurs during scheduled windows (prevents mid-session changes)

**Advantages:**
- ✅ Automated bulk IP rotation
- ✅ Automatic device discovery
- ✅ No manual IP entry required
- ✅ Coordinated network-wide IP refresh
- ✅ Scheduled security rotation

**Router API Integration:**
- Console module calls router API to:
  - Query current DHCP reservations
  - Bulk update reservations for IP rotation
  - Verify device connectivity
- Supports multiple router vendors (UniFi, Cisco, pfSense, etc.)
- Fallback to manual mode if API unavailable

</div>

</details>

<details>
<summary><strong>UDP Port Configuration</strong></summary>

<div style="margin-left: 20px;">

### UDP Port Configuration

**Default Port Assignments:**
- **Game Engine Server**: 8888 (receives from Gunship ECU, sends to Gunship ECU)
- **Gunship ECU**: 8888 (receives from Game Engine), 8892 (receives from Gun ECUs)
- **Gun ECU Station 0**: 8888 (receives from Gunship ECU)
- **Gun ECU Station 1**: 8889 (receives from Gunship ECU)
- **Gun ECU Station 2**: 8890 (receives from Gunship ECU)
- **Gun ECU Station 3**: 8891 (receives from Gunship ECU)
- **Command Console**: 7778 (Server Beacon), 7779 (Command Protocol)

**Port Conflicts:**
- Console interface allows reassignment if ports conflict with other services
- All ports configurable per device
- Port changes require device restart to take effect

</div>

</details>

<details>
<summary><strong>NAT Punchthrough (Optional)</strong></summary>

<div style="margin-left: 20px;">

### NAT Punchthrough (Optional)

**When to Use:**
- Remote device connections (devices behind different NATs)
- Firewall traversal for off-site monitoring
- Backup connection method if DHCP reservations fail

**How It Works:**
- Custom NAT punchthrough implementation for LBEAST UDP protocol
- Establishes connection through firewalls/NAT devices
- Automatic fallback if primary connection fails
- Not required for local network deployments (DHCP reservations preferred)

**Configuration:**
- Enable in console Network Configuration
- Configure punchthrough server (if using relay server)
- Devices automatically attempt punchthrough if direct connection fails

</div>

</details>

<details>
<summary><strong>Best Practices</strong></summary>

<div style="margin-left: 20px;">

### Best Practices

1. **Use DHCP Reservations (Static DHCP)**
   - Centralized management via router
   - Devices always get same IP
   - Easier than device-side static IP configuration

2. **Keep IPs Outside DHCP Dynamic Range**
   - Prevents conflicts between reserved and dynamic IPs
   - Example: DHCP pool 192.168.1.100-200, use 192.168.1.10-99 for reservations

3. **Document All Manual IP Assignments** (Consumer Router Deployments)
   - Maintain spreadsheet or documentation of all device IPs
   - Include MAC addresses for reference
   - Update when IPs rotate
   - **Note:** Professional router deployments with API integration automatically maintain this documentation

4. **Verify Connections at Session Start**
   - Console automatically verifies all device connections
   - Prevents gameplay with missing devices
   - Automatic recovery via NAT punchthrough if needed

5. **Schedule IP Rotation Outside Operating Hours**
   - Configure rotation for morning (before open) or evening (after close)
   - Prevents mid-session network disruptions
   - Router DHCP lease times can enforce timing

6. **Use Professional Router for Production**
   - API integration enables automated management
   - Bulk operations save time
   - Better suited for multi-device deployments

</div>

</details>

---

## 🗺️ Roadmap

<details>
<summary><strong>v0.1.0 (Complete)</strong></summary>

<div style="margin-left: 20px;">

### ✅ Completed (v0.1.0)
- ✅ Core VR tracking abstraction
- ✅ 4DOF hydraulic platform support (4 & 6 actuators)
- ✅ 2DOF gyroscope support
- ✅ **Dedicated Server** architecture
- ✅ **Server Manager GUI** (UI Toolkit-based)
- ✅ **Automatic Server Discovery** (UDP broadcast)
- ✅ Normalized input system (-1 to +1)
- ✅ HOTAS integration framework
- ✅ AI facial animation control
- ✅ Embedded systems (Arduino, ESP32, STM32)
- ✅ LAN multiplayer (Unity NetCode)
- ✅ Experience Genre Templates (AIFacemask, MovingPlatform, Gunship, CarSim, FlightSim, EscapeRoom)
- ✅ **NVIDIA ACE Integration Architecture** (data structures, visitor pattern, component architecture)

</div>

</details>

<details>
<summary><strong>v0.1.2 (Current)</strong></summary>

<div style="margin-left: 20px;">

### 🎯 Planned (v0.1.2 - Current)
- ✅ **24V Large Solenoid Kicker with Dual-Handle Thumb Triggers** - 24V large solenoid kicker with dual-handle thumb triggers connected to an embedded system to simulate a large gun/rocket/laser/plasma mini-gun/rifle/launcher mounted to the hydraulic rig in the GunshipExperience
- ✅ **Implementing HOTAS integration** - Full Unity Input System HOTAS profiles and complete HOTAS controller support (completed for FlightSimExperience; other experiences can migrate from FlightSimExperience if needed)
- ✅ **Cashless Tap Card Payment Interface** - Implement cashless tap card payment interface for VR tap-to-play capability. Enables players to tap NFC/RFID cards or devices to initiate gameplay sessions without cash transactions.
- [ ] **Finishing AIFacemask functionality** - Complete all NOOP implementations for NVIDIA ACE service integration:
  - **AIFaceController**: Receive facial animation data from NVIDIA ACE endpoint (HTTP/WebSocket client), apply blend shape weights to SkinnedMeshRenderer morph targets, apply facial texture to mesh material
  - **ACE Script Manager**: Request script playback from NVIDIA ACE server (HTTP POST), request script pre-baking (TTS → Audio, Audio → Facial data), async pre-baking support (background processing)
  - **ACE ASR Manager**: Request ASR transcription from local ASR service (gRPC/HTTP to NVIDIA Riva ASR), trigger improv after transcription (wire to ACEImprovManager)
  - **ACE Improv Manager**: Request LLM response from local LLM (HTTP to Ollama/vLLM/NVIDIA NIM), request TTS conversion from local TTS (gRPC to NVIDIA Riva TTS), request Audio2Face conversion from local Audio2Face (HTTP/gRPC to NVIDIA NIM), auto-trigger Audio2Face after TTS completes (callback chain), monitor async response generation status (track LLM/TTS/Audio2Face operations)
  - **AIFacemaskExperience**: Configure NVIDIA ACE endpoint URL (load from project settings/config), register ASR Manager as visitor with VOIPManager (wire visitor pattern), configure NVIDIA ACE server base URL (load from project settings/config)
  - **VOIPManager**: Decode Opus to PCM for visitors (decode Mumble Opus before passing to visitors), integrate with player replication system (get remote player positions)
  - **Server Beacon**: Get server port from project settings (load port configuration), track actual player count (query Unity networking)
  - **Optimization**: Optimize blend shape application (batch updates, interpolation, caching), texture streaming optimization (efficient texture updates, compression)
- [ ] **Go-Kart Experience** - Electric go-karts, bumper cars, race boats, or bumper boats augmented by passthrough VR or AR headsets enabling overlaid virtual weapons and pickups that affect the performance of the vehicles
- [ ] **VR Player Transport (Server ↔ VR Clients)** - Bidirectional communication between game server and VR players:
  - **Server → VR Players**: Relay gun button events (Ch 310), gun state (firing, intensity), gun transforms (from trackers), and platform motion state. Use Unity NetCode for reliable state synchronization and optional UDP multicast for low-latency events.
  - **VR Players → Server**: Receive fire commands from VR controllers/triggers, relay to Gunship ECU → Gun ECU for solenoid firing. Support both centralized (via Gunship ECU) and direct (to Gun ECU) routing modes for latency optimization.
  - **Implementation**: Integrate with Unity's multiplayer networking system (NetCode for GameObjects) for state management, with optional custom UDP transport for time-critical events. Handle player connection/disconnection, station assignment, and network recovery.

</div>

</details>

<details>
<summary><strong>v1.0 (Planned)</strong></summary>

<div style="margin-left: 20px;">

### 🎯 Planned (v1.0)
- [ ] **Adding Weight & Height Safety Check Embedded Firmware** - Safety firmware for motion platforms to prevent operation if weight/height limits are exceeded
- [ ] **Network Configuration Module** - Build a network configuration system with console interface for OpsTech to manage IP addresses and UDP ports:
  - **IP & Port Configuration Console**: Centralized console interface for assigning static IP addresses and UDP ports to all LBEAST devices (ECUs, game engine servers, VR clients, console). Manual entry interface for consumer router deployments (requires keying-in IP addresses from router admin panel). Automatic device discovery for professional router deployments (via router API integration).
  - **Connection Verification**: At session start, verify connection to all devices and confirm IP addresses match expected values. If any device connection fails, automatically attempt reconnection via NAT punchthrough or re-authentication. Ensures all devices are reachable before gameplay begins.
  - **NAT Punchthrough Support**: Custom NAT punchthrough implementation for embedded systems (since Unity's NGO NAT punchthrough doesn't cover our custom UDP protocol). Enables remote device connections through firewalls/NAT when needed, with automatic reconnection if devices are reassigned. Primary deployment uses static DHCP (reservations) for local network stability.

- [ ] **Router API Integration Module (Optional, Advanced)** - Professional router integration for automated network management:
  - **Router API Connectivity**: Support for enterprise router APIs (Ubiquiti UniFi, Cisco, pfSense, MikroTik RouterOS, etc.) to programmatically query and manage DHCP reservations. Automatic device discovery by querying router for all LBEAST device reservations.
  - **Network-Wide IP Refresh**: Queue network-wide IP address rotation via router API - updates all DHCP reservations simultaneously, then triggers network-wide NAT punchthrough to re-establish all connections. Optional module for advanced users with professional routers. Consumer router users must manually update IPs in router admin panel and console (see Network Configuration documentation).
  - **Scheduled Rotation**: Configure IP rotation schedules (morning/evening, before/after hours) that trigger router API bulk updates. Prevents IP changes during work hours or mid-session. Router DHCP lease times and reservation rules handle timing enforcement.

#### Gunship Experience — Alpha Readiness

> **📋 Hardware Specifications:** See **[FirmwareExamples/GunshipExperience/Gunship_Hardware_Specs.md](Packages/com.ajcampbell.lbeast/FirmwareExamples/GunshipExperience/Gunship_Hardware_Specs.md)** for complete hardware specifications including solenoid selection, PWM driver modules, thermal management, redundancy, and communication architecture.

- [ ] **Guns Subsystem (Per-Station Solenoid Kicker)**
  - **Hardware**: One embedded controller per play station (4 total), each with:
    - Dual thumb buttons (fire mode controls)
    - 24V solenoid kicker (haptic recoil) — see [Gunship_Hardware_Specs.md](Packages/com.ajcampbell.lbeast/FirmwareExamples/GunshipExperience/Gunship_Hardware_Specs.md) for detailed specifications
    - One SteamVR Ultimate tracker on the gun nose
  - **Networking**:
    - Station ECUs sync over UDP to the primary Gunship ECU (mounted on scissor lift) — see [Gunship_Hardware_Specs.md](Packages/com.ajcampbell.lbeast/FirmwareExamples/GunshipExperience/Gunship_Hardware_Specs.md) for communication architecture
    - Primary ECU relays aggregated per-station state to game engine:
      - Button states (debounced, rate-limited)
      - Gun orientation vectors (from trackers)
      - Optional kicker telemetry (duty, temp, faults)
  - **Engine-Side APIs**:
    - Real-time tracker node per gun transform (C# component + Unity Inspector access)
    - Event/delegate surface for fire presses, fire rate gating, and safety lockouts
    - Sample rendering helpers for minigun/grenade/alt-fire archetypes
    - Per-station ID mapping and replication-safe routing

- [ ] **Virtual Scissor Lift Platform Representation**
  - **Tracking**:
    - At least one SteamVR Ultimate tracker on the platform chassis
    - Engine-side transform fusion: commanded pose vs tracker pose
  - **Performance Optimization Module**:
    - Measure responsiveness to tilt/translation commands (latency, overshoot, settling)
    - Rolling KPIs exposed to Command Console/UI Toolkit (basic performance dashboard)
  - **Unity GameObject Locator**:
    - Simple GameObject representing the moving platform root
    - Easy attachment point for chopper/gunship/spaceship meshes
    - Auto-follows fused platform transform (commanded ⊕ tracker correction)
  - **Grounded/In-Flight State**:
    - Digital state surfaced from ECU and engine logic:
      - Grounded: calibration zeroed; tilt/move commands ignored
      - In-Flight: motion enabled within safety limits
    - Visual state and API for mode transitions, with safety interlocks

- [ ] **ESP32 Shield Design (Hardware)**
  - **Example Shield Designs**: Design example shields/breakout boards for ESP32 plus needed modules for both ECU types:
    - **GunshipExperience_ECU**: ESP32 + Ethernet PHY (LAN8720A) + actuator control interfaces + scissor lift control interfaces
    - **Gun_ECU**: ESP32 + Ethernet PHY (LAN8720A) + dual thumb button inputs + N× solenoid PWM driver interfaces + NTC thermistor ADC inputs
  - **Source Files**: Include PCB design source files in KiCAD format (`.kicad_pcb`, `.kicad_sch`) for maximum extensibility
    - **Note**: EasyEDA projects can be exported to KiCAD format for cross-tool compatibility
    - **Alternative**: Include source files in EasyEDA format if preferred, but provide KiCAD export
  - **Manufacturing Files**: Include GERBER files (industry standard) for direct PCB manufacturing
    - GERBER files are tool-agnostic and can be used with any PCB manufacturer (JLCPCB, PCBWay, OSH Park, etc.)
    - Include drill files, pick-and-place files, and assembly drawings
  - **Documentation**: Include schematics (PDF), PCB layouts (PDF), BOM (CSV/Excel), and assembly instructions
  - **Purpose**: Provide reference designs for developers building custom hardware or adapting existing ESP32 boards
  - **File Structure**: Organize in `Hardware/Shields/` directory with subdirectories for each shield type

</div>

</details>

<details>
<summary><strong>v1.1 (Future)</strong></summary>

<div style="margin-left: 20px;">

### 🔄 In Progress (v1.1)
- [ ] Meta Quest 3 native integration
- [ ] Sample Arduino/ESP32 firmware
- [ ] WebSocket alternative for live actor streaming

</div>

</details>

<details>
<summary><strong>v2.0 (Future)</strong></summary>

<div style="margin-left: 20px;">

### 🎯 Planned (v2.0)
- [ ] Apple Vision Pro support
- [ ] **Holographic Render Target Support** - Support for holographic display technologies including swept-plane, swept-volume, Pepper's Ghost, lenticular, and other volumetric display methods. Enables rendering to specialized holographic hardware for immersive product visualization and LBE installations.
- [ ] **GunshipExperience HOTAS Pilot Support** - Add optional 5th player (pilot) support to GunshipExperience with HOTAS controller integration. Enables pilot-controlled flight while 4 gunners operate weapons, expanding gameplay possibilities for multi-crew vehicle experiences.
- [ ] Advanced inverse kinematics for custom actuator configs
- [ ] Visual scripting (Bolt/Visual Scripting) support
- [ ] Cloud multiplayer (Photon/Mirror)
- [ ] Prefab packages (ready-to-use scene templates)
- [ ] **RenderTexture Arrays and Matrices** - Support for RenderTexture arrays and matrices with hardware-agnostic output to video spheres, 360 video, stereoscopic 3D billboards, stereoscopic 360 video, LED walls, projectors (front projection, rear projection, variable-depth projection), and drone swarm renderers. Enables synchronized multi-display installations for immersive LBE experiences.
- [ ] **OTA Firmware Updates** - Implement and test OTA (Over-The-Air) firmware flashing for ESP32-based reference design, APT package management for Raspberry Pi and Jetson Nano, and ESP32-as-wireless-adapter for STM32 OTA based on the rounakdatta open-source project. **Note:** The rounakdatta project will be included as a git submodule when implementing OTA functionality.
- [ ] **3D-Printable 1/8th-Scale Platform Model** - Design a 3D-printable 1/8th-scale model of the scissor lift and tilt platform with complete ECU prototype integration capability for use in off-site network debugging. Enables developers to test network configurations, firmware updates, and communication protocols without requiring access to full-scale hardware. Includes mounting points for ESP32 ECUs, mock actuators, and all necessary interfaces for full system validation.

</div>

</details>

---

## 📄 License

This project is licensed under the **MIT License** - see the [LICENSE](../LICENSE) file for details.

**TL;DR:** Free to use, modify, and distribute for personal or commercial projects. Attribution appreciated but not required.

## Contributing

LBEAST is open-source under the MIT License. Contributions are welcome!

<details>
<summary><strong>Development Workflow</strong></summary>

<div style="margin-left: 20px;">

### Development Workflow

1. Fork this repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

</div>

</details>

<details>
<summary><strong>Code Standards</strong></summary>

<div style="margin-left: 20px;">

### Code Standards

- Follow [Microsoft C# Coding Conventions](https://docs.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions)
- Add XML documentation comments to public APIs
- Write Unity-friendly code with proper MonoBehaviour lifecycle awareness
- Test with both C# and Unity Inspector workflows

</div>

</details>

## Credits

Created by **AJ Campbell**.

---

*LBEAST: Making location-based entertainment development accessible to everyone.*

