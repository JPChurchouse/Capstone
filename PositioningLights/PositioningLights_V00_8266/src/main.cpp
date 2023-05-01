#include <Arduino.h>

#include <Adafruit_NeoPixel.h>

#define NUM 8
#define PIN 6

Adafruit_NeoPixel strip = Adafruit_NeoPixel(NUM, PIN, NEO_GRB + NEO_KHZ800);
uint8 c[3] {255, 0, 0};


void setup() 
{
  strip.begin();
  strip.setBrightness(50);
  strip.show(); // Initialize all pixels to 'off'
}

void loop() 
{
  for (int i = 0; i < NUM; i++)
  {
    strip.setPixelColor(i, strip.Color(c[0], c[1], c[2]));
  }
  return;
}