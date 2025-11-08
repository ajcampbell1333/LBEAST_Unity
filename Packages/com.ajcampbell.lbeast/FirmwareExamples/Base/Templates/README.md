# LBEAST Communication Templates

**Standalone header templates for easy integration of wireless and CAN bus communication in microcontroller firmware sketches.**

---

## 📦 Overview

These header templates provide a simple, drop-in solution for bidirectional wireless communication between microcontrollers and Unity Engine using the LBEAST EmbeddedSystems protocol.

### Available Templates

| Template | Purpose | Platforms |
|----------|---------|-----------|
| **`LBEAST_Wireless_TX.h`** | Transmit to game engine | ESP32, ESP8266, Arduino+WiFi, STM32+WiFi, RPi, Jetson |
| **`LBEAST_Wireless_RX.h`** | Receive from game engine | ESP32, ESP8266, Arduino+WiFi, STM32+WiFi, RPi, Jetson |
| **`LBEAST_CAN.h`** | CAN bus communication | ESP32, Arduino (MCP2515), STM32, Linux (SocketCAN) |
| **`ScissorLift_Controller.h`** | Scissor lift control | All platforms (CAN or GPIO mode) |
| **`ActuatorSystem_Controller.h`** | Actuator system control | All platforms |

### Supported Platforms

| Platform | Wireless | CAN Bus | Template Files |
|----------|----------|---------|----------------|
| **ESP32** | ✅ Built-in WiFi | ✅ Native TWAI | All templates |
| **ESP8266** | ✅ Built-in WiFi | ✅ MCP2515 | Wireless templates |
| **Arduino + WiFi Shield** | ✅ Via shield | ✅ MCP2515 | All templates |
| **STM32 + WiFi Module** | ✅ Via module | ✅ Native CAN | All templates |
| **Raspberry Pi** | ✅ Built-in WiFi | ✅ SocketCAN | All templates |
| **Jetson Nano** | ✅ Built-in WiFi | ✅ SocketCAN | All templates |
| **Arduino (no WiFi)** | ❌ | ✅ MCP2515 | CAN templates only |
| **STM32 (no WiFi)** | ❌ | ✅ Native CAN | CAN templates only |

---

## 🚀 Quick Start

### **Transmitting to Unity (TX)**

```cpp
#include "LBEAST_Wireless_TX.h"

void setup() {
  // Initialize wireless communication
  LBEAST_Wireless_Init(
    "VR_Arcade_LAN",                    // WiFi SSID
    "your_password",                     // WiFi password
    IPAddress(192, 168, 1, 100),        // Unity PC IP
    8888                                 // UDP port
  );
}

void loop() {
  // Send button press
  if (digitalRead(BUTTON_PIN) == LOW) {
    LBEAST_SendBool(0, true);
  }
  
  // Send sensor value
  float sensorValue = analogRead(SENSOR_PIN) / 1024.0f;
  LBEAST_SendFloat(1, sensorValue);
  
  delay(10);
}
```

### **Receiving from Unity (RX)**

```cpp
#include "LBEAST_Wireless_RX.h"

// Implement handlers
void LBEAST_HandleBool(uint8_t channel, bool value) {
  if (channel == 0) {
    // Unlock door
    digitalWrite(LOCK_PIN, value ? HIGH : LOW);
  }
}

void LBEAST_HandleFloat(uint8_t channel, float value) {
  if (channel == 1) {
    // Set motor speed (0.0-1.0)
    analogWrite(MOTOR_PIN, (int)(value * 255));
  }
}

void setup() {
  // Initialize wireless communication
  LBEAST_Wireless_Init(
    "VR_Arcade_LAN",                    // WiFi SSID
    "your_password",                     // WiFi password
    8888                                 // UDP port
  );
}

void loop() {
  // Process incoming commands
  LBEAST_ProcessIncoming();
  
  delay(10);
}
```

---

## 🚀 CAN Bus Quick Start

### **Using CAN Bus Template**

