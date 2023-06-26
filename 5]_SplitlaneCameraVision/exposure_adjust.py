from __future__ import print_function
import os
import sys
import cv2 as cv
import numpy as np
import argparse



def main():

    #code
    path = os.getcwd() + "/video_trial.avi"
    camera = cv.VideoCapture(0)
    camera.set(cv.CAP_PROP_AUTOFOCUS, 100)
    if (not camera.isOpened()):
        print(f"Error:Camera not found!")
        return 1
    
    exposure = 40

    while True:
        ret, frame = camera.read()

        if not ret:
            raise IOError("Error with webcam")
        
        frame = cv.putText(frame, f"exposure = {exposure}", (50,50), cv.FONT_HERSHEY_SIMPLEX, 
                        1, (200, 50, 50), 2, cv.LINE_AA)     
                        ##start coords ##end coords

        cv.imshow("frame", frame)
        #output.write(frame)

        if cv.waitKey(1) & 0xFF == ord('u'):
            exposure = exposure + 5
            camera.set(cv.CAP_PROP_AUTO_EXPOSURE, exposure)
        
        if cv.waitKey(1) & 0xFF == ord('d'):
            exposure = exposure - 5
            camera.set(cv.CAP_PROP_AUTO_EXPOSURE, exposure)

        if cv.waitKey(1) & 0xFF == ord('q'):
            break
        

    return 0


if __name__ == "__main__":
    sys.exit(main())