#include "includes.h"
#include "defines.h"

bool isConnected = false;
String deviceId;

void setup() {
  Serial.begin(115200);
  initWifi();
  initIotHub();
  deviceId = WiFi.macAddress();
  configTime(3600, 0, NTPSERVER);
  pinMode(SHK_PIN, INPUT);
}

void loop() {

  char payload[MESSAGE_LEN_MAX];
  DynamicJsonDocument doc(MESSAGE_LEN_MAX);
  doc["deviceid"] = deviceId;
  doc["time"] = getTime();

  doc["message"] = "Shock Detected";

  Serial.println(digitalRead(SHK_PIN));

  serializeJson(doc, payload);

  if(isConnected && digitalRead(SHK_PIN) == LOW) {

    EVENT_INSTANCE* message = Esp32MQTTClient_Event_Generate(payload, MESSAGE);
    Esp32MQTTClient_Event_AddProp(message, "School", "Nackademin");
    Esp32MQTTClient_Event_AddProp(message, "Name", "Linus");
    Esp32MQTTClient_Event_AddProp(message, "Type", "ShockSensor");
    Esp32MQTTClient_SendEventInstance(message);

    delay(200);  
    }
}
