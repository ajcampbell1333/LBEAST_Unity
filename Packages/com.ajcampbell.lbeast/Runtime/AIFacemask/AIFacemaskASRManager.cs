// Copyright (c) 2025 AJ Campbell. Licensed under the MIT License.

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using LBEAST.VOIP;

namespace LBEAST.AIFacemask
{
    /// <summary>
    /// Event for ASR transcription events
    /// </summary>
    [System.Serializable]
    public class ASRTranscriptionCompleteEvent : UnityEvent<int, string> { }

    [System.Serializable]
    public class ASRTranscriptionStartedEvent : UnityEvent<int> { }

    /// <summary>
    /// NVIDIA ACE ASR Manager Component
    /// 
    /// Handles Automatic Speech Recognition (ASR) for player voice input.
    /// Converts player speech (received via Mumble) to text for improv responses.
    /// 
    /// ARCHITECTURE:
    /// - Runs on dedicated server (receives audio from Mumble)
    /// - Receives audio streams from players via Mumble/VOIP
    /// - Converts speech to text using local ASR (NVIDIA Riva) or cloud ASR
    /// - Triggers improv manager with transcribed text
    /// 
    /// WORKFLOW:
    /// 1. Player speaks into HMD microphone
    /// 2. Audio captured by VOIPManager → Sent to Mumble server
    /// 3. Server receives audio via Mumble → ASR Manager processes it
    /// 4. ASR converts audio → text
    /// 5. Text sent to ACEImprovManager → Generates improvised response
    /// 6. Response converted to facial animation → Streamed to live actor's HMD
    /// 
    /// INTEGRATION:
    /// - Subscribes to VOIPManager audio events (OnRemotePlayerAudioReceived)
    /// - Buffers audio until speech ends (voice activity detection)
    /// - Sends buffered audio to ASR service
    /// - Forwards transcribed text to ACEImprovManager
    /// </summary>
    [AddComponentMenu("LBEAST/AI Facemask ASR Manager")]
    public class AIFacemaskASRManager : MonoBehaviour, IVOIPAudioVisitor
    {
        [Header("Configuration")]
        [Tooltip("Configuration for ASR")]
        public AIFacemaskASRConfig asrConfig = new AIFacemaskASRConfig();

        [Header("Events")]
        [Tooltip("Event fired when transcription completes")]
        public ASRTranscriptionCompleteEvent OnTranscriptionComplete = new ASRTranscriptionCompleteEvent();

        [Tooltip("Event fired when transcription starts")]
        public ASRTranscriptionStartedEvent OnTranscriptionStarted = new ASRTranscriptionStartedEvent();

        [Header("Status (Read-Only)")]
        [Tooltip("Whether the ASR manager is initialized")]
        public bool isInitialized = false;

        /// <summary>
        /// Reference to ACE Improv Manager (for triggering improv after transcription)
        /// Set by AIFacemaskExperience during initialization
        /// </summary>
        [HideInInspector]
        public AIFacemaskACEImprovManager aceImprovManager = null;

        private Dictionary<int, List<float>> playerAudioBuffers = new Dictionary<int, List<float>>();
        private Dictionary<int, float> playerAudioStartTimes = new Dictionary<int, float>();
        private Dictionary<int, bool> playerSpeakingStates = new Dictionary<int, bool>();
        private Dictionary<int, bool> playerTranscribingStates = new Dictionary<int, bool>();
        private float voiceActivityTimer = 0.0f;

        [Tooltip("Silence duration threshold (seconds) - if exceeded, trigger transcription")]
        [Range(0.1f, 5f)]
        public float silenceThreshold = 1.0f;

        #region Unity Lifecycle

