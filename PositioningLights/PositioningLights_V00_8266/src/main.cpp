#include <Arduino.h>

#include <Adafruit_NeoPixel.h>

#define NUM 10
#define PIN 2

Adafruit_NeoPixel strip (NUM, PIN, NEO_GRB + NEO_KHZ800);
uint8 c[3] {255, 0, 0};


void setup() 
{
  strip.begin();
  //strip.setBrightness(255);
  strip.show(); // Initialize all pixels to 'off'
  Serial.begin(115200);
}

void loop() 
{
  for (int i = 0; i < NUM; i++)
  {
    strip.setPixelColor(i, strip.Color(c[0], c[1], c[2]));
  }
  strip.show();
  Serial.println("Ping");
  delay(1000);
}