






//######################################################################################################
//---------------------------------------------- DRIVE Motor & Wheel Things ----------------------------
    #define PI 3.1415926535897932384626433832795
    float WheelRadiusCM = 1.65;           //-- approximatly
    float WheelCircumferenceCM;
    int MotorGearRatioToOne = 603;        //-- gear ratio of 603:1
    void CalculateWheelCircumferenceCM()
    {
      WheelCircumferenceCM = (2 * PI * WheelRadiusCM);   //-- C = 2*π*r
    }
    
    
    
    float CaculateRequiredRevs( float DistanceCM ) //-- caculate the numver of revolutions required to travel distance "x" using the circumference of the wheels we have.
    {
      //-- the distance we want to travel devided by the circumfrence of our wheels tells us how many revs the weel must make to reach that number.
      //-- DistanceCM/WheelCircumferenceCM = REVS
      float REVS = DistanceCM / WheelCircumferenceCM;
    
      return(REVS);
    }
    
    int CaculateRequiredMotorRevs( float DistanceCM ) //-- caculate the numver of revolutions required to travel distance "x" using the circumference of the wheels we have.
    {
      //-- the distance we want to travel devided by the circumfrence of our wheels tells us how many revs the weel must make to reach that number.
      //-- DistanceCM/WheelCircumferenceCM = REVS
      float REVS = DistanceCM / WheelCircumferenceCM;
    
      //-- we want to measure in motor revs so that we have a higher resolution.
      //-- so REVS * MotorGearRatioToOne from the ???:1 gear ratio = MotorREVS
      int MotorREVS = REVS * MotorGearRatioToOne;
    
      return(MotorREVS);
    }
//\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\





//######################################################################################################
//---------------------------------------------- DRIVE Motor Structs -----------------------------------
    struct Button {
      const uint8_t PIN;
      uint32_t numberKeyPresses;
      bool pressed;
    };


    struct Motor_RotaryEncoder {
      const uint8_t PIN;
      uint32_t Count;
    };
//\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\

//######################################################################################################
//---------------------------------------------- DRIVE Motor Struct Instances --------------------------
    Button button1 = {19, 0, false};

     Motor_RotaryEncoder DriveMotor_Left_RotaryEncoder  = {34, 0};
     Motor_RotaryEncoder DriveMotor_Right_RotaryEncoder = {35, 0};
//\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\



//######################################################################################################
//---------------------------------------------- DRIVE Motors I/O Pins ---------------------------------
    #define DriveMotor_Left_Frwd_Pin   33  //-- [Forward] GPIO pin
    #define DriveMotor_Left_Bkwd_Pin   32  //-- [Backward] GPIO pin
    #define DriveMotor_Right_Frwd_Pin  26
    #define DriveMotor_Right_Bkwd_Pin  25

    #define SideBrushMotor_On_Pin       2   //-- [On] GPIO pin
    #define MainBrushMotor_On_Pin       4   //-- [On] GPIO pin
    #define VacuumImpellerMotor_On_Pin  16  //-- [On] GPIO pin
//\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\



//######################################################################################################
//---------------------------------------------- Motor PWM Channel Definitions -------------------------
    #define DriveMotor_Left_Frwd_PwmChannel     0
    #define DriveMotor_Left_Bkwd_PwmChannel     1
    #define DriveMotor_Right_Frwd_PwmChannel    2
    #define DriveMotor_Right_Bkwd_PwmChannel    3

    #define SideBrushMotor_On_PwmChannel       4
    #define MainBrushMotor_On_PwmChannel       5
    #define VacuumImpellerMotor_On_PwmChannel  6
//\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\


//######################################################################################################
//---------------------------------------------- Motor PWM Resoltions & Frequencies --------------------
    #define PWM1_Res   8
    #define PWM1_Freq  1000
    #define PWM2_Res   8
    #define PWM2_Freq  1000


    #define PWM_SideBrush_Res       8
    #define PWM_SideBrush_Freq      1000
    
    #define PWM_MainBrush_Res       8
    #define PWM_MainBrush_Freq      1000
    
    #define PWM_VacuumInpeller_Res  8
    #define PWM_VacuumInpeller_Freq 1000
