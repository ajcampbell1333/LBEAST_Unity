// Copyright (c) 2025 AJ Campbell. Licensed under the MIT License.

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;
using UnityEngine.XR.Hands;
using System.Collections.Generic;

namespace LBEAST.Core
{
    /// <summary>
    /// Hand gesture types that can be recognized
    /// </summary>
    public enum LBEASTHandGesture
    {
        None,
        FistClosed,
        HandOpen,
        Pointing,
        ThumbsUp,
        PeaceSign
    }

    /// <summary>
    /// LBEAST Hand Gesture Recognizer
    /// 
    /// Recognizes hand gestures using Unity's native OpenXR hand tracking APIs (XRHandSubsystem).
    /// Maps gestures to UnityEvents for easy integration with experience templates.
    /// 
    /// Uses Unity's native OpenXR hand tracking - no wrapper components needed.
    /// </summary>
    public class LBEASTHandGestureRecognizer : MonoBehaviour
    {
        [Header("Configuration")]
        [Tooltip("Only process gestures for locally controlled players (multiplayer safety). When false, all players' gestures are processed (useful for debugging or experiences that need to track all players).")]
        [SerializeField] private bool onlyProcessLocalPlayer = true;

        [Tooltip("Fist detection threshold - fingertips must be within this distance (inches) of hand center")]
        [SerializeField] [Range(0.5f, 5.0f)] private float fistDetectionThreshold = 2.0f;  // 2 inches (~5cm)

        [Tooltip("Minimum number of fingertips that must be close to center for fist detection (out of 5)")]
        [SerializeField] [Range(1, 5)] private int minFingersClosedForFist = 4;

        [Tooltip("Update rate for gesture recognition (Hz)")]
        [SerializeField] [Range(1.0f, 120.0f)] private float updateRate = 60.0f;

        [Header("Events")]
        [Tooltip("Event fired when a gesture is detected")]
        public HandGestureEvent OnHandGestureDetected = new HandGestureEvent();

        // UnityEvent for gesture detection
        [System.Serializable]
        public class HandGestureEvent : UnityEvent<bool, LBEASTHandGesture, float> { }

        // Cached references
        private XRHandSubsystem handSubsystem;
        private float updateTimer = 0.0f;

        // Current detected gestures
        private LBEASTHandGesture leftHandGesture = LBEASTHandGesture.None;
        private LBEASTHandGesture rightHandGesture = LBEASTHandGesture.None;

        void Start()
        {
            // Auto-initialize if hand tracking is available
            InitializeRecognizer();
        }

        void Update()
        {
            updateTimer += Time.deltaTime;
            float updateInterval = 1.0f / updateRate;

            if (updateTimer >= updateInterval)
            {
                UpdateGestureRecognition(updateTimer);
                updateTimer = 0.0f;
            }
        }

        /// <summary>
        /// Initialize gesture recognizer
        /// </summary>
        public bool InitializeRecognizer()
        {
            // Get XR Hand Subsystem (Unity's OpenXR hand tracking)
            if (handSubsystem == null)
            {
                // Try to get subsystem directly via SubsystemManager
                List<XRHandSubsystem> subsystems = new List<XRHandSubsystem>();
                SubsystemManager.GetSubsystems(subsystems);
                
                if (subsystems.Count > 0)
                {
                    handSubsystem = subsystems[0];
                }
            }

            if (handSubsystem == null)
            {
                Debug.LogWarning("[LBEAST] LBEASTHandGestureRecognizer: XR Hand Subsystem not available. Ensure OpenXR is enabled and hand tracking is configured in Project Settings > XR Plug-in Management > OpenXR.");
                return false;
            }

            if (!handSubsystem.running)
            {
                Debug.LogWarning("[LBEAST] LBEASTHandGestureRecognizer: Hand tracking subsystem not running");
                return false;
            }

            Debug.Log("[LBEAST] LBEASTHandGestureRecognizer: Initialized");
            return true;
        }

        /// <summary>
        /// Check if a specific hand is in fist state
        /// </summary>
        public bool IsHandFistClosed(bool leftHand)
        {
            if (handSubsystem == null || !handSubsystem.running)
            {
                return false;
            }

            XRHand hand = leftHand ? handSubsystem.leftHand : handSubsystem.rightHand;

            if (!hand.isTracked)
            {
                return false;
            }

            // Get hand center (middle knuckle/MCP joint)
            XRHandJoint handCenterJoint = hand.GetJoint(XRHandJointID.MiddleMetacarpal);
            if (!handCenterJoint.TryGetPose(out Pose handCenterPose))
            {
                return false; // Hand center not tracking
            }

            Vector3 handCenter = handCenterPose.position;

            // Get all fingertip positions
            XRHandJointID[] fingertipKeypoints = {
                XRHandJointID.ThumbTip,
                XRHandJointID.IndexTip,
                XRHandJointID.MiddleTip,
                XRHandJointID.RingTip,
                XRHandJointID.LittleTip
            };

            int fingersClosed = 0;
            foreach (XRHandJointID keypoint in fingertipKeypoints)
            {
                XRHandJoint tipJoint = hand.GetJoint(keypoint);
                if (!tipJoint.TryGetPose(out Pose tipPose))
                {
                    continue; // Tip not tracking
                }

                float distanceToCenter = Vector3.Distance(tipPose.position, handCenter);
                // Convert cm to inches (Unity uses meters, so: distance * 100cm/m / 2.54cm/inch)
                float distanceInches = (distanceToCenter * 100.0f) / 2.54f;

                if (distanceInches < fistDetectionThreshold)
                {
                    fingersClosed++;
                }
            }

            return fingersClosed >= minFingersClosedForFist;
        }

