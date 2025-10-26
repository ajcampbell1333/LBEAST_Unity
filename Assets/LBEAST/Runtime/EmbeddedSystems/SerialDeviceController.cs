// Copyright (c) 2025 AJ Campbell. Licensed under the MIT License.

using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

namespace LBEAST.EmbeddedSystems
{
    /// <summary>
    /// Supported microcontroller types
    /// </summary>
    public enum MicrocontrollerType
    {
        Arduino,
        ESP32,
        STM32,
        RaspberryPi,
        JetsonNano,
        Custom
    }

    /// <summary>
    /// Serial device configuration
    /// </summary>
    [Serializable]
    public class SerialDeviceConfig
    {
        public string portName = "COM3";
        public int baudRate = 115200;
        public Parity parity = Parity.None;
        public int dataBits = 8;
        public StopBits stopBits = StopBits.One;
        public int readTimeout = 100;
        public int writeTimeout = 100;
    }

    /// <summary>
    /// Button state data
    /// </summary>
    [Serializable]
    public class ButtonState
    {
        public int buttonID;
        public bool isPressed;
        public float pressDuration;
        public DateTime lastPressTime;

        public ButtonState(int id)
        {
            buttonID = id;
            isPressed = false;
            pressDuration = 0f;
            lastPressTime = DateTime.MinValue;
        }
    }

    /// <summary>
    /// Controls serial communication with embedded systems (Arduino, ESP32, STM32, etc.)
    /// Manages buttons, haptic feedback, and custom hardware integration
    /// </summary>
    public class SerialDeviceController : MonoBehaviour
    {
        [Header("Device Configuration")]
        [SerializeField] private MicrocontrollerType deviceType = MicrocontrollerType.Arduino;
        [SerializeField] private SerialDeviceConfig config = new SerialDeviceConfig();
        [SerializeField] private bool autoConnect = true;

        [Header("Button Configuration")]
        [SerializeField] private int numberOfButtons = 8;

        private SerialPort serialPort;
        private bool isConnected = false;
        private Dictionary<int, ButtonState> buttonStates = new Dictionary<int, ButtonState>();
        private Queue<string> incomingMessages = new Queue<string>();
        private Queue<string> outgoingMessages = new Queue<string>();

        #region Initialization

        private void Awake()
        {
            // Initialize button states
            for (int i = 0; i < numberOfButtons; i++)
            {
                buttonStates[i] = new ButtonState(i);
            }
        }

        private void Start()
        {
            if (autoConnect)
            {
                ConnectToDevice();
            }
        }

        private void OnDestroy()
        {
            DisconnectFromDevice();
        }

        /// <summary>
        /// Connect to the serial device
        /// </summary>
        public bool ConnectToDevice()
        {
            if (isConnected)
            {
                Debug.LogWarning("[LBEAST] Already connected to device");
                return true;
            }

            try
            {
                serialPort = new SerialPort(config.portName, config.baudRate, config.parity, config.dataBits, config.stopBits);
                serialPort.ReadTimeout = config.readTimeout;
                serialPort.WriteTimeout = config.writeTimeout;
                serialPort.Open();

                isConnected = true;
                Debug.Log($"[LBEAST] Connected to {deviceType} on {config.portName} at {config.baudRate} baud");
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError($"[LBEAST] Failed to connect to device: {e.Message}");
                return false;
            }
        }

        /// <summary>
        /// Disconnect from the serial device
        /// </summary>
        public void DisconnectFromDevice()
        {
            if (serialPort != null && serialPort.IsOpen)
            {
                serialPort.Close();
                serialPort.Dispose();
                isConnected = false;
                Debug.Log("[LBEAST] Disconnected from device");
            }
        }

        #endregion

        #region Update Loop

        private void Update()
        {
            if (!isConnected)
            {
                return;
            }

            // Read incoming data
            ReadIncomingData();

            // Send outgoing messages
            SendOutgoingMessages();

            // Process incoming messages
            ProcessIncomingMessages();
        }

        private void ReadIncomingData()
        {
            if (serialPort == null || !serialPort.IsOpen)
            {
                return;
            }

            try
            {
                while (serialPort.BytesToRead > 0)
                {
                    string message = serialPort.ReadLine().Trim();
                    if (!string.IsNullOrEmpty(message))
                    {
                        incomingMessages.Enqueue(message);
                    }
                }
            }
            catch (TimeoutException)
            {
                // Normal timeout, no data available
            }
            catch (Exception e)
            {
                Debug.LogError($"[LBEAST] Error reading from device: {e.Message}");
            }
        }

        private void SendOutgoingMessages()
        {
            if (serialPort == null || !serialPort.IsOpen)
            {
                return;
            }

            while (outgoingMessages.Count > 0)
            {
                try
                {
                    string message = outgoingMessages.Dequeue();
                    serialPort.WriteLine(message);
                }
                catch (Exception e)
                {
                    Debug.LogError($"[LBEAST] Error sending to device: {e.Message}");
                }
            }
        }

        private void ProcessIncomingMessages()
        {
            while (incomingMessages.Count > 0)
            {
                string message = incomingMessages.Dequeue();
                ParseMessage(message);
            }
        }

