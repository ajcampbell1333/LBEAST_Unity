# LBEAST VOIP Module

**Low-latency VOIP system with 3D HRTF spatialization for location-based entertainment.**

Integrates **Mumble** (low-latency VOIP) with **Steam Audio** (3D HRTF spatialization) for high-quality positional voice communication in multiplayer LBE experiences.

---

## 🎯 Features

- ✅ **Low-Latency VOIP** - Mumble protocol for LAN communication (< 50ms latency)
- ✅ **3D HRTF Spatialization** - Steam Audio for realistic positional audio
- ✅ **Per-User Audio Sources** - Automatic spatialization for each remote player
- ✅ **Unity-Friendly** - Easy integration via MonoBehaviour component
- ✅ **Automatic Connection Management** - Handles connection lifecycle
- ✅ **Microphone Control** - Mute/unmute support
- ✅ **Volume Control** - Per-component audio volume
- ✅ **HMD-Agnostic** - Works with any HMD's microphone and headphones (Oculus, Vive, Pico, etc.)

---

## 📋 Prerequisites

1. **Murmur Server** - Mumble server running on your LAN
   - Download: https://www.mumble.info/downloads/
   - Default port: 64738
   - Run: `murmurd -ini murmur.ini`

2. **Steam Audio Plugin** - Git submodule (see Setup below)
3. **MumbleLink Plugin** - Git submodule (see Setup below)
4. **SOFA HRTF Files** - For Steam Audio spatialization
   - Place in `Assets/HRTF/` directory
   - Recommended: IRCAM HRTF database

---

## 🚀 Quick Start

### **1. Set Up Git Submodules**

The VOIP module requires two git submodules:

```bash
# Navigate to package root
cd Packages/com.ajcampbell.lbeast

# Add Steam Audio submodule
git submodule add https://github.com/ValveSoftware/steam-audio.git Plugins/SteamAudio

# Add MumbleLink submodule (replace with actual MumbleLink repository URL)
# git submodule add <mumble-link-repo-url> Plugins/MumbleLink

# Initialize submodules
git submodule update --init --recursive
```

**Note:** MumbleLink plugin repository URL needs to be determined. Common options:
- Official Mumble C++ library: https://github.com/mumble-voip/mumble
- Unity-specific wrapper (may need to be created or found)

### **2. Update Submodules (Pull Latest)**

Run the update script to pull latest changes:

```powershell
.\Runtime\VOIP\Common\PullLatest.ps1
```

Or manually:

```bash
git submodule update --remote
```

### **3. Enable Plugins in Unity**

1. Open Unity Package Manager (Window → Package Manager)
2. Install Steam Audio package (from submodule)
3. Install MumbleLink package (from submodule, if available)
4. Ensure LBEAST package is imported

### **4. Configure Steam Audio**

1. Edit → Project Settings → Audio → Steam Audio
2. Set HRTF file path: `Assets/HRTF/IRCAM_1001.sofa` (or your HRTF file)
3. Configure audio settings as needed

### **5. Add VOIP to Your Experience**

#### **Option A: Inspector**

1. Select your HMD/Player GameObject
2. Add Component → **LBEAST → VOIP Manager**
3. Set properties:
   - **Server IP**: Your Murmur server IP (e.g., `192.168.1.100`)
   - **Server Port**: `64738` (default)
   - **Player Name**: Auto-generated or set manually
   - **Auto Connect**: `true` (connects on Start)

#### **Option B: C#**

```csharp
using LBEAST.VOIP;

// In your HMD/Player MonoBehaviour
VOIPManager voipManager = gameObject.AddComponent<VOIPManager>();
voipManager.serverIP = "192.168.1.100";
voipManager.serverPort = 64738;
voipManager.autoConnect = true;
```

### **6. Start Murmur Server**

On your server machine:

```bash
murmurd -ini murmur.ini
```

Or use Docker:

```bash
docker run -d -p 64738:64738/udp -p 64738:64738/tcp mumble-server/murmur
```

### **7. Test**

