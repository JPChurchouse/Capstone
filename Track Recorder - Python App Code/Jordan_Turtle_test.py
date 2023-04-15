
from tkinter import *
 
root = Tk()
root.geometry("300x300")
root.title(" Q&A ")

import turtle
# Creating turtle screen
t = turtle.Turtle()
# To stop the screen to display


#screen = turtle.Screen()
    #screen.bgcolor("cyan")
#canvas = screen.getcanvas()
#canvas.size
#button = tk.Button(canvas.master, text="Press me", command=press)
#canvas.create_window(-50, -50, window=button)
#my_lovely_turtle = turtle.Turtle(shape="turtle")
#turtle.done()


TempString = '1,0.2,0.2,0'
x = TempString.split(', ')

turtle.tracer(0, 0) #-- pause graphics updates


PrevAngle = 0

import csv
with open('Book1.csv', newline='') as f:
    reader = csv.reader(f)
    for row in reader:
        #print(row) #-- print the row you just read
        t.right(float(row[3])-PrevAngle)
        PrevAngle = float(row[3])
        t.forward(float(row[1])*30)


turtle.update() #-- unpause graphics updates
print('Done')




# saves the current TKinter object in postscript format
root.postscript(file="image.eps", colormode='color')

# Convert from eps format to gif format using PIL
from PIL import Image as NewImage
img = NewImage.open("image.eps")
img.save("blank.gif", "gif")



turtle.mainloop()