//\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\







// INTERRUPT PROTOTYPES
void IRAM_ATTR DriveMotor_Left_RotaryEncoder_ISR();
void IRAM_ATTR DriveMotor_Right_RotaryEncoder_ISR();


// FUNCTION PROTOTYPES
void BothForward_Test();
void AllMotorsOFF();

void SetSpeed_SideBrushMotor(int);
void SetSpeed_MainBrushMotor(int);
void SetSpeed_VacuumInpellerMotor(int);



void AllMotorsOFF()
{
  ledcWrite(DriveMotor_Right_Frwd_PwmChannel, 0);
  ledcWrite(DriveMotor_Right_Bkwd_PwmChannel, 0);

  ledcWrite(DriveMotor_Left_Frwd_PwmChannel, 0);
  ledcWrite(DriveMotor_Left_Bkwd_PwmChannel, 0);
}







/*class CLASS_PathMove_Forward
{  
  public:

    bool Running = false;
  
    void Go()
    {
      Running = true;
      ledcWrite(DriveMotor_Left_Bkwd_PwmChannel, 255);
      ledcWrite(DriveMotor_Right_Frwd_PwmChannel, 255);
    }
    
    void CheckEnd()
    {
      int DistanceToGo = 70; //-- 1cm
      //int TargetRevolutions = CaculateRequiredMotorRevs(DistanceToGo);
      //float TargetRevolutions = (2/WheelCircumferenceCM)*(603*7);
      int TargetRevolutions = 1675; //(603*7)/2;
      int CurrentRevolutions = DriveMotor_Right_RotaryEncoder.Count;    ///7)/603);   //-- convert revolutions count to the number of wheel revolutions
    
      //if(TargetRevolutions >= TargetRevolutions)
      if(CurrentRevolutions >= TargetRevolutions)
      {
        Running = false;
        AllMotorsOFF();
        Serial.println("Done: " + (String)DriveMotor_Right_RotaryEncoder.Count);
        DriveMotor_Right_RotaryEncoder.Count = 0;
      }
    }
};*/

class CLASS_PathMove_Forward
{  
  public:

    bool Running = false;
  
    void Go()
    {
      attachInterrupt(  DriveMotor_Left_RotaryEncoder.PIN,  DriveMotor_Left_RotaryEncoder_ISR, RISING);
      attachInterrupt(  DriveMotor_Right_RotaryEncoder.PIN, DriveMotor_Right_RotaryEncoder_ISR, RISING);
      Running = true;
      ledcWrite(DriveMotor_Left_Frwd_PwmChannel, 255);
      ledcWrite(DriveMotor_Right_Frwd_PwmChannel, 255);\
    }
    
    void CheckEnd()
    {
      int DistanceToGo = 70; //-- 1cm
      int TargetRevolutions = (603*7)/4;
      int CurrentRevolutions = DriveMotor_Right_RotaryEncoder.Count;    ///7)/603);   //-- convert revolutions count to the number of wheel revolutions
      
      if(CurrentRevolutions >= TargetRevolutions)
      {
        detachInterrupt(DriveMotor_Left_RotaryEncoder.PIN);
        detachInterrupt(DriveMotor_Right_RotaryEncoder.PIN);
        Running = false;
        AllMotorsOFF();
        Serial.println("Done: " + (String)DriveMotor_Left_RotaryEncoder.Count + " | " + (String)DriveMotor_Right_RotaryEncoder.Count);
        DriveMotor_Right_RotaryEncoder.Count = 0;
        DriveMotor_Left_RotaryEncoder.Count = 0;
      }
    }
};

class CLASS_PathMove_Backward
{  
  public:

    bool Running = false;
  