        /// <summary>
        /// Get wrist position for a hand
        /// </summary>
        public Vector3 GetWristPosition(bool leftHand)
        {
            if (handSubsystem == null || !handSubsystem.running)
            {
                return Vector3.zero;
            }

            XRHand hand = leftHand ? handSubsystem.leftHand : handSubsystem.rightHand;
            if (!hand.isTracked)
            {
                return Vector3.zero;
            }

            XRHandJoint wristJoint = hand.GetJoint(XRHandJointID.Wrist);
            if (wristJoint.TryGetPose(out Pose wristPose))
            {
                return wristPose.position;
            }

            return Vector3.zero;
        }

        /// <summary>
        /// Get hand center position (middle knuckle/MCP joint)
        /// </summary>
        public Vector3 GetHandCenterPosition(bool leftHand)
        {
            if (handSubsystem == null || !handSubsystem.running)
            {
                return Vector3.zero;
            }

            XRHand hand = leftHand ? handSubsystem.leftHand : handSubsystem.rightHand;
            if (!hand.isTracked)
            {
                return Vector3.zero;
            }

            XRHandJoint handCenterJoint = hand.GetJoint(XRHandJointID.MiddleMetacarpal);
            if (handCenterJoint.TryGetPose(out Pose handCenterPose))
            {
                return handCenterPose.position;
            }

            return Vector3.zero;
        }

        /// <summary>
        /// Get all fingertip positions
        /// </summary>
        public void GetFingertipPositions(bool leftHand, List<Vector3> outPositions)
        {
            outPositions.Clear();

            if (handSubsystem == null || !handSubsystem.running)
            {
                return;
            }

            XRHand hand = leftHand ? handSubsystem.leftHand : handSubsystem.rightHand;
            if (!hand.isTracked)
            {
                return;
            }

            XRHandJointID[] fingertipKeypoints = {
                XRHandJointID.ThumbTip,
                XRHandJointID.IndexTip,
                XRHandJointID.MiddleTip,
                XRHandJointID.RingTip,
                XRHandJointID.LittleTip
            };

            foreach (XRHandJointID keypoint in fingertipKeypoints)
            {
                XRHandJoint tipJoint = hand.GetJoint(keypoint);
                if (tipJoint.TryGetPose(out Pose tipPose))
                {
                    outPositions.Add(tipPose.position);
                }
                else
                {
                    outPositions.Add(Vector3.zero);
                }
            }
        }

        /// <summary>
        /// Get current detected gesture for a hand
        /// </summary>
        public LBEASTHandGesture GetCurrentGesture(bool leftHand)
        {
            return leftHand ? leftHandGesture : rightHandGesture;
        }

        /// <summary>
        /// Check if hand tracking is currently active
        /// </summary>
        public bool IsHandTrackingActive()
        {
            return handSubsystem != null && handSubsystem.running;
        }

        /// <summary>
        /// Check if this component is processing gestures for the local player
        /// In multiplayer, only locally controlled objects should process gestures
        /// </summary>
        public bool IsProcessingForLocalPlayer()
        {
            return ShouldProcessGestures();
        }

        // ========================================
        // Private Methods
        // ========================================

        private void UpdateGestureRecognition(float deltaTime)
        {
            // Only process gestures for locally controlled objects (multiplayer safety)
            if (!ShouldProcessGestures())
            {
                return;
            }

            if (!IsHandTrackingActive())
            {
                return;
            }

            // Detect gestures for both hands
            LBEASTHandGesture newLeftGesture = DetectGesture(true);
            LBEASTHandGesture newRightGesture = DetectGesture(false);

            // Fire events if gestures changed
            if (newLeftGesture != leftHandGesture)
            {
                OnHandGestureDetected?.Invoke(true, newLeftGesture, 1.0f);
                leftHandGesture = newLeftGesture;
            }

            if (newRightGesture != rightHandGesture)
            {
                OnHandGestureDetected?.Invoke(false, newRightGesture, 1.0f);
                rightHandGesture = newRightGesture;
            }
        }

        private LBEASTHandGesture DetectGesture(bool leftHand)
        {
            // For now, just detect fist vs open hand
            // Future: Add more gesture recognition (pointing, thumbs up, peace sign, etc.)
            
            if (IsHandFistClosed(leftHand))
            {
                return LBEASTHandGesture.FistClosed;
            }
            
            return LBEASTHandGesture.HandOpen;
        }

        private bool ShouldProcessGestures()
        {
            // If configured to process all players, skip the local-only check
            if (!onlyProcessLocalPlayer)
            {
                return true;
            }

            // In Unity, we check if this is the local player by checking if we're on a local player object
            // For single-player, always process
            // For multiplayer, check if this is a local player (via NetworkBehaviour or similar)
            // NOOP: Multiplayer local player check will be implemented when VR Player Transport replication is added
            // For now, assume single-player (always process)
            return true;
        }
    }
}

