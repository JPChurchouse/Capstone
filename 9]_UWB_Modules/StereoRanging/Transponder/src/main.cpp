/*


*/

// INCLUDES AND DEFINES
#include <Arduino.h>
#include <SPI.h>
#include "DW1000Ranging.h"     //https://github.com/thotro/arduino-dw1000

#define MY_ID "01:00:00:00:00:00:00:00"

#define SPI_SCK 18
#define SPI_MISO 19
#define SPI_MOSI 23
#define DW_CS 4


// UWB INIT
const uint8_t PIN_RST = 27; // reset pin
const uint8_t PIN_IRQ = 34; // irq pin
const uint8_t PIN_SS = 4;   // spi select pin

void uwb_setup();
void uwb_newRange();
void uwb_newBlink(DW1000Device*);
void uwb_inactiveDevice(DW1000Device*);


// SETUP
void setup()
{
  Serial.begin(115200);
  uwb_setup();
  delay(1000);

}

bool sent = false;
// MAIN LOOP
void loop()
{
  DW1000Ranging.loop();
}

// UWB Setup
void uwb_setup()
{
    //init the configuration
    SPI.begin(SPI_SCK, SPI_MISO, SPI_MOSI);
    DW1000Ranging.initCommunication(PIN_RST, PIN_SS, PIN_IRQ); //Reset, CS, IRQ pin
    //define the sketch as anchor. It will be great to dynamically change the type of module
    //DW1000Ranging.attachNewRange(newRange);
    //DW1000Ranging.attachNewDevice(newDevice);
    //DW1000Ranging.attachInactiveDevice(inactiveDevice);
    //Enable the filter to smooth the distance
    //DW1000Ranging.useRangeFilter(true);

    DW1000.setAntennaDelay(16438);

    //we start the module as a tag
    DW1000Ranging.startAsTag(MY_ID, DW1000.MODE_SHORTDATA_FAST_ACCURACY,false);
}

// Detect UWB
void uwb_newRange()
{
    uint16_t who = DW1000Ranging.getDistantDevice()->getShortAddress();
    float dist = DW1000Ranging.getDistantDevice()->getRange();

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