    void Go()
    {
      attachInterrupt(  DriveMotor_Left_RotaryEncoder.PIN,  DriveMotor_Left_RotaryEncoder_ISR, RISING);
      attachInterrupt(  DriveMotor_Right_RotaryEncoder.PIN, DriveMotor_Right_RotaryEncoder_ISR, RISING);
      Running = true;
      ledcWrite(DriveMotor_Left_Bkwd_PwmChannel, 255);
      ledcWrite(DriveMotor_Right_Bkwd_PwmChannel, 255);
    }
    
    void CheckEnd()
    {
      int DistanceToGo = 70; //-- 1cm
      int TargetRevolutions = (603*7)/4;
      int CurrentRevolutions = DriveMotor_Right_RotaryEncoder.Count;    ///7)/603);   //-- convert revolutions count to the number of wheel revolutions
      
      if(CurrentRevolutions >= TargetRevolutions)
      {
        detachInterrupt(DriveMotor_Left_RotaryEncoder.PIN);
        detachInterrupt(DriveMotor_Right_RotaryEncoder.PIN);
        Running = false;
        AllMotorsOFF();
        Serial.println("Done: " + (String)DriveMotor_Left_RotaryEncoder.Count + " | " + (String)DriveMotor_Right_RotaryEncoder.Count);
        DriveMotor_Right_RotaryEncoder.Count = 0;
        DriveMotor_Left_RotaryEncoder.Count = 0;
      }
    }
};

class CLASS_PathArcTurn_Left90
{  
  public:

    bool Running = false;
  
    void Go()
    {
      attachInterrupt(  DriveMotor_Right_RotaryEncoder.PIN, DriveMotor_Right_RotaryEncoder_ISR, RISING);
      Running = true;
      //ledcWrite(DriveMotor_Left_Bkwd_PwmChannel, 255);
      ledcWrite(DriveMotor_Right_Frwd_PwmChannel, 255);
    }
    
    void CheckEnd()
    {
      int DistanceToGo = 70; //-- 1cm
      int TargetRevolutions = (603*7)/4;
      int CurrentRevolutions = DriveMotor_Right_RotaryEncoder.Count;    ///7)/603);   //-- convert revolutions count to the number of wheel revolutions
      
      if(CurrentRevolutions >= TargetRevolutions)
      {
        detachInterrupt(DriveMotor_Right_RotaryEncoder.PIN);
        Running = false;
        AllMotorsOFF();
        Serial.println("Done: " + (String)DriveMotor_Left_RotaryEncoder.Count + " | " + (String)DriveMotor_Right_RotaryEncoder.Count);
        DriveMotor_Right_RotaryEncoder.Count = 0;
        DriveMotor_Left_RotaryEncoder.Count = 0;
      }
    }
};

class CLASS_PathArcTurn_Right90
{  
  public:

    bool Running = false;
  
    void Go()
    {
      attachInterrupt(  DriveMotor_Left_RotaryEncoder.PIN, DriveMotor_Left_RotaryEncoder_ISR, RISING);
      Running = true;
      //ledcWrite(DriveMotor_Left_Bkwd_PwmChannel, 255);
      ledcWrite(DriveMotor_Left_Frwd_PwmChannel, 255);
    }
    
    void CheckEnd()
    {
      int DistanceToGo = 70; //-- 1cm
      int TargetRevolutions = (603*7)/4;
      int CurrentRevolutions = DriveMotor_Left_RotaryEncoder.Count;    ///7)/603);   //-- convert revolutions count to the number of wheel revolutions
      
      if(CurrentRevolutions >= TargetRevolutions)
      {
        detachInterrupt(DriveMotor_Left_RotaryEncoder.PIN);
        Running = false;
        AllMotorsOFF();
        Serial.println("Done: " + (String)DriveMotor_Left_RotaryEncoder.Count + " | " + (String)DriveMotor_Right_RotaryEncoder.Count);
        DriveMotor_Right_RotaryEncoder.Count = 0;
        DriveMotor_Left_RotaryEncoder.Count = 0;
      }
    }
};


