/*


*/

// Are we at XKarts or at home?
//#define XKARTS

// INCLUDES AND DEFINES
#include <Arduino.h>
#include <SPI.h>
#include "DW1000Ranging.h"     //https://github.com/thotro/arduino-dw1000
#include <WiFi.h>
#include <PubSubClient.h>
#include <ESP32Time.h>

#define SPI_SCK 18
#define SPI_MISO 19
#define SPI_MOSI 23
#define DW_CS 4


// WIFI AND MQTT INIT
#ifndef XKARTS
const char* ssid = "Lodge Wireless Internet";
const char* password = "JulietCharlieHotelQuebec";
#else
const char* ssid = "SPARK-UKRV3Z";
const char* password = "JollyDuckY92?";
#endif

#define ANCHOR_ADD "00:00:00:00:00:00:00:00"

const char* mqtt_server = "192.168.1.20";
char msg[64];

WiFiClient espClient;
PubSubClient client(espClient);

void wifi_setup();
void mqtt_setup();
void mqtt_message(char*, byte* , uint);
void mqtt_reconnect() ;
void mqtt_subscribe();


// UWB INIT
const uint8_t PIN_RST = 27; // reset pin
const uint8_t PIN_IRQ = 34; // irq pin
const uint8_t PIN_SS = 4;   // spi select pin

const uint16_t UWB_DELAY = 16438;

void uwb_setup();
void uwb_newRange();
void uwb_newBlink(DW1000Device*);
void uwb_inactiveDevice(DW1000Device*);


// RTC INIT
ESP32Time rtc(0);
ulong rtc_time(ulong);


// Detection handling
const float detect_in = 80;
const float detect_out = 100;
const float detect_thr = 40;
const uint8_t buff_size = 16;
uint16_t list_names[buff_size];
uint16_t list_shortest[buff_size];

void detect_init();
void detect_packet(uint16_t, bool);
void detection(uint16_t, float);



// SETUP
void setup()
{
  Serial.begin(115200);

  delay(1000);
  wifi_setup();

  delay(1000);
  mqtt_setup();

  delay(1000);
  uwb_setup();

  delay(1000);
  detect_init();
}

bool sent = false;
// MAIN LOOP
void loop()
{
  // Wifi reconnect
  if (WiFi.status() != WL_CONNECTED) 
  {
    wifi_setup;
  }

  // MQTT reconnect
  if (!client.connected())
  {
    mqtt_reconnect();
  } 

  // Loops
  client.loop();
  DW1000Ranging.loop();

  // RTC
  ulong time = rtc_time(0);
  if (time % 5 == 0)
  {
    if (!sent) Serial.println(time);
    sent = true;
  }
  else sent = false;
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
  while (!client.connected());
  mqtt_subscribe();
  client.publish("detection/status", "online");
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

  if (String(topic) == "detection/command") 
  {
  }

  if (String(topic) == "timestamp") 
  {
    rtc_time(message.toInt());
    ulong time = rtc_time(0);
    Serial.print("Timestamp updated: ");
    Serial.println(time);
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

  DW1000.setAntennaDelay(UWB_DELAY);

  DW1000Ranging.startAsAnchor(ANCHOR_ADD, DW1000.MODE_SHORTDATA_FAST_ACCURACY,false);
}

// Detect UWB
void uwb_newRange()
{
    uint16_t who = DW1000Ranging.getDistantDevice()->getShortAddress();
    uint16_t dist = DW1000Ranging.getDistantDevice()->getRange() * 10;

    //Serial.println(who,HEX);
    char message[64];
    sprintf(message, "Who:%u, Dist:%u", who, dist);
  
    Serial.println(message);
    detection(who,dist);

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
ulong rtc_time(ulong value)
{
  if (value != 0)
  {
    rtc.setTime(value);
  }
  return rtc.getEpoch();
}


// Initalise detection
void detect_init()
{
  for (int i = 0; i < buff_size; i++)
  {
    list_names[i] = 100;
    list_shortest[i] = 100;
  }
}

// Detection event
void detection(uint16_t kart, uint16_t dist)
{
  // Index of a clear slot
  int clr = 0;

  // Loop through array
  for (int i = 0; i < buff_size; i++)
  {
    // Kart in [i] slot
    uint16_t who = list_names[i];

    // Clear slot
    if (who == 100)
    {
      // Make note of a clear index
      clr = i;

      // Move to next index
      continue;
    }

    // Slot is not clear
    else

    // Slot is in use by another kart
    if (kart != who)
    {
      // Move to next index
      continue;
    }

    // Slot is not a different Kart ID
    else

    // This kart ID is already in the array
    if (kart == who)
    {
      // Kart has left AOI -> detection event
      if (dist > detect_out)
      {
        // Did kart cross threshold?
        bool right = list_shortest[i] < detect_thr;

        // Clear index
        list_names[i] = 100;
        list_shortest[i] = 100;

        // Send detection packet
        detect_packet(kart,right);
      }

      // Kart still in AOI
      else

      // New distance is lower than previous
      if (dist < list_shortest[i])
      {
        // Update shortest
        list_shortest[i] = dist;
      }

      // Exit function
      return;
    } // {kart == who}
  }

  // Kart ID not found -> add it
  if (dist < detect_in)
  {
    // Assign parameters to arrays
    list_names[clr] = kart;
    list_shortest[clr] = dist;
  }
}

// Send detection packet
void detect_packet(uint16_t who, bool rightlane)
{
  // Message buffer
  char message[64];

  // Get timestamp
  ulong when = rtc_time(0);

  // Get lane string
  char* what;
  if (rightlane) what = "right";
  else what = "left";

  // Generate JSON message
  // {"Time": 1687638447,"Colour": "red","Lane": "left"}
  sprintf(message, "{\"Time\":%u000,\"Colour\":\"%X\",\"Lane\":\"%s\"}", when, who, what);

  // Publish message
  client.publish("detection/event", message);

  // Seral print
  Serial.println(message);
}