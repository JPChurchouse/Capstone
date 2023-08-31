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

file_infodump = open("debug\\" + str(TimeNow()) + "json","a")


# Callback func for when a message is RXD
def cb_MqttOnReceive(client, userdata, msg):
  info = "MQTT received:\n"+msg.topic+" - "+str(msg.payload)+"\n"
  print(info)
  file_infodump.write(info)
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
  cont = TimeNow() + ms
  while TimeNow() < cont : pass
  return

def DebugThat(info):
  info = info.replace("\'","\"")
  for i in range(10): info = info.replace(str(i)+": {","\""+str(i)+"\": {")

  f = open("debug\\"+str(TimeNow())+".json","w")
  f.write(info)
  f.close()



list_lanes = ["left","right"]
list_karts = ["A","B","C","D"]
list_lapsinfo = [4,4,10]

ran_numread = [5, 15, 1] # number of readings per detection
ran_close = [50, 400, 100] # values for a close reading - deci metres
ran_far = [350, 600, 100] # values for a far reading - deci metres
ran_laptime = [8, 16, 1/1000] # lap time
ran_readtime = [400, 800, 1] # time between readings
ran_bias = [90, 140, 100] # driver speed multiplier
def ran(arr) : return random.randint(arr[0],arr[1]) / arr[2]

data_kartinfo = {}
data_eventlist = []

handle_id = "id"
handle_speedbias = "speed"
handle_events = "events"
handle_readfrom = "read"

handle_timestamp = "time"
handle_station = "station"
handle_reading = "reading"



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

def SendIt():
  filename = "debug/"+str(TimeNow()) + "_.json"
  file = open(filename,"w")

  time_start = TimeNow()
  time_min = ran_readtime [0]
  count_events = len(data_eventlist)

  # Start sending the packets
  index_event = 0
  while index_event < count_events :

    station = data_eventlist [index_event] [handle_station]
    stamp = data_eventlist [index_event] [handle_timestamp]

    id = data_eventlist [index_event] [handle_id]
    read = data_eventlist [index_event] [handle_reading]

    str_list = GenReadList(id, read)

    # See if we can add any more to this packet
    idx = index_event + 1
    while idx < count_events :

      # If wrong station or too late -> give up
      if data_eventlist [idx] [handle_station] != station : break
      if data_eventlist [idx] [handle_timestamp] >= stamp+time_min : break
      if data_eventlist [idx] [handle_id] == id : break

      # Correct station & in timeframe -> add to packet
      id = data_eventlist [idx] [handle_id]
      read = data_eventlist [idx] [handle_reading]

      str_list += ","
      str_list += GenReadList(id,read)

      index_event = idx
      idx += 1

      continue

    index_event += 1

    # Generate packet
    info = GenReadPacket(station, time_start + stamp, str_list)
    file.write(info)

    # Wait for the right time
    while TimeNow() < time_start + stamp : pass

    # Send it!
    print(info)
    MqttPublish(topic_rawinfo, info)

    continue

  file.close()
  return




def CreateKart(id):
  kart = {}
  kart [handle_id] = id
  kart [handle_speedbias] = ran(ran_bias)
  kart [handle_events] = []
  kart [handle_readfrom] = 0
  return kart

def NewReading(time, station, id, value):
  thing = {}
  thing [handle_timestamp] = time
  thing [handle_station] = station
  thing [handle_id] = id
  thing [handle_reading] = value
  return thing