1. Play in Editor with multiple players
2. Each player should automatically connect to Mumble
3. Speak into microphone - other players should hear you with spatialization
4. Move players around - audio should spatialize based on positions

---

## 📁 Module Structure

```
Runtime/VOIP/
├── VOIPManager.cs              # Main component (attach to HMD/Player)
├── MumbleClient.cs              # Mumble protocol wrapper
├── SteamAudioSourceComponent.cs  # Per-user spatial audio source
├── VOIPTypes.cs                  # Connection state enum
├── Common/
│   └── PullLatest.ps1            # Script to update submodules
└── README.md                     # This file
```

---

## 🔧 Architecture

### **Component Hierarchy**

```
HMD/Player GameObject
└── VOIPManager (MonoBehaviour)
    ├── MumbleClient (MonoBehaviour)
    │   └── Handles Mumble protocol connection
    │       └── Microphone capture (via OS audio system)
    └── AudioSourceMap (Dictionary<UserId, SteamAudioSourceComponent>)
        └── One component per remote player
            └── Handles Steam Audio spatialization
            └── Audio output (via OS audio system)
```

### **HMD Compatibility**

The VOIP system is **HMD-agnostic** and works with any VR headset:

- **Microphone Input**: Uses Unity's audio system → OS audio APIs (WASAPI on Windows) → Any microphone device
  - Oculus Quest/Pro/Rift microphones ✅
  - HTC Vive/Vive Pro microphones ✅
  - Pico 4/Enterprise microphones ✅
  - Windows Mixed Reality microphones ✅
  - Any USB/Bluetooth microphone ✅

- **Headphone Output**: Routes through Unity's audio system → OS audio APIs → Selected audio output device
  - Oculus Quest/Pro/Rift headphones ✅
  - HTC Vive/Vive Pro headphones ✅
  - Pico 4/Enterprise headphones ✅
  - Windows Mixed Reality headphones ✅
  - Any audio output device ✅

**Note**: The HMD's microphone and headphones must be selected as the default audio input/output devices in Windows Settings, or configured in Unity's audio settings.

### **Data Flow**

1. **Microphone Input** → MumbleClient → Encode to Opus → Send to Mumble Server
2. **Remote Audio** → MumbleClient → Decode Opus → SteamAudioSourceComponent
3. **Steam Audio** → Process HRTF → Binaural Audio → Unity Audio System
4. **Player Positions** → Replicated via Unity Netcode → Update spatialization

### **Replication**

- Uses Unity Netcode for GameObjects replication system
- Player positions are replicated automatically (via NetworkTransform)
- Audio data is NOT replicated (streamed via Mumble)
- VOIPManager component replicates connection state

---

## 📡 Protocol Details

### **Mumble Protocol**

- **Control Channel**: TCP (connection, user management)
- **Audio Channel**: UDP (low-latency audio streaming)
- **Codec**: Opus (high quality, low latency)
- **Sample Rate**: 48kHz
- **Positional Audio**: 3D coordinates sent with audio packets

### **Steam Audio**

- **HRTF**: Head-Related Transfer Function for 3D spatialization
- **Format**: SOFA files (Spatially Oriented Format for Acoustics)
- **Processing**: Real-time binaural rendering
- **Output**: Stereo binaural audio

---

## 🛠️ Integration with Player Replication

The VOIP system relies on Unity Netcode for GameObjects for player positions:

```csharp
// In your Player NetworkBehaviour class
public class YourPlayerNetworkBehaviour : NetworkBehaviour
{
    [SerializeField] private NetworkVariable<Vector3> replicatedPosition = 
        new NetworkVariable<Vector3>();

    // VOIPManager will automatically query player positions from replicated data
    // Update this in Update() or FixedUpdate()
    void Update()
    {
        if (IsServer)
        {
            replicatedPosition.Value = transform.position;
        }
    }
}
```

---

## 🔌 API Reference

### **VOIPManager Component**

#### **Connection Management**

```csharp
// Connect to Mumble server
bool Connect();

// Disconnect from server
void Disconnect();

// Check connection state
bool IsConnected();
VOIPConnectionState ConnectionState { get; }
```

#### **Audio Control**

