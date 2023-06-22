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

    while True:
        ret, frame = camera.read()

        #first iteration don't have a prior frame to work with, set them the same and dont display anything!



        # later frames (dumb format but stick with me)    
        if not ret:
            raise IOError("Error with webcam")

        ## convert to greyscale
        blur = cv.GaussianBlur(frame, (31, 31), 0)
        
        #gray = cv.cvtColor(blur, cv.COLOR_BGR2GRAY)
        #edge = cv.Canny(gray, 5, 51) 
        #circles = cv.HoughCircles(gray, cv.HOUGH_GRADIENT, 1.4, minDist = 100, param1 = 120, param2 = 20, minRadius = 50, maxRadius = 150)

       # if circles is not None:
            #get the array circle coordinates and radii as integers 
       #     circles = np.round(circles[0, :]).astype("int")

            #iterate through each circle
        #    for (x, y, r) in circles:
                #checks for "ghost" apples and only draws circle if the hough circle found is a negative part of the mask
        #        cv.circle(frame, (x, y), r, (0, 0, 255), 3)
                        
        # Using cv2.putText() method
        x = 50
        y = 200
        cb, cg, cr = blur[x, y]
        frame = cv.putText(frame, f"Colour = {(cb, cg, cr)}", (50,50), cv.FONT_HERSHEY_SIMPLEX, 
                        1, (200, 50, 50), 2, cv.LINE_AA)
        #video_writer.write(frame)
        #fg mask
        cv.rectangle(frame, (x, y), (x+20, y+20), color = (20, 20, 20), thickness = 2)
        cv.imshow("frame", frame)
        cv.imshow("blur", blur)
        #cv.imshow("edge", edge)
        #cv.imshow("frame", frame)
        #find brightest moving thing
        #(minVal, maxVal, minLoc, maxLoc) = cv.minMaxLoc(subbed)
        #cv.circle(frame, maxLoc, 100, (255, 0, 0), 2)

        #cv.imshow("Light?", frame)

        if cv.waitKey(1) & 0xFF == ord('q'):
            break
        

    return 0


if __name__ == "__main__":
    sys.exit(main())