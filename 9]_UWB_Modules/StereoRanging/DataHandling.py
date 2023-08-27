import os
import sys
import time

master = {}

size_buffer = 20
numof_karts = 10

handle_kartid = "id"
handle_lastupdate = "last"
handle_leftstation = "left"
handle_rightstation = "right"

handle_timestamp = "time"
handle_range = "range"
handle_index = "index"


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
         handle_range: [0] * size_buffer,
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

# Add a detection range
def AddRange(id, station, time, range):

  # Validate the station
  han_station = "none"
  if station == "left": han_station = handle_leftstation
  elif station == "right": han_station = handle_rightstation
  else: return -1

  # Find kart index
  han_kart = IndexFromKartId(id)
  if han_kart == -1: return -1

  master [han_kart] [handle_lastupdate] = time

  index = master [han_kart] [handle_index]
  master [han_kart] [han_station] [handle_timestamp] [index] = range

  # Increment the index, reset if at end
  index = index + 1 if index + 1 <= size_buffer else 0
  master [han_kart] [han_station] [handle_index] = index




