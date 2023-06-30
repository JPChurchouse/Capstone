#include <Arduino.h>

const uint8_t pins[22] = {32,33,25,26,27,14,12,13,23,22,1,3,21,19,18,5,17,16,4,0,2,15};

void setup() 
{
  for (int i = 0; i < 22; i++)
  {
    pinMode(pins[i], OUTPUT);
  }

  Serial.begin(115200);
}

void loop() 
{
  for (int i = 0; i < 22; i++)
  {
    Serial.println(pins[i]);
    digitalWrite(pins[i], HIGH);
    delay(500);
    digitalWrite(pins[i], LOW);
  }
  
}
