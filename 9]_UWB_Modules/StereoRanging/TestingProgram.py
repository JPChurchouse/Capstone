# Install the MQTT library using the folloing pip command
#pip install paho-mqtt

# cb_Function   -> callback, dont call directly
# MqttFunction  -> MQTT function

# Import libraries
import os
import sys
import paho.mqtt.client as mqtt
import time
import json

import DataHandling as data


# MQTT constants
topic_status    = "detect/status"
topic_rawinfo   = "detect/info"
topic_timestamp = "detect/ts"
topic_detection = "race/detect"
mqtt_address    = "192.168.1.20"     # MQTT broker address - will be "192.168.1.20" on site
mqtt_port       = 1883               # Port number for MQTT broker connection


# Callback func for when a message is RXD
def cb_MqttOnReceive(client, userdata, msg):
  print("MQTT received:\n"+msg.topic+" - "+str(msg.payload)+"\n")
  return


# Callback func for when client connects
def cb_MqttOnConnect(client, userdata, flags, rc):
  print("Connected with result code "+str(rc))
  #client.will_set(topic_status,"Offline",1,True)    # Set will
  #client.subscribe(topic_rawinfo)                   # Sub to detection commands
  #client.subscribe("#")
  #MqttPublish(topic_status,"Online")
  return


# Disconnect the client
def MqttDisconnect():
  #MqttPublish(topic_status,"Offline")
  client.disconnect()
  return


# Func to publish to mqtt
def MqttPublish(topic, payload):
  client.publish(topic, payload, 1, True)
  return



# Get time now ms
def TimeNow(): return int(round(time.time()*1000))




# Init MQTT Client and connect
client = mqtt.Client()
client.on_connect = cb_MqttOnConnect
client.on_message = cb_MqttOnReceive
client.connect(mqtt_address, mqtt_port, 60)
#client.loop_forever()

stations = ["one","two","two","two","one","two","one"]
karts = ["a","b","a","b","a","b","a"]
ranges = [1.2,3.4,1.6,2.0,4.3,1.0,0.6]

def gimme(index):
  json = "{\"Station\":\""+stations[index]+"\",\"Time\":"+str(TimeNow())+",\"List\":[{\"Kart\":\""+karts[index]+"\",\"Range\":"+str(ranges[index])+"}]}"

  print(json)
  return json

# Main while-loop
def main():
  for i in range(0,len(karts)-1):
    MqttPublish(topic_rawinfo,gimme(i))
    print(i)
    time.sleep(1)
  return


if __name__ == "__main__":
    sys.exit(main())