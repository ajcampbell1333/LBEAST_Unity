# VR Player Transport System

## Overview

The VR Player Transport system provides experience-agnostic replication of OpenXR HMD and hand tracking data for LAN multiplayer VR experiences. This enables all LBEAST experiences to support real-time 6DOF multiplayer where each player's head and hands are visible to other players.

## Architecture

### Components

1. **`LBEASTVRPlayerReplicationComponent`** - NetworkBehaviour component that captures OpenXR data from local player and replicates it to all clients
2. **`LBEASTXRReplicatedData`** - Data structure containing HMD and hand tracking transforms (implements `INetworkSerializable`)
3. **`ReplicatedHandData`** - Hand tracking data structure (implements `INetworkSerializable`)
4. **`ReplicatedHandKeypoint`** - Individual keypoint transform data (implements `INetworkSerializable`)

### Data Flow

```
Local Player (Client)
  ↓
Capture OpenXR Data (HMD + Hand Keypoints)
  ↓
LBEASTVRPlayerReplicationComponent
  ↓
NetworkVariable<LBEASTXRReplicatedData>
  ↓
Unity NetCode Replication (Client → Server → All Clients)
  ↓
Remote Players Receive Data
  ↓
LBEASTHandGestureRecognizer Uses Replicated Data
```

## Usage

### Basic Setup

1. **Add NetworkObject** to your VR player GameObject (required for NetworkBehaviour)
2. **Add LBEASTVRPlayerReplicationComponent** to the same GameObject
3. The component automatically captures OpenXR data on the local client
4. Data is replicated to server, then to all clients

### Example: Adding to Player GameObject

```csharp
// In your player setup code
GameObject playerObject = // ... your VR player GameObject

// Add NetworkObject (required for NetworkBehaviour)
NetworkObject networkObject = playerObject.AddComponent<NetworkObject>();

// Add VR replication component
LBEASTVRPlayerReplicationComponent replicationComp = 
    playerObject.AddComponent<LBEASTVRPlayerReplicationComponent>();

// Configure if needed
replicationComp.replicationUpdateRate = 60f;
replicationComp.enableReplication = true;
```

### Integration with Hand Gesture Recognition

The `LBEASTHandGestureRecognizer` automatically uses replicated data for remote players when:

1. `onlyProcessLocalPlayer` is set to `false`
2. `LBEASTVRPlayerReplicationComponent` is present on the GameObject

```csharp
// In your experience setup
LBEASTHandGestureRecognizer gestureRecognizer = 
    playerObject.GetComponent<LBEASTHandGestureRecognizer>();
if (gestureRecognizer != null)
{
    // Enable gesture recognition for all players (local + remote)
    gestureRecognizer.onlyProcessLocalPlayer = false;
}
```

## Replicated Data

### HMD Data
- Position (Vector3) - World space
- Rotation (Quaternion) - World space
- Tracking state (bool)

### Hand Data (per hand, left and right)
- Wrist transform
- Hand center (MiddleMetacarpal)
- All 5 fingertip transforms (Thumb, Index, Middle, Ring, Little)
- Hand tracking active state

## Configuration

### Replication Update Rate
- Default: 60 Hz
- Configurable: 10-120 Hz
- Higher = smoother but more bandwidth

### Enable/Disable Replication
- Set `enableReplication = false` to disable (e.g., single-player)

## XR Origin Setup

**Important:** The component assumes it's attached to the XR Origin GameObject (or player root that contains the XR Origin). 

- **InputDevices** returns HMD positions relative to XR Origin
- **XRHandSubsystem** returns hand poses relative to XR Origin
- The component converts these to world space using `transform.TransformPoint()` and `transform.rotation`

If your component is on a child of the XR Origin, you may need to adjust the world space conversion logic.

## Performance Considerations

- **Bandwidth**: ~2-5 KB/s per player at 60 Hz (depends on compression)
- **CPU**: Minimal overhead (capture only on local player)
- **Network**: Uses Unity NetCode for GameObjects (reliable, ordered)

## Future Enhancements

- Replicate all hand keypoints (full hand skeleton) for advanced gesture recognition
- Compression for bandwidth optimization
- Interpolation for smoother remote player movement
- Prediction for reduced latency
- Support for XR Origin detection (automatic transform hierarchy resolution)

## Example: Gunship Experience Integration

```csharp
// In your GunshipExperience or player setup
void SetupVRPlayer(GameObject playerObject)
{
    // Ensure NetworkObject exists
    if (playerObject.GetComponent<NetworkObject>() == null)
    {
        playerObject.AddComponent<NetworkObject>();
    }
    
    // Add replication component if not present
    if (playerObject.GetComponent<LBEASTVRPlayerReplicationComponent>() == null)
    {
        LBEASTVRPlayerReplicationComponent replicationComp = 
            playerObject.AddComponent<LBEASTVRPlayerReplicationComponent>();
        replicationComp.replicationUpdateRate = 60f;
    }
    
    // Configure gesture recognition for all players
    LBEASTHandGestureRecognizer gestureRecognizer = 
        playerObject.GetComponent<LBEASTHandGestureRecognizer>();
    if (gestureRecognizer != null)
    {
        gestureRecognizer.onlyProcessLocalPlayer = false;
    }
}
```

## Troubleshooting

### Replication Not Working
- Ensure GameObject has `NetworkObject` component
- Check that `NetworkManager` is running
- Verify `enableReplication = true`
- Check NetworkObject ownership: Should be `IsOwner = true` on local player

### Hand Tracking Not Visible on Remote Players
- Ensure `LBEASTVRPlayerReplicationComponent` is present on GameObject
- Check `enableReplication = true`
- Verify OpenXR hand tracking is active on local player
- Check that XR Origin is correctly set up

### Gesture Recognition Not Working for Remote Players
- Set `onlyProcessLocalPlayer = false` on `LBEASTHandGestureRecognizer`
- Ensure `LBEASTVRPlayerReplicationComponent` is present on GameObject
- Check that hand tracking data is being replicated (use debug visualization)

### World Space Issues
- Ensure component is on XR Origin GameObject (or player root)
- Verify XR Origin transform is correct
- Check that world space conversion is working (positions should be in world space, not local)

