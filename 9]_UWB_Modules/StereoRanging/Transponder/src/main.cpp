/*


*/

// INCLUDE LIBS
#include <Arduino.h>
#include <SPI.h>
#include "DW1000Ranging.h" //https://github.com/thotro/arduino-dw1000

// CHANGE THESE AS REQUIRED
#define MY_ID "02:00:00:00:00:00:00:00"
#define ANT_DEL 16438

// PINS
#define SPI_SCK 18
#define SPI_MISO 19
#define SPI_MOSI 23
#define DW_CS 4

#define PIN_RST 27 // reset pin
#define PIN_IRQ 34 // irq pin
#define PIN_SS 4   // spi select pin

// FUNC PROTOTYPES
void uwb_setup();

// SETUP
void setup()
{
  //Serial.begin(115200);
  uwb_setup();
}

// MAIN LOOP
void loop()
{
  DW1000Ranging.loop();
}

// UWB Setup
void uwb_setup()
{
  SPI.begin(SPI_SCK, SPI_MISO, SPI_MOSI);
  DW1000Ranging.initCommunication(PIN_RST, PIN_SS, PIN_IRQ);
  DW1000.setAntennaDelay(ANT_DEL);
  DW1000Ranging.startAsTag(MY_ID, DW1000.MODE_LONGDATA_RANGE_LOWPOWER,false);
}