        private void Update()
        {
            if (!isInitialized)
            {
                return;
            }

            // Check for silence periods (voice activity detection)
            // If a player was speaking but has been silent for SilenceThreshold seconds, trigger transcription
            voiceActivityTimer += Time.deltaTime;

            if (voiceActivityTimer >= silenceThreshold)
            {
                voiceActivityTimer = 0.0f;

                foreach (var kvp in playerSpeakingStates)
                {
                    int playerId = kvp.Key;
                    bool wasSpeaking = kvp.Value;

                    if (wasSpeaking && !playerTranscribingStates.ContainsKey(playerId))
                    {
                        // Player was speaking but has been silent - trigger transcription
                        TriggerTranscriptionForPlayer(playerId);
                    }
                }
            }
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Initialize the ASR manager
        /// </summary>
        /// <returns>true if initialization was successful</returns>
        public bool InitializeASRManager()
        {
            if (isInitialized)
            {
                Debug.LogWarning("[LBEAST] AIFacemaskASRManager: Already initialized");
                return true;
            }

            if (!asrConfig.enableASR)
            {
                Debug.Log("[LBEAST] AIFacemaskASRManager: ASR is disabled in config");
                return false;
            }

            isInitialized = true;

            Debug.Log($"[LBEAST] AIFacemaskASRManager: Initialized with local ASR: {(asrConfig.useLocalASR ? asrConfig.localASREndpointURL : "Cloud")} (language: {asrConfig.languageCode})");

            return true;
        }

        #endregion

        #region IVOIPAudioVisitor Implementation

        /// <summary>
        /// Called when player audio is received via VOIP/Mumble (implements IVOIPAudioVisitor)
        /// </summary>
        public void OnPlayerAudioReceived(int playerId, float[] audioData, int sampleRate, Vector3 position)
        {
            // This is called by VOIPManager via the visitor interface
            // Process the audio for ASR
            ProcessPlayerAudio(playerId, new List<float>(audioData), sampleRate);
        }

        #endregion

        #region Audio Processing

        /// <summary>
        /// Process audio data from a player (called by VOIPManager when audio is received)
        /// </summary>
        /// <param name="playerId">Player ID who spoke</param>
        /// <param name="audioData">PCM audio data (from Mumble, decoded from Opus)</param>
        /// <param name="sampleRate">Audio sample rate (typically 48000 for Mumble)</param>
        public void ProcessPlayerAudio(int playerId, List<float> audioData, int sampleRate)
        {
            if (!isInitialized || !asrConfig.enableASR)
            {
                return;
            }

            // Detect voice activity
            bool hasVoiceActivity = DetectVoiceActivity(audioData);

            // Update speaking state
            playerSpeakingStates[playerId] = hasVoiceActivity;

            if (hasVoiceActivity)
            {
                // Reset silence timer
                voiceActivityTimer = 0.0f;

                // Add audio to buffer
                if (!playerAudioBuffers.ContainsKey(playerId))
                {
                    playerAudioBuffers[playerId] = new List<float>();
                    playerAudioStartTimes[playerId] = Time.time;
                }

                playerAudioBuffers[playerId].AddRange(audioData);

                // Check if buffer exceeds max duration
                float audioDuration = playerAudioBuffers[playerId].Count / (float)sampleRate;
                if (audioDuration >= asrConfig.maxAudioDuration)
                {
                    // Buffer is full, trigger transcription
                    TriggerTranscriptionForPlayer(playerId);
                }
            }
        }

        /// <summary>
        /// Manually trigger transcription for a player (if audio buffering is enabled)
        /// </summary>
        /// <param name="playerId">Player ID to transcribe</param>
        public void TriggerTranscriptionForPlayer(int playerId)
        {
            if (!playerAudioBuffers.ContainsKey(playerId))
            {
                return;
            }

            List<float> audioBuffer = playerAudioBuffers[playerId];

            // Check minimum duration
            float audioDuration = audioBuffer.Count / 48000.0f;  // Assume 48kHz (Mumble default)
            if (audioDuration < asrConfig.minAudioDuration)
            {
                // Audio too short, clear buffer and skip
                ClearPlayerAudioBuffer(playerId);
                return;
            }

            // Mark as transcribing
            playerTranscribingStates[playerId] = true;
            playerSpeakingStates[playerId] = false;

            // Broadcast transcription started
            OnTranscriptionStarted?.Invoke(playerId);

            Debug.Log($"[LBEAST] AIFacemaskASRManager: Starting transcription for player {playerId} (duration: {audioDuration:F2}s)");

            // Request ASR transcription
            RequestASRTranscription(playerId, audioBuffer, 48000);  // Mumble uses 48kHz

            // Clear buffer
            ClearPlayerAudioBuffer(playerId);
        }

        /// <summary>
        /// Check if a player is currently being transcribed
        /// </summary>
        public bool IsPlayerBeingTranscribed(int playerId)
        {
            return playerTranscribingStates.ContainsKey(playerId) && playerTranscribingStates[playerId];
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Request ASR transcription from local ASR service
        /// NOOP: TODO - Implement gRPC/HTTP request to local ASR service
        /// </summary>
        private void RequestASRTranscription(int playerId, List<float> audioData, int sampleRate)
        {
            // NOOP: TODO - Implement gRPC/HTTP request to local ASR service
            // 
            // BACKEND-AGNOSTIC IMPLEMENTATION:
            // Supports multiple backends (all available as options):
            // 
            // 1. NVIDIA Riva ASR (gRPC on port 50051)
            //    - Service: nvidia.riva.asr.RivaSpeechRecognition
            //    - Method: Recognize
            //    - Input: audio_data (PCM), sample_rate, language_code
            //    - Output: transcript (text)
            // 
            // 2. Other local ASR services (HTTP REST API)
            //    - Standard HTTP POST request
            //    - Format depends on service (e.g., Whisper API, DeepSpeech, etc.)
            //    - Should accept audio and return transcript
            //
            // After transcription completes, call HandleTranscriptionResult()

            Debug.Log($"[LBEAST] AIFacemaskASRManager: Requesting ASR transcription from {asrConfig.localASREndpointURL} (player: {playerId}, samples: {audioData.Count})");

            // For now, simulate transcription result (remove this when implementing actual ASR)
            // HandleTranscriptionResult(playerId, $"[Transcribed speech from player {playerId}]");
        }

        /// <summary>
        /// Handle transcription result and trigger improv
        /// </summary>
        public void HandleTranscriptionResult(int playerId, string transcribedText)
        {
            // Clear transcribing state
            playerTranscribingStates.Remove(playerId);

            if (string.IsNullOrEmpty(transcribedText))
            {
                Debug.LogWarning($"[LBEAST] AIFacemaskASRManager: Transcription returned empty text for player {playerId}");
                return;
            }

            Debug.Log($"[LBEAST] AIFacemaskASRManager: Transcription complete for player {playerId}: '{transcribedText}'");

            // Broadcast transcription complete
            OnTranscriptionComplete?.Invoke(playerId, transcribedText);

            // Auto-trigger improv if enabled
            if (asrConfig.autoTriggerImprov && aceImprovManager != null)
            {
                aceImprovManager.GenerateAndPlayImprovResponse(transcribedText, true);
            }
            else if (asrConfig.autoTriggerImprov)
            {
                Debug.LogWarning($"[LBEAST] AIFacemaskASRManager: Auto-trigger improv enabled but ACEImprovManager not set");
            }
        }

        /// <summary>
        /// Detect voice activity in audio buffer (simple energy-based VAD)
        /// </summary>
        private bool DetectVoiceActivity(List<float> audioData)
        {
            if (audioData == null || audioData.Count == 0)
            {
                return false;
            }

            // Simple energy-based voice activity detection
            // Calculate RMS (Root Mean Square) energy
            float energy = 0.0f;
            foreach (float sample in audioData)
            {
                energy += sample * sample;
            }
            energy = Mathf.Sqrt(energy / audioData.Count);

            // Threshold for voice activity (adjust based on testing)
            float voiceThreshold = 0.01f;  // Normalized audio threshold

            return energy > voiceThreshold;
        }

        /// <summary>
        /// Clear audio buffer for a player
        /// </summary>
        private void ClearPlayerAudioBuffer(int playerId)
        {
            playerAudioBuffers.Remove(playerId);
            playerAudioStartTimes.Remove(playerId);
            playerSpeakingStates.Remove(playerId);
        }

        #endregion
    }
}



