// Copyright (c) 2025 AJ Campbell. Licensed under the MIT License.

using UnityEngine;

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

        [Header("Core Systems")]
        [SerializeField] protected LBEASTTrackingSystem trackingSystem;
        [SerializeField] protected LBEASTNetworkManager networkManager;

        protected bool isInitialized = false;
        protected bool isRunning = false;

        #region Unity Lifecycle

        protected virtual void Awake()
        {
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
            {
                InitializeExperience();
            }
        }

        protected virtual void OnDestroy()
        {
            if (isRunning)
            {
                ShutdownExperience();
            }
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
            {
                return;
            }

            Debug.Log($"[LBEAST] Shutting down {GetType().Name}...");

            // Call derived class shutdown
            ShutdownExperienceImpl();

            // Shutdown networking
            if (enableMultiplayer && networkManager != null)
            {
                networkManager.Shutdown();
            }

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
            {
                return networkManager.StartHost();
            }

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
            {
                return networkManager.StartClient();
            }

            Debug.LogError("[LBEAST] Network manager not available");
            return false;
        }

        /// <summary>
        /// Get the maximum number of players supported
        /// </summary>
        public int GetMaxPlayers()
        {
            return maxPlayers;
        }

        #endregion

        #region State Queries

        /// <summary>
        /// Check if the experience is initialized
        /// </summary>
        public bool IsInitialized()
        {
            return isInitialized;
        }

        /// <summary>
        /// Check if the experience is currently running
        /// </summary>
        public bool IsRunning()
        {
            return isRunning;
        }

        /// <summary>
        /// Check if multiplayer is enabled for this experience
        /// </summary>
        public bool IsMultiplayerEnabled()
        {
            return enableMultiplayer;
        }

        #endregion
    }
}

