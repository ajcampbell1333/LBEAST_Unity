// Copyright (c) 2025 AJ Campbell. Licensed under the MIT License.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace LBEAST.AIFacemask
{
    /// <summary>
    /// Event for improvised response events
    /// </summary>
    [System.Serializable]
    public class ImprovResponseGeneratedEvent : UnityEvent<string, string> { }

    [System.Serializable]
    public class ImprovResponseStartedEvent : UnityEvent<string> { }

    [System.Serializable]
    public class ImprovResponseFinishedEvent : UnityEvent<string> { }

    /// <summary>
    /// NVIDIA ACE Real-Time Improv Manager Component
    /// 
    /// Handles real-time improvised responses for AI-facemasked actors.
    /// Enables player-to-AI conversations where:
    /// 1. Player provides text input
    /// 2. Local LLM (with optional LoRA) generates improvised response
    /// 3. Local TTS (NVIDIA Riva) converts text → audio
    /// 4. Local Audio2Face (NVIDIA NIM) converts audio → facial animation
    /// 5. Facial animation streamed to AIFaceController in real-time
    /// 
    /// ALL PROCESSING IS LOCAL - No internet connection required for improv responses.
    /// 
    /// ARCHITECTURE:
    /// - Local LLM: Supports Ollama, vLLM, NVIDIA NIM, or any OpenAI-compatible API (with custom LoRA support)
    /// - Local TTS: Supports NVIDIA Riva (gRPC), or any HTTP REST TTS service
    /// - Local Audio2Face: Supports NVIDIA NIM Audio2Face, Audio2Face-3D plugin, or other Audio2Face services
    /// - All components run on the same dedicated server PC as the Unity Engine server
    /// - Developers can mix and match backends based on their needs, hardware, and preferences
    /// 
    /// FLEXIBLE BACKEND SUPPORT:
    /// The system is backend-agnostic - configure endpoint URLs to point to any compatible service.
    /// Example configurations:
    /// - Option 1: NVIDIA NIM (all NVIDIA services) - Recommended for best integration
    /// - Option 2: Ollama + Riva + Audio2Face - Good for open-source stack
    /// - Option 3: vLLM + Riva + Audio2Face - Good for high-performance LLM inference
    /// - Option 4: Mix and match - Use best tool for each component
    /// </summary>
    [AddComponentMenu("LBEAST/AI Facemask ACE Improv Manager")]
    public class AIFacemaskACEImprovManager : MonoBehaviour
    {
        [Header("Configuration")]
        [Tooltip("Configuration for improvised responses")]
        public AIFacemaskACEImprovConfig improvConfig = new AIFacemaskACEImprovConfig();

        [Header("Conversation")]
        [Tooltip("Conversation history (for context-aware responses)")]
        public List<string> conversationHistory = new List<string>();

        [Tooltip("Maximum conversation history entries to keep")]
        [Range(1, 50)]
        public int maxConversationHistory = 10;

        [Header("Status (Read-Only)")]
        [Tooltip("Whether we're currently generating a response")]
        public bool isGeneratingResponse = false;

        [Tooltip("Current player input being processed")]
        public string currentPlayerInput = "";

        [Tooltip("Current AI response being generated/played")]
        public string currentAIResponse = "";

        [Header("Events")]
        [Tooltip("Event fired when an improvised response is generated (text only)")]
        public ImprovResponseGeneratedEvent OnImprovResponseGenerated = new ImprovResponseGeneratedEvent();

        [Tooltip("Event fired when improvised response playback starts (audio + facial animation)")]
        public ImprovResponseStartedEvent OnImprovResponseStarted = new ImprovResponseStartedEvent();

        [Tooltip("Event fired when improvised response playback finishes")]
        public ImprovResponseFinishedEvent OnImprovResponseFinished = new ImprovResponseFinishedEvent();

        /// <summary>
        /// Reference to AIFaceController (for streaming facial animation)
        /// Set by AIFacemaskExperience during initialization
        /// </summary>
        [HideInInspector]
        public AIFaceController aiFaceController = null;

        private bool isInitialized = false;

        #region Unity Lifecycle

        private void Update()
        {
            if (!isInitialized)
            {
                return;
            }

            // NOOP: TODO - Monitor async response generation status
            // Check if LLM/TTS/Audio2Face requests have completed
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Initialize the improv manager
        /// </summary>
        /// <returns>true if initialization was successful</returns>
        public bool InitializeImprovManager()
        {
            if (isInitialized)
            {
                Debug.LogWarning("[LBEAST] AIFacemaskACEImprovManager: Already initialized");
                return true;
            }

            if (!improvConfig.enableImprov)
            {
                Debug.Log("[LBEAST] AIFacemaskACEImprovManager: Improv is disabled in config");
                return false;
            }

            isInitialized = true;

            Debug.Log($"[LBEAST] AIFacemaskACEImprovManager: Initialized with local LLM: {improvConfig.localLLMEndpointURL}, " +
                      $"Local TTS: {(improvConfig.useLocalTTS ? improvConfig.localTTSEndpointURL : "Cloud")}, " +
                      $"Local Audio2Face: {(improvConfig.useLocalAudio2Face ? improvConfig.localAudio2FaceEndpointURL : "Cloud")}");

            return true;
        }

        #endregion

        #region Response Generation

        /// <summary>
        /// Generate an improvised response to player input
        /// </summary>
        /// <param name="playerInput">Player's text input/question</param>
        /// <returns>Generated AI response text (empty if generation failed)</returns>
        public string GenerateImprovResponse(string playerInput)
        {
            if (!isInitialized || !improvConfig.enableImprov)
            {
                Debug.LogWarning("[LBEAST] AIFacemaskACEImprovManager: Cannot generate response - not initialized or disabled");
                return "";
            }

            if (isGeneratingResponse)
            {
                Debug.LogWarning("[LBEAST] AIFacemaskACEImprovManager: Already generating a response, ignoring new request");
                return "";
            }

            isGeneratingResponse = true;
            currentPlayerInput = playerInput;

            Debug.Log($"[LBEAST] AIFacemaskACEImprovManager: Generating improvised response to: '{playerInput}'");

            // Build conversation context
            string conversationContext = BuildConversationContext(playerInput);

            // Request LLM to generate response
            string aiResponse = RequestLLMResponse(playerInput, improvConfig.characterSystemPrompt, conversationHistory);

            if (!string.IsNullOrEmpty(aiResponse))
            {
                currentAIResponse = aiResponse;

                // Add to conversation history
                conversationHistory.Add($"Player: {playerInput}");
                conversationHistory.Add($"AI: {aiResponse}");

                // Trim conversation history if needed
                if (conversationHistory.Count > maxConversationHistory * 2)  // *2 because we store both player and AI messages
                {
                    conversationHistory.RemoveRange(0, conversationHistory.Count - maxConversationHistory * 2);
                }

                // Broadcast response generated event
                OnImprovResponseGenerated?.Invoke(playerInput, aiResponse);
            }
            else
            {
                Debug.LogError("[LBEAST] AIFacemaskACEImprovManager: Failed to generate LLM response");
            }

            isGeneratingResponse = false;
            return aiResponse;
        }

        /// <summary>
        /// Generate and play an improvised response (text → LLM → TTS → Audio2Face → Facial animation)
        /// This is the main function to call when a player interacts with the AI actor
        /// </summary>
        /// <param name="playerInput">Player's text input/question</param>
        /// <param name="async">If true, generation happens asynchronously</param>
        public void GenerateAndPlayImprovResponse(string playerInput, bool async = true)
        {
            if (!isInitialized || !improvConfig.enableImprov)
            {
                Debug.LogWarning("[LBEAST] AIFacemaskACEImprovManager: Cannot generate and play response - not initialized or disabled");
                return;
            }

            // Generate text response
            string aiResponse = GenerateImprovResponse(playerInput);

            if (string.IsNullOrEmpty(aiResponse))
            {
                Debug.LogError("[LBEAST] AIFacemaskACEImprovManager: Failed to generate text response, cannot play");
                return;
            }

            Debug.Log($"[LBEAST] AIFacemaskACEImprovManager: Playing improvised response: '{aiResponse}'");

            // Broadcast response started event
            OnImprovResponseStarted?.Invoke(aiResponse);

            // Convert text to speech (local TTS)
            if (improvConfig.useLocalTTS)
            {
                RequestTTSConversion(aiResponse, improvConfig.voiceType);
            }
            else
            {
                // NOOP: TODO - Use cloud TTS
                Debug.LogWarning("[LBEAST] AIFacemaskACEImprovManager: Cloud TTS not yet implemented");
            }

            // NOOP: TODO - After TTS completes, automatically trigger Audio2Face conversion
            // This should happen in the TTS completion callback
        }

        /// <summary>
        /// Clear conversation history
        /// </summary>
        public void ClearConversationHistory()
        {
            conversationHistory.Clear();
            Debug.Log("[LBEAST] AIFacemaskACEImprovManager: Conversation history cleared");
        }

        /// <summary>
        /// Check if improv is currently generating/playing a response
        /// </summary>
        public bool IsGeneratingResponse()
        {
            return isGeneratingResponse;
        }

        /// <summary>
        /// Stop current improv response generation/playback
        /// </summary>
        public void StopCurrentResponse()
        {
            if (!isGeneratingResponse)
            {
                return;
            }

            isGeneratingResponse = false;
            currentPlayerInput = "";
            currentAIResponse = "";

            Debug.Log("[LBEAST] AIFacemaskACEImprovManager: Stopped current response generation");
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Request LLM to generate response (local LLM endpoint)
        /// NOOP: TODO - Implement HTTP request to local LLM (Ollama, vLLM, or NVIDIA NIM)
        /// </summary>
        private string RequestLLMResponse(string playerInput, string systemPrompt, List<string> conversationHistory)
        {
            // NOOP: TODO - Implement HTTP request to local LLM endpoint
            // 
            // BACKEND-AGNOSTIC IMPLEMENTATION:
            // This function should detect the endpoint type and use appropriate API format.
            // Supports multiple backends (all available as options):
            // 
            // 1. Ollama API (http://localhost:11434/api/generate)
            //    - POST /api/generate
            //    - Body: {"model": "llama3.2:3b", "prompt": "...", "stream": false}
            //    - Supports custom LoRA models via model name
            // 
            // 2. vLLM API (http://localhost:8000/v1/chat/completions)
            //    - POST /v1/chat/completions
            //    - OpenAI-compatible format
            //    - Supports LoRA via model name or API parameters
            // 
            // 3. NVIDIA NIM API (http://localhost:8000/v1/chat/completions)
            //    - POST /v1/chat/completions
            //    - OpenAI-compatible format
            //    - NVIDIA-optimized inference
            // 
            // 4. Any OpenAI-compatible API endpoint
            //    - Standard OpenAI API format
            //    - Works with any compatible service

            Debug.Log($"[LBEAST] AIFacemaskACEImprovManager: Requesting LLM response from {improvConfig.localLLMEndpointURL} (model: {improvConfig.llmModelName})");

            // For now, return placeholder
            return $"[Improv Response to: {playerInput}]";
        }

        /// <summary>
        /// Request TTS conversion (local TTS endpoint - NVIDIA Riva or other)
        /// NOOP: TODO - Implement gRPC/HTTP request to local TTS service
        /// </summary>
        private void RequestTTSConversion(string text, ACEVoiceType voiceType)
        {
            // NOOP: TODO - Implement gRPC/HTTP request to local TTS service
            // 
            // BACKEND-AGNOSTIC IMPLEMENTATION:
            // Supports multiple backends (all available as options):
            // 
            // 1. NVIDIA Riva TTS (gRPC on port 50051)
            //    - Service: nvidia.riva.tts.RivaSpeechSynthesis
            //    - Method: Synthesize
            //    - Input: text, voice_name, sample_rate
            //    - Output: audio_data (PCM/WAV)
            // 
            // 2. Other local TTS services (HTTP REST API)
            //    - Standard HTTP POST request
            //    - Format depends on service (e.g., Coqui TTS, Piper TTS, etc.)
            //    - Should accept text and voice parameters
            //
            // After TTS completes, save audio to temp file and trigger Audio2Face conversion

            Debug.Log($"[LBEAST] AIFacemaskACEImprovManager: Requesting TTS conversion from {improvConfig.localTTSEndpointURL} (voice: {voiceType})");
        }

        /// <summary>
        /// Request Audio2Face conversion (local Audio2Face endpoint - NVIDIA NIM)
        /// NOOP: TODO - Implement HTTP/gRPC request to local Audio2Face microservice
        /// </summary>
        private void RequestAudio2FaceConversion(string audioFilePath)
        {
            // NOOP: TODO - Implement HTTP/gRPC request to local Audio2Face microservice
            // 
            // BACKEND-AGNOSTIC IMPLEMENTATION:
            // Supports multiple backends (all available as options):
            // 
            // 1. NVIDIA NIM Audio2Face (HTTP REST API on port 8000)
            //    - POST /api/audio2face
            //    - Body: {"audio_file": "<base64>", "format": "wav", "stream": true}
            //    - Response: Stream of facial animation data (blend shapes + textures)
            // 
            // 2. Audio2Face-3D plugin (if integrated directly)
            //    - Direct function calls to plugin API
            //    - No HTTP/gRPC needed
            // 
            // 3. Other Audio2Face services
            //    - HTTP/gRPC endpoints as configured
            //    - Should accept audio and return facial animation data
            //
            // Response: Stream of facial animation data (blend shapes + textures)
            // This should be forwarded to AIFaceController.ReceiveFacialAnimationData()

            Debug.Log($"[LBEAST] AIFacemaskACEImprovManager: Requesting Audio2Face conversion from {improvConfig.localAudio2FaceEndpointURL} (audio: {audioFilePath})");
        }

        /// <summary>
        /// Build conversation context for LLM (includes system prompt + history)
        /// </summary>
        private string BuildConversationContext(string playerInput)
        {
            string context = improvConfig.characterSystemPrompt + "\n\n";

            // Add conversation history
            foreach (string historyEntry in conversationHistory)
            {
                context += historyEntry + "\n";
            }

            // Add current player input
            context += $"Player: {playerInput}\nAI:";

            return context;
        }

        #endregion
    }
}



