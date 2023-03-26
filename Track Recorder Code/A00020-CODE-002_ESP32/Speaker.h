#include "Speaker_TunePlayer.h"
#include "Speaker_BuiltInTunes.h"


TunePlayer tp(13); //- define the tune player class and define the buzzer pin as Digital 3

#define NUMITEMS(arg) ((unsigned int) (sizeof (arg) / sizeof (arg [0]))) //- number of items in an array


/*void setup()
{
  
}*/

/*void loop()
{
  DecodeBinaryInputIntoRequests();
}*/


/*void DecodeBinaryInputIntoRequests()
{  
  if( 1 == 1 )
  {
    CallSoundBassedOnRequest("TUNE");
  }
  else if( 1 == 2 )
  {
    CallSoundBassedOnRequest("DING");
  }
  else if( 1 == 3 )
  {
    CallSoundBassedOnRequest("DANG");
  }
}*/

void CallSoundBassedOnRequest(String request)
{
  if(request == "TUNE"){
    tp.PlayPassedTune( Tune[0], NUMITEMS(Tune[0]), Tune[1], 10000 );
  }
  else if(request == "DING"){
    tp.PlayPassedTune( GoodBuzz[0], NUMITEMS(GoodBuzz[0]), GoodBuzz[1], 10000 );
  }
  else if(request == "DANG"){
    tp.PlayPassedTune( BadBuzz[0], NUMITEMS(BadBuzz[0]), BadBuzz[1], 10000 );
  }
  else if(request == "StartPath"){
    tp.PlayPassedTune( StartPathTune[0], NUMITEMS(StartPathTune[0]), StartPathTune[1], 10000 );
  }
  else if(request == "WaypointReached"){
    tp.PlayPassedTune( WaypointTune[0], NUMITEMS(WaypointTune[0]), WaypointTune[1], 10000 );
  }
  else if(request == "LedgeDetected"){
    tp.PlayPassedTune( LedgeDetectedTune[0], NUMITEMS(LedgeDetectedTune[0]), LedgeDetectedTune[1], 10000 );
  }
}
