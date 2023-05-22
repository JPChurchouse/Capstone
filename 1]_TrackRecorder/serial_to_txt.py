import serial  # ensure that pyserial is installed on PC and that 'serial' has been uninstalled
import time

## Serial configuration
esp_port = serial.Serial(port = 'COM4', baudrate = 115200, parity = serial.PARITY_NONE, stopbits = 1, bytesize = 8)
esp_port.flushInput()

# File Configuration
new_file = open("test.csv", "a")
#write_file = csv.writer(new_file, delimiter = ',')
header = "Count,Inc,Dist,Angle\n"
#','.join(header)
new_file.write(str(header))

count   = 0
inc     = 0
dist    = 0
angle   = 0

while True:
    try:
        # Get Serial Input
        esp_port_bytes = esp_port.readline()
        esp_port_line = str(esp_port_bytes[0:len(esp_port_bytes)-2].decode("utf-8"))
        # Add to file
        if(esp_port_line[0] != '>'):
            new_file.write(esp_port_line+'\n')
        #print(esp_port_line) # to test
    except:
        print("Keyboard Interrupt")
        new_file.close()
        break
# close file
new_file.close()  
