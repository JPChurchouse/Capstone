/*
* Library written by Jordan Poynter 2021/09/20
* this library can be used to play notes or tunes on speakers or buzzers.
*/

#ifndef TUNEPLAYER_H
#define TUNEPLAYER_H

#include <Arduino.h>
#include <Wire.h>


class TunePlayer
{
    private :
      int speakerOut = 9; //- the defult pin for the speaker is 9 if you use "TunePlayer tp;" but can be set with the overloaded constructor "TunePlayer tp(NUMBER HERE);"

    public :
      // TONES  ==========================================
      // Start by defining the relationship between
      //       note, period, &  frequency.
      #define  TONE_c     3830    // 261 Hz
      #define  TONE_d     3400    // 294 Hz
      #define  TONE_e     3038    // 329 Hz
      #define  TONE_f     2864    // 349 Hz
      #define  TONE_g     2550    // 392 Hz
      #define  TONE_a     2272    // 440 Hz
      #define  TONE_b     2028    // 493 Hz
      #define  TONE_C     1912    // 523 Hz
      #define  TONE_R     0       //-- this was originally 0 but i believe that the 0 causes dizide by zero crashes
      int rest_count = 10;


      TunePlayer();
      TunePlayer(int speakerPin);
      void PlayPassedTune( int Melody[], int M_len, int Note_len[], long Speed );
};












#endif // TUNEPLAYER_H
