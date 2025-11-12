// Copyright (c) 2025 AJ Campbell. Licensed under the MIT License.

using System;
using System.Collections.Generic;
using UnityEngine;

namespace LBEAST.AIFacemask
{
    /// <summary>
    /// Single script line/segment for NVIDIA ACE performance
    /// 
    /// Represents one piece of dialogue that will be:
    /// 1. Converted from text-to-speech (TTS) → Audio file (pre-baked or real-time)
    /// 2. Processed through audio-to-face → Facial textures + blend shapes
    /// 3. Streamed to Unity Engine for real-time application
    /// 
    /// Can operate in two modes:
    /// - PreBaked: Text → TTS → Audio-to-Face (all cached on ACE server)
    /// - RealTime: Text → TTS → Audio-to-Face (generated on-the-fly, supports improv)
    /// </summary>
    [Serializable]
    public class AIFacemaskACEScriptLine
    {
        [Header("Script Line Configuration")]
        [Tooltip("Execution mode for this script line")]
        public ACEScriptMode scriptMode = ACEScriptMode.PreBaked;

        [Tooltip("Text prompt/dialogue for this script line")]
        [TextArea(3, 10)]
        public string textPrompt = "";

        [Tooltip("Voice type for text-to-speech conversion")]
        public ACEVoiceType voiceType = ACEVoiceType.Default;

        [Tooltip("Custom voice model ID (if VoiceType is Custom)")]
        public string customVoiceModelID = "";

        [Tooltip("Emotion preset for audio-to-face conversion")]
        public ACEEmotionPreset emotionPreset = ACEEmotionPreset.Neutral;

        [Tooltip("Custom emotion parameters (if EmotionPreset is Custom)")]
        public Dictionary<string, float> customEmotionParams = new Dictionary<string, float>();

        [Header("Pre-Baking Status (Read-Only)")]
        [Tooltip("Pre-baked audio file path (on ACE server) - set after TTS conversion")]
        public string preBakedAudioPath = "";

        [Tooltip("Estimated duration in seconds (calculated after TTS conversion)")]
        public float estimatedDuration = 0.0f;

        [Tooltip("Whether this script line has been pre-baked (TTS + Audio-to-Face processed)")]
        public bool isPreBaked = false;

        [Tooltip("Unique identifier for this script line (for ACE server caching)")]
        public string scriptLineID = "";

        [Tooltip("Whether this is an improvised line (generated dynamically, not from script)")]
        public bool isImprovLine = false;

        public AIFacemaskACEScriptLine()
        {
            // Generate unique ID for this script line
            scriptLineID = Guid.NewGuid().ToString();
        }
    }
}


