import os
import sys

import cv2
import numpy as np

def main():
    
    camera = cv2.VideoCapture(1)
    if not camera.isOpened():
        raise IOError("Can't open webcam!")
    
    video_writer = cv2.VideoWriter('output.avi', -1, 20.0, (640,480))
    
    while True:
        ret, frame = camera.read()
        
        if not ret:
            raise IOError("Error with webcam")

        ##dark = cv.convertScaleAbs(frame, 0.80, 100)
        gray = cv.cvtColor(frame, cv.COLOR_BGR2GRAY)
        gray = cv.GaussianBlur(gray, (15, 15), 0)
        (minVal, maxVal, minLoc, maxLoc) = cv.minMaxLoc(gray)

        cv.circle(frame, maxLoc, 100, (255, 0, 0), 2)

        cv.imshow("Light?", frame)

        video_writer.write(frame)
        
        if cv.waitKey(1) & 0xFF == ord('q'):
            break


    camera.release()
    cv2.destroyAllWindows()
    
    return 0


if __name__ == "__main__":
    sys.exit(main())
