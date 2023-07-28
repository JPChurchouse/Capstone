/*


*/

// INCLUDES AND DEFINES
#include <Arduino.h>
#include <SPI.h>
#include "DW1000Ranging.h"     //https://github.com/thotro/arduino-dw1000
#include <WiFi.h>
#include <PubSubClient.h>
#include <ESP32Time.h>

#define ANCHOR_ADD "00:00:00:00:00:00:00:00"

#define SPI_SCK 18
#define SPI_MISO 19
#define SPI_MOSI 23
#define DW_CS 4


// WIFI AND MQTT INIT  
const char* ssid = "Lodge Wireless Internet";
const char* password = "JulietCharlieHotelQuebec";
const char* mqtt_server = "192.168.1.20";
long lastMsg = 0;
char msg[50];

WiFiClient espClient;
PubSubClient client(espClient);

void wifi_setup();
void mqtt_setup();
void mqtt_message(char*, byte *, unsigned int);
void mqtt_reconnect() ;
void mqtt_subscribe();


// UWB INIT
const uint8_t PIN_RST = 27; // reset pin
const uint8_t PIN_IRQ = 34; // irq pin
const uint8_t PIN_SS = 4;   // spi select pin

void uwb_setup();
void uwb_newRange();
void uwb_newBlink(DW1000Device*);
void uwb_inactiveDevice(DW1000Device*);


// RTC INIT
ESP32Time rtc(12 * 3600);
ulong rtc_time(ulong);


// Detection handling
const float detect_in = 8/2;
const float detect_out = 10/2;
const float detect_thr = 4 /2;
const uint8_t buff_size = 16;
uint16_t list_names[buff_size];
float list_shortest[buff_size];

void detect_init();
void detect_packet(uint16_t, bool);
void detection(uint16_t, float);


// SETUP
void setup()
{
  Serial.begin(115200);

  wifi_setup();
  delay(1000);

  mqtt_setup();
  delay(1000);

  uwb_setup();
  delay(1000);

  detect_init();
}
 

// MAIN LOOP
void loop()
{
  if (WiFi.status() != WL_CONNECTED) 
  {
    wifi_setup;
  }

  if (!client.connected())
  {
    mqtt_reconnect();
  } 

  client.loop();
  DW1000Ranging.loop();
}


// Setup Wifi
void wifi_setup() 
{
  delay(10);
  Serial.println();
  Serial.print("Connecting to ");
  Serial.println(ssid);

  WiFi.begin(ssid, password);

  while (WiFi.status() != WL_CONNECTED) 
  {
    delay(500);
    Serial.print(".");
  }

  Serial.println("");
  Serial.println("WiFi connected");
  Serial.println("IP address: ");
  Serial.println(WiFi.localIP());
}


// Setup MQTT
void mqtt_setup()
{
  client.setServer(mqtt_server, 1883);
  client.setCallback(mqtt_message);
  client.publish("init", "hello");
}

// Subscribe to MQTT topics
void mqtt_subscribe()
{
client.subscribe("timestamp");
}

// New MQTT message arrived
void mqtt_message(char* topic, byte* msg, unsigned int length) 
{
  Serial.print("Message arrived on topic: ");
  Serial.print(topic);
  Serial.print(". Message: ");
  String message;
  
  for (int i = 0; i < length; i++) 
  {
    Serial.print((char)msg[i]);
    message += (char)msg[i];
  }

  Serial.println();

  if (String(topic) == "command/dock") 
  {
  }

  if (String(topic) == "timestamp") 
  {
    rtc_time(message.toInt());
  }
}

// Reconnect to MQTT
void mqtt_reconnect() 
{
  // Loop until we're reconnected
  while (!client.connected()) 
  {
    Serial.print("Attempting MQTT connection...");

    // Attempt to connect
    if (client.connect("Dock_Client")) 
    {
      Serial.println("connected");
      // Subscribe
      mqtt_subscribe();
    } 

    else 
    {
      Serial.print("failed, rc=");
      Serial.print(client.state());
      Serial.println(" try again in 5 seconds");
      // Wait 5 seconds before retrying
      delay(5000);
    }
  }
}
 

