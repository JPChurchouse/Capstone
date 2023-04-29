/*
 * This program demonstrates .....
 * 
*/

/*
 * Arduino IDE tools settings:
 * Board            = ESP32 Dev Module
 * Upload Speed     = 921600
 * CPU Frequency    = 240MHz (WiFi/BT)
 * Flash Frequency  = 80MHz
 * Flash Mode       = QIO
 * Flash Size       = 4MB (32Mb)
 * Partition Scheme = Default 4MB with spiffs (1.2MB APP/1.5MB SPIFFS)
 * Core Debug Level = None
 * PSRAM            = Disabled
*/

//#############################################################################
//------------------------------Include Jordan's Custom Libraries--------------
    #include "CustomDataTypes.h"
//\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

//#############################################################################
//------------------------------------------LCD Libraries----------------------
    #include <Wire.h>
    #include <Adafruit_GFX.h>
    #include <Adafruit_SSD1306.h>
    
    #include "CustomLcdIcons.h"
//\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

//#############################################################################
//-------------------------------Background timer library----------------------
    #include "elapsedMillis.h"                      //-- [top]-[Sketch]-[include library]-[library manager]-[search]-["elapsedMillis"]-[install "elapsedMillis" by Paul Stoffregen]
//\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

//#############################################################################
//------------------------------------Gyroscope Libraries----------------------
    #include "I2Cdev.h"
    //#include "MPU6050_6Axis_MotionApps_V6_12.h"  //changed to this version 10/04/19
    #include "MPU6050_6Axis_MotionApps20.h"
    #include "elapsedMillis.h"                      //-- [top]-[Sketch]-[include library]-[library manager]-[search]-["elapsedMillis"]-[install "elapsedMillis" by Paul Stoffregen]
//\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\






//######################################################################################################
//------------------------------------------UNIVERSAL Positioning Variables-----------------------------
    float x_Rotation = 0;
    float y_Rotation = 0;
    float z_Rotation = 0;
    
    float GlobalHeadding = 0;         //-- θ°
    float LocalHeaddingOffset = 0;    //-- θ°
    float LocalHeadding = 0;          //-- θ°
//\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

//######################################################################################################
//------------------------------------------LCD Variables-----------------------------------------------
    #define SCREEN_WIDTH 128 // OLED display width, in pixels
    #define SCREEN_HEIGHT 64 // OLED display height, in pixels
    Adafruit_SSD1306 display(SCREEN_WIDTH, SCREEN_HEIGHT, &Wire, -1); //-- Declaration for an SSD1306 display connected to I2C (SDA, SCL pins)
//\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

//######################################################################################################
//---------------------------------------Gyroscope Variables--------------------------------------------
    // class default I2C address is 0x68
    // specific I2C addresses may be passed as a parameter here
    // AD0 low = 0x68 (default for SparkFun breakout and InvenSense evaluation board)
    // AD0 high = 0x69
    MPU6050 mpu;
    //MPU6050 mpu(0x69); // <-- use for AD0 high
    
    
    
    // MPU control/status vars
    bool dmpReady = false;  // set true if DMP init was successful
    uint8_t mpuIntStatus;   // holds actual interrupt status byte from MPU
    uint8_t devStatus;      // return status after each device operation (0 = success, !0 = error)
    uint16_t packetSize;    // expected DMP packet size (default is 42 bytes)
    uint16_t fifoCount;     // count of all bytes currently in FIFO
    uint8_t fifoBuffer[64]; // FIFO storage buffer
    
    // orientation/motion vars
    Quaternion q;           // [w, x, y, z]         quaternion container
    VectorInt16 aa;         // [x, y, z]            accel sensor measurements
    VectorInt16 aaReal;     // [x, y, z]            gravity-free accel sensor measurements
    VectorInt16 aaWorld;    // [x, y, z]            world-frame accel sensor measurements
    VectorFloat gravity;    // [x, y, z]            gravity vector
    float euler[3];         // [psi, theta, phi]    Euler angle container
    float ypr[3];           // [yaw, pitch, roll]   yaw/pitch/roll container and gravity vector
    
    //extra stuff
    int IMU_CHECK_INTERVAL_MSEC = 100;
    elapsedMillis sinceLastIMUCheck;
    float global_yawval = 0.0; //contains most recent yaw value from IMU
    int global_fifo_count = 0; //made global so can monitor from outside GetIMUHeadingDeg() fcn
//\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

