#include <Arduino.h>
#include <Adafruit_NeoPixel.h>
#define NUM 16
#define PIN 2


Adafruit_NeoPixel strip = Adafruit_NeoPixel(NUM, PIN, NEO_GRB + NEO_KHZ800);

int  c[3] = {0, 0, 0};
int  g[10] = {100,  255,  0,    0,   255,  0,   150,  200, 250,  90};
int  b[10] = {0,    100,  255,  90,  0,    255, 100,  200, 90,   90};
int  r[10] = {255,  0,    100,  200, 255,  255, 0,    0,   100,  250};
int j;

void setup() 
{
  strip.begin();
  strip.setBrightness(10);
  strip.show(); // Initialize all pixels to 'off'
  
  j = 0;
  //c[3] = {0, 0, 0};
  //g[10] = {100,  255,  0,    0,   255,  0,   150,  200, 250,  0};
  //b[10] = {0,    100,  255,  90,  0,    255, 100,  200, 90,   90};
  //r[10] = {255,  0,    100,  200, 255,  255, 0,    0,   100,  250};
}

void loop() 
{
  c[0] = r[j];
  c[1] = g[j];
  c[2] = b[j];
  for (int i = 0; i < NUM; i++)
  {
    strip.setPixelColor(i, strip.Color(c[0], c[1], c[2]));
  }
  strip.show();
  //return;
  delay(3000);
  
  if(j<10){
    j++;
  }
  else{
    j = 0;
  }
  
}
