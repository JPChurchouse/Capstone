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

# JSON handles
handle_station  = "Station"
handle_timestamp= "Time"
handle_kartlist = "List"
handle_id       = "ID"
handle_range    = "Range"

invalid         = "null"



# Callback func for when a message is RXD
def cb_MqttOnReceive(client, userdata, msg) :
  print("MQTT received:\n"+msg.topic+" - "+str(msg.payload)+"\n")
  if msg.topic == topic_rawinfo :
      DataParseAndProcess(msg.payload.decode('UTF-8'))
  return


# Callback func for when client connects
def cb_MqttOnConnect(client, userdata, flags, rc) :
  print("Connected with result code "+str(rc))
  client.will_set(topic_status, "Offline", 1, True) # Set will
  client.subscribe(topic_rawinfo)                   # Sub to detection commands
  #client.subscribe("#")
  MqttPublish(topic_status, "Online")
  return


# Disconnect the client
def MqttDisconnect() :
  MqttPublish(topic_status, "Offline")
  client.disconnect()
  return


# Func to publish to mqtt
def MqttPublish(topic, payload, retain = False) :
  client.publish(topic, payload, 1, retain)
  return


# Update ESP32 UWB timestamps
def TxTimestamp() :
  now = str(TimeNow())
  print("Sending timestamp: "+now+"\n")
  client.publish(topic_timestamp, now, 1, False)
  return


# Get time now ms
def TimeNow() : return int(round(time.time()*1000))


# Process received raw data
# https://www.w3schools.com/python/python_json.asp
def DataParseAndProcess(raw) :

  # Clean up the data
  print("Parsing data:")
  clean = raw.replace("\n","").replace("\r","").replace(" ","")
  print(clean)

  # Chance of a JSON failure
  try :

    # Import JSON data as a dictionary
    my_data = json.loads(clean)

    # Read in station and time information
    station = my_data [handle_station]
    time = my_data [handle_timestamp]
    print("Stat: "+station+", Time: "+str(time))

    # Iterate over each kart in the received data
    for kart in my_data [handle_kartlist] :

      # Read kart ID and range
      id = kart [handle_id]
      value = kart [handle_range]
      print("Kart: "+id+", Val: "+str(value))

      # Add this reading to the database
      ret = data.AddValue(id, station, time, value)
      if ret == invalid : raise Exception("return invalid")

    # Run detection checker
    DataCheckDetections()

  # Catch any exceptions, show them
  except Exception as e :
    print("Exception:\n", e)
    MqttPublish(topic_status, "Error_Parsing")

  print()
  return


# Func to check for detections and then send them
def DataCheckDetections() :

  # Read array of detected karts
  arr = data.CheckForDetections()

  if arr == [] : return
  DebugThat()

  # Iterate over each given index (index of kart in the data module)
  for i in arr :
    payload = data.GenerateDetectionPacket(i)
    print("New detection:\n"+payload+"\n")
    MqttPublish(topic_detection, payload)

  return



# Init MQTT Client and connect
client = mqtt.Client()
client.on_connect = cb_MqttOnConnect
client.on_message = cb_MqttOnReceive
client.connect(mqtt_address, mqtt_port, 60)
#client.loop_forever()


def DebugThat():

  info = str(data.printout())
  info = info.replace("\'","\"")
  for i in range(10): info = info.replace(str(i)+": {","\""+str(i)+"\": {")

  f = open("debug\\"+str(TimeNow())+".json","w")
  f.write(info)
  f.close()


# Main while-loop
def main() :

  # Init our database
  data.NewRace()

  TxTimestamp()

  # Iteration value
  timeout = 0
  delay   = 3000
  periods = 20

  # Main while-loop
  while True :

    # Run MQTT stuff
    client.loop(timeout = 1.0, max_packets = 1)

    # Detection checking
    if TimeNow() > timeout : # - not assigning TimeNow to a variable so theres no need for multiple read write operations
      
      # Recalculate timeout, check for detections
      timeout = TimeNow() + delay
      DataCheckDetections()

      # Occasionally update timestamps and status
      # Delay factor to only run every x delay periods
      if (TimeNow() % (periods * delay)) < delay :
        TxTimestamp()
        MqttPublish(topic_status, "Online", True)

    continue

  return

if __name__ == "__main__" :
  sys.exit(main())