def GenerateMasterList():

  # Generate data kart by kart
  for kart_id in list_karts:

    data_kartinfo [kart_id] = CreateKart(kart_id)

    # Order of lanes
    count_lane_a = random.randint(list_lapsinfo [0], list_lapsinfo [2] - list_lapsinfo [1]) # 8, 20-8 => 8, 12
    list_lane_selections = [0] * list_lapsinfo[2]

    for lap in range(list_lapsinfo[2]) :
      list_lane_selections [lap] = list_lanes [0] if lap < count_lane_a else list_lanes [1]
    random.shuffle(list_lane_selections)

    # Generate lap times
    list_laptimes = [0] * list_lapsinfo[2]
    tally = 0

    for lap in range(list_lapsinfo[2]) : 
      t = ran(ran_laptime) * data_kartinfo [kart_id] [handle_speedbias] # 20-30s * 1.00-2.00
      tally += t
      list_laptimes [lap] = tally

    # Generate all the readings
    for lap in range(list_lapsinfo[2]):

      # Get the start time of the detection
      time_start = int(list_laptimes [lap])

      len_reads_close = int(ran(ran_numread))
      len_reads_far = int(ran(ran_numread))

      list_reads_close = [0] * len_reads_close
      list_reads_far = [0] * len_reads_far

      list_reads_left = []
      list_reads_right = []

      for i in range(len_reads_close): list_reads_close [i] = ran(ran_close)
      for i in range(len_reads_far): list_reads_far [i] = ran(ran_far)

      if list_lane_selections [lap] == list_lanes [0] :
        list_reads_left = list_reads_close
        list_reads_right = list_reads_far
      else:
        list_reads_left = list_reads_far
        list_reads_right = list_reads_close


      list_events_left = []
      list_events_right = []

      t = time_start
      for reading in list_reads_left:
        t += ran(ran_readtime)
        asdf = NewReading(t, list_lanes [0], kart_id, reading)
        list_events_left.append (asdf)
        asdf = 5
      
      t = time_start
      for reading in list_reads_right:
        t += ran(ran_readtime)
        list_events_right.append (NewReading(t, list_lanes [1], kart_id, reading))

      index_left = 0
      size_left = len(list_events_left)
      index_right = 0
      size_right = len(list_events_right)
      # Keep going until all readings are processed
      while index_left < size_left and index_right < size_right:
        
        # If LEFT has run out
        if not (index_left < size_left):
          data_kartinfo [kart_id] [handle_events] += list_events_right [index_right]
          index_right += 1

        # If RIGHT has run out
        elif not (index_right < size_right):
          data_kartinfo [kart_id] [handle_events] += list_events_left [index_left]
          index_left += 1
        
        # Both still valid
        else:
          left = list_events_left [index_left]
          right = list_events_right [index_right]
          if left [handle_timestamp] < right [handle_timestamp]:
            data_kartinfo [kart_id] [handle_events].append(dict(left))
            index_left += 1
          else:
            data_kartinfo [kart_id] [handle_events].append(dict(right))
            index_right += 1

        continue

  DebugThat(str(data_kartinfo))
  # Sort them all together
  karts = list_karts

  while len(karts) > 0:

    stamp_best = 3600000
    id_best = ""

    for id in karts:
      index_readfrom = data_kartinfo [id] [handle_readfrom]
      stamp = data_kartinfo [id] [handle_events] [index_readfrom] [handle_timestamp]

      if stamp < stamp_best :
        stamp_best = stamp
        id_best = id

    index_readfrom = data_kartinfo [id_best] [handle_readfrom]
    event = data_kartinfo [id_best] [handle_events] [index_readfrom]
    data_eventlist.append(event)

    readfrom_new = index_readfrom + 1

    if readfrom_new < len(data_kartinfo [id_best] [handle_events]) :
      data_kartinfo [id_best] [handle_readfrom] += 1
    else:
      karts.remove(id_best)

  DebugThat(str(data_eventlist))

  return

def GenReadPacket(station,timestamp,list_ranges):
  return "{\"Station\":\""+station+"\",\"Time\":"+str(int(timestamp))+",\"List\":["+list_ranges+"]}"
def GenReadList(id,range):
  return "{\"ID\":\""+id+"\",\"Range\":"+str(range)+"}"

# Main while-loop
def main() :
  InitRace()
  GenerateMasterList()
  StartRace()
  SendIt()
  EndRace()
  MqttWait(5000)
  MqttDisconnect()
  return




if __name__ == "__main__":
    sys.exit(main())