//######################################################################################################
//---------------------------------------Hall Effect Sensor Variables-----------------------------------
    float SamplingDistanceInM = 0.0309839375;
    float TotalDistanceTrackedInM = 0;
    int NumberOfPasses;

    //variables to keep track of the timing of recent interrupts
    unsigned long HallEffect_time = 0;  
    unsigned long last_HallEffect_time = 0; 
    
    struct HallEffectSensor
    {
      const uint8_t PIN;
      int counter;
      bool pressed;
    };
    
    HallEffectSensor HalEftSns = {18, 0, false};
    
    void IRAM_ATTR HallEffectISR()
    {
      HallEffect_time = millis();
      if (HallEffect_time - last_HallEffect_time > 100) //--  <- debounce delay in milliseconds
      {
        HalEftSns.counter++;
        HalEftSns.pressed = true;
        last_HallEffect_time = HallEffect_time;
      }
    }
//\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

















//######################################################################################################
//----------------------------------------------Setup LCD-----------------------------------------------
    void Setup_LCD()
    {
      // SSD1306_SWITCHCAPVCC = generate display voltage from 3.3V internally
      if(!display.begin(SSD1306_SWITCHCAPVCC, 0x3C))    //-- Address 0x3C for 128x32 -- Address 0x3D for 128x64
      {
        Serial.println(F("SSD1306 allocation failed"));
        for(;;);
      }
    
      // Clear the buffer.
      display.clearDisplay();
      display.display();
    
    
      // text display tests
      display.setTextColor(SSD1306_WHITE);
    
      display.clearDisplay();
      display.setTextSize(2);
      LCD_PrintLn(0, 0, "Loading...");

      display.dim(true);  //-- dim the display
      
      display.display();
    }
//\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

//######################################################################################################
//-------------------------------------------Setup Gyroscope--------------------------------------------
    void Setup_Gyroscope()
    {
      // I2C bus init done in SBWIRE.h
    
      // initialize device
      /*
      Serial.println(F("Initializing MPU6050..."));
      */
      mpu.initialize();
    
      // verify connection
      /*
      Serial.println(F("Testing device connections..."));
      Serial.println(mpu.testConnection() ? F("MPU6050 connection successful") : F("MPU6050 connection failed"));
      */
    
      // load and configure the DMP
      /*
      Serial.println(F("Initializing DMP..."));
      */
      devStatus = mpu.dmpInitialize();
    
      // supply your own gyro offsets here, scaled for min sensitivity
      mpu.setXGyroOffset(220);
      mpu.setYGyroOffset(76);
      mpu.setZGyroOffset(-85);
      mpu.setZAccelOffset(1788); // 1688 factory default for my test chip
    
      // make sure it worked (returns 0 if so)
      if (devStatus == 0)
      {
        // Calibration Time: generate offsets and calibrate our MPU6050
        mpu.CalibrateAccel(6);
        mpu.CalibrateGyro(6);
        /*
        mpu.PrintActiveOffsets();
        */
    
        // turn on the DMP, now that it's ready
        /*
        Serial.println(F("Enabling DMP..."));
        */
        mpu.setDMPEnabled(true);
    
    
        // set our DMP Ready flag so the main loop() function knows it's okay to use it
        /*
        Serial.println(F("DMP ready! Waiting for first interrupt..."));
        */
        dmpReady = true;
    
        // get expected DMP packet size for later comparison
        packetSize = mpu.dmpGetFIFOPacketSize();
      }
      else
      {
        // ERROR!
        // 1 = initial memory load failed
        // 2 = DMP configuration updates failed
        // (if it's going to break, usually the code will be 1)
        /*
        Serial.print(F("DMP Initialization failed (code "));
        Serial.print(devStatus);
        Serial.println(F(")"));
        */
      }
    
      sinceLastIMUCheck = 0; //this manages 'other program stuff' cycle
    
      //print out column headers
      /*
      Serial.println("MSec\tYawDeg");
      */
    }
//\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

//######################################################################################################
//----------------------------------------------Setup Hall Effect Sensor -------------------------------
    void Setup_HallEffectSensor()
    {
      pinMode(HalEftSns.PIN, INPUT_PULLUP);
      attachInterrupt(HalEftSns.PIN, HallEffectISR, FALLING);
    }
//\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\



void setup()
{
  Serial.begin(115200);       //-- setup serial communication

  Setup_LCD();                //-- [Internal] setup the LCD

  Setup_Gyroscope();          //-- [Internal] The code does not work with the gyroscope setup.
  Serial.println("");         //-- end the "......>......" initilisation line

  Setup_HallEffectSensor();   //-- [Internal]
}













//##################################################
//\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

