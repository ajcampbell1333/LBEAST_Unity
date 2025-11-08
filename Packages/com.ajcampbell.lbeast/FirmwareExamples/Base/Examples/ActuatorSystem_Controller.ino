/*
 * LBEAST Actuator System Controller - Standalone Example
 * 
 * Standalone embedded module for controlling 4-gang hydraulic actuator systems.
 * This example uses the ActuatorSystem_Controller.h header.
 * 
 * For use in combined systems (e.g., GunshipExperience), include the header directly.
 * 
 * Copyright (c) 2025 AJ Campbell. Licensed under the MIT License.
 */

#include "LBEAST_Wireless_RX.h"
#include "../Templates/ActuatorSystem_Controller.h"

// WiFi credentials (change to match your LAN)
const char* ssid = "VR_Arcade_LAN";
const char* password = "your_password_here";

// Configuration for the ActuatorSystemController
ActuatorSystemConfig actuatorConfig = {
  {12, 13, 14, 15},  // valvePins
  {32, 33, 34, 35},  // sensorPins
  {16, 17, 18, 19},  // lowerLimitPins
  {20, 21, 22, 23},  // upperLimitPins
  10.0f,             // maxPitchDeg
  10.0f,             // maxRollDeg
  7.62f,             // actuatorStrokeCm (3 inches)
  150.0f,            // platformWidthCm
  200.0f,            // platformLengthCm
  2.0f, 0.1f, 0.5f,   // kp, ki, kd
  true,              // autoCalibrateMode
  2000               // autoCalibrateTimeoutMs
};

ActuatorSystemController actuatorController; // Instantiate the controller

void setup() {
  Serial.begin(115200);
  delay(1000);
  
  Serial.println("\n\nLBEAST Actuator System Controller (Standalone)");
  Serial.println("===============================================\n");
  
  LBEAST_Wireless_Init(ssid, password, 8888);
  actuatorController.begin(actuatorConfig); // Initialize the controller
  
  Serial.println("\nActuator System Controller Ready!");
  Serial.println("Waiting for commands from game engine...\n");
  Serial.println("NOTICE: Actuators must be calibrated before use.");
  Serial.println("Send calibration command (Channel 2 = true) to enter calibration mode.\n");
}

void loop() {
  LBEAST_ProcessIncoming();
  actuatorController.update(); // Update the controller
  delay(10);
}

void LBEAST_HandleFloat(uint8_t channel, float value) {
  actuatorController.handleFloatCommand(channel, value);
}

void LBEAST_HandleBool(uint8_t channel, bool value) {
  actuatorController.handleBoolCommand(channel, value);
}
