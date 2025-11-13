// Copyright (c) 2025 AJ Campbell. Licensed under the MIT License.

using System;
using UnityEngine;

namespace LBEAST.AIFacemask
{
    /// <summary>
    /// Configuration for real-time improvised responses
    /// </summary>
    [Serializable]
    public class AIFacemaskACEImprovConfig
    {
        [Header("Improv Settings")]
        [Tooltip("Whether improvised responses are enabled")]
        public bool enableImprov = true;

        [Header("Local LLM Configuration")]
        [Tooltip("Local LLM endpoint URL\n" +
                 "Supports multiple backends:\n" +
                 "- Ollama: \"http://localhost:11434\"\n" +
                 "- vLLM: \"http://localhost:8000\"\n" +
                 "- NVIDIA NIM: \"http://localhost:8000\"\n" +
                 "- Any OpenAI-compatible API endpoint")]
        public string localLLMEndpointURL = "http://localhost:11434";

        [Tooltip("LLM model name/ID\n" +
                 "Examples:\n" +
                 "- Ollama: \"llama3.2:3b\", \"mistral:7b\", or custom LoRA model name\n" +
                 "- vLLM/NIM: Model name as configured in your deployment\n" +
                 "- Custom LoRA: Specify the LoRA model identifier")]
        public string llmModelName = "llama3.2:3b";

        [Tooltip("System prompt/character context for the AI actor")]
        [TextArea(5, 10)]
        public string characterSystemPrompt = "You are a helpful AI character in a VR experience.";

        [Tooltip("Maximum response length in tokens")]
        [Range(10, 500)]
        public int maxResponseTokens = 150;

        [Tooltip("Temperature for LLM generation (0.0 = deterministic, 1.0+ = creative)")]
        [Range(0f, 2f)]
        public float llmTemperature = 0.7f;

        [Header("Local TTS Configuration")]
        [Tooltip("Whether to use local TTS or cloud TTS")]
        public bool useLocalTTS = true;

        [Tooltip("Local TTS endpoint URL\n" +
                 "Supports multiple backends:\n" +
                 "- NVIDIA Riva TTS (gRPC): \"localhost:50051\"\n" +
                 "- Other TTS services: HTTP REST API endpoints\n" +
                 "- Format depends on service (gRPC for Riva, HTTP for others)")]
        public string localTTSEndpointURL = "http://localhost:50051";  // Riva TTS default gRPC port

        [Tooltip("Voice type for improvised responses")]
        public ACEVoiceType voiceType = ACEVoiceType.Default;

        [Header("Local Audio2Face Configuration")]
        [Tooltip("Whether to use local Audio2Face or cloud Audio2Face")]
        public bool useLocalAudio2Face = true;

        [Tooltip("Local Audio2Face endpoint URL\n" +
                 "Supports multiple backends:\n" +
                 "- NVIDIA NIM Audio2Face: \"http://localhost:8000\"\n" +
                 "- Audio2Face-3D plugin: Direct integration (if available)\n" +
                 "- Other Audio2Face services: HTTP/gRPC endpoints")]
        public string localAudio2FaceEndpointURL = "http://localhost:8000";  // NIM Audio2Face default

        public AIFacemaskACEImprovConfig()
        {
            enableImprov = true;
            localLLMEndpointURL = "http://localhost:11434";
            llmModelName = "llama3.2:3b";
            characterSystemPrompt = "You are a helpful AI character in a VR experience.";
            maxResponseTokens = 150;
            llmTemperature = 0.7f;
            useLocalTTS = true;
            localTTSEndpointURL = "http://localhost:50051";
            voiceType = ACEVoiceType.Default;
            useLocalAudio2Face = true;
            localAudio2FaceEndpointURL = "http://localhost:8000";
        }
    }
}