class CLASS_PathPivotTurn_Left90
{  
  public:

    bool Running = false;
  
    void Go()
    {
      attachInterrupt(  DriveMotor_Left_RotaryEncoder.PIN,  DriveMotor_Left_RotaryEncoder_ISR, RISING);
      attachInterrupt(  DriveMotor_Right_RotaryEncoder.PIN, DriveMotor_Right_RotaryEncoder_ISR, RISING);
      Running = true;
      ledcWrite(DriveMotor_Left_Bkwd_PwmChannel, 255);
      ledcWrite(DriveMotor_Right_Frwd_PwmChannel, 255);
    }
    
    void CheckEnd()
    {
      int DistanceToGo = 70; //-- 1cm
      int TargetRevolutions = (603*7)/4;
      int CurrentRevolutions = DriveMotor_Right_RotaryEncoder.Count;    ///7)/603);   //-- convert revolutions count to the number of wheel revolutions
      
      if(CurrentRevolutions >= TargetRevolutions)
      {
        detachInterrupt(DriveMotor_Left_RotaryEncoder.PIN);
        detachInterrupt(DriveMotor_Right_RotaryEncoder.PIN);
        Running = false;
        AllMotorsOFF();
        Serial.println("Done: " + (String)DriveMotor_Left_RotaryEncoder.Count + " | " + (String)DriveMotor_Right_RotaryEncoder.Count);
        DriveMotor_Right_RotaryEncoder.Count = 0;
        DriveMotor_Left_RotaryEncoder.Count = 0;
      }
    }
};

class CLASS_PathPivotTurn_Right90
{  
  public:

    bool Running = false;
  
    void Go()
    {
      attachInterrupt(  DriveMotor_Left_RotaryEncoder.PIN,  DriveMotor_Left_RotaryEncoder_ISR, RISING);
      attachInterrupt(  DriveMotor_Right_RotaryEncoder.PIN, DriveMotor_Right_RotaryEncoder_ISR, RISING);
      Running = true;
      ledcWrite(DriveMotor_Left_Frwd_PwmChannel, 255);
      ledcWrite(DriveMotor_Right_Bkwd_PwmChannel, 255);
    }
    
    void CheckEnd()
    {
      int DistanceToGo = 70; //-- 1cm
      int TargetRevolutions = (603*7)/4;
      int CurrentRevolutions = DriveMotor_Right_RotaryEncoder.Count;    ///7)/603);   //-- convert revolutions count to the number of wheel revolutions
      
      if(CurrentRevolutions >= TargetRevolutions)
      {
        detachInterrupt(DriveMotor_Left_RotaryEncoder.PIN);
        detachInterrupt(DriveMotor_Right_RotaryEncoder.PIN);
        Running = false;
        AllMotorsOFF();
        Serial.println("Done: " + (String)DriveMotor_Left_RotaryEncoder.Count + " | " + (String)DriveMotor_Right_RotaryEncoder.Count);
        DriveMotor_Right_RotaryEncoder.Count = 0;
        DriveMotor_Left_RotaryEncoder.Count = 0;
      }
    }
};


CLASS_PathMove_Forward      PathMove_Forward;
CLASS_PathMove_Backward     PathMove_Backward;
CLASS_PathArcTurn_Left90    PathArcTurn_Left90;
CLASS_PathArcTurn_Right90   PathArcTurn_Right90;
CLASS_PathPivotTurn_Left90  PathPivotTurn_Left90;
CLASS_PathPivotTurn_Right90 PathPivotTurn_Right90;





bool MotorRunning = false;


