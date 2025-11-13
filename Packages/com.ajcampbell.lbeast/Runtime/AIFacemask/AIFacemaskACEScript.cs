// Copyright (c) 2025 AJ Campbell. Licensed under the MIT License.

using System;
using System.Collections.Generic;
using UnityEngine;

namespace LBEAST.AIFacemask
{
    /// <summary>
    /// Complete script for a narrative state
    /// 
    /// Contains all script lines that will be performed when this narrative state is active.
    /// Scripts are pre-baked on the NVIDIA ACE server:
    /// - Text prompts → Text-to-Speech → Audio files (cached on ACE server)
    /// - Audio files → Audio-to-Face → Facial animation data (cached on ACE server)
    /// 
    /// When narrative state changes, the corresponding script is triggered and played sequentially.
    /// </summary>
    [Serializable]
    public class AIFacemaskACEScript
    {
        [Header("Script Configuration")]
        [Tooltip("Narrative state name this script is associated with")]
        public string associatedStateName = "";

        [Tooltip("Human-readable description of this script")]
        [TextArea(2, 5)]
        public string description = "";

        [Tooltip("Script lines to perform (played sequentially)")]
        public List<AIFacemaskACEScriptLine> scriptLines = new List<AIFacemaskACEScriptLine>();

        [Tooltip("Whether to loop this script (repeat when finished)")]
        public bool loopScript = false;

        [Tooltip("Delay before starting script playback (seconds)")]
        [Range(0f, 10f)]
        public float startDelay = 0.0f;

        [Header("Pre-Baking Status (Read-Only)")]
        [Tooltip("Total estimated duration (sum of all script lines)")]
        public float totalEstimatedDuration = 0.0f;

        [Tooltip("Whether all script lines have been pre-baked")]
        public bool isFullyPreBaked = false;

        public AIFacemaskACEScript()
        {
            associatedStateName = "";
            description = "";
            loopScript = false;
            startDelay = 0.0f;
            totalEstimatedDuration = 0.0f;
            isFullyPreBaked = false;
        }
    }

    /// <summary>
    /// Collection of pre-baked scripts for NVIDIA ACE facemask performances
    /// 
    /// Maps narrative states to scripts that will be automatically triggered when states change.
    /// Scripts are pre-baked on the NVIDIA ACE server to ensure smooth, low-latency playback.
    /// 
    /// WORKFLOW:
    /// 1. Define scripts in this collection (text prompts + voice/emotion settings)
    /// 2. Pre-bake scripts on ACE server (Text-to-Speech → Audio, Audio-to-Face → Facial data)
    /// 3. When narrative state changes, corresponding script is automatically triggered
    /// 4. ACE server streams pre-baked facial animation data to Unity Engine
    /// 5. AIFaceController receives and applies facial animation in real-time
    /// </summary>
    [Serializable]
    public class AIFacemaskACEScriptCollection
    {
        [Header("Collection Configuration")]
        [Tooltip("Collection name/identifier")]
        public string collectionName = "Default";

        [Tooltip("Scripts mapped by narrative state name")]
        public Dictionary<string, AIFacemaskACEScript> scriptsByState = new Dictionary<string, AIFacemaskACEScript>();

        [Tooltip("Whether to auto-trigger scripts on narrative state changes")]
        public bool autoTriggerOnStateChange = true;

        [Header("Pre-Baking Status (Read-Only)")]
        [Tooltip("Whether all scripts in this collection have been pre-baked")]
        public bool isFullyPreBaked = false;

        public AIFacemaskACEScriptCollection()
        {
            collectionName = "Default";
            autoTriggerOnStateChange = true;
            isFullyPreBaked = false;
        }

        /// <summary>
        /// Get script for a specific narrative state
        /// </summary>
        public AIFacemaskACEScript GetScriptForState(string stateName)
        {
            if (scriptsByState.TryGetValue(stateName, out AIFacemaskACEScript script))
            {
                return script;
            }
            return null;
        }

        /// <summary>
        /// Check if a script exists for a state
        /// </summary>
        public bool HasScriptForState(string stateName)
        {
            return scriptsByState.ContainsKey(stateName);
        }
    }
}



