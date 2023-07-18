from __future__ import print_function
import os
import sys
import cv2 as cv
import numpy as np
import argparse

def col_dict(r, g, b, h):
    b1 = [0, 122, 199, 89]
    b2 = [0, 210, 127, 87]
    g1 = [25, 137, 0, 104]
    g2 = [116, 214, 0, 124]
    pp1 = [1, 2, 3, 4]

def main():

    #code
    path = os.getcwd() + "/video_trial.avi"
    camera = cv.VideoCapture(0)
    if (not camera.isOpened()):
        print(f"Error:Camera not found!")
        return 1
    
    # write video file
    frame_width = int(camera.get(3))
    frame_height = int(camera.get(4))
    frame_size = (frame_width,frame_height)
    fps = 30
    # write video file
    output = cv.VideoWriter(path, cv.VideoWriter_fourcc('M','J','P','G'), fps, frame_size)
  
    #variables
    frame_num = 0
    avg_h = 0
    avg_r = 0
    avg_g = 0
    avg_b = 0
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
        x = 30
        y = 180
        size_x = 70
        size_y = 30
        ch, cr, cg, cb = 0, 0, 0, 0

        for xc in range(x, x+size_x):
            for yc in range(y+1,y+size_y):
                h, s, val = hsv[yc, xc]
                ch = ch + h

                b, g, r = blur[yc, xc]
                cb = cb + b
                cg = cg + g
                cr = cr + r

        cb = cb/(size_x*size_y)
        cg = cg/(size_x*size_y)
        cr = cr/(size_x*size_y)
        ch = ch/(size_x*size_y)


        if frame_num == 20:
            avg_h = ch
            avg_b = cb
            avg_g = cg
            avg_r = cr

        elif frame_num > 20:
            percent = (1/5)
            avg_h = avg_h*(0.9) + (ch*0.1)
            avg_b = avg_b*(1-percent) + (cb*percent)
            avg_g = avg_g*(1-percent) + (cg*percent)
            avg_r = avg_r*(1-percent) + (cr*percent)



            frame = cv.putText(frame, f"Colour hrgb = {(int(avg_h), int(avg_r), int(avg_g), int(avg_b))}", (50,50), cv.FONT_HERSHEY_SIMPLEX, 
                        1, (200, 50, 50), 2, cv.LINE_AA)
        else: 
            frame = cv.putText(frame, f"hrgb loading", (50,50), cv.FONT_HERSHEY_SIMPLEX, 
                        1, (200, 50, 50), 2, cv.LINE_AA)     
                        ##start coords ##end coords
        cv.rectangle(frame, (x, y), (x+size_x, y+size_y), color = (20, 20, 20), thickness = 2)
        cv.imshow("frame", frame)
        output.write(frame)

        if cv.waitKey(1) & 0xFF == ord('r'):
            frame_num = 0
            avg_h, avg_r, avg_g, avg_b = 0, 0, 0, 0
            ch, cr, cg, cb = 0, 0, 0, 0

        if cv.waitKey(1) & 0xFF == ord('q'):
            break
        

    return 0


if __name__ == "__main__":
    sys.exit(main())