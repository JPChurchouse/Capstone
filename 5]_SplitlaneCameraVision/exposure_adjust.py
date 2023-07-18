from __future__ import print_function
import os
import sys
import cv2 as cv
import numpy as np
import argparse
import subprocess

#              exposure_absolute (int)    : min=3 max=2047 step=1 default=250 value=333

def main():
    exposures = [3, 10, 20, 30, 40, 50, 75, 100, 150, 200, 250]

    for exposure in exposures:
        #code
        path = os.getcwd() + f"/exposure_{str(exposure)}.avi"

        #or depending on camera
        subprocess.check_call(f"v4l2-ctl -d /dev/video0 -c exposure_absolute={str(exposure)}", shell = True)

        camera = cv.VideoCapture(0)
        if (not camera.isOpened()):
            print(f"Error:Camera not found!")
            return 1
        
        frame_width = int(camera.get(3))
        frame_height = int(camera.get(4))
        frame_size = (frame_width,frame_height)
        fps = 30
        # write video file
        output = cv.VideoWriter(path, cv.VideoWriter_fourcc('M','J','P','G'), fps, frame_size)

        #frame counter
        frame_num = 0
        while frame_num < 40:
            ret, frame = camera.read()

            if not ret:
                raise IOError("Error with webcam")
            
            frame = cv.putText(frame, f"exposure = {exposure}", (50,50), cv.FONT_HERSHEY_SIMPLEX, 
                            1, (200, 50, 50), 2, cv.LINE_AA)     
                            ##start coords ##end coords

            cv.imshow("frame", frame)
            output.write(frame)
            frame_num = frame_num + 1

            if cv.waitKey(1) & 0xFF == ord('q'):
                    break
        
        camera.release()

    return 0


if __name__ == "__main__":
    sys.exit(main())