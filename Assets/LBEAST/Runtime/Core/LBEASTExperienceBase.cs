// Copyright (c) 2025 AJ Campbell. Licensed under the MIT License.

using System.Net;
using UnityEngine;
using LBEAST.Core.Input;

namespace LBEAST.Core
{
    /// <summary>
    /// Base class for all LBEAST experience templates
    /// Provides common initialization, multiplayer support, and lifecycle management
    /// </summary>
    public abstract class LBEASTExperienceBase : MonoBehaviour
    {
        [Header("Experience Configuration")]
        [SerializeField] protected bool autoInitialize = true;
        [SerializeField] protected bool enableMultiplayer = false;
        [SerializeField] protected int maxPlayers = 1;

        [Header("Components")]
        [Tooltip("Input adapter for handling all input sources (embedded systems, VR, keyboard, etc.)")]
        public LBEASTInputAdapter inputAdapter;

        [Tooltip("Command protocol for receiving remote commands from Command Console (auto-created on dedicated server)")]
        public LBEASTServerCommandProtocol commandProtocol;

        [Header("Core Systems")]
        [SerializeField] protected LBEASTTrackingSystem trackingSystem;
        [SerializeField] protected LBEASTNetworkManager networkManager;

        protected bool isInitialized = false;
        protected bool isRunning = false;

        #region Unity Lifecycle

        protected virtual void Awake()
        {
            // Create input adapter component if not already present
            if (inputAdapter == null)
            {
                inputAdapter = GetComponent<LBEASTInputAdapter>();
                if (inputAdapter == null)
                {
                    inputAdapter = gameObject.AddComponent<LBEASTInputAdapter>();
                    Debug.Log("[LBEAST] InputAdapter component auto-created.");
                }
            }

            // Find or create core systems
            if (trackingSystem == null)
            {
                trackingSystem = FindObjectOfType<LBEASTTrackingSystem>();
                if (trackingSystem == null)
                {
                    GameObject trackingObj = new GameObject("LBEAST_TrackingSystem");
                    trackingSystem = trackingObj.AddComponent<LBEASTTrackingSystem>();
                }
            }

            if (enableMultiplayer && networkManager == null)
            {
                networkManager = FindObjectOfType<LBEASTNetworkManager>();
                if (networkManager == null)
                {
                    GameObject networkObj = new GameObject("LBEAST_NetworkManager");
                    networkManager = networkObj.AddComponent<LBEASTNetworkManager>();
                }
            }
        }

        protected virtual void Start()
        {
            if (autoInitialize)
                InitializeExperience();
        }

        protected virtual void OnDestroy()
        {
            if (isRunning)
                ShutdownExperience();
        }

        #endregion

        #region Experience Lifecycle

        /// <summary>
        /// Initialize the experience and all required systems
        /// </summary>
        public bool InitializeExperience()
        {
            if (isInitialized)
            {
                Debug.LogWarning("[LBEAST] Experience already initialized");
                return true;
            }

            Debug.Log($"[LBEAST] Initializing {GetType().Name}...");

            // Initialize tracking
            if (trackingSystem != null && !trackingSystem.InitializeTracking())
            {
                Debug.LogError("[LBEAST] Failed to initialize tracking system");
                return false;
            }

            // Initialize networking if enabled
            if (enableMultiplayer && networkManager != null)
            {
                if (!networkManager.InitializeNetwork())
                {
                    Debug.LogError("[LBEAST] Failed to initialize network manager");
                    return false;
                }
                networkManager.SetMaxPlayers(maxPlayers);
            }

            // Initialize command protocol if running as dedicated server
            InitializeCommandProtocol();

            // Call derived class initialization
            if (!InitializeExperienceImpl())
            {
                Debug.LogError($"[LBEAST] Failed to initialize {GetType().Name} implementation");
                return false;
            }

            isInitialized = true;
            isRunning = true;

            Debug.Log($"[LBEAST] {GetType().Name} initialized successfully");
            return true;
        }

        /// <summary>
        /// Shutdown the experience and clean up resources
        /// </summary>
        public void ShutdownExperience()
        {
            if (!isRunning)
                return;

            Debug.Log($"[LBEAST] Shutting down {GetType().Name}...");

            // Call derived class shutdown
            ShutdownExperienceImpl();

            // Shutdown networking
            if (enableMultiplayer && networkManager != null)
                networkManager.Shutdown();

            // Stop command protocol if running
            if (commandProtocol != null && commandProtocol.IsListening())
                commandProtocol.StopListening();

            isRunning = false;
            isInitialized = false;

            Debug.Log($"[LBEAST] {GetType().Name} shutdown complete");
        }

        /// <summary>
        /// Restart the experience (shutdown and reinitialize)
        /// </summary>
        public void RestartExperience()
        {
            ShutdownExperience();
            InitializeExperience();
        }

        #endregion

        #region Abstract Methods

