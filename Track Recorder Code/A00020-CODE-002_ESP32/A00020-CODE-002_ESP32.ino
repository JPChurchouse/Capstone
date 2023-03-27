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
    #include "MotorDrive.h"
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
//---------------------------------------IR Distance Sensor Variables-----------------------------------
    #define IR_DistanceSensorPin 0    //-- input pin for the IR distance sensor
    int IR_DistanceSensorValue;       //-- an analogue value from 0-255
    float IR_CalculatedDistance;      //-- the analogue IR_DistanceSensorValue converted to a cm distance
    bool IR_Sensor_LedgeDetected = false;
//\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

//######################################################################################################
//---------------------------------------Hall Effect Sensor Variables-----------------------------------
    float SamplingDistanceInM = 0.2;
    float TotalDistanceTrackedInM = 0;
    int NumberOfPasses;
    
    struct HallEffectSensor
    {
      const uint8_t PIN;
      int counter;
      bool pressed;
    };
    
    HallEffectSensor HalEftSns = {18, 0, false};
    
    void IRAM_ATTR HallEffectISR()
    {
      HalEftSns.counter++;
      HalEftSns.pressed = true;
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
//----------------------------------------------Setup IR Distance Sensor -------------------------------
    void Setup_IR_DistanceSensor()
    {
      pinMode(IR_DistanceSensorPin, OUTPUT);
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

  Setup_IR_DistanceSensor();  //-- [Internal]

  Setup_HallEffectSensor();   //-- [Internal]

  Setup_MotorDrive();         //-- [External] A LocalFolder library, Saved next to the .ino
}













//##################################################
//-- Battery variables -----------------------------
    int BatteryLevel_Count = 1;
    
    int WifiSymbol_State = 0;
    
    int  batteryLevel = 0;
    bool batteryLow = false;
    bool charging = true;
    bool displayBattery = true;

    
    bool WifiIcon_Blinking = true;
    bool WifiIcon_Blink = true;
    
    int BatteryBlink_TIMER_INTERVAL = 600;      //-- elapsed timer interval
    elapsedMillis BatteryBlink_ELAPSED_TIMER;   //-- elapsed timer

    int WifiBlink_TIMER_INTERVAL = 1000;        //-- elapsed timer interval
    elapsedMillis WifiBlink_ELAPSED_TIMER;      //-- elapsed timer
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


  if (button1.pressed) {
      Serial.printf("Btn1 pressed %u times\n", button1.numberKeyPresses);
      button1.pressed = false;
  }

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


    void UpdateAndDraw_OBar()
    {
    
      if (BatteryBlink_ELAPSED_TIMER >= BatteryBlink_TIMER_INTERVAL)
      {
        BatteryBlink_ELAPSED_TIMER = 0;
        
    
        if(BatteryLevel_Count <= 0){                     //-- if the count is 0, turn on the icon and set it to 0
          displayBattery = true;
          batteryLevel = 0;
        }
        else if(BatteryLevel_Count <= 4){                //-- if between 1 and 4 then set the charge level between 1 and 4
          batteryLevel = BatteryLevel_Count;
        }
        else if(BatteryLevel_Count == 5){                //-- if 5 then turn off the icon and reset the counter to -1
          displayBattery = false;
          BatteryLevel_Count = -1;
            
        }
        BatteryLevel_Count ++;                            //-- counter ++
      }
    
      if(displayBattery)
        darwBattery();
      if(charging)
        darwChargingIcon();
    
    
      Update_WifiIcon();
      Darw_WifiIcon();
      
    }

    bool WifiConnected = false;
    
    void Update_WifiIcon()
    {
      if(WifiConnected == true)
      {
        WifiSymbol_State = 3;                       //-- show state 3                           [Dot +2 lines]
      }
      else
      {
        if(WifiBlink_ELAPSED_TIMER <= 700)          //-- for 700ms show state 1                 [Dot]
        {
          WifiSymbol_State = 1;
        }
        else if(WifiBlink_ELAPSED_TIMER <= 1400)    //-- between 700ms -> 1400ms show state 2   [Dot +1 line]
        {
          WifiSymbol_State = 2;
        }
        else if(WifiBlink_ELAPSED_TIMER <= 2100)    //-- between 1400ms -> 2100ms show state 3  [Dot +2 lines]
        {
          WifiSymbol_State = 3;
        }
        else if(WifiBlink_ELAPSED_TIMER <= 3200)    //-- between 2100ms -> 3200ms show state 0  [Dot + Error exclamation mark]
        {
          WifiSymbol_State = 0;
        }
        else if(WifiBlink_ELAPSED_TIMER >= 3200)    //-- after 3200ms or more reset the elapsed counter back to 0
        {
          WifiBlink_ELAPSED_TIMER = 0;
        }
      }
    
    
    }
    
    void darwBattery()
    {  
      if(batteryLevel == 0)
        display.drawBitmap(105, 0, batteryIcons[0], battery_WIDTH, battery_HEIGHT, 1);
      else if(batteryLevel == 1)
        display.drawBitmap(105, 0, batteryIcons[1], battery_WIDTH, battery_HEIGHT, 1);
      else if(batteryLevel == 2)
        display.drawBitmap(105, 0, batteryIcons[2], battery_WIDTH, battery_HEIGHT, 1);
      else if(batteryLevel == 3)
        display.drawBitmap(105, 0, batteryIcons[3], battery_WIDTH, battery_HEIGHT, 1);
      else if(batteryLevel == 4)
        display.drawBitmap(105, 0, batteryIcons[4], battery_WIDTH, battery_HEIGHT, 1);
    }
    
    void darwChargingIcon()
    {
      display.drawBitmap(98, 0, chargingIcon, chargingIcon_WIDTH, chargingIcon_HEIGHT, 1);
    }
    
    void Darw_WifiIcon()
    {  
      if(WifiSymbol_State == 0)
        display.drawBitmap(1, 1, WifiSymbol_Dots_0of3, WifiSymbol_Dots_WIDTH, WifiSymbol_Dots_HEIGHT, 1);
      else if(WifiSymbol_State == 1)
        display.drawBitmap(1, 1, WifiSymbol_Dots_1of3, WifiSymbol_Dots_WIDTH, WifiSymbol_Dots_HEIGHT, 1);
      else if(WifiSymbol_State == 2)
        display.drawBitmap(1, 1, WifiSymbol_Dots_2of3, WifiSymbol_Dots_WIDTH, WifiSymbol_Dots_HEIGHT, 1);
      else if(WifiSymbol_State == 3)
        display.drawBitmap(1, 1, WifiSymbol_Dots_3of3, WifiSymbol_Dots_WIDTH, WifiSymbol_Dots_HEIGHT, 1);
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

//######################################################################################################
//-------------------------------------Loops for IR Distance Sensor ------------------------------------
    void Read_IR_DistanceSensor()
    {
      IR_DistanceSensorValue = analogRead(IR_DistanceSensorPin);
    //IR_CalculatedDistance = ????
      IR_CalculatedDistance = IR_DistanceSensorValue;

      if(IR_CalculatedDistance > 50)
        IR_Sensor_LedgeDetected = true;
      else
        IR_Sensor_LedgeDetected = false;
    }
//\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\







//######################################################################################################
//-------------------------------------------Loops for Navigation----------------------------------------------
    void NavigatePath_Path1()
    {
      CallSoundBassedOnRequest("StartPath");
      CallSoundBassedOnRequest("StartPath");
      delay(300);

      //DriveMotor_Go();
      
      PathMove_Forward.Go();
      while(PathMove_Forward.Running == true){ delay(10); }
      CallSoundBassedOnRequest("WaypointReached");
      delay(100);

      PathArcTurn_Left90.Go();
      while(PathArcTurn_Left90.Running == true){ delay(10); }
      CallSoundBassedOnRequest("WaypointReached");
      delay(100);

      PathMove_Forward.Go();
      while(PathMove_Forward.Running == true){ delay(10); }
      CallSoundBassedOnRequest("WaypointReached");
      delay(100);

      PathArcTurn_Right90.Go();
      while(PathArcTurn_Right90.Running == true){ delay(10); }
      CallSoundBassedOnRequest("WaypointReached");
      delay(100);

      PathArcTurn_Right90.Go();
      while(PathArcTurn_Right90.Running == true){ delay(10); }
      CallSoundBassedOnRequest("WaypointReached");
      delay(100);

      PathMove_Forward.Go();
      while(PathMove_Forward.Running == true){ delay(10); }
      CallSoundBassedOnRequest("WaypointReached");
      delay(100);

      PathArcTurn_Left90.Go();
      while(PathArcTurn_Left90.Running == true){ delay(10); }
      CallSoundBassedOnRequest("WaypointReached");
      delay(100);

      PathMove_Backward.Go();
      while(PathMove_Backward.Running == true){ delay(10); }
      CallSoundBassedOnRequest("WaypointReached");
      delay(100);

      PathMove_Backward.Go();
      while(PathMove_Backward.Running == true){ delay(10); }
      CallSoundBassedOnRequest("WaypointReached");
      delay(100);

      PathMove_Backward.Go();
      while(PathMove_Backward.Running == true){ delay(10); }
      CallSoundBassedOnRequest("WaypointReached");
      delay(100);

      PathMove_Backward.Go();
      while(PathMove_Backward.Running == true){ delay(10); }
      delay(100);
      CallSoundBassedOnRequest("StartPath");
      CallSoundBassedOnRequest("StartPath");
    }
//\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
