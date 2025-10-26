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
    /// Pre-configured experience for LAN multiplayer VR with immersive theater live actors.
    /// 
    /// Architecture:
    /// - AI facial animation operates AUTONOMOUSLY (driven by NVIDIA Audio2Face)
    /// - Live actors wear wrist-mounted button controls (4 buttons: 2 left, 2 right)
    /// - Buttons control the Experience Loop state machine (not the AI face)
    /// 
    /// Button Layout:
    /// - Left Wrist:  Button 0 (Forward), Button 1 (Backward)
    /// - Right Wrist: Button 2 (Forward), Button 3 (Backward)
    /// 
    /// The live actor directs the experience flow, while the AI face handles
    /// natural conversation and emotional responses autonomously.
    /// 
    /// Perfect for escape rooms, interactive theater, live actor-driven VR experiences,
    /// and any LBE installation requiring live human performance.
    /// </summary>
    public class AIFacemaskExperience : LBEASTExperienceBase
    {
        [Header("Components")]
        [SerializeField] private FacialAnimationController facialController;
        [SerializeField] private SerialDeviceController costumeController;
        [SerializeField] private ExperienceStateMachine experienceLoop;

        [Header("Live Actor Configuration")]
        [SerializeField] private GameObject avatarPrefab;
        [SerializeField] private SkinnedMeshRenderer liveActorMesh;
        [SerializeField] private int numberOfLiveActors = 1;
        [SerializeField] private int numberOfPlayers = 4;

        [Header("Live Actor Connection")]
        [SerializeField] private string liveActorStreamIP = "192.168.1.50";
        [SerializeField] private int liveActorStreamPort = 9000;

        private GameObject spawnedAvatar;
        private bool[] previousButtonStates = new bool[4];

        protected override void Awake()
        {
            base.Awake();

            // Enable multiplayer
            enableMultiplayer = true;
            maxPlayers = numberOfLiveActors + numberOfPlayers;

            // Find or create facial controller
            if (facialController == null)
            {
                facialController = GetComponentInChildren<FacialAnimationController>();
            }

            // Find or create serial device controller
            if (costumeController == null)
            {
                costumeController = GetComponent<SerialDeviceController>();
                if (costumeController == null)
                {
                    costumeController = gameObject.AddComponent<SerialDeviceController>();
                }
            }

            // Find or create experience loop
            if (experienceLoop == null)
            {
                experienceLoop = GetComponent<ExperienceStateMachine>();
                if (experienceLoop == null)
                {
                    experienceLoop = gameObject.AddComponent<ExperienceStateMachine>();
                }
            }
        }

        protected override bool InitializeExperienceImpl()
        {
            // Spawn avatar if needed
            if (avatarPrefab != null && spawnedAvatar == null)
            {
                spawnedAvatar = Instantiate(avatarPrefab, transform);
                
                // Find facial controller and mesh on spawned avatar
                if (facialController == null)
                {
                    facialController = spawnedAvatar.GetComponentInChildren<FacialAnimationController>();
                }
                
                if (liveActorMesh == null)
                {
                    liveActorMesh = spawnedAvatar.GetComponentInChildren<SkinnedMeshRenderer>();
                }
            }

            // Initialize facial animation (autonomous)
            if (facialController != null)
            {
                if (!facialController.Initialize())
                {
                    Debug.LogWarning("[LBEAST] AIFacemaskExperience: Failed to initialize facial animation");
                }
                else
                {
                    facialController.SetAnimationMode(FacialAnimationMode.Live);  // Autonomous AI-driven
                    facialController.StartAnimation();
                    Debug.Log("[LBEAST] AIFacemaskExperience: AI Face initialized (autonomous mode)");
                }
            }

            // Initialize costume controller (wrist-mounted buttons + haptics)
            if (costumeController != null)
            {
                if (!costumeController.ConnectToDevice())
                {
                    Debug.LogWarning("[LBEAST] AIFacemaskExperience: Failed to connect to embedded device");
                }
                else
                {
                    Debug.Log("[LBEAST] AIFacemaskExperience: Wrist controls connected (4 buttons)");
                }
            }

            // Initialize Experience Loop with default states
            if (experienceLoop != null)
            {
                var defaultStates = new System.Collections.Generic.List<ExperienceState>
                {
                    new ExperienceState("Intro", "Introduction sequence"),
                    new ExperienceState("Tutorial", "Player tutorial"),
                    new ExperienceState("Act1", "First act"),
                    new ExperienceState("Act2", "Second act"),
                    new ExperienceState("Finale", "Finale sequence"),
                    new ExperienceState("Credits", "End credits")
                };

                experienceLoop.Initialize(defaultStates);
                experienceLoop.onStateChanged.AddListener(OnExperienceStateChanged);
                experienceLoop.StartExperience();

                Debug.Log($"[LBEAST] AIFacemaskExperience: Experience Loop initialized with {defaultStates.Count} states");
            }

            Debug.Log($"[LBEAST] AIFacemaskExperience: Initialized with {numberOfLiveActors} live actors and {numberOfPlayers} players");
            return true;
        }

        protected override void ShutdownExperienceImpl()
        {
            // Stop experience loop
            if (experienceLoop != null)
            {
                experienceLoop.StopExperience();
            }

            // Stop facial animation
            if (facialController != null)
            {
                facialController.StopAnimation();
            }

            // Disconnect embedded systems
            if (costumeController != null)
            {
                costumeController.DisconnectFromDevice();
            }

            // Clean up spawned avatar
            if (spawnedAvatar != null)
            {
                Destroy(spawnedAvatar);
            }
        }

        private void Update()
        {
            if (!isInitialized)
                return;

            // Process button input from wrist-mounted controls
            ProcessButtonInput();
        }

        private void ProcessButtonInput()
        {
            if (costumeController == null || !costumeController.IsConnected() || experienceLoop == null)
            {
                return;
            }

            // Read current button states
            bool[] currentButtonStates = new bool[4];
            for (int i = 0; i < 4; i++)
            {
                currentButtonStates[i] = costumeController.IsButtonPressed(i);
            }

            // Button 0 (Left Wrist Forward) or Button 2 (Right Wrist Forward)
            if ((currentButtonStates[0] && !previousButtonStates[0]) ||
                (currentButtonStates[2] && !previousButtonStates[2]))
            {
                AdvanceExperience();
            }

            // Button 1 (Left Wrist Backward) or Button 3 (Right Wrist Backward)
            if ((currentButtonStates[1] && !previousButtonStates[1]) ||
                (currentButtonStates[3] && !previousButtonStates[3]))
            {
                RetreatExperience();
            }

            // Store current states for next frame
            for (int i = 0; i < 4; i++)
            {
                previousButtonStates[i] = currentButtonStates[i];
            }
        }

        #region Experience Loop Control

        /// <summary>
        /// Get the current experience state
        /// </summary>
        public string GetCurrentExperienceState()
        {
            if (experienceLoop != null)
            {
                return experienceLoop.GetCurrentStateName();
            }
            return "";
        }

        /// <summary>
        /// Manually advance the experience to the next state (usually triggered by buttons)
        /// </summary>
        public bool AdvanceExperience()
        {
            if (experienceLoop != null)
            {
                return experienceLoop.AdvanceState();
            }
            return false;
        }

        /// <summary>
        /// Manually retreat the experience to the previous state (usually triggered by buttons)
        /// </summary>
        public bool RetreatExperience()
        {
            if (experienceLoop != null)
            {
                return experienceLoop.RetreatState();
            }
            return false;
        }

        /// <summary>
        /// Handle state change events
        /// Override this in your derived class to trigger game events
        /// </summary>
        protected virtual void OnExperienceStateChanged(string oldState, string newState, int newStateIndex)
        {
            Debug.Log($"[LBEAST] AIFacemaskExperience: State changed from '{oldState}' to '{newState}' (Index: {newStateIndex})");

            // Override this method to trigger game events based on state changes
        }

        #endregion

        #region Public API - Live Actor Connection

        /// <summary>
        /// Set the live actor stream IP address
        /// </summary>
        public void SetLiveActorStreamIP(string ip)
        {
            liveActorStreamIP = ip;
        }

        /// <summary>
        /// Set the live actor stream port
        /// </summary>
        public void SetLiveActorStreamPort(int port)
        {
            liveActorStreamPort = port;
        }

        #endregion
    }
}
