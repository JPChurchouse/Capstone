import os
import sys
import time

master = {}

size_buffer = 20
numof_karts = 10
detection_timeout = 3000

handle_kartid = "id"
handle_lastupdate = "last"
handle_leftstation = "left"
handle_rightstation = "right"

handle_timestamp = "time"
handle_value = "range"
handle_index = "index"

# Get time now in ms
def TimeNow(): return int(round(time.time()*1000))


# Function to add a new kart to the database
def NewKart(id):
    
  # Init kart dict
  kart = {}

  # Kart ID
  kart[handle_kartid] = id if id is not None else 0

  # Last update timestamp
  kart[handle_lastupdate] = 0

  # Left and right station data
  arr = {handle_timestamp: [0] * size_buffer,
         handle_value: [0] * size_buffer,
         handle_index: 0}

  kart[handle_leftstation] = arr
  kart[handle_rightstation] = arr

  return kart


# Generate a new race
def NewRace():
  for i in range(numof_karts):
    master[i] = NewKart(i)


# Find the index of a kart from it's ID
def IndexFromKartId(id):

  # Note a free slot
  free = -1

  # Iterate over slots in race
  for i in range(numof_karts):

    _id = master[i] [handle_kartid]

    # Index found
    if _id == id: return i

    # Free index found
    elif (_id == 0 and free == -1): free = i

  # Return the index of a free slot (it'll be -1 if failed)
  return free


# Add a detection value
def AddValue(id, station, time, value):

  # Validate the station
  han_station = station
  if (han_station is not handle_leftstation and
      han_station is not handle_rightstation):
    return -1

  # Find kart index - will create new entry as needed
  han_kart = IndexFromKartId(id)
  if han_kart == -1: return -1

  # Write kart ID in case its a new slot
  master [han_kart] [handle_kartid] = id

  # Update last reading timestamp
  # kart -> update latest reading to this one
  master [han_kart] [handle_lastupdate] = time

  # Read index and write values to the arrays
  # kart -> read index to write to next
  write_index = master [han_kart] [handle_index]
  # kart -> station (left or right) -> timestamp/values list -> write at index
  master [han_kart] [han_station] [handle_timestamp] [write_index] = time
  master [han_kart] [han_station] [handle_value] [write_index] = value

  # Increment the index, reset if at end, write
  write_index = write_index+1 if write_index+1 <= size_buffer else 0
  # kart -> station (L or R) -> update next write index
  master [han_kart] [han_station] [handle_index] = write_index

  # All pass
  return 0


# Checks if any karts meet detection criteria
def CheckForDetections():
  out = []
  now = TimeNow()

  # Iterate over each kart
  for index in range (numof_karts):

    # Read in last update value, if zero continue
    val = master [index] [handle_lastupdate]
    if val == 0: continue

    # Check if the kart meets detection criteria
    # (kart -> last_update)+timeout < now, -> detection
    if (val+detection_timeout < now ):
      out.append(index)

  # Return the array of kart indecies
  return out


# Function to return the detection packet of an index and reset it (reset the index)
def GenerateDetectionPacket(index):
  # Init all values
  now = TimeNow() - detection_timeout
  colour = master [index] [handle_kartid]
  lane = WhichLane(index)

  # Generate string
  json = "{\"Time\": "+now+",\"Colour\": \""+colour+"\",\"Lane\": \""+lane+"\"}"
  print(json)

  # Overright this kart's index
  master [index] = NewKart()

  # Return the detection json
  return json


# Which lane did the kart go down function
def WhichLane(index):

  # Arrays (easy to access)
  arr_l = master [index] [handle_leftstation] [handle_value]
  arr_r = master [index] [handle_rightstation] [handle_value]

  # Sum and count vars
  sum_l = 0
  sum_r = 0
  cnt_l = 0
  cnt_r = 0

  # Iterate over each reading
  for i in range (size_buffer):
    # Read val, if not zero, add to sum and increment count

    # Left
    l = arr_l [i]
    if l > 0:
      sum_l += l
      cnt_l += 1

    # Right
    r = arr_r [i]
    if r > 0:
      sum_r += r
      cnt_r += 1

  # Average
  avg_l = sum_l / cnt_l
  avg_r = sum_r / cnt_r

  # Return the lane
  # Low value means "closer to" means "I went that way"
  return "left" if avg_l < avg_r else "right"

