/*


*/

// INCLUDES //
#include <Arduino.h>
#include <SPI.h>
#include "DW1000Ranging.h" //https://github.com/thotro/arduino-dw1000
#include <WiFi.h>
#include <PubSubClient.h>


// SETTINGS //

// Station
#define STATION_NAME        "left"
#define MODULE_MAC          "00:00:00:00:00:00:00:02"
#define ANTENNA_DELAY       16438

// Network
#define WIFI_SSID           "hotspot"
#define WIFI_PASS           "password"
#define MQTT_IP             "192.168.1.21"

// Readings
#define READS_MAX           0.5
#define READS_PERIOD        100
#define READS_BUFFSIZE       04


// GLOBAL CONSTS //

// MQTT topics
#define TOPIC_DATA          "detect/info"
#define TOPIC_TIMESTAMP     "detect/ts"
#define TOPIC_STATUS        "detect/status"
#define TOPIC_COMMAND       "detect/cmd"

#define TOPIC_STATUS_LOCAL   TOPIC_STATUS  "/" STATION_NAME
#define TOPIC_COMMAND_LOCAL  TOPIC_COMMAND "/" STATION_NAME

// SPI pins
#define SPI_SCK   18
#define SPI_MISO  19
#define SPI_MOSI  23
#define SPI_CS_DW  4

// DW100 pins
#define PIN_RST   27 // reset pin
#define PIN_IRQ   34 // irq pin
#define PIN_SS     4 // spi select pin


// GLOBAL VARIABLES & OBJECTS

// Network
WiFiClient WifiClient;
PubSubClient MqttClient(WifiClient);

// Readings
float reads_list_readings [READS_BUFFSIZE];
uint16_t reads_list_ids [READS_BUFFSIZE];

// Timekeeping
uint64_t time_offset = 0;


// FUNCTION PROTOTYPES

// Network
void Wifi_Setup();
void Mqtt_Setup();
void Mqtt_Reconnect();
void Mqtt_Subscribe();
void cb_Mqtt_Message(char*, byte *, unsigned int); // cb == callback, don't call this one directly

// UWB
void Uwb_Setup();
void Uwb_NewRange();

// Readings
void Reads_Clear();
void Reads_AddReading(uint16_t, float);
void Reads_SendPacket();

// Timekeeping
uint64_t Time_Get();
uint64_t Time_Set(uint64_t);


// MAIN FUNCTIONS

// Running macros
void Run_Infrequent(uint64_t);
int  run_infreq_period = 100;
bool run_infreq_flag = false;

// Setup
void setup()
{
  Serial.begin(115200);

  Wifi_Setup();
  delay(1000);

  Mqtt_Setup();
  delay(1000);

  Uwb_Setup();
  delay(1000);

  Reads_Clear();
}

// Main loop
void loop()
{
  // Reconnect wifi
  if (WiFi.status() != WL_CONNECTED) Wifi_Setup;

  // Reconnect MQTT
  if (!MqttClient.connected()) Mqtt_Reconnect();

  // Run MQTT and UWB
  MqttClient.loop();
  DW1000Ranging.loop();

  // Run infrequent macro
  uint64_t now = Time_Get();
  if (now % run_infreq_period == 0)
  {
    if (!run_infreq_flag) Run_Infrequent(now);
    run_infreq_flag = true;
  }
  else run_infreq_flag = false;
}

// Infrequent macro
void Run_Infrequent(uint64_t now)
{
  Serial.println(now);
  Reads_SendPacket();
}


// NETWORK FUNCTIONS

