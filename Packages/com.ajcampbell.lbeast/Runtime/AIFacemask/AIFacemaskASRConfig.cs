// Copyright (c) 2025 AJ Campbell. Licensed under the MIT License.

using System;
using UnityEngine;

namespace LBEAST.AIFacemask
{
    /// <summary>
    /// Configuration for ASR (Automatic Speech Recognition)
    /// </summary>
    [Serializable]
    public class AIFacemaskASRConfig
    {
        [Header("ASR Settings")]
        [Tooltip("Whether ASR is enabled")]
        public bool enableASR = true;

        [Tooltip("Local ASR endpoint URL\n" +
                 "Supports multiple backends:\n" +
                 "- NVIDIA Riva ASR (gRPC): \"localhost:50051\"\n" +
                 "- Other ASR services: HTTP REST API endpoints\n" +
                 "- Format depends on service (gRPC for Riva, HTTP for others)")]
        public string localASREndpointURL = "localhost:50051";  // Riva ASR default gRPC port

        [Tooltip("Whether to use local ASR or cloud ASR")]
        public bool useLocalASR = true;

        [Tooltip("Language code for ASR (e.g., \"en-US\", \"en-GB\")")]
        public string languageCode = "en-US";

        [Tooltip("Minimum audio duration to trigger ASR (seconds) - filters out brief noises")]
        [Range(0.1f, 5f)]
        public float minAudioDuration = 0.5f;

        [Tooltip("Maximum audio duration to process (seconds) - prevents processing very long audio")]
        [Range(1f, 30f)]
        public float maxAudioDuration = 10.0f;

        [Tooltip("Whether to automatically trigger improv after transcription")]
        public bool autoTriggerImprov = true;

        public AIFacemaskASRConfig()
        {
            enableASR = true;
            localASREndpointURL = "localhost:50051";
            useLocalASR = true;
            languageCode = "en-US";
            minAudioDuration = 0.5f;
            maxAudioDuration = 10.0f;
            autoTriggerImprov = true;
        }
    }
}



