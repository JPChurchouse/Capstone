/*


*/

// INCLUDES AND DEFINES
#include <Arduino.h>
#include <SPI.h>
#include "DW1000Ranging.h"     //https://github.com/thotro/arduino-dw1000
#include <WiFi.h>
#include <PubSubClient.h>

#define ANCHOR_ADD "00:00:00:00:00:00:00:00"

#define SPI_SCK 18
#define SPI_MISO 19
#define SPI_MOSI 23
#define DW_CS 4

#define topic_rawdata "detect/info"
#define topic_timestamp "detect/ts"
#define topic_status "detect/status/one"
#define topic_cmd "detect/cmd/one"
#define station_name "left"

// WIFI AND MQTT INIT  
const char* ssid = "Lodge Wireless Internet";
const char* password = "JulietCharlieHotelQuebec";
const char* mqtt_server = "192.168.1.20";
char msg[64];

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
const uint16_t uwb_adel = 16438;

void uwb_setup();
void uwb_newRange();
void uwb_newBlink(DW1000Device*);
void uwb_inactiveDevice(DW1000Device*);


// TIMEKEEPING
uint64_t time_offset = 0;
uint64_t time_get();
uint64_t time_set(uint64_t);


// Detection handling
const float read_max = 6.0;
const uint16_t read_period = 100;
uint64_t read_lastreading = 0;
String read_packet = "";
const uint8_t read_buffsize = 10;
float read_list_readings [read_buffsize];
uint16_t read_list_ids [read_buffsize];

void read_clear();
void read_addreading(uint16_t, float);
void read_sendpacket();


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

  read_clear();
}

void infrequent(uint64_t);

int period = 100;
bool sent = false;
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

  uint64_t time = time_get();
  if (time % period == 0)
  {
    if (!sent) infrequent(time);
    sent = true;
  }
  else sent = false;
}
void infrequent(uint64_t now)
{
  if (now % (period * 100) == 0)
  {
    for (int i = 0; i < 6; i++)
    {
      read_list_ids [i] = i;
      read_list_readings [i] = (i * i - i) + 2.0/i;
    }
  }
  Serial.println(now);
  read_sendpacket();
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
  client.publish(topic_status, "online");
}

// Subscribe to MQTT topics
void mqtt_subscribe()
{
  client.subscribe(topic_timestamp);
  client.subscribe(topic_cmd);
}

// New MQTT message arrived
void mqtt_message(char* topic, byte* msg, unsigned int length) 
{
  Serial.print("Message arrived on topic: ");
  Serial.print(topic);
  Serial.print(". Message: ");
  String message;
  uint64_t count = 0;
  
  // Process message
  for (int i = 0; i < length; i++) 
  {
    char c = (char) msg[i];
    message += c;

    uint16_t val = uint16_t(c - '0');
    count = count * 10 + val;
    Serial.println(count);
  }

  Serial.println(message);

  if (String(topic) == topic_cmd) 
  {
  }

  if (String(topic) == topic_timestamp) 
  {
    Serial.println(count);
    time_set(count);
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

  DW1000.setAntennaDelay(uwb_adel);

  DW1000Ranging.startAsAnchor(ANCHOR_ADD, DW1000.MODE_SHORTDATA_FAST_ACCURACY,false);
}

// Detect UWB
void uwb_newRange()
{
    uint16_t who = DW1000Ranging.getDistantDevice()->getShortAddress();
    float dist = DW1000Ranging.getDistantDevice()->getRange();

    Serial.println(who,HEX);
    if (dist > read_max) return;
    
    read_addreading(who,dist);

    return;

    char message[32];
    sprintf(message, "{\"ID\":\"%X\",\"Dist\":%f}",who,dist);

    //client.publish("detect", message);
    

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


// Time
uint64_t time_get()
{
  return time_offset + millis();
}
uint64_t time_set(uint64_t value)
{
  time_offset = value - millis();
  return time_get();
}

void read_clear()
{
  for (int i = 0; i < read_buffsize; i++)
  {
    read_list_ids [i] = 0;
    read_list_readings [i] = 0;
  }
}

void read_addreading(uint16_t who, float dist)
{
  // Note a free slot
  int free = -1;

  // Iterate over each item in the array
  for (int i = 0; i < read_buffsize; i++)
  {
    // ID at index
    uint16_t id = read_list_ids [i];

    // Overwrite
    if (id == who)
    {
      read_list_readings [i] = dist;
      return;
    }

    // Note a free slot
    if (id == 0 && free == -1) free = i;
  }

  // All slots are taken by other IDs (shouldn't be possible)
  if (free == -1) return;

  // Write values at free index
  read_list_ids [free] = who;
  read_list_readings [free] = dist;

  return;
}

// Generate packet
void read_sendpacket()
{
  // Check ther is actually some data
  bool ret = true;
  for (int i = 0; i < read_buffsize; i++)
  {
    if (read_list_ids [i] == 0) continue;
    ret = false;
    break;
  }
  if (ret) return;

  // Init buffer and time val
  char m[512];
  uint64_t now = time_get();

  // Packet header
  sprintf(m, "{\"Station\":\"%s\",\"Time\":%u,\"List\":[", station_name, now);

  // Bring in the readings
  for (int i = 0; i < read_buffsize; i++)
  {
    // Read values
    uint16_t id = read_list_ids [i];
    float dist = read_list_readings [i];

    // If not a reading -> continue
    if (id == 0) continue;

    // Append this info to the string
    sprintf(m + strlen(m), "{\"ID\":\"%X\",\"Range\":%f},", id, dist);
  }

  // End the packet [-1 to overwrite the last comma]
  sprintf(m + strlen(m) - 1, "]}");

  // Send
  client.publish(topic_rawdata, m);

  // Clear the arrays
  read_clear();

  return;
}