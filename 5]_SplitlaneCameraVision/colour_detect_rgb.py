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
    avg_g = 0
    avg_b = 0
    avg_r = 0
    while True:
        ret, frame = camera.read()

        #first iteration don't have a prior frame to work with, set them the same and dont display anything!



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
        cb, cg, cr = 0, 0, 0

        for xc in range(x, x+size):
            for yc in range(y+1,y+size):
                b, g, r = blur[xc, yc]
                cb = cb + b
                cg = cg + g
                cr = cr + r

        cb = cb/(size*size)
        cg = cg/(size*size)
        cr = cr/(size*size)

        if frame_num == 50:
            avg_b = cb
            avg_g = cg
            avg_r = cr
        elif frame_num > 50:
            percent = (1/(frame_num-49))
            avg_b = avg_b*(1-percent) + (cb*percent)
            avg_g = avg_g*(1-percent) + (cg*percent)
            avg_r = avg_r*(1-percent) + (cr*percent)
            frame = cv.putText(frame, f"Colour rgb = {(int(avg_r), int(avg_g), int(avg_b))}", (50,50), cv.FONT_HERSHEY_SIMPLEX, 
                        1, (200, 50, 50), 2, cv.LINE_AA)

        cv.rectangle(frame, (x, y), (x+20, y+20), color = (20, 20, 20), thickness = 2)
        cv.imshow("frame", frame)


        if cv.waitKey(1) & 0xFF == ord('q'):
            break
        

    return 0


if __name__ == "__main__":
    sys.exit(main())