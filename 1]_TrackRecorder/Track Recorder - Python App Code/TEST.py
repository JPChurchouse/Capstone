from tkinter import *  # Python 3
#from Tkinter import *  # Python 2
import turtle


turtle.forward(100)
ts = turtle.getscreen()

ts.getcanvas().postscript(file="duck.eps")
