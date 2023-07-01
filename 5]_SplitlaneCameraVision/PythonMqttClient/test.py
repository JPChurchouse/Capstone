# Install the MQTT library using the folloing pip command
#pip install paho-mqtt

# Import libraries
import paho.mqtt.client as mqtt
import time


# Global vars
broker_address = "10.2.245.108"     # MQTT broker address - will be "192.168.1.30" on site
running = False                     # Bool to tell if the race is running


# Callback func for when client connects
def on_connect(client, userdata, flags, rc):
    print("Connected with result code "+str(rc))
    client.will_set("status/detection","offline",1,True)    # Set will
    client.subscribe("command/detection")                   # Sub to detection commands
    #client.subscribe("#")
    publish_status("online")

# Callback func for when a message is RXD
def on_message(client, userdata, msg):
    print(msg.topic+" "+str(msg.payload))
    if msg.topic == "command/detection":
        process_command(msg.payload.decode('UTF-8'))
    
# Func to update the network on this devices status
def publish_status(msg):
    client.publish("status/detection", msg, 1, True)

# Func to publish detction info
def publish_detect(colour,lane):
    timenow = str(TimeNowMs())
    json = "{\"Time\": " + timenow + ",\"Colour\": \"" + colour + "\",\"Lane\": \"" + lane + "\"}"
    client.publish("detect", json, 1, False)

# Func to process new commands for this device
def process_command(command):
    if command == "terminate":
        exit(0)
    elif command == "start":
        running = True
    elif command == "stop":
        running = False

# Func to get the time now in unix ms
def TimeNowMs():
    return int( time.time_ns() / 1000000 )


# Init MQTT Client and connect
client = mqtt.Client()
client.on_connect = on_connect
client.on_message = on_message
client.connect(broker_address, 1883, 60)
#client.loop_forever()

# Vars for testing
time_start = TimeNowMs()
lapmsg_cols = ["red","green","red","blue","green","green","red","red","blue","green"]
lapmsg_lane = ["left","right","right","right","left","left","right","right","right","left"]
time_count = 0

# Main While loop
while True:
    client.loop(timeout=1.0, max_packets=1)     # Run MQTT stuff
    
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