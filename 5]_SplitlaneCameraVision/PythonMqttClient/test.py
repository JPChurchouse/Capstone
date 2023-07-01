# pip install paho-mqtt

import paho.mqtt.client as mqtt
import time

broker_address = "10.2.245.108"
running = False

# The callback for when the client receives a CONNACK response from the server.
def on_connect(client, userdata, flags, rc):
    print("Connected with result code "+str(rc))
    client.will_set("status/detection","offline",1,True)
    client.subscribe("command/detection")
    client.subscribe("#")
    publish_status("online")

# The callback for when a PUBLISH message is received from the server.
def on_message(client, userdata, msg):
    print(msg.topic+" "+str(msg.payload))
    if msg.topic == "command/detection":
        process_command(msg.payload.decode('UTF-8'))
    
# Func to publish system status
def publish_status(msg):
    client.publish("status/detection", msg, 1, True)

# Func to publish detction
def publish_detect(colour,lane):
    timenow = str(TimeNowMs())
    json = "{\"Time\": " + timenow + ",\"Colour\": \"" + colour + "\",\"Lane\": \"" + lane + "\"}"
    client.publish("detect", json, 1, False)

def process_command(command):
    if command == "terminate":
        exit(0)
    elif command == "start":
        running = True
    elif command == "stop":
        running = False

def TimeNowMs():
    return int( time.time_ns() / 1000000 )

client = mqtt.Client()
client.on_connect = on_connect
client.on_message = on_message
client.connect(broker_address, 1883, 60)

#client.loop_forever()

time_start = TimeNowMs()
lapmsg_cols = ["red","green","red","blue","green","green","red","red","blue","green"]
lapmsg_lane = ["left","right","right","right","left","left","right","right","right","left"]
time_count = 0

while True:
    client.loop(timeout=1.0, max_packets=1)
    
    timenow = TimeNowMs()
    #print(timenow)
    if timenow > (time_start + 2):
        time_start = timenow
        
        publish_detect(lapmsg_cols[time_count],lapmsg_lane[time_count])
        time_count += 1
        
        if time_count >= 10:
            client.publish("command/race","end")
            #client.disconnect()
            exit(0)