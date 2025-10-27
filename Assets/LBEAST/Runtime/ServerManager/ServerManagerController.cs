// Copyright (c) 2025 AJ Campbell. Licensed under the MIT License.

using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using UnityEngine;
using LBEAST.Core;

namespace LBEAST.ServerManager
{
    /// <summary>
    /// Server configuration data
    /// </summary>
    [System.Serializable]
    public class ServerConfiguration
    {
        public string ExperienceType = "AIFacemask";
        public string ServerName = "LBEAST Server";
        public int MaxPlayers = 4;
        public int Port = 7777;
        public string SceneName = "LBEASTScene";
    }

    /// <summary>
    /// Server runtime status
    /// </summary>
    [System.Serializable]
    public class ServerStatus
    {
        public bool IsRunning = false;
        public int CurrentPlayers = 0;
        public string ExperienceState = "Idle";
        public float Uptime = 0.0f;
        public int ProcessID = 0;
    }

    /// <summary>
    /// Omniverse Audio2Face status
    /// </summary>
    [System.Serializable]
    public class OmniverseStatus
    {
        public bool IsConnected = false;
        public string StreamStatus = "Inactive";
        public int ActiveFaceStreams = 0;
    }

    /// <summary>
    /// LBEAST Server Manager Controller
    /// 
    /// Unity implementation of the server management system.
    /// Manages dedicated game servers, monitors status, and integrates with Omniverse.
    /// </summary>
    public class ServerManagerController : MonoBehaviour
    {
        [Header("Configuration")]
        public ServerConfiguration ServerConfig = new ServerConfiguration();

        [Header("Status (Read-Only)")]
        public ServerStatus ServerStatus = new ServerStatus();
        public OmniverseStatus OmniverseStatus = new OmniverseStatus();

        [Header("Settings")]
        [SerializeField] private float statusPollInterval = 1.0f;

        // Events for UI updates
        public event Action<string> OnLogMessageAdded;
        public event Action<ServerStatus> OnServerStatusChanged;

        // Private members
        private Process serverProcess;
        private LBEASTServerBeacon serverBeacon;
        private float statusPollTimer = 0.0f;

        private void Awake()
        {
            // Initialize server beacon for real-time status updates
            serverBeacon = gameObject.AddComponent<LBEASTServerBeacon>();
            serverBeacon.OnServerDiscovered += OnServerStatusReceived;
            serverBeacon.StartClientDiscovery();

            AddLogMessage("Server Manager initialized");
            AddLogMessage("Server status beacon initialized (listening on port 7778)");
        }

        private void Update()
        {
            // Poll server status
            statusPollTimer += Time.deltaTime;
            if (statusPollTimer >= statusPollInterval)
            {
                statusPollTimer = 0.0f;
                PollServerStatus();
            }

            // Update uptime if server is running
            if (ServerStatus.IsRunning)
            {
                ServerStatus.Uptime += Time.deltaTime;
                OnServerStatusChanged?.Invoke(ServerStatus);
            }
        }

        /// <summary>
        /// Start the dedicated server with current configuration
        /// </summary>
        public bool StartServer()
        {
            if (ServerStatus.IsRunning)
            {
                AddLogMessage("ERROR: Server is already running");
                return false;
            }

            string serverPath = GetServerExecutablePath();
            if (!System.IO.File.Exists(serverPath))
            {
                AddLogMessage($"ERROR: Server executable not found at {serverPath}");
                AddLogMessage("Please build the dedicated server target first.");
                return false;
            }

            string commandLine = BuildServerCommandLine();
            AddLogMessage($"Starting server: {serverPath} {commandLine}");

            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = serverPath,
                    Arguments = commandLine,
                    UseShellExecute = false,
                    CreateNoWindow = false,
                    RedirectStandardOutput = false,
                    RedirectStandardError = false
                };

                serverProcess = Process.Start(startInfo);

