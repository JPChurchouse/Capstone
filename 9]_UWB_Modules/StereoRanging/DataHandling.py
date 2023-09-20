"""

Terminology:
kart:   object that holds the data associated to one kart, referred to with it's id
id:     identifier of the kart - number, colour, driver
index:  index of the kart of interest in the master database
time:   timestamp of when an event ocurred in unix ms
now:    timestamp of the instant the function is running

"""

import time

master = {}

buffer_size = 20
numof_karts = 10
detection_timeout = 3000

invalid = "null"

handle_kartid = "id"
handle_lastupdate = "last"
handle_leftstation = "left"
handle_rightstation = "right"

handle__timestamp = "time"
handle__value = "range"
handle__writeto = "write"

# Get time now in ms
def TimeNow() : return int(round(time.time()*1000))


# Function to add a new kart to the database
def NewKart() :
    
  # Init kart dict
  kart = {}

  # Kart ID
  kart [handle_kartid] = invalid

  # Last update timestamp
  kart [handle_lastupdate] = 0

  # NOTE FOR FUTURE ME - MAKE THIS MORE EFFICIENT
  kart [handle_leftstation] = {
    handle__timestamp : [0] * buffer_size,
    handle__value : [0] * buffer_size,
    handle__writeto : 0}

  kart [handle_rightstation] = {
    handle__timestamp : [0] * buffer_size,
    handle__value : [0] * buffer_size,
    handle__writeto : 0}

  return kart


# Generate a new race
def NewRace() :
  for index in range(numof_karts):
    master [index] = NewKart()


# Find the index of a kart from it's ID
def IndexFromKartId(id) :

  # Note a free slot
  free_index = -1

  # Iterate over slots in race
  for index in master :

    # Read id at index
    _id = master [index] [handle_kartid]

    # Index found
    if _id == id : return index

    # Free index found
    elif (_id == invalid and free_index == -1) :
      free_index = index

  # Kart not found

  # Write kart id to free slot (if not failure)
  if free_index != -1 :
    master [free_index] [handle_kartid] = id

  # Return the index of the new slot (it'll be -1 if failed)
  return free_index


# Add a detection value
# Ret of -1 is fail, 0 is pass
def AddValue(id, station, time, value) :

  # Validate the station
  han_station = station
  if (han_station != handle_leftstation and
      han_station != handle_rightstation) :
    return invalid

  # Find kart index - will create new entry as needed
  index = IndexFromKartId(id)

  if index == -1 : return invalid

  # Write kart ID in case its a new instance
  master [index] [handle_kartid] = id

  # Update last reading timestamp
  # kart -> update latest reading to this one
  master [index] [handle_lastupdate] = time

  # Read index and write values to the arrays
  # kart -> read index to write to next
  write_index = master [index] [han_station] [handle__writeto]
  # kart -> station (left or right) -> timestamp/values list -> write at index
  master [index] [han_station] [handle__timestamp] [write_index] = time
  master [index] [han_station] [handle__value] [write_index] = value

  # Increment the index, reset if at end, write
  write_index += 1

  if write_index >= buffer_size : # an index at buff_size is invalid
    write_index = 0

  # kart -> station (L or R) -> update next write index
  master [index] [han_station] [handle__writeto] = write_index

  # All done, return success
  return 0


# Checks if any karts meet detection criteria
def CheckForDetections() :

  # Init return list
  kartlist = []
  now = TimeNow()

  # Iterate over each kart
  for index in master :

    # Read in last update value, if zero continue
    last = master [index] [handle_lastupdate]
    if last == 0 : continue

    # Check if the kart meets detection criteria
    # (kart -> last_update)+timeout < now, -> detection
    if (last+detection_timeout < now ) :
      kartlist.append(index)

  # Return the array of kart indexs, empty if none
  return kartlist


# Function to return the detection packet of an index and reset it (reset the index)
def GenerateDetectionPacket(index) :

  # Init all parameters
  time = str(TimeNow() - detection_timeout)
  colour = master [index] [handle_kartid]
  lane = WhichLane(index)

  # Generate string
  json = "{\"Time\":"+time+",\"Colour\":\""+colour+"\",\"Lane\":\""+lane+"\"}"

  # Overright this kart's index
  master [index] = NewKart()

  # Return the detection json
  return json


# Which lane did the kart go down function
def WhichLane(index) :

  # If no valid readings -> return
  if master [index] [handle_lastupdate] == 0 : return invalid

  # Arrays (easy to access)
  arr_l = master [index] [handle_leftstation] [handle__value]
  arr_r = master [index] [handle_rightstation] [handle__value]

  # Init sum and count vars
  sum_l, sum_r, cnt_l, cnt_r = 0, 0, 0, 0

  # Iterate over each reading
  for i in range(buffer_size) :

    # Read val, if not zero, add to sum and increment count (left, right)
    l = arr_l [i]
    if l > 0 :
      sum_l += l
      cnt_l += 1

    r = arr_r [i]
    if r > 0 :
      sum_r += r
      cnt_r += 1


  # Invalid arguments
  
  # If there are NULL readings on LEFT side...
  if cnt_l == 0 :
    # ... and NULL readings on RIGHT side...
    if cnt_r == 0 :
      # ...there are NULL readings total so RETURN ERROR.
      return invalid
    
    # There are NULL readings on the LEFT, but VALID readings on RIGHT...
    else:
      # ...must be closer to right so RETURN "RIGHT".
      return "right"

  # There are VALID readings on the LEFT, but if there are NULL readings on the right...
  elif cnt_r == 0 :
    # ...must be closer to left so RETURN "LEFT"
    return "left"
  
  # There are VALID reading on BOTH sides so calculate.


  # Average
  avg_l = sum_l / cnt_l
  avg_r = sum_r / cnt_r

  # Return the lane
  # Low value means "closer to" means "I went that way"
  return "left" if avg_l < avg_r else "right"


def printout():
  return str(master)