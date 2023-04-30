

class Vector3
{  
  public:
    float x;
    float y;
    float z;
};

Vector3 StringToVector3( String PassedString )                                                //-- = 10,12,7
    {
      String X = PassedString.substring(0, PassedString.indexOf(","));                            //-- = 10
      String Temp = PassedString.substring(PassedString.indexOf(",")+1, PassedString.length());   //-- = 12,7
      String Y = Temp.substring(0, PassedString.indexOf(","));                                    //-- = 12
      String Z = PassedString.substring(PassedString.indexOf(",")+1, PassedString.length());      //-- = 7

      Vector3 OutputValue;
      OutputValue.x = X.toInt();
      OutputValue.y = Y.toInt();
      OutputValue.z = Z.toInt();

      return(OutputValue);
    }


class VectorWaypoint
{  
  public:
    Vector3 Position;
    Vector3 Rotation;
};