void DriveMotor_CheckTurnOff();
//######################################################################################################
//---------------------------------------------- DRIVE Motor ISRs --------------------------------------
    void IRAM_ATTR isr() {
      button1.numberKeyPresses += 1;
      button1.pressed = true;
    }


    void CheckMovementEnds(); //-- prototype

    void IRAM_ATTR DriveMotor_Left_RotaryEncoder_ISR() {
      DriveMotor_Left_RotaryEncoder.Count++;
      
      CheckMovementEnds();
    }
    void IRAM_ATTR DriveMotor_Right_RotaryEncoder_ISR() {
      DriveMotor_Right_RotaryEncoder.Count++;

      CheckMovementEnds();
    }

    void CheckMovementEnds()
    {
      if(MotorRunning == true)
        DriveMotor_CheckTurnOff();

      if(PathMove_Forward.Running == true)  //-- if we are currently moving forward, then check when to stop moving
        PathMove_Forward.CheckEnd();

      else if(PathMove_Backward.Running == true) //-- if we are currently moving backward, then check when to stop moving
        PathMove_Backward.CheckEnd();

      else if(PathArcTurn_Left90.Running == true)
        PathArcTurn_Left90.CheckEnd();

      else if(PathArcTurn_Right90.Running == true)
        PathArcTurn_Right90.CheckEnd();
        
      else if(PathPivotTurn_Left90.Running == true)
        PathPivotTurn_Left90.CheckEnd();

      else if(PathPivotTurn_Right90.Running == true)
        PathPivotTurn_Right90.CheckEnd();
    }
//\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\















 
int PWM1_DutyCycle = 0;



void Setup_MotorDrive_AttachChannels()  //-- Attach the PWM Channels to the I/O pins
{
  ledcAttachPin(DriveMotor_Left_Frwd_Pin,   DriveMotor_Left_Frwd_PwmChannel);
  ledcAttachPin(DriveMotor_Left_Bkwd_Pin,   DriveMotor_Left_Bkwd_PwmChannel);
  ledcAttachPin(DriveMotor_Right_Frwd_Pin,  DriveMotor_Right_Frwd_PwmChannel);
  ledcAttachPin(DriveMotor_Right_Bkwd_Pin,  DriveMotor_Right_Bkwd_PwmChannel);

  ledcAttachPin(SideBrushMotor_On_Pin,      SideBrushMotor_On_PwmChannel);
  ledcAttachPin(MainBrushMotor_On_Pin,      MainBrushMotor_On_PwmChannel);
  ledcAttachPin(VacuumImpellerMotor_On_Pin, VacuumImpellerMotor_On_PwmChannel);
}

void Setup_MotorDrive_AttachFrequenicesAndResolutions()  //-- attach the PWM frequencies and resolutions to the PWM channels
{
  ledcSetup(DriveMotor_Left_Frwd_PwmChannel, PWM1_Freq, PWM1_Res);
  ledcSetup(DriveMotor_Left_Bkwd_PwmChannel, PWM1_Freq, PWM1_Res);
  ledcSetup(DriveMotor_Right_Frwd_PwmChannel, PWM2_Freq, PWM2_Res);
  ledcSetup(DriveMotor_Right_Bkwd_PwmChannel, PWM2_Freq, PWM2_Res);

  ledcSetup(SideBrushMotor_On_PwmChannel,       PWM_SideBrush_Freq,       PWM_SideBrush_Res);
  ledcSetup(MainBrushMotor_On_PwmChannel,       PWM_MainBrush_Freq,       PWM_MainBrush_Res);
  ledcSetup(VacuumImpellerMotor_On_PwmChannel,  PWM_VacuumInpeller_Freq,  PWM_VacuumInpeller_Res);
}

void Setup_MotorDrive_AttachRotartEncoderISRs()
{
  pinMode(button1.PIN, INPUT_PULLUP);
  attachInterrupt(button1.PIN, isr, RISING);


  pinMode(        DriveMotor_Left_RotaryEncoder.PIN,  INPUT_PULLUP);
  pinMode(        DriveMotor_Right_RotaryEncoder.PIN, INPUT_PULLUP);

  detachInterrupt(DriveMotor_Left_RotaryEncoder.PIN);
  detachInterrupt(DriveMotor_Right_RotaryEncoder.PIN);
}

