import numpy as np
import cv2
  
kernel = cv2.getStructuringElement(cv2.MORPH_ELLIPSE, (5, 5))
  
# creating object
fgbg = cv2.createBackgroundSubtractorMOG2(varThreshold = 20, detectShadows = False)
  
# capture frames from a camera 
cap = cv2.VideoCapture(0)
while(1):
    # read frames
    ret, img = cap.read()
      
    # apply mask for background subtraction
    fgmask = fgbg.apply(img)
      
    # with noise frame
    cv2.imshow('GMG noise', fgmask)
      
    # apply transformation to remove noise
    fgmask = cv2.morphologyEx(fgmask, cv2.MORPH_OPEN, kernel)

    edge = cv2.Canny(fgmask, 175, 175) 
    
    contours, hierarchy = cv2.findContours(edge, cv2.RETR_TREE, cv2.CHAIN_APPROX_SIMPLE)

    
    # Find the index of the largest contour
    areas = [cv2.contourArea(c) for c in contours]
    if areas is not None:
        max_index = np.argmax(areas)
        cnt = contours[max_index]

        x, y, w, h = cv2.boundingRect(cnt)
        cv2.rectangle(img, (x, y), (x+w, y+h), (0, 255, 0), 2)

        cv2.imshow('threshold', img)
        cv2.imshow('edge', edge)
    
    # after removing noise
    #cv2.imshow('GMG', fgmask)
      
    if cv2.waitKey(1) & 0xFF == ord('q'):
        break

  
cap.release()
cv2.destroyAllWindows()