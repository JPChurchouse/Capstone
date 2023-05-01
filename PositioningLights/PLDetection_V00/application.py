import os
import sys

import cv2
import numpy as np

def main():
    
    camera = cv2.VideoCapture(1)
    if not camera.isOpened():
        raise IOError("Can't open webcam!")
    
    while True:
        ret, frame = camera.read()
        
        if not ret:
            raise IOError("Error with webcam")

        dark = cv2.convertScaleAbs(frame, 0.80, 100)
        cv2.imshow("Frame", frame)
        
        if cv2.waitKey(1) & 0xFF == ord('q'):
            break


    camera.release()
    cv2.destroyAllWindows()
    
    return 0


if __name__ == "__main__":
    sys.exit(main())