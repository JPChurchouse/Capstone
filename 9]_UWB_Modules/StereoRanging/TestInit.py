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
import random

import DataHandling as data


# MQTT constants
topic_status    = "detect/status"
topic_rawinfo   = "detect/info"
topic_timestamp = "detect/ts"
topic_detection = "race/detect"
topic_config    = "race/config"
topic_command   = "race/cmd"
mqtt_address    = "192.168.1.20"     # MQTT broker address - will be "192.168.1.20" on site
mqtt_port       = 1883               # Port number for MQTT broker connection

# Get time now ms
def TimeNow(): return int(round(time.time()*1000))

# Callback func for when a message is RXD
def cb_MqttOnReceive(client, userdata, msg):
  info = "MQTT received:\n"+msg.topic+" - "+str(msg.payload)+"\n"
  print(info)
  return


# Callback func for when client connects
def cb_MqttOnConnect(client, userdata, flags, rc):
  print("Connected with result code "+str(rc))
  #client.will_set(topic_status,"Offline",1,True)    # Set will
  #client.subscribe(topic_rawinfo)                   # Sub to detection commands
  #client.subscribe("#")
  #MqttPublish(topic_status,"Online")
  return

# Init MQTT Client and connect
client = mqtt.Client()
client.on_connect = cb_MqttOnConnect
client.on_message = cb_MqttOnReceive
client.connect(mqtt_address, mqtt_port, 60)
client.loop_start()

# Disconnect the client
def MqttDisconnect():
  #MqttPublish(topic_status,"Offline")
  client.disconnect()
  return

# Func to publish to mqtt
def MqttPublish(topic, payload):
  print ("Publishing: \""+ topic +"\", \""+ payload +"\"\r\n")
  client.publish(topic, payload, 1, False)
  return

def MqttWait(ms):
  client.loop_stop()
  cont = TimeNow() + ms
  while TimeNow() < cont : client.loop(timeout = 1.0, max_packets = 1)
  client.loop_start()
  return


list_lanes = ["left","right"]
list_karts = ["01"]
list_lapsinfo = [8,8,20]



def GetJsonKartList():
  list_usednums = []
  info = ""

  for kart in list_karts:

    # Generate a random kart number
    num = 0
    while True:
      num = random.randint(1,30)
      try:
        list_usednums.index(num)
      except:
        list_usednums.append(num)
        break

    # Add a comma
    if info != "": info += ","

    info += "{\"Colour\":\""
    info += kart
    info += "\",\"Number\":"
    info += str(num)
    info += "}"

  return info

def GetJsonLaps():
  return str(list_lapsinfo[0])+","+str(list_lapsinfo[1])+","+str(list_lapsinfo[2])

def GetJsonConfig():
  return "{\"KartList\":["+GetJsonKartList()+"],\"RequiredLaps\":["+GetJsonLaps()+"]}"

def InitRace():
  info = GetJsonConfig()
  print(info)
  MqttPublish(topic_config, info)
  return

def StartRace():
  MqttPublish(topic_command, "start")
  return

def EndRace():
  MqttPublish(topic_command, "end")
  return



def GenReadPacket(station,timestamp,list_ranges):
  return "{\"Station\":\""+station+"\",\"Time\":"+str(int(timestamp))+",\"List\":["+list_ranges+"]}"
def GenReadList(id,range):
  return "{\"ID\":\""+id+"\",\"Range\":"+str(range)+"}"

# Main while-loop
def main() :
  InitRace()
  StartRace()
  #MqttWait(5000)
  #EndRace()
  #MqttDisconnect()
  return




if __name__ == "__main__":
    sys.exit(main())