```cpp
#include "LBEAST_CAN.h"

void setup() {
  // Initialize CAN bus (500 kbps, MCP2515 CS pin 10)
  LBEAST_CAN_Init(500000, 10);
  
  // Send joystick command to scissor lift ECU
  LBEAST_CAN_SendLiftJoystickCommand(0.5f, 0.0f, 0x180);
  // Vertical: 0.5 (up), Forward: 0.0 (neutral), CAN ID: 0x180
  
  // Send emergency stop
  LBEAST_CAN_SendLiftEmergencyStop(true, 0x200);
}

void loop() {
  // CAN bus communication happens here
  delay(10);
}
```

**Platform Notes:**
- **ESP32:** Uses native TWAI (no MCP2515 needed)
- **Arduino:** Requires MCP2515 CAN controller module
- **STM32:** Uses native CAN controller
- **Linux (RPi/Jetson):** Uses SocketCAN (interface: "can0")

**See `GunshipExperience/README.md` for complete CAN bus configuration guide.**

---

## 📋 API Reference

### **TX Template (`LBEAST_Wireless_TX.h`)**

#### Initialization
```cpp
void LBEAST_Wireless_Init(const char* ssid, const char* password, IPAddress targetIP, uint16_t targetPort = 8888);
```

#### Transmission Functions
```cpp
void LBEAST_SendBool(uint8_t channel, bool value);
void LBEAST_SendInt32(uint8_t channel, int32_t value);
void LBEAST_SendFloat(uint8_t channel, float value);
void LBEAST_SendString(uint8_t channel, const char* str);
```

### **RX Template (`LBEAST_Wireless_RX.h`)**

#### Initialization
```cpp
void LBEAST_Wireless_Init(const char* ssid, const char* password, uint16_t localPort = 8888);
```

#### Reception Functions
```cpp
void LBEAST_ProcessIncoming();  // Call regularly in loop()
```

#### Handler Functions (Implement in your sketch)
```cpp
void LBEAST_HandleBool(uint8_t channel, bool value);
void LBEAST_HandleInt32(uint8_t channel, int32_t value);
void LBEAST_HandleFloat(uint8_t channel, float value);
void LBEAST_HandleString(uint8_t channel, const char* str, uint8_t length);
```

---

## 🔌 CAN Bus Template (`LBEAST_CAN.h`)

### **Initialization**
```cpp
bool LBEAST_CAN_Init(uint32_t baudRate = 500000, int csPin = 10, const char* interface = "can0");
```

### **Send Commands**
```cpp
bool LBEAST_CAN_SendCommand(uint32_t canId, uint8_t* data, uint8_t dataLength);
void LBEAST_CAN_SendLiftJoystickCommand(float verticalCommand, float forwardCommand, uint32_t canIdBase = 0x180);
void LBEAST_CAN_SendLiftEmergencyStop(bool enable, uint32_t canIdBase = 0x200);
```

### **Platform Support**

| Platform | CAN Implementation | Notes |
|----------|-------------------|-------|
| **ESP32** | Native TWAI | GPIO 4 (TX), GPIO 5 (RX) by default |
| **Arduino** | MCP2515 via SPI | Requires MCP2515 module, specify CS pin |
| **STM32** | Native CAN | Uses STM32 CAN library |
| **Linux** | SocketCAN | Interface name: "can0" (configurable) |

**Important:** 
- CAN bus protocol documentation (including CAN IDs) is typically **proprietary** and not publicly available
- Default values (0x180, 0x200, 0x280) are **examples only** - replace with your manufacturer's actual CAN IDs
- You may need to:
  - Contact manufacturer support for protocol documentation
  - Use a CAN bus analyzer to reverse-engineer the protocol
  - Work with authorized dealers/service centers who have access to proprietary documentation

---

## 📄 License

MIT License - Copyright (c) 2025 AJ Campbell

---

**Built for LBEAST - Location-Based Entertainment Activation Standard**