        #endregion

        #region Message Parsing

        private void ParseMessage(string message)
        {
            // Expected format: "CMD:VALUE" or "BTN:ID:STATE"
            string[] parts = message.Split(':');

            if (parts.Length < 2)
            {
                return;
            }

            string command = parts[0];

            switch (command)
            {
                case "BTN":
                    if (parts.Length >= 3)
                    {
                        if (int.TryParse(parts[1], out int buttonID) && int.TryParse(parts[2], out int state))
                        {
                            UpdateButtonState(buttonID, state == 1);
                        }
                    }
                    break;

                case "STATUS":
                    Debug.Log($"[LBEAST] Device status: {parts[1]}");
                    break;

                default:
                    Debug.Log($"[LBEAST] Unknown command: {message}");
                    break;
            }
        }

        #endregion

        #region Button Control

        private void UpdateButtonState(int buttonID, bool pressed)
        {
            if (!buttonStates.ContainsKey(buttonID))
            {
                buttonStates[buttonID] = new ButtonState(buttonID);
            }

            ButtonState state = buttonStates[buttonID];
            bool wasPressed = state.isPressed;
            state.isPressed = pressed;

            if (pressed && !wasPressed)
            {
                // Button just pressed
                state.lastPressTime = DateTime.Now;
                state.pressDuration = 0f;
            }
            else if (pressed && wasPressed)
            {
                // Button held
                state.pressDuration = (float)(DateTime.Now - state.lastPressTime).TotalSeconds;
            }
        }

        /// <summary>
        /// Check if a button is currently pressed
        /// </summary>
        public bool IsButtonPressed(int buttonID)
        {
            if (buttonStates.TryGetValue(buttonID, out ButtonState state))
            {
                return state.isPressed;
            }
            return false;
        }

        /// <summary>
        /// Get how long a button has been pressed (in seconds)
        /// </summary>
        public float GetButtonPressDuration(int buttonID)
        {
            if (buttonStates.TryGetValue(buttonID, out ButtonState state))
            {
                return state.pressDuration;
            }
            return 0f;
        }

        /// <summary>
        /// Check if a button was just pressed this frame
        /// </summary>
        public bool GetButtonDown(int buttonID)
        {
            if (buttonStates.TryGetValue(buttonID, out ButtonState state))
            {
                return state.isPressed && state.pressDuration < Time.deltaTime;
            }
            return false;
        }

        #endregion

        #region Haptic Output

        /// <summary>
        /// Send haptic pulse to device
        /// </summary>
        /// <param name="intensity">Pulse intensity (0-255)</param>
        /// <param name="duration">Duration in milliseconds</param>
        public void SendHapticPulse(byte intensity, int duration)
        {
            if (!isConnected)
            {
                return;
            }

            string message = $"HAPTIC:{intensity}:{duration}";
            outgoingMessages.Enqueue(message);
        }

        /// <summary>
        /// Activate a specific haptic motor
        /// </summary>
        /// <param name="motorID">Motor ID</param>
        /// <param name="intensity">Intensity (0-255)</param>
        /// <param name="duration">Duration in milliseconds</param>
        public void ActivateMotor(int motorID, byte intensity, int duration)
        {
            if (!isConnected)
            {
                return;
            }

            string message = $"MOTOR:{motorID}:{intensity}:{duration}";
            outgoingMessages.Enqueue(message);
        }

        #endregion

        #region LED Control

        /// <summary>
        /// Set LED color (RGB)
        /// </summary>
        /// <param name="ledID">LED ID</param>
        /// <param name="r">Red (0-255)</param>
        /// <param name="g">Green (0-255)</param>
        /// <param name="b">Blue (0-255)</param>
        public void SetLEDColor(int ledID, byte r, byte g, byte b)
        {
            if (!isConnected)
            {
                return;
            }

            string message = $"LED:{ledID}:{r}:{g}:{b}";
            outgoingMessages.Enqueue(message);
        }

        /// <summary>
        /// Turn LED on/off
        /// </summary>
        public void SetLEDState(int ledID, bool on)
        {
            if (!isConnected)
            {
                return;
            }

            string message = $"LED:{ledID}:{(on ? "ON" : "OFF")}";
            outgoingMessages.Enqueue(message);
        }

        #endregion

        #region Custom Commands

        /// <summary>
        /// Send a custom command to the device
        /// </summary>
        public void SendCustomCommand(string command)
        {
            if (!isConnected)
            {
                return;
            }

            outgoingMessages.Enqueue(command);
        }

        #endregion

        #region Accessors

        /// <summary>
        /// Check if device is connected
        /// </summary>
        public bool IsConnected()
        {
            return isConnected && serialPort != null && serialPort.IsOpen;
        }

        /// <summary>
        /// Get device type
        /// </summary>
        public MicrocontrollerType GetDeviceType()
        {
            return deviceType;
        }

        /// <summary>
        /// Get list of available COM ports
        /// </summary>
        public static string[] GetAvailablePorts()
        {
            return SerialPort.GetPortNames();
        }

        #endregion
    }
}

