#include <Arduino.h>
#include <Adafruit_NeoPixel.h>
#define NUM 10
#define PIN 2


Adafruit_NeoPixel strip = Adafruit_NeoPixel(NUM, PIN, NEO_GRB + NEO_KHZ800);

int  c[3] = {0, 0, 0};
int  g[31] = {119,127,0,  205,107,63, 254,52, 255,197,255,254,0,  255,78, 255,0,  255,255,255,43, 42, 88, 173,0,  239,40, 53, 0,  0, 255};
int  b[31] = {122,210,137,116,36, 66, 255,69, 136,128,184,244,214,160,104,179,255,200,233,216,59, 83, 255,201,0,  239,190,207,127,255,0};
int  r[31] = {0,  0,  2,  24, 38, 70, 77, 87, 94, 96, 99, 113,116,125,140,152,173,180,199,202,206,206,228,231,238,240,247,252,255,255,0};
int j;

void setup() 
{
  strip.begin();
  strip.setBrightness(50);
  strip.show(); // Initialize all pixels to 'off'
  
  j = 0;
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

  delay(5000);
  
  if(j<30){
    j++;
  }
  else{
    j = 0;
  }
  
}