void loop()
{

                      //-- MicroController CODE version #4 uses a CPU-requested poll driven Gyroscope function
                      //-- this means the CPU isnt constantly interrupted by both the SPI and an inerrupt driven gyroscope
                      //--
  Read_Gyroscope();   //-- read the values from the gyroscope.
                      //-- the gyroscope read function waits for an elapsed 100ms before reading as decleared in "IMU_CHECK_INTERVAL_MSEC"
                      //-- you MUST read the gyroscope outside of the SPI communication function.
                      //-- trying to read from the gyro from inside the SPI communcation function causes a "FIFO overflow!"

  if (HalEftSns.pressed)
  {
    
    NumberOfPasses = HalEftSns.counter;
    TotalDistanceTrackedInM = NumberOfPasses * SamplingDistanceInM;
    
    Serial.print( NumberOfPasses);
    Serial.print( ',' );
    Serial.print( SamplingDistanceInM );
    Serial.print( ',' );
    Serial.print( TotalDistanceTrackedInM );
    Serial.print( ',' );
    Serial.print( String(x_Rotation) );
    Serial.println("");

    HalEftSns.pressed = false;
  }
  


  display.clearDisplay();
  
  x_Rotation = global_yawval;
  y_Rotation = 5;
  z_Rotation = 7;
  display.setTextSize(2);
  LCD_PrintLn(55, 0, (String(x_Rotation)).substring(0,6));  //-- print the [yaw-pitch-roll] measured from the gyroscope //-- cut off the number after 6 characters so it doesnt wrap text to the next line

  display.setTextSize(2);
  LCD_PrintLn(0, 0, String(NumberOfPasses) );

  display.setTextSize(3);
  LCD_PrintLn(0, 42, String(TotalDistanceTrackedInM) );

  display.setTextSize(1);
  LCD_PrintLn(0, 16, "Count:" );
  LCD_PrintLn(55, 16, "Angle:" );
  LCD_PrintLn(0, 33, "Total Distance(m):" );
  
  display.display();
}









//######################################################################################################
//-------------------------------------------Loops for LCD----------------------------------------------
    void LCD_PrintLn( int xPos, int yPos, String text )  //-- my custom print line fuction that includes the cursor position and the text in one
    {
      display.setCursor( xPos , yPos );  //-- offset each of the lines by 9 pixels so they dont overlap
      display.println(text);
    }
//\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

//######################################################################################################
//-------------------------------------Loops for Gyroscope----------------------------------------------
    void Read_Gyroscope()
    {
      // if programming failed, don't try to do anything
      if (!dmpReady) return;
    
      if (mpu.dmpPacketAvailable())
      {
        global_yawval = GetIMUHeadingDeg(); //retreive the most current yaw value from IMU
      }
    
      //other program stuff block - executes every IMU_CHECK_INTERVAL_MSEC Msec
      //for this test program, there's nothing here except diagnostics printouts
      if (sinceLastIMUCheck >= IMU_CHECK_INTERVAL_MSEC)
      {
        sinceLastIMUCheck -= IMU_CHECK_INTERVAL_MSEC;
        //Serial.print(millis());
        //Serial.print("\t");
        /*
        Serial.println(global_yawval);
    
        if (global_fifo_count != 0)
        {
          Serial.print("FIFO Reset!");
          mpu.resetFIFO();
        }
        */
      }
    }


    float GetIMUHeadingDeg()
    {
      // At least one data packet is available
    
      mpuIntStatus = mpu.getIntStatus();
      fifoCount = mpu.getFIFOCount();// get current FIFO count
    
      // check for overflow (this should never happen unless our code is too inefficient)
      if ((mpuIntStatus & _BV(MPU6050_INTERRUPT_FIFO_OFLOW_BIT)) || fifoCount >= 1024)
      {
        // reset so we can continue cleanly
        mpu.resetFIFO();
        /*
        Serial.println(F("FIFO overflow!"));
        */
    
        // otherwise, check for DMP data ready interrupt (this should happen frequently)
      }
      else if (mpuIntStatus & _BV(MPU6050_INTERRUPT_DMP_INT_BIT))
      {
        // read all available packets from FIFO
        while (fifoCount >= packetSize) // Lets catch up to NOW, in case someone is using the dreaded delay()!
        {
          mpu.getFIFOBytes(fifoBuffer, packetSize);
          // track FIFO count here in case there is > 1 packet available
          // (this lets us immediately read more without waiting for an interrupt)
          fifoCount -= packetSize;
        }
        global_fifo_count = mpu.getFIFOCount(); //should be zero here
    
        // display Euler angles in degrees
        mpu.dmpGetQuaternion(&q, fifoBuffer);
        mpu.dmpGetGravity(&gravity, &q);
        mpu.dmpGetYawPitchRoll(ypr, &q, &gravity);
      }
    
      float yawval = ypr[0] * 180 / M_PI;
      return  yawval;
    }
//\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\








//\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
