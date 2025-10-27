// Copyright (c) 2025 AJ Campbell. Licensed under the MIT License.

using UnityEngine;
using LBEAST.Core;
using LBEAST.LargeHaptics;

namespace LBEAST.ExperienceTemplates
{
    /// <summary>
    /// 2DOF Flight Sim Experience Template
    /// 
    /// Single-player flight simulator using a two-axis gyroscope for continuous rotation beyond 360 degrees.
    /// Combines:
    /// - 2DOF gyroscope system (continuous pitch/roll)
    /// - HOTAS controller integration (Logitech X56, Thrustmaster T.Flight)
    /// - Joystick, throttle, and pedal controls
    /// - Configurable sensitivity and axis inversion
    /// 
    /// Perfect for realistic flight arcade games, space combat, and aerobatic simulators.
    /// </summary>
    public class FlightSimExperience : LBEASTExperienceBase
    {
        [Header("Platform Configuration")]
        [SerializeField] private HapticPlatformController platformController;

        [Header("Gyroscope Settings")]
        [SerializeField] private bool enableContinuousPitch = true;
        [SerializeField] private bool enableContinuousRoll = true;
        [Range(10f, 120f)]
        [SerializeField] private float maxRotationSpeed = 60f;  // Degrees per second

        [Header("Axis Configuration")]
        [SerializeField] private bool invertPitchAxis = false;
        [SerializeField] private bool invertRollAxis = false;

        [Header("HOTAS Configuration")]
        [SerializeField] private HOTASType hotasType = HOTASType.None;
        [Range(0.1f, 5f)]
        [SerializeField] private float joystickSensitivity = 1f;
        [Range(0.1f, 5f)]
        [SerializeField] private float throttleSensitivity = 1f;

        private float currentPitch = 0f;
        private float currentRoll = 0f;

        protected override void Awake()
        {
            base.Awake();

            // Find or create platform controller
            if (platformController == null)
            {
                platformController = GetComponent<HapticPlatformController>();
                if (platformController == null)
                {
                    platformController = gameObject.AddComponent<HapticPlatformController>();
                }
            }
        }

        protected override bool InitializeExperienceImpl()
        {
            if (platformController == null)
            {
                Debug.LogError("[LBEAST] FlightSimExperience: Platform controller is null");
                return false;
            }

            // Configure 2DOF flight sim
            HapticPlatformConfig config = new HapticPlatformConfig
            {
                platformType = PlatformType.FlightSim_2DOF,
                maxPitchDegrees = 360f,  // Unlimited rotation
                maxRollDegrees = 360f,   // Unlimited rotation
                controllerIPAddress = "192.168.1.100",
                controllerPort = 8080
            };

            // Configure gyroscope
            config.gyroscopeConfig = new GyroscopeConfig
            {
                enableContinuousPitch = enableContinuousPitch,
                enableContinuousRoll = enableContinuousRoll,
                maxRotationSpeed = maxRotationSpeed,
                invertPitchAxis = invertPitchAxis,
                invertRollAxis = invertRollAxis,
                hotasType = hotasType,
                joystickSensitivity = joystickSensitivity,
                throttleSensitivity = throttleSensitivity
            };

            if (!platformController.InitializePlatform(config))
            {
                Debug.LogError("[LBEAST] FlightSimExperience: Failed to initialize platform");
                return false;
            }

            Debug.Log($"[LBEAST] FlightSimExperience initialized with {hotasType} HOTAS");
            return true;
        }

        protected override void ShutdownExperienceImpl()
        {
            if (platformController != null)
            {
                platformController.ReturnToNeutral(2f);
            }
        }

        protected void Update()
        {
            if (!isRunning)
            {
                return;
            }

            // Update HOTAS input if connected
            if (platformController != null && platformController.IsHOTASConnected())
            {
                UpdateHOTASControl();
            }
        }

