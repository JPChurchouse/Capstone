

int Tune[2][8] = { { TONE_f,  TONE_g,  TONE_a,  TONE_R,  TONE_C,  TONE_b,  TONE_e,  TONE_a },
                   { 16,      16,      16,      1,       16,      16,      16,      16 } };

int GoodBuzz[2][2] = { { TONE_a,  TONE_b },
                       { 11,      11 } };
                       
int BadBuzz[2][2] = { { TONE_d,  TONE_c },
                      { 12,      12 } };





int StartPathTune[2][2]     = { { TONE_g,  TONE_C },
                                { 11,      11     } };

int WaypointTune[2][1]      = { { TONE_g },
                                { 11     } };

int LedgeDetectedTune[2][1] = { { TONE_c },
                                { 12     } };





//int GoodThreeTune[2][8] = { { TONE_d,  TONE_c,  TONE_C },
//                            { 12,      12,      12 } };


//int LedgeDetectedTune[2][8] = { { TONE_c, TONE_g },
//                                { 12,     12 } };


//int LedgeDetectedTune[2][8] = { { TONE_c, TONE_d },
//                                { 12,     12 } };

//int LedgeDetectedTune[2][4] = { { TONE_e,  TONE_c },
  //                              { 12,      12 } };



//    FOR AN UNKNOWN REASON, TUNES MUST BE 2 OR 8 TONES LONG. OTHERWISE THERE IS A DIVIDE BY ZERO CRASH THAT FORCES THE CONTROLLER TO RESTART.
