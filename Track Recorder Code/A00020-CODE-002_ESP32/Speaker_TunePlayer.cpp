/*
* Library written by Jordan Poynter 2021/09/20
* this library can be used to play notes or tunes on speakers or buzzers.
*/

#include "Speaker_TunePlayer.h"


TunePlayer::TunePlayer()
{
  pinMode(speakerOut, OUTPUT);
}
TunePlayer::TunePlayer(int speakerPin)
{
  speakerOut = speakerPin;
  pinMode(speakerOut, OUTPUT);
}




void TunePlayer::PlayPassedTune( int Melody[], int M_len, int Note_len[], long Speed )
{
  //int MAX_COUNT = M_len / 2; // Melody length, for looping.
  for (int i=0; i<(M_len); i++)
  {
    int note = Melody[i];
    int leng = Note_len[i];
    
    long duration = leng * Speed; // Set up timing
    
    long elapsed_time = 0;
    if (note > 0)
    {
      while (elapsed_time < duration)
      {
        digitalWrite(speakerOut, HIGH);
        delayMicroseconds(note / 2);
        digitalWrite(speakerOut, LOW);
        delayMicroseconds(note / 2);
  
        elapsed_time += (note);
      }
    }
    else
    {
      for (int j = 0; j < rest_count; j++)
        delayMicroseconds(duration);
    }
  }
}