        private void UpdateHOTASControl()
        {
            Vector2 joystickInput = platformController.GetHOTASJoystickInput();

            // Apply sensitivity and inversion
            float pitchInput = joystickInput.y * joystickSensitivity;
            float rollInput = joystickInput.x * joystickSensitivity;

            if (invertPitchAxis) pitchInput = -pitchInput;
            if (invertRollAxis) rollInput = -rollInput;

            // Update continuous rotation
            if (enableContinuousPitch)
            {
                currentPitch += pitchInput * maxRotationSpeed * Time.deltaTime;
            }
            else
            {
                currentPitch = Mathf.Lerp(currentPitch, pitchInput * 45f, Time.deltaTime * 5f);
            }

            if (enableContinuousRoll)
            {
                currentRoll += rollInput * maxRotationSpeed * Time.deltaTime;
            }
            else
            {
                currentRoll = Mathf.Lerp(currentRoll, rollInput * 45f, Time.deltaTime * 5f);
            }

            // Send to gyroscope
            SendGyroscopeRotation(currentPitch, currentRoll);
        }

        #region Public API

        /// <summary>
        /// Send rotation command to gyroscope
        /// Supports continuous rotation beyond 360 degrees
        /// </summary>
        /// <param name="pitch">Pitch rotation in degrees (can exceed 360)</param>
        /// <param name="roll">Roll rotation in degrees (can exceed 360)</param>
        /// <param name="duration">Time to reach target (seconds)</param>
        public void SendGyroscopeRotation(float pitch, float roll, float duration = 0.1f)
        {
            if (platformController != null)
            {
                PlatformMotionCommand command = new PlatformMotionCommand
                {
                    pitch = pitch,
                    roll = roll,
                    translationY = 0f,
                    translationZ = 0f,
                    duration = duration,
                    useContinuousRotation = true
                };

                platformController.SendMotionCommand(command);
            }
        }

        /// <summary>
        /// Apply normalized flight stick input (-1 to +1)
        /// </summary>
        /// <param name="pitchInput">Pitch input (-1 = pull back, +1 = push forward)</param>
        /// <param name="rollInput">Roll input (-1 = roll left, +1 = roll right)</param>
        public void ApplyFlightInput(float pitchInput, float rollInput)
        {
            float pitchSpeed = pitchInput * maxRotationSpeed * Time.deltaTime;
            float rollSpeed = rollInput * maxRotationSpeed * Time.deltaTime;

            if (invertPitchAxis) pitchSpeed = -pitchSpeed;
            if (invertRollAxis) rollSpeed = -rollSpeed;

            currentPitch += pitchSpeed;
            currentRoll += rollSpeed;

            SendGyroscopeRotation(currentPitch, currentRoll, Time.deltaTime);
        }

        /// <summary>
        /// Return gyroscope to level position
        /// </summary>
        public void ReturnToLevel(float duration = 2f)
        {
            currentPitch = 0f;
            currentRoll = 0f;

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
        /// Get current HOTAS joystick input
        /// </summary>
        public Vector2 GetJoystickInput()
        {
            if (platformController != null)
            {
                return platformController.GetHOTASJoystickInput();
            }
            return Vector2.zero;
        }

        /// <summary>
        /// Get current HOTAS throttle input (0-1)
        /// </summary>
        public float GetThrottleInput()
        {
            if (platformController != null)
            {
                return platformController.GetHOTASThrottleInput();
            }
            return 0f;
        }

        /// <summary>
        /// Get current HOTAS pedal input (-1 to +1)
        /// </summary>
        public float GetPedalInput()
        {
            if (platformController != null)
            {
                return platformController.GetHOTASPedalInput();
            }
            return 0f;
        }

        /// <summary>
        /// Check if HOTAS is connected
        /// </summary>
        public bool IsHOTASConnected()
        {
            return platformController != null && platformController.IsHOTASConnected();
        }

        #endregion
    }
}

