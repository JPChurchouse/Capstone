from __future__ import print_function
import os
import sys
import cv2 as cv
import numpy as np
import argparse

#def detect_movement(frame, prior_frame):



def main():

    #code
    path = "video_trial.avi"
    camera = cv.VideoCapture(1)
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
    frame_num = 0

    while True:
        ret, frame = camera.read()
        #bg_sub = cv.createBackgroundSubtractorKNN(history = 500, dist2Threshold = 160, detectShadows = False)
        #first iteration don't have a prior frame to work with, set them the same and dont display anything!
        if frame_num <= 5:
            if not ret:
                raise IOError("Error with webcam")

            ## convert to greyscale
            gray = cv.cvtColor(frame, cv.COLOR_BGR2GRAY)
            gray = cv.GaussianBlur(gray, (45, 45), 0)

            #video_writer.write(frame)
            # image subtraction
            prior_frame = frame
            prior_gray = gray
            frame_num = frame_num + 1


        # later frames (dumb format but stick with me)    
        if not ret:
            raise IOError("Error with webcam")

        ## convert to greyscale
        blur = cv.GaussianBlur(frame, (15, 15), 0)
        gray = cv.cvtColor(blur, cv.COLOR_BGR2GRAY)
        

        #video_writer.write(frame)
        #fg mask
        #fg_mask = bg_sub.apply(frame)
        # image subtraction
        subbed = cv.absdiff(gray, prior_gray)
        prior_frame = frame
        prior_gray = gray
        #cv.imshow("subtraction?", fg_mask)
        #cv.imshow("frame", frame)
        #find brightest moving thing
        (minVal, maxVal, minLoc, maxLoc) = cv.minMaxLoc(subbed)
        cv.circle(frame, maxLoc, 70, (255, 0, 0), 2)

        cv.imshow("Light?", frame)
        output.write(frame)

        if cv.waitKey(1) & 0xFF == ord('q'):
            break
        
    camera.release()
    output.release()            
    return 0


if __name__ == "__main__":
    sys.exit(main())