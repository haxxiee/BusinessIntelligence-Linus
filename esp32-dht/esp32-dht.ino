#include "includes.h"
#include "defines.h"

/* DHT11 */
DHT dht(DHT_PIN, DHT_TYPE);

float temp;
float treshold = 0;

bool isConnected = false;

String deviceId;

const char* ntpServer = "pool.ntp.org";

void setup() {
  Serial.begin(115200);
  dht.begin();
  initWifi();
  deviceId = WiFi.macAddress(); // Sätter mac adressen till device ID
  initIotHub();
  configTime(3600, 0, ntpServer);
}

void loop() {

  char payload[MESSAGE_LEN_MAX];
  
  DynamicJsonDocument doc(MESSAGE_LEN_MAX);
  doc["deviceid"] = deviceId;
  doc["time"] = getTime();
  
  doc["data"]["temp"] = dht.readTemperature();
  doc["data"]["hum"] = dht.readHumidity();

  temp = dht.readTemperature();

  serializeJson(doc, payload);

  if(isConnected && (temp >= (treshold +0.5) || temp <= (treshold -0.5))) {

    EVENT_INSTANCE* message = Esp32MQTTClient_Event_Generate(payload, MESSAGE);
    Esp32MQTTClient_Event_AddProp(message, "School", "Nackademin");
    Esp32MQTTClient_Event_AddProp(message, "Name", "Linus");
    Esp32MQTTClient_Event_AddProp(message, "Type", "DHT");
    Esp32MQTTClient_SendEventInstance(message);

    treshold = temp;
    }
}
