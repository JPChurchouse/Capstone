#include <Arduino.h>

const uint8_t pinEncoder = 36;

void setup() 
{
  pinMode(pinEncoder,INPUT);
  Serial.begin(115200);
}

void loop() 
{
  Serial.println(digitalRead(pinEncoder) ? "HIGH" : "LOW");
  delay(5);
}