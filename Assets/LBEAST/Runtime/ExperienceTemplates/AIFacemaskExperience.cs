// Copyright (c) 2025 AJ Campbell. Licensed under the MIT License.

using UnityEngine;
using LBEAST.Core;
using LBEAST.AIFacemask;
using LBEAST.EmbeddedSystems;

namespace LBEAST.ExperienceTemplates
{
    /// <summary>
    /// AI Facemask Experience Template
    /// 
    /// Actor-driven multiplayer VR experience with AI facial animation.
    /// Combines:
    /// - Live actor-driven facial animation
    /// - Multiple networked players
    /// - Embedded systems integration (buttons, haptics)
    /// - Narrative state machine support
    /// 
    /// Perfect for escape rooms, interactive theater, actor-driven VR experiences,
    /// and any LBE installation requiring live human performance.
    /// </summary>
    public class AIFacemaskExperience : LBEASTExperienceBase
    {
        [Header("Facial Animation")]
        [SerializeField] private FacialAnimationController facialController;
        [SerializeField] private GameObject avatarPrefab;
        [SerializeField] private FacialAnimationMode defaultAnimationMode = FacialAnimationMode.Live;

        [Header("Embedded Systems")]
        [SerializeField] private SerialDeviceController serialDevice;
        [SerializeField] private int numberOfButtons = 8;

        [Header("Actor Connection")]
        [SerializeField] private string actorStreamIP = "192.168.1.50";
        [SerializeField] private int actorStreamPort = 9000;

        private GameObject spawnedAvatar;

        protected override void Awake()
        {
            base.Awake();

            // Enable multiplayer
            enableMultiplayer = true;
            maxPlayers = 4;  // Can be configured

            // Find or create facial controller
            if (facialController == null)
            {
                facialController = GetComponentInChildren<FacialAnimationController>();
            }

            // Find or create serial device controller
            if (serialDevice == null)
            {
                serialDevice = GetComponent<SerialDeviceController>();
                if (serialDevice == null)
                {
                    serialDevice = gameObject.AddComponent<SerialDeviceController>();
                }
            }
        }

        protected override bool InitializeExperienceImpl()
        {
            // Spawn avatar if needed
            if (avatarPrefab != null && spawnedAvatar == null)
            {
                spawnedAvatar = Instantiate(avatarPrefab, transform);
                
                // Find facial controller on spawned avatar
                if (facialController == null)
                {
                    facialController = spawnedAvatar.GetComponentInChildren<FacialAnimationController>();
                }
            }

            // Initialize facial animation
            if (facialController != null)
            {
                if (!facialController.Initialize())
                {
                    Debug.LogWarning("[LBEAST] AIFacemaskExperience: Failed to initialize facial animation");
                }
                else
                {
                    facialController.SetAnimationMode(defaultAnimationMode);
                    facialController.StartAnimation();
                }
            }

            // Initialize embedded systems
            if (serialDevice != null)
            {
                if (!serialDevice.ConnectToDevice())
                {
                    Debug.LogWarning("[LBEAST] AIFacemaskExperience: Failed to connect to embedded device");
                }
            }

            Debug.Log("[LBEAST] AIFacemaskExperience initialized successfully");
            return true;
        }

        protected override void ShutdownExperienceImpl()
        {
            // Stop facial animation
            if (facialController != null)
            {
                facialController.StopAnimation();
            }

            // Disconnect embedded systems
            if (serialDevice != null)
            {
                serialDevice.DisconnectFromDevice();
            }

            // Clean up spawned avatar
            if (spawnedAvatar != null)
            {
                Destroy(spawnedAvatar);
            }
        }

        #region Public API - Facial Animation

        /// <summary>
        /// Set facial animation mode
        /// </summary>
        public void SetAnimationMode(FacialAnimationMode mode)
        {
            if (facialController != null)
            {
                facialController.SetAnimationMode(mode);
            }
        }

        /// <summary>
        /// Set a specific facial blend shape weight
        /// </summary>
        public void SetBlendShapeWeight(string blendShapeName, float weight)
        {
            if (facialController != null)
            {
                facialController.SetBlendShapeWeight(blendShapeName, weight);
            }
        }

        /// <summary>
        /// Start recording facial animation
        /// </summary>
        public void StartRecording()
        {
            if (facialController != null)
            {
                facialController.StartRecording();
            }
        }

        /// <summary>
        /// Stop recording and save facial animation
        /// </summary>
        public void StopRecording()
        {
            if (facialController != null)
            {
                var recording = facialController.StopRecording();
                Debug.Log($"[LBEAST] Recorded {recording.Count} frames");
            }
        }

        #endregion

        #region Public API - Embedded Systems

        /// <summary>
        /// Check if a button is currently pressed
        /// </summary>
        public bool IsButtonPressed(int buttonID)
        {
            if (serialDevice != null)
            {
                return serialDevice.IsButtonPressed(buttonID);
            }
            return false;
        }

        /// <summary>
        /// Check if button was just pressed this frame
        /// </summary>
        public bool GetButtonDown(int buttonID)
        {
            if (serialDevice != null)
            {
                return serialDevice.GetButtonDown(buttonID);
            }
            return false;
        }

        /// <summary>
        /// Send haptic pulse to embedded device
        /// </summary>
        public void SendHapticPulse(byte intensity, int duration)
        {
            if (serialDevice != null)
            {
                serialDevice.SendHapticPulse(intensity, duration);
            }
        }

        /// <summary>
        /// Set LED color on embedded device
        /// </summary>
        public void SetLEDColor(int ledID, byte r, byte g, byte b)
        {
            if (serialDevice != null)
            {
                serialDevice.SetLEDColor(ledID, r, g, b);
            }
        }

        /// <summary>
        /// Send custom command to embedded device
        /// </summary>
        public void SendCustomCommand(string command)
        {
            if (serialDevice != null)
            {
                serialDevice.SendCustomCommand(command);
            }
        }

        #endregion

        #region Public API - Actor Connection

        /// <summary>
        /// Set the actor stream IP address
        /// </summary>
        public void SetActorStreamIP(string ip)
        {
            actorStreamIP = ip;
        }

        /// <summary>
        /// Set the actor stream port
        /// </summary>
        public void SetActorStreamPort(int port)
        {
            actorStreamPort = port;
        }

        #endregion
    }
}