                if (serverProcess != null)
                {
                    ServerStatus.IsRunning = true;
                    ServerStatus.Uptime = 0.0f;
                    ServerStatus.ExperienceState = "Starting...";
                    ServerStatus.ProcessID = serverProcess.Id;

                    AddLogMessage($"Server started successfully (PID: {ServerStatus.ProcessID})");
                    AddLogMessage("Listening for server status broadcasts...");
                    OnServerStatusChanged?.Invoke(ServerStatus);
                    return true;
                }
                else
                {
                    AddLogMessage("ERROR: Failed to start server process");
                    return false;
                }
            }
            catch (Exception ex)
            {
                AddLogMessage($"ERROR: Exception starting server: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Stop the running dedicated server
        /// </summary>
        public bool StopServer()
        {
            if (!ServerStatus.IsRunning)
            {
                AddLogMessage("ERROR: No server is currently running");
                return false;
            }

            if (serverProcess == null || serverProcess.HasExited)
            {
                AddLogMessage("ERROR: Server process not found or already exited");
                ServerStatus.IsRunning = false;
                OnServerStatusChanged?.Invoke(ServerStatus);
                return false;
            }

            AddLogMessage("Stopping server...");

            try
            {
                serverProcess.Kill();
                serverProcess.WaitForExit(5000); // Wait up to 5 seconds
                serverProcess.Dispose();
                serverProcess = null;

                ServerStatus.IsRunning = false;
                ServerStatus.CurrentPlayers = 0;
                ServerStatus.ExperienceState = "Stopped";
                ServerStatus.ProcessID = 0;

                AddLogMessage("Server stopped");
                OnServerStatusChanged?.Invoke(ServerStatus);
                return true;
            }
            catch (Exception ex)
            {
                AddLogMessage($"ERROR: Exception stopping server: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Get available experience types
        /// </summary>
        public string[] GetAvailableExperienceTypes()
        {
            return new string[]
            {
                "AIFacemask",
                "MovingPlatform",
                "Gunship",
                "CarSim",
                "FlightSim"
            };
        }

        /// <summary>
        /// Add a log message
        /// </summary>
        public void AddLogMessage(string message)
        {
            string timestamp = System.DateTime.Now.ToString("HH:mm:ss");
            string logEntry = $"[{timestamp}] {message}";
            UnityEngine.Debug.Log($"[ServerManager] {message}");
            OnLogMessageAdded?.Invoke(logEntry);
        }

        /// <summary>
        /// Open Omniverse configuration panel
        /// </summary>
        public void OpenOmniverseConfig()
        {
            AddLogMessage("Omniverse configuration not yet implemented");
            // TODO: Open Omniverse configuration dialog
        }

        private void PollServerStatus()
        {
            if (!ServerStatus.IsRunning)
            {
                return;
            }

            // Check if process is still running
            if (serverProcess != null && serverProcess.HasExited)
            {
                AddLogMessage("WARNING: Server process terminated unexpectedly");
                ServerStatus.IsRunning = false;
                ServerStatus.CurrentPlayers = 0;
                ServerStatus.ExperienceState = "Crashed";
                OnServerStatusChanged?.Invoke(ServerStatus);
            }

            // Real-time status updates come via OnServerStatusReceived callback
            // from the network beacon. This function just verifies process is alive.
        }

        private void OnServerStatusReceived(ServerInfo serverInfo)
        {
            // Only process status for our managed server
            // (Match by port since IP might be reported differently for localhost)
            if (serverInfo.ServerPort != ServerConfig.Port)
            {
                return; // This is a different server on the network
            }

            // Only process if our server is marked as running
            if (!ServerStatus.IsRunning)
            {
                return;
            }

            // Update real-time status from server broadcast
            bool stateChanged = false;

            if (ServerStatus.CurrentPlayers != serverInfo.CurrentPlayers)
            {
                ServerStatus.CurrentPlayers = serverInfo.CurrentPlayers;
                stateChanged = true;
                AddLogMessage($"Player count changed to: {ServerStatus.CurrentPlayers}/{ServerConfig.MaxPlayers}");
            }

            if (ServerStatus.ExperienceState != serverInfo.ExperienceState)
            {
                ServerStatus.ExperienceState = serverInfo.ExperienceState;
                stateChanged = true;
                AddLogMessage($"Server state changed to: {ServerStatus.ExperienceState}");
            }

            if (stateChanged)
            {
                OnServerStatusChanged?.Invoke(ServerStatus);
            }
        }

        private string GetServerExecutablePath()
        {
            // Build path to dedicated server executable
            string projectDir = Application.dataPath.Replace("/Assets", "");
            string buildDir = System.IO.Path.Combine(projectDir, "Builds", "Server");

#if UNITY_STANDALONE_WIN
            string executableName = "LBEAST_UnityServer.exe";
#elif UNITY_STANDALONE_LINUX
            string executableName = "LBEAST_UnityServer.x86_64";
#elif UNITY_STANDALONE_OSX
            string executableName = "LBEAST_UnityServer.app";
#else
            string executableName = "LBEAST_UnityServer";
#endif

            return System.IO.Path.Combine(buildDir, executableName);
        }

        private string BuildServerCommandLine()
        {
            // Unity dedicated server command-line arguments
            string args = $"-batchmode -nographics";
            args += $" -port {ServerConfig.Port}";
            args += $" -scene {ServerConfig.SceneName}";
            args += $" -experienceType {ServerConfig.ExperienceType}";
            args += $" -maxPlayers {ServerConfig.MaxPlayers}";
            args += $" -logFile ServerLog.txt";

            return args;
        }

        private void OnDestroy()
        {
            // Clean up
            if (serverProcess != null && !serverProcess.HasExited)
            {
                serverProcess.Kill();
                serverProcess.Dispose();
            }

            if (serverBeacon != null)
            {
                serverBeacon.Stop();
            }
        }
    }
}