void Setup_MotorDrive()
{
  CalculateWheelCircumferenceCM();

  
  Setup_MotorDrive_AttachChannels();                    //-- Attach the PWM Channels to the I/O pins

  Setup_MotorDrive_AttachFrequenicesAndResolutions();   //-- attach the PWM frequencies and resolutions to the PWM channels

  Setup_MotorDrive_AttachRotartEncoderISRs();
}











/*bool PathMove_Forward   = false;
bool PathMove_Backward  = false;
bool PathTurn_Left90    = false;
bool PathTurn_Right90   = false;
bool LedgeReversing     = false;*/





void DriveMotor_Go()
{
  MotorRunning = true;
  ledcWrite(DriveMotor_Left_Bkwd_PwmChannel, 255);
  ledcWrite(DriveMotor_Right_Frwd_PwmChannel, 255);

  //CallSoundBassedOnRequest("WaypointReached");
}


void DriveMotor_CheckTurnOff()
{
  int DistanceToGo = 70; //-- 1cm
  //int TargetRevolutions = CaculateRequiredMotorRevs(DistanceToGo);
  //float TargetRevolutions = (2/WheelCircumferenceCM)*(603*7);
  int TargetRevolutions = 1675; //(603*7)/2;
  int CurrentRevolutions = DriveMotor_Right_RotaryEncoder.Count;    ///7)/603);   //-- convert revolutions count to the number of wheel revolutions

  //if(TargetRevolutions >= TargetRevolutions)
  if(CurrentRevolutions >= TargetRevolutions)
  {
    MotorRunning = false;
    AllMotorsOFF();
    Serial.println("Done: " + (String)DriveMotor_Right_RotaryEncoder.Count);
    DriveMotor_Right_RotaryEncoder.Count = 0;
    
    //CallSoundBassedOnRequest("DANG");
    //delay(200);
    //CallSoundBassedOnRequest("LedgeDetected");
  }
}












void BothForward_Test()
{
  SetSpeed_SideBrushMotor(255);
  SetSpeed_MainBrushMotor(255);
  SetSpeed_VacuumInpellerMotor(255);
  
  for (int i = 0; i < 100; i++)
  {
    ledcWrite(DriveMotor_Left_Frwd_PwmChannel,  255);
    ledcWrite(DriveMotor_Right_Frwd_PwmChannel, 255);
    
    //ledcWrite(PWM_Motor1_Ch2, 255);
    delay(10);
  }
}










//######################################################################################################
//------------------------------------------Loops for Wifi GENERAL--------------------------------------
    void SetSpeed_SideBrushMotor( int Speed )
    {
      ledcWrite(SideBrushMotor_On_PwmChannel, Speed);
    }
    
    
    void SetSpeed_MainBrushMotor( int Speed )
    {
      ledcWrite(MainBrushMotor_On_PwmChannel, Speed);
    }
    
    
    void SetSpeed_VacuumInpellerMotor( int Speed )
    {
      ledcWrite(VacuumImpellerMotor_On_PwmChannel, Speed);
    }
//\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\







/*void loop()
{
  delay(500);
  
  while(PWM1_DutyCycle < 255)
  {
    ledcWrite(PWM1_Ch1, PWM1_DutyCycle++);
    delay(10);
  }
  while(PWM1_DutyCycle > 0)
  {
    ledcWrite(PWM1_Ch1, PWM1_DutyCycle--);
    delay(10);
  }

  delay(500);

  while(PWM1_DutyCycle < 255)
  {
    ledcWrite(PWM1_Ch2, PWM1_DutyCycle++);
    delay(10);
  }
  while(PWM1_DutyCycle > 0)
  {
    ledcWrite(PWM1_Ch2, PWM1_DutyCycle--);
    delay(10);
  }
}*/
