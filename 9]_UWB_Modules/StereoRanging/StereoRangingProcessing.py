# Install the MQTT library using the folloing pip command
#pip install paho-mqtt

# cb_Function   -> callback, dont call directly
# TxFunction    -> Transmit
# MqttFunction  -> MQTT function

# Import libraries
import os
import sys
import paho.mqtt.client as mqtt
import time
import json

import DataHandling as data



# Topics
const_topic_status    = "detect/status"
const_topic_rawinfo   = "detect/info"
const_topic_timestamp = "detect/ts"
const_topic_detection = "race/detect"
const_mqtt_address    = "192.168.1.20"     # MQTT broker address - will be "192.168.1.20" on site
const_mqtt_port       = 1883               # Port number for MQTT broker connection


# Callback func for when a message is RXD
def cb_MqttOnReceive(client, userdata, msg):
  print(msg.topic+" "+str(msg.payload))
  if msg.topic == const_topic_rawinfo:
      DataParse(msg.payload.decode('UTF-8'))


# Callback func for when client connects
def cb_MqttOnConnect(client, userdata, flags, rc):
  print("Connected with result code "+str(rc))
  client.will_set(const_topic_status,"Offline",1,True)    # Set will
  client.subscribe(const_topic_rawinfo)                   # Sub to detection commands
  #client.subscribe("#")
  TxStatus("Online")


# Disconnect the client
def MqttDisconnect():
  TxStatus("Offline")
  client.disconnect()


# Func to update the network on this devices status
def TxStatus(msg):
  client.publish(const_topic_status, msg, 1, True)


# Func to publish detction info
def TxDetection(colour,lane):
  now = str(TimeNow)
  json = "{\"Time\": " + now + ",\"Colour\": \"" + colour + "\",\"Lane\": \"" + lane + "\"}"
  client.publish(const_topic_detection, json, 1, False)


# Update ESP32 UWB timestamps
def TxTimestamp():
  now = str(TimeNow)
  client.publish(const_topic_timestamp, now, 1, False)


# Get current ms
def TimeNow():
  return int(time.time_ns() / 1000000)


# Process received raw data
# https://www.w3schools.com/python/python_json.asp
def DataParse(json):

  # Chance of a JSON failure
  try:

    # Import JSON data as a dictionary
    my_data = json.loads(json)

    # Read in station and time information
    station = my_data["Station"]
    time = my_data["Time"]
    period = my_data["Period"]

    # Iterate over each kart in the received data
    for kart in my_data["List"]:

      # Read kart ID and range
      id = kart["Kart"]
      range = kart["Range"]

      # Process this data
      DataProcess(station, time, id, range)

    return

  # Catch any exceptions, show them, return
  except Exception:
    print(Exception)
    print(json)
    return


# Process parsed data into storage
def DataProcess(station, time, kart, distance):
  pass

# Reset data storage
# index of kart, 0 for all
def DataClear(index):
  pass


# Init MQTT Client and connect
client = mqtt.Client()
client.on_connect = cb_MqttOnConnect
client.on_message = cb_MqttOnReceive
client.connect(const_mqtt_address, const_mqtt_port, 60)
#client.loop_forever()

# Main While loop
def main():
  while True:
    client.loop(timeout=1.0, max_packets=1)     # Run MQTT stuff



if __name__ == "__main__":
    sys.exit(main())