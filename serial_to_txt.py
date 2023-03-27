import serial
import time
import csv


## Serial configuration
esp_port = serial.Serial('COM1')
esp_port.flushInput()
esp_port.baudrate = 115200
esp_port.bytesize = 8
esp_port.parity = serial.PARITY_NONE
esp_port.stopbits = 1

# File Configuration
new_file = open("test.csv", "a")
write_file = csv.writer(new_file, delimiter="   ")


while True:
    try:
        # Get Serial Input
        esp_port_bytes = esp_port.readline()
        esp_port_line = str(esp_port_bytes[0:len(esp_port_bytes)-2].decode("utf-8"))
        esp_port_line.split() # maybe delete
        # Add to file
        write_file.writerow([time.time(), esp_port_line])
        #print(esp_port_line) # to test
    except:
        print("Keyboard Interrupt")
        break
# close file
new_file.close()
    
        
