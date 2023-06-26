from __future__ import print_function
import os
import sys
import cv2 as cv
import numpy as np
import argparse

#def detect_movement(frame, prior_frame):



def main():

    #code
    path = os.getcwd() + "video_trial.avi"
    camera = cv.VideoCapture(0)
    if (not camera.isOpened()):
        print(f"Error:Camera not found!")
        return 1
    
    # write video file
    #fourcc = cv.CV_FOURCC('m', 'p', '4', 'v')
    #video_writer = cv.VideoWriter('output.avi', -1, 20.0, (640,480))
    frame_num = 0
    avg_h = 0
    avg_s = 0
    avg_v = 0
    while True:
        ret, frame = camera.read()

        # later frames (dumb format but stick with me)    
        if not ret:
            raise IOError("Error with webcam")

        ## convert to greyscale
        blur = cv.GaussianBlur(frame, (31, 31), 0)
        hsv = cv.cvtColor(blur, cv.COLOR_BGR2HSV)
        frame_num = frame_num + 1                
        # Using cv2.putText() method
        x = 50
        y = 200
        size = 20
        
        ch, cs, cval = 0, 0, 0

        for xc in range(x, x+size):
            for yc in range(y+1,y+size):
                h, s, val = hsv[xc, yc]
                ch = ch + h
                cs = cs + s
                cval = cval + val

        ch = ch/(size*size)
        cs = cs/(size*size)
        cval = cval/(size*size)

        if frame_num == 50:
            avg_h = ch
            avg_s = cs
            avg_v = cval
        elif frame_num > 50:
            percent = (1/(frame_num-49))
            avg_h = avg_h*(1-percent) + (ch*percent)
            avg_s = avg_s*(1-percent) + (cs*percent)
            avg_v = avg_v*(1-percent) + (cval*percent)
            frame = cv.putText(frame, f"Colour rgb = {(int(avg_h), int(avg_s), int(avg_v))}", (50,50), cv.FONT_HERSHEY_SIMPLEX, 
                        1, (200, 50, 50), 2, cv.LINE_AA)

        cv.rectangle(frame, (x, y), (x+20, y+20), color = (20, 20, 20), thickness = 2)
        cv.imshow("frame", frame)


        if cv.waitKey(1) & 0xFF == ord('q'):
            break
        

    return 0


if __name__ == "__main__":
    sys.exit(main())