// Setup Wifi
void Wifi_Setup()
{
  delay(10);
  Serial.println();
  Serial.print("Connecting to ");
  Serial.println(WIFI_SSID);

  WiFi.begin(WIFI_SSID, WIFI_PASS);

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
void Mqtt_Setup()
{
  MqttClient.setServer(MQTT_IP, 1883);
  MqttClient.setCallback(cb_Mqtt_Message);
  MqttClient.publish(TOPIC_STATUS_LOCAL, "Online");
}

// Subscribe to MQTT topics
void Mqtt_Subscribe()
{
  MqttClient.subscribe(TOPIC_TIMESTAMP);
  MqttClient.subscribe(TOPIC_COMMAND_LOCAL);
}

// New MQTT message arrived
void cb_Mqtt_Message(char* topic, byte* msg, unsigned int length)
{
  Serial.print("Message arrived on topic: ");
  Serial.print(topic);
  Serial.print(". Message: ");
  String message;
  uint64_t count = 0;

  // Process message
  for (int i = 0; i < length; i++)
  {
    char c = (char)msg[i];
    message += c;

    uint16_t val = uint16_t(c - '0');
    count = count * 10 + val;
    Serial.println(count);
  }

  Serial.println(message);

  if (String(topic) == TOPIC_COMMAND_LOCAL)
  {
    if (message.indexOf("ping") >= 0)
    {
      MqttClient.publish(TOPIC_STATUS_LOCAL,"Ping "+Time_Get());
    }
  }

  if (String(topic) == TOPIC_TIMESTAMP)
  {
    Serial.println(count);
    Time_Set(count);
  }
}

// Reconnect to MQTT
void Mqtt_Reconnect()
{
  // Loop until we're reconnected
  while (!MqttClient.connected())
  {
    Serial.print("Attempting MQTT connection...");

    // Attempt to connect
    if (MqttClient.connect(STATION_NAME))
    {
      Serial.println("connected");
      Mqtt_Subscribe();
    } 

    else
    {
      Serial.print("failed, rc=");
      Serial.print(MqttClient.state());
      Serial.println(" try again in 5 seconds");
      // Wait 5 seconds before retrying
      delay(5000);
    }
  }
}


// UWB FUNCTIONS

// UWB Setup
void Uwb_Setup()
{
  //init the configuration
  SPI.begin(SPI_SCK, SPI_MISO, SPI_MOSI);
  DW1000Ranging.initCommunication(PIN_RST, PIN_SS, PIN_IRQ); //Reset, CS, IRQ pin
  //define the sketch as anchor. It will be great to dynamically change the type of module
  DW1000Ranging.attachNewRange(Uwb_NewRange);
  //DW1000Ranging.attachBlinkDevice(uwb_newBlink);
  //DW1000Ranging.attachInactiveDevice(uwb_inactiveDevice);
  //Enable the filter to smooth the distance
  //DW1000Ranging.useRangeFilter(true);

  DW1000.setAntennaDelay(ANTENNA_DELAY);

  DW1000Ranging.startAsAnchor(MODULE_MAC, DW1000.MODE_LONGDATA_RANGE_LOWPOWER, false);
}

// Detect UWB
void Uwb_NewRange()
{
  uint16_t who = DW1000Ranging.getDistantDevice()->getShortAddress();
  float what = DW1000Ranging.getDistantDevice()->getRange();

  Serial.print("New reading: ");
  Serial.print(who, HEX);
  Serial.print(" ");
  Serial.print(what);

  if (what < 0.0)
  {
    Serial.println("Rejected reading");
    return;
  }

  if (what < READS_MAX) Reads_AddReading(who, what);
}


// TIME FUNCTIONS

uint64_t Time_Get()
{
  return time_offset + millis();
}

uint64_t Time_Set(uint64_t value)
{
  time_offset = value - millis();
  return Time_Get();
}


// READINGS FUNCTIONS

// Clear the readings buffers
void Reads_Clear()
{
  for (int i = 0; i < READS_BUFFSIZE; i++)
  {
    reads_list_ids [i] = 0;
    reads_list_readings [i] = 0;
  }
}

// Add a reading
void Reads_AddReading(uint16_t who, float dist)
{
  // Note a free slot
  int free = -1;

  // Iterate over each item in the array
  for (int i = 0; i < READS_BUFFSIZE; i++)
  {
    // ID at index
    uint16_t id = reads_list_ids [i];

    // Overwrite
    if (id == who)
    {
      reads_list_readings [i] = dist;
      return;
    }

    // Note a free slot
    if (id == 0 && free == -1) free = i;
  }

  // All slots are taken by other IDs (shouldn't be possible)
  if (free == -1) return;

  // Write values at free index
  reads_list_ids [free] = who;
  reads_list_readings [free] = dist;
}

// Generate and send a packet from current info, clear buffer
void Reads_SendPacket()
{
  // Check ther is actually some data
  bool ret = true;
  for (int i = 0; i < READS_BUFFSIZE; i++)
  {
    if (reads_list_ids [i] == 0) continue;
    ret = false;
    break;
  }
  if (ret) return;

  // Init buffer and time val
  char m [512];
  String now = String(Time_Get());

  // Packet header
  sprintf(m, "{\"Station\":\"%s\",\"Time\":%s,\"List\":[", STATION_NAME, now);

  // Bring in the readings
  for (int i = 0; i < READS_BUFFSIZE; i++)
  {
    // Read values
    uint16_t id = reads_list_ids [i];
    float dist = reads_list_readings [i];

    // If not a reading -> continue
    if (id == 0) continue;

    // Append this info to the string
    sprintf(m + strlen(m), "{\"ID\":\"%X\",\"Range\":%f},", id, dist);
  }

  // End the packet [-1 to overwrite the last comma]
  sprintf(m + strlen(m)- 1, "]}");

  // Send
  MqttClient.publish(TOPIC_DATA, m);
  Serial.println(m);

  // Clear the arrays
  Reads_Clear();
}