```csharp
// Mute/unmute microphone
void SetMicrophoneMuted(bool muted);
bool IsMicrophoneMuted();

// Set output volume (0.0 to 1.0)
void SetOutputVolume(float volume);
float GetOutputVolume();
```

#### **Events**

```csharp
// Connection state changed
UnityEvent<VOIPConnectionState> OnConnectionStateChanged;

// Remote player audio received
UnityEvent<int, Vector3> OnRemotePlayerAudioReceived;
```

---

## 🐛 Troubleshooting

### **"MumbleLink plugin not found"**

- Ensure MumbleLink submodule is initialized: `git submodule update --init --recursive`
- Check that MumbleLink package is imported in Unity Package Manager
- Verify MumbleLink plugin is in `Plugins/MumbleLink/` directory

### **"Steam Audio plugin not found"**

- Ensure Steam Audio submodule is initialized
- Check that Steam Audio package is imported in Unity Package Manager
- Verify Steam Audio plugin is in `Plugins/SteamAudio/` directory

### **"Connection failed"**

- Verify Murmur server is running: `netstat -an | findstr 64738`
- Check firewall allows UDP/TCP on port 64738
- Verify Server IP and Port are correct
- Check Murmur server logs for connection attempts

### **"No audio spatialization"**

- Verify HRTF file is in `Assets/HRTF/` directory
- Check Steam Audio plugin settings (HRTF file path)
- Ensure player positions are being replicated
- Check audio source components are being created (enable logging)

### **"High latency"**

- Ensure Mumble server is on same LAN (not over internet)
- Check network latency: `ping <server-ip>`
- Verify Opus codec settings (should be low-latency mode)
- Check audio buffer sizes in Mumble configuration

---

## 📚 Related Documentation

- **[Steam Audio Documentation](https://valvesoftware.github.io/steam-audio/)** - Valve's Steam Audio documentation
- **[Mumble Documentation](https://wiki.mumble.info/)** - Mumble protocol documentation
- **[Unity Netcode Documentation](https://docs-multiplayer.unity3d.com/)** - Unity networking

---

## 🔗 Submodule Management

### **Adding Submodules**

```bash
# From package root
cd Packages/com.ajcampbell.lbeast

# Add Steam Audio
git submodule add https://github.com/ValveSoftware/steam-audio.git Plugins/SteamAudio

# Add MumbleLink (replace with actual URL)
git submodule add <mumble-link-repo-url> Plugins/MumbleLink
```

### **Updating Submodules**

```powershell
# Run update script
.\Runtime\VOIP\Common\PullLatest.ps1

# Or manually
git submodule update --remote
git submodule update --init --recursive
```

### **Cloning with Submodules**

```bash
# Clone repository with submodules
git clone --recursive <repo-url>

# Or if already cloned
git submodule update --init --recursive
```

---

## 📝 TODO / Integration Status

### **Completed**
- ✅ Module structure and component classes
- ✅ VOIPManager component (MonoBehaviour)
- ✅ MumbleClient wrapper interface
- ✅ SteamAudioSourceComponent interface
- ✅ UnityEvent-based events
- ✅ Submodule update script

### **Pending Integration**
- ⏳ MumbleLink plugin integration (waiting for submodule)
- ⏳ Steam Audio plugin integration (waiting for submodule)
- ⏳ Opus encoding/decoding implementation
- ⏳ HRTF processing implementation
- ⏳ Audio playback via Unity audio system
- ⏳ Player position replication integration

### **Future Enhancements**
- 🔮 WebRTC fallback if Mumble unavailable
- 🔮 Echo cancellation
- 🔮 Noise suppression
- 🔮 Audio quality settings (bitrate, sample rate)
- 🔮 Channel/room support (multiple VOIP channels)

---

## 📄 License

MIT License - Copyright (c) 2025 AJ Campbell

**Dependencies:**
- **Steam Audio**: Apache 2.0 License (Valve Software)
- **Mumble**: BSD 3-Clause License (Mumble VOIP)

---

**Built for LBEAST - Location-Based Entertainment Activation Standard**