        /// <summary>
        /// Derived classes implement their specific initialization logic here
        /// </summary>
        protected abstract bool InitializeExperienceImpl();

        /// <summary>
        /// Derived classes implement their specific shutdown logic here
        /// </summary>
        protected abstract void ShutdownExperienceImpl();

        #endregion

        #region Multiplayer Control

        /// <summary>
        /// Start this experience as a multiplayer host
        /// </summary>
        public bool StartAsHost()
        {
            if (!enableMultiplayer)
            {
                Debug.LogWarning("[LBEAST] Multiplayer not enabled for this experience");
                return false;
            }

            if (networkManager != null)
                return networkManager.StartHost();

            Debug.LogError("[LBEAST] Network manager not available");
            return false;
        }

        /// <summary>
        /// Start this experience as a multiplayer client
        /// </summary>
        public bool StartAsClient()
        {
            if (!enableMultiplayer)
            {
                Debug.LogWarning("[LBEAST] Multiplayer not enabled for this experience");
                return false;
            }

            if (networkManager != null)
                return networkManager.StartClient();

            Debug.LogError("[LBEAST] Network manager not available");
            return false;
        }

        /// <summary>
        /// Get the maximum number of players supported
        /// </summary>
        public int GetMaxPlayers() => maxPlayers;

        #endregion

        #region State Queries

        /// <summary>
        /// Check if the experience is initialized
        /// </summary>
        public bool IsInitialized() => isInitialized;

        /// <summary>
        /// Check if the experience is currently running
        /// </summary>
        public bool IsRunning() => isRunning;

        /// <summary>
        /// Check if multiplayer is enabled for this experience
        /// </summary>
        public bool IsMultiplayerEnabled() => enableMultiplayer;

        #endregion

        #region Command Protocol

        /// <summary>
        /// Initialize command protocol for dedicated server mode
        /// </summary>
        protected virtual void InitializeCommandProtocol()
        {
            // Check if running as dedicated server (in Unity, this would be checked via command-line args or network mode)
            // For now, we'll check if we have a network manager configured as server
            if (!enableMultiplayer || networkManager == null)
                return; // Not running as dedicated server, skip command protocol

            // Create command protocol component if not already present
            if (commandProtocol == null)
                commandProtocol = gameObject.AddComponent<LBEASTServerCommandProtocol>();

            // Start listening for commands
            if (commandProtocol.StartListening())
            {
                // Bind to command received event
                commandProtocol.OnCommandReceived += OnCommandReceived;
                Debug.Log("[LBEAST] Command protocol listening on port 7779");
            }
            else
                Debug.LogWarning("[LBEAST] Failed to start command protocol");
        }

        /// <summary>
        /// Handle incoming command from Command Console
        /// Override this to handle custom commands in derived classes
        /// </summary>
        protected virtual void OnCommandReceived(ServerCommandMessage command, IPEndPoint sender)
        {
            Debug.Log($"[LBEAST] Received command {(int)command.Command} (seq: {command.SequenceNumber})");

            // Handle base commands
            switch (command.Command)
            {
                case ServerCommand.RequestStatus:
                    {
                        // Get current player count
                        int currentPlayerCount = 0;
                        if (networkManager != null)
                        {
                            // NOOP: TODO - Implement proper player count tracking via network manager
                            currentPlayerCount = 0;
                        }

                        // Build status JSON response
                        string statusData = $"{{\"IsRunning\":{isRunning.ToString().ToLower()},\"IsInitialized\":{isInitialized.ToString().ToLower()},\"CurrentPlayers\":{currentPlayerCount},\"MaxPlayers\":{maxPlayers},\"ExperienceState\":\"{(isInitialized ? "Active" : "Idle")}\"}}";

                        // Send response back to client
                        if (commandProtocol != null)
                        {
                            IPEndPoint senderAddr = commandProtocol.GetLastSenderAddress();
                            if (senderAddr != null)
                            {
                                ServerResponseMessage response = new ServerResponseMessage(true, "Status", statusData);
                                commandProtocol.SendResponse(response, senderAddr);
                                Debug.Log($"[LBEAST] Sent status response (Players: {currentPlayerCount}/{maxPlayers})");
                            }
                        }
                        break;
                    }
                case ServerCommand.Shutdown:
                    {
                        Debug.Log("[LBEAST] Shutdown command received");
                        ShutdownExperience();

                        // Send confirmation response
                        if (commandProtocol != null)
                        {
                            IPEndPoint senderAddr = commandProtocol.GetLastSenderAddress();
                            if (senderAddr != null)
                            {
                                ServerResponseMessage response = new ServerResponseMessage(true, "Shutdown initiated");
                                commandProtocol.SendResponse(response, senderAddr);
                            }
                        }
                        break;
                    }
                default:
                    // Other commands handled by derived classes
                    break;
            }
        }

        #endregion
    }
}

