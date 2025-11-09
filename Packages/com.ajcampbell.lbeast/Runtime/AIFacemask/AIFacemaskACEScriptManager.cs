// Copyright (c) 2025 AJ Campbell. Licensed under the MIT License.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace LBEAST.AIFacemask
{
    /// <summary>
    /// Event for script playback events
    /// </summary>
    [System.Serializable]
    public class ACEScriptStartedEvent : UnityEvent<string, AIFacemaskACEScript> { }

    [System.Serializable]
    public class ACEScriptLineStartedEvent : UnityEvent<string, int, AIFacemaskACEScriptLine> { }

    [System.Serializable]
    public class ACEScriptFinishedEvent : UnityEvent<string, AIFacemaskACEScript> { }

    [System.Serializable]
    public class ACEScriptPreBakeCompleteEvent : UnityEvent<string> { }

    /// <summary>
    /// NVIDIA ACE Script Manager Component
    /// 
    /// Manages pre-baked script collections for NVIDIA ACE facemask performances.
    /// Automatically triggers scripts when narrative states change.
    /// 
    /// WORKFLOW:
    /// 1. Define script collection (text prompts + voice/emotion settings)
    /// 2. Pre-bake scripts on ACE server (Text-to-Speech → Audio, Audio-to-Face → Facial data)
    /// 3. When narrative state changes, corresponding script is automatically triggered
    /// 4. ACE server streams pre-baked facial animation data to AIFaceController
    /// 
    /// INTEGRATION:
    /// - Subscribes to narrative state machine OnStateChanged events
    /// - Maps narrative states to pre-baked ACE scripts
    /// - Communicates with NVIDIA ACE server for pre-baking and playback
    /// - Supports advance/retreat through script lines
    /// </summary>
    [AddComponentMenu("LBEAST/AI Facemask ACE Script Manager")]
    public class AIFacemaskACEScriptManager : MonoBehaviour
    {
        [Header("Configuration")]
        [Tooltip("Script collection for this experience")]
        public AIFacemaskACEScriptCollection scriptCollection = new AIFacemaskACEScriptCollection();

        [Tooltip("NVIDIA ACE server base URL (e.g., \"http://192.168.1.100:8000\")")]
        public string aceServerBaseURL = "http://localhost:8000";

        [Tooltip("Whether to auto-trigger scripts on narrative state changes")]
        public bool autoTriggerOnStateChange = true;

        [Header("Status (Read-Only)")]
        [Tooltip("Currently playing script (if any)")]
        public AIFacemaskACEScript currentScript = new AIFacemaskACEScript();

        [Tooltip("Current script line index being played")]
        public int currentScriptLineIndex = -1;

        [Tooltip("Whether a script is currently playing")]
        public bool isPlayingScript = false;

        [Header("Events")]
        [Tooltip("Event fired when a script starts playing")]
        public ACEScriptStartedEvent OnScriptStarted = new ACEScriptStartedEvent();

        [Tooltip("Event fired when a script line starts playing")]
        public ACEScriptLineStartedEvent OnScriptLineStarted = new ACEScriptLineStartedEvent();

        [Tooltip("Event fired when a script finishes playing")]
        public ACEScriptFinishedEvent OnScriptFinished = new ACEScriptFinishedEvent();

        [Tooltip("Event fired when script pre-baking completes")]
        public ACEScriptPreBakeCompleteEvent OnScriptPreBakeComplete = new ACEScriptPreBakeCompleteEvent();

        private bool isInitialized = false;
        private float scriptPlaybackTimer = 0.0f;
        private float currentScriptLineStartTime = 0.0f;
        private bool waitingForStartDelay = false;
        private float startDelayTimer = 0.0f;

        #region Unity Lifecycle

        private void Update()
        {
            if (!isInitialized || !isPlayingScript)
            {
                return;
            }

            // Handle start delay
            if (waitingForStartDelay)
            {
                startDelayTimer += Time.deltaTime;
                if (startDelayTimer >= currentScript.startDelay)
                {
                    waitingForStartDelay = false;
                    startDelayTimer = 0.0f;
                    // Start playing first script line
                    if (currentScript.scriptLines != null && currentScript.scriptLines.Count > 0)
                    {
                        StartScriptLine(0);
                    }
                }
                return;
            }

            // Handle script line playback
            if (currentScriptLineIndex >= 0 && currentScriptLineIndex < currentScript.scriptLines.Count)
            {
                AIFacemaskACEScriptLine currentLine = currentScript.scriptLines[currentScriptLineIndex];

                scriptPlaybackTimer += Time.deltaTime;

                // Check if current line has finished (if we have duration estimate)
                if (currentLine.estimatedDuration > 0.0f)
                {
                    float elapsedTime = scriptPlaybackTimer - currentScriptLineStartTime;
                    if (elapsedTime >= currentLine.estimatedDuration)
                    {
                        // Advance to next line or finish script
                        AdvanceToNextScriptLine();
                    }
                }
                // If no duration estimate, we rely on ACE server to signal completion
                // (This would require additional integration with ACE server callbacks)
            }
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Initialize the script manager
        /// </summary>
        /// <param name="inACEServerBaseURL">Base URL for NVIDIA ACE server</param>
        /// <returns>true if initialization was successful</returns>
        public bool InitializeScriptManager(string inACEServerBaseURL)
        {
            if (isInitialized)
            {
                Debug.LogWarning("[LBEAST] AIFacemaskACEScriptManager: Already initialized");
                return true;
            }

            aceServerBaseURL = inACEServerBaseURL;
            isInitialized = true;

            Debug.Log($"[LBEAST] AIFacemaskACEScriptManager: Initialized with ACE server URL: {aceServerBaseURL}");

            return true;
        }

        #endregion

        #region Script Playback

        /// <summary>
        /// Trigger a script for a specific narrative state
        /// </summary>
        /// <param name="stateName">Narrative state name to trigger script for</param>
        /// <returns>true if script was found and triggered</returns>
        public bool TriggerScriptForState(string stateName)
        {
            if (!isInitialized)
            {
                Debug.LogWarning("[LBEAST] AIFacemaskACEScriptManager: Cannot trigger script - not initialized");
                return false;
            }

            // Stop any currently playing script
            if (isPlayingScript)
            {
                StopCurrentScript();
            }

            // Find script for this state
            AIFacemaskACEScript script = scriptCollection.GetScriptForState(stateName);
            if (script == null)
            {
                Debug.LogWarning($"[LBEAST] AIFacemaskACEScriptManager: No script found for state '{stateName}'");
                return false;
            }

            // Check if script is pre-baked
            if (!script.isFullyPreBaked)
            {
                Debug.LogWarning($"[LBEAST] AIFacemaskACEScriptManager: Script for state '{stateName}' is not pre-baked. Pre-baking now...");
                // Attempt to pre-bake synchronously (this may take time)
                PreBakeScriptForState(stateName, false);
            }

            // Start playing the script
            currentScript = script;
            currentScriptLineIndex = -1;
            scriptPlaybackTimer = 0.0f;
            isPlayingScript = true;
            waitingForStartDelay = (currentScript.startDelay > 0.0f);
            startDelayTimer = 0.0f;

            // Broadcast script started event
            OnScriptStarted?.Invoke(stateName, currentScript);

            Debug.Log($"[LBEAST] AIFacemaskACEScriptManager: Started script for state '{stateName}' ({currentScript.scriptLines.Count} lines)");

            // Request script playback from ACE server
            RequestScriptPlaybackFromACE(currentScript, 0);

            return true;
        }

        /// <summary>
        /// Stop the currently playing script
        /// </summary>
        public void StopCurrentScript()
        {
            if (!isPlayingScript)
            {
                return;
            }

            string currentStateName = currentScript.associatedStateName;

            isPlayingScript = false;
            currentScriptLineIndex = -1;
            scriptPlaybackTimer = 0.0f;
            currentScriptLineStartTime = 0.0f;
            waitingForStartDelay = false;
            startDelayTimer = 0.0f;

            Debug.Log($"[LBEAST] AIFacemaskACEScriptManager: Stopped script for state '{currentStateName}'");

            // Broadcast script finished event
            OnScriptFinished?.Invoke(currentStateName, currentScript);
        }

        /// <summary>
        /// Check if a script exists for a state
        /// </summary>
        public bool HasScriptForState(string stateName)
        {
            return scriptCollection.HasScriptForState(stateName);
        }

        /// <summary>
        /// Get script for a specific state
        /// </summary>
        public AIFacemaskACEScript GetScriptForState(string stateName)
        {
            return scriptCollection.GetScriptForState(stateName);
        }

        /// <summary>
        /// Handle narrative state change (called by experience base)
        /// </summary>
        /// <param name="oldState">Previous state name</param>
        /// <param name="newState">New state name</param>
        /// <param name="newStateIndex">Index of new state</param>
        public void HandleNarrativeStateChanged(string oldState, string newState, int newStateIndex)
        {
            if (!isInitialized || !autoTriggerOnStateChange)
            {
                return;
            }

            // Trigger script for the new state
            TriggerScriptForState(newState);
        }

        #endregion

        #region Pre-Baking

        /// <summary>
        /// Pre-bake all scripts in the collection on the ACE server
        /// This converts text prompts to audio and processes audio-to-face
        /// </summary>
        /// <param name="async">If true, pre-baking happens asynchronously</param>
        public void PreBakeAllScripts(bool async = true)
        {
            if (!isInitialized)
            {
                Debug.LogWarning("[LBEAST] AIFacemaskACEScriptManager: Cannot pre-bake - not initialized");
                return;
            }

            Debug.Log($"[LBEAST] AIFacemaskACEScriptManager: Pre-baking all scripts (async: {async})");

            foreach (var kvp in scriptCollection.scriptsByState)
            {
                PreBakeScriptForState(kvp.Key, async);
            }
        }

        /// <summary>
        /// Pre-bake a specific script for a state
        /// </summary>
        /// <param name="stateName">State name to pre-bake script for</param>
        /// <param name="async">If true, pre-baking happens asynchronously</param>
        public void PreBakeScriptForState(string stateName, bool async = true)
        {
            if (!isInitialized)
            {
                Debug.LogWarning("[LBEAST] AIFacemaskACEScriptManager: Cannot pre-bake - not initialized");
                return;
            }

            AIFacemaskACEScript script = scriptCollection.GetScriptForState(stateName);
            if (script == null)
            {
                Debug.LogWarning($"[LBEAST] AIFacemaskACEScriptManager: No script found for state '{stateName}'");
                return;
            }

            Debug.Log($"[LBEAST] AIFacemaskACEScriptManager: Pre-baking script for state '{stateName}' (async: {async})");

            if (async)
            {
                StartCoroutine(PreBakeScriptCoroutine(script));
            }
            else
            {
                RequestScriptPreBakeFromACE(script);
            }
        }

        private IEnumerator PreBakeScriptCoroutine(AIFacemaskACEScript script)
        {
            RequestScriptPreBakeFromACE(script);
            // NOOP: TODO - Wait for pre-baking to complete (poll status or use callback)
            yield return null;
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Start playing a script line
        /// </summary>
        private void StartScriptLine(int lineIndex)
        {
            if (lineIndex < 0 || lineIndex >= currentScript.scriptLines.Count)
            {
                return;
            }

            currentScriptLineIndex = lineIndex;
            currentScriptLineStartTime = scriptPlaybackTimer;

            AIFacemaskACEScriptLine line = currentScript.scriptLines[lineIndex];

            Debug.Log($"[LBEAST] AIFacemaskACEScriptManager: Started script line {lineIndex} for state '{currentScript.associatedStateName}'");

            // Broadcast script line started event
            OnScriptLineStarted?.Invoke(currentScript.associatedStateName, lineIndex, line);
        }

        /// <summary>
        /// Advance to next script line (or finish script)
        /// </summary>
        private void AdvanceToNextScriptLine()
        {
            if (currentScriptLineIndex < 0 || currentScriptLineIndex >= currentScript.scriptLines.Count - 1)
            {
                // Finished all lines
                FinishCurrentScript();
            }
            else
            {
                // Advance to next line
                StartScriptLine(currentScriptLineIndex + 1);
            }
        }

        /// <summary>
        /// Finish current script
        /// </summary>
        private void FinishCurrentScript()
        {
            string stateName = currentScript.associatedStateName;

            isPlayingScript = false;
            currentScriptLineIndex = -1;
            scriptPlaybackTimer = 0.0f;
            currentScriptLineStartTime = 0.0f;

            Debug.Log($"[LBEAST] AIFacemaskACEScriptManager: Finished script for state '{stateName}'");

            // Broadcast script finished event
            OnScriptFinished?.Invoke(stateName, currentScript);

            // Handle looping
            if (currentScript.loopScript)
            {
                // Restart script
                TriggerScriptForState(stateName);
            }
        }

        /// <summary>
        /// Request script playback from ACE server
        /// NOOP: TODO - Implement HTTP request to ACE server
        /// </summary>
        private void RequestScriptPlaybackFromACE(AIFacemaskACEScript script, int startLineIndex = 0)
        {
            // NOOP: TODO - Implement HTTP request to ACE server
            // 
            // BACKEND-AGNOSTIC IMPLEMENTATION:
            // Supports multiple backends (all available as options):
            // 
            // 1. NVIDIA ACE Server (HTTP REST API)
            //    - Endpoint: {aceServerBaseURL}/api/script/play
            //    - Method: POST
            //    - Body: { script_id, start_line_index, state_name }
            //    - Response: { status, stream_url }
            // 
            // 2. Custom ACE server implementations
            //    - Format depends on server implementation
            //    - Should accept script data and return stream URL or callback endpoint
            //
            // After playback starts, ACE server streams facial animation data to AIFaceController

            Debug.Log($"[LBEAST] AIFacemaskACEScriptManager: Requesting script playback from ACE server for state '{script.associatedStateName}' (start line: {startLineIndex})");
        }

        /// <summary>
        /// Request script pre-baking from ACE server
        /// NOOP: TODO - Implement HTTP request to ACE server for pre-baking
        /// </summary>
        private void RequestScriptPreBakeFromACE(AIFacemaskACEScript script)
        {
            // NOOP: TODO - Implement HTTP request to ACE server for pre-baking
            // 
            // BACKEND-AGNOSTIC IMPLEMENTATION:
            // Supports multiple backends (all available as options):
            // 
            // 1. NVIDIA ACE Server (HTTP REST API)
            //    - Endpoint: {aceServerBaseURL}/api/script/prebake
            //    - Method: POST
            //    - Body: { script_data (all lines with text prompts, voice settings, etc.) }
            //    - Response: { status, prebake_id, estimated_duration }
            // 
            // 2. Custom ACE server implementations
            //    - Format depends on server implementation
            //    - Should accept script data and return pre-baking status
            //
            // Pre-baking process:
            // 1. Text prompts → Text-to-Speech → Audio files (cached on server)
            // 2. Audio files → Audio-to-Face → Facial animation data (cached on server)
            // 3. Server returns pre-baking status and estimated durations
            //
            // After pre-baking completes, call OnScriptPreBakeComplete event

            Debug.Log($"[LBEAST] AIFacemaskACEScriptManager: Requesting script pre-baking from ACE server for state '{script.associatedStateName}'");
        }

        #endregion
    }
}

