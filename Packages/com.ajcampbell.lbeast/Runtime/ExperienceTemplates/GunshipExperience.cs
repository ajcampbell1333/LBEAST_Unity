// Copyright (c) 2025 AJ Campbell. Licensed under the MIT License.

using UnityEngine;
using LBEAST.Core;
using LBEAST.LargeHaptics;

namespace LBEAST.ExperienceTemplates
{
    /// <summary>
    /// 5DOF Gunship Experience Template
    /// 
    /// Pre-configured four-player seated VR experience on a hydraulic platform.
    /// Combines:
    /// - 5DOF hydraulic platform (pitch, roll, Y/Z translation)
    /// - Four player seated positions
    /// - LAN multiplayer support
    /// - Synchronized motion for all passengers
    /// 
    /// Perfect for gunship, helicopter, spaceship, or any multi-crew vehicle
    /// experiences requiring shared motion simulation.
    /// </summary>
    public class GunshipExperience : LBEASTExperienceBase
    {
        [Header("Platform Configuration")]
        [SerializeField] private HapticPlatformController platformController;

        [Header("Seating")]
        [SerializeField] private Transform[] seatTransforms = new Transform[4];

        [Header("Motion Limits")]
        [Range(1f, 15f)]
        [SerializeField] private float maxPitch = 10f;
        [Range(1f, 15f)]
        [SerializeField] private float maxRoll = 10f;

        protected override void Awake()
        {
            base.Awake();

            // Enable multiplayer for gunship
            enableMultiplayer = true;
            maxPlayers = 4;

            // Find or create platform controller
            if (platformController == null)
            {
                platformController = GetComponent<HapticPlatformController>();
                if (platformController == null)
                {
                    platformController = gameObject.AddComponent<HapticPlatformController>();
                }
            }

            // Create default seat positions if not assigned
            if (seatTransforms.Length != 4 || seatTransforms[0] == null)
            {
                CreateDefaultSeats();
            }
        }

        private void CreateDefaultSeats()
        {
            seatTransforms = new Transform[4];
            
            // Front left seat
            seatTransforms[0] = CreateSeat("Seat_FrontLeft", new Vector3(-0.5f, 0f, 0.5f));
            // Front right seat
            seatTransforms[1] = CreateSeat("Seat_FrontRight", new Vector3(0.5f, 0f, 0.5f));
            // Rear left seat
            seatTransforms[2] = CreateSeat("Seat_RearLeft", new Vector3(-0.5f, 0f, -0.5f));
            // Rear right seat
            seatTransforms[3] = CreateSeat("Seat_RearRight", new Vector3(0.5f, 0f, -0.5f));
        }

        private Transform CreateSeat(string name, Vector3 localPosition)
        {
            GameObject seatObj = new GameObject(name);
            seatObj.transform.SetParent(transform);
            seatObj.transform.localPosition = localPosition;
            seatObj.transform.localRotation = Quaternion.identity;
            return seatObj.transform;
        }

        protected override bool InitializeExperienceImpl()
        {
            if (platformController == null)
            {
                Debug.LogError("[LBEAST] GunshipExperience: Platform controller is null");
                return false;
            }

            // Configure platform for 4-player gunship
            HapticPlatformConfig config = new HapticPlatformConfig
            {
                platformType = PlatformType.Gunship_FourPlayer,
                maxPitchDegrees = maxPitch,
                maxRollDegrees = maxRoll,
                maxTranslationY = 100f,
                maxTranslationZ = 100f,
                controllerIPAddress = "192.168.1.100",
                controllerPort = 8080
            };

            if (!platformController.InitializePlatform(config))
            {
                Debug.LogError("[LBEAST] GunshipExperience: Failed to initialize platform");
                return false;
            }

            Debug.Log("[LBEAST] GunshipExperience initialized for 4 players");
            return true;
        }

        protected override void ShutdownExperienceImpl()
        {
            if (platformController != null)
            {
                platformController.ReturnToNeutral(1f);
            }
        }

        #region Public API

        /// <summary>
        /// Send normalized gunship tilt (RECOMMENDED - hardware-agnostic)
        /// </summary>
        /// <param name="tiltX">Left/Right tilt (-1.0 = full left, +1.0 = full right)</param>
        /// <param name="tiltY">Forward/Backward tilt (-1.0 = full backward, +1.0 = full forward)</param>
        /// <param name="verticalOffset">Vertical translation (-1.0 to +1.0)</param>
        /// <param name="duration">Time to reach target (seconds)</param>
        public void SendGunshipTilt(float tiltX, float tiltY, float verticalOffset = 0f, float duration = 1f)
        {
            if (platformController != null)
            {
                platformController.SendNormalizedMotion(tiltX, tiltY, verticalOffset, duration);
            }
        }

        /// <summary>
        /// Send motion command to platform (ADVANCED - uses absolute angles)
        /// </summary>
        public void SendGunshipMotion(float pitch, float roll, float lateralOffset, float verticalOffset, float duration = 1f)
        {
            if (platformController != null)
            {
                PlatformMotionCommand command = new PlatformMotionCommand
                {
                    pitch = pitch,
                    roll = roll,
                    translationY = lateralOffset,
                    translationZ = verticalOffset,
                    duration = duration
                };

                platformController.SendMotionCommand(command);
            }
        }

        /// <summary>
        /// Return platform to neutral
        /// </summary>
        public void ReturnToNeutral(float duration = 2f)
        {
            if (platformController != null)
            {
                platformController.ReturnToNeutral(duration);
            }
        }

        /// <summary>
        /// Emergency stop
        /// </summary>
        public void EmergencyStop()
        {
            if (platformController != null)
            {
                platformController.EmergencyStop();
            }
        }

        /// <summary>
        /// Get seat transform for a specific player
        /// </summary>
        public Transform GetSeatTransform(int playerIndex)
        {
            if (playerIndex >= 0 && playerIndex < seatTransforms.Length)
            {
                return seatTransforms[playerIndex];
            }
            return null;
        }

        #endregion
    }
}



