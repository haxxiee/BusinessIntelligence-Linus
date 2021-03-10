// Initierar WiFi anslutning
void initWifi() {
  Serial.print("\nConneting to WIFI. Please wait.");
  WiFi.begin(WIFI_SSID, WIFI_PASS);

  while(WiFi.status() != WL_CONNECTED) {
      delay(500);
      Serial.print(".");
    }
   
  Serial.println("\nWiFi initialized");
  getWifiInfo();
}

// Anger WiFi info, IP adress
void getWifiInfo() {
  Serial.print("IP address: ");
  Serial.println(WiFi.localIP());
}

// Initierar IOT hubben
void initIotHub() {
  if(!Esp32MQTTClient_Init((const uint8_t *) CONNECTION_STRING)) {
    isConnected = false;
    return;
  }

  isConnected = true;
}

// Funktion som returnerar tiden
unsigned long getTime() {
  time_t now;
  struct tm timeinfo;
  if (!getLocalTime(&timeinfo)) {
    return(0);
  }
  time(&now);
  return now;
}