// UWB Setup
void uwb_setup()
{
  //init the configuration
    SPI.begin(SPI_SCK, SPI_MISO, SPI_MOSI);
    DW1000Ranging.initCommunication(PIN_RST, PIN_SS, PIN_IRQ); //Reset, CS, IRQ pin
    //define the sketch as anchor. It will be great to dynamically change the type of module
    DW1000Ranging.attachNewRange(uwb_newRange);
    DW1000Ranging.attachBlinkDevice(uwb_newBlink);
    DW1000Ranging.attachInactiveDevice(uwb_inactiveDevice);
    //Enable the filter to smooth the distance
    //DW1000Ranging.useRangeFilter(true);

    DW1000.setAntennaDelay(16438);
 
    DW1000Ranging.startAsAnchor(ANCHOR_ADD, DW1000.MODE_SHORTDATA_FAST_ACCURACY,false);
}

// Detect UWB
void uwb_newRange()
{
    uint16_t who = DW1000Ranging.getDistantDevice()->getShortAddress();
    float dist = DW1000Ranging.getDistantDevice()->getRange();

    detection(who,dist);


    char message[32];
    sprintf(message, "{\"ID\":\"%X\",\"Dist\":%f}",who,dist);

    //client.publish("detect", message);
    Serial.println(who,HEX);

    return;
    Serial.print("from: ");
    Serial.print(DW1000Ranging.getDistantDevice()->getShortAddress(), HEX);
    Serial.print("\t Range: ");
    Serial.print(DW1000Ranging.getDistantDevice()->getRange());
    Serial.print(" m");
    Serial.print("\t RX power: ");
    Serial.print(DW1000Ranging.getDistantDevice()->getRXPower());
    Serial.println(" dBm");
}
 
// Not sure
void uwb_newBlink(DW1000Device *device)
{
    return;
    Serial.print("blink; 1 device added ! -> ");
    Serial.print(" short:");
    Serial.println(device->getShortAddress(), HEX);
}
 
// UWB out of range
void uwb_inactiveDevice(DW1000Device *device)
{
    return;
    Serial.print("delete inactive device: ");
    Serial.println(device->getShortAddress(), HEX);
}


// RTC handler
ulong rtc_time(ulong value = 0)
{
  if (value != 0) rtc.setTime(value);
  return rtc.getEpoch();
}



void detect_init()
{
  for (int i = 0; i < buff_size; i++)
  {
    list_names[i] = 100;
    list_shortest[i] = 100;
  }
}

void detection(uint16_t kart, float value)
{
  int clr;

  for (int i = 0; i < buff_size; i++)
  {
    uint16_t who = list_names[i];

    
    if (who == 100) 
    {
      clr = i; // Make note of a clear index
      continue;
    }

    // If the name is already in buffer
    else if (who == kart)
    {
      // Check for out of AOI
      if (value > detect_out)
      {
        // Kart has left Area Of Interest - send detection packet
        bool right = list_shortest[i] < detect_thr;
        
        list_names[i] = 100;
        list_shortest[i] = 100;

        detect_packet(kart,right);
        return;
      }

      // Update shortest
      else if (value < list_shortest[i])
      {
        list_shortest[i] = value;
      }
      
      return;
    }
  }

  // Not already in the list - add it in
  if (value < detect_in)
  {
    list_names[clr] = kart;
    list_shortest[clr] = value;
  }
}

void detect_packet(uint16_t who, bool rightlane)
{
  char message[128];
  ulong time = rtc_time();

  //{"Time": 1687638447,"Colour": "red","Lane": "left"}
  if (rightlane)  sprintf(message, "{\"Time\": \"%u\",\"Colour\": \"%X\",\"Lane\": \"right\"}",time,who);
  else            sprintf(message, "{\"Time\": \"%u\",\"Colour\": \"%X\",\"Lane\": \"left\"}" ,time,who);

  client.publish("detect", message);
  Serial.println(who,HEX);
}