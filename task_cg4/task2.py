from tkinter import *
from tkinter import colorchooser, messagebox

from PIL import Image, ImageDraw
import numpy as np
from random import randint

x = 0
y = 0
count_click = 0


root = Tk()
root.title("Ну почти paint")
root.geometry('800x600')

brush_size = 10
color = 'blue'

root.columnconfigure(6, weight = 1)
root.rowconfigure(2, weight=1)

canvas = Canvas(root, bg = 'white')
canvas.grid(row=2, column =0, columnspan=7, padx=5, pady=5, sticky=E+W+S+N )


def draw(event):
    x1, y1= (event.x - brush_size), (event.y - brush_size)
    x2, y2 = (event.x + brush_size), (event.y + brush_size)
    canvas.create_oval(x1, y1, x2, y2, fill=color, width=0)
    draw_img.ellipse((x1, y1, x2, y2), fill=color, width=0)


def choose_color():
    global color
    (rgb, hx) = colorchooser.askcolor()
    color = hx
    color_lab['bg'] = hx


def select(value):
    global brush_size
    brush_size= int(value)

def clear_scene():
    canvas.delete('all')
    canvas['bg']='white'
    draw_img.rectangle((0,0, 800, 570), width=0, fill='white')

def popup(event):
    global x, y
    x = event.x
    y = event.y
    menu.post(event.x_root, event.y_root)

def square():
    canvas.create_rectangle(x, y, x+brush_size, y+brush_size, fill=color, width=0)
    draw_img.polygon((x, y, x+brush_size, y, x+brush_size, y+brush_size, x, y+brush_size), fill = color)

def point():
    canvas.create_oval(x, y, x+1, y+1, fill=color, width=0)
    draw_img.ellipse((x, y, x+1, y+1), fill=color)

def cyrcle():
    canvas.create_oval(x, y, x+brush_size, y+brush_size, fill=color, width=0)
    draw_img.ellipse((x, y, x+brush_size, y+brush_size), fill=color)

def segment():
    canvas.bind('<Button-3>', click)


def click(event):
    global prev_x, prev_y
    x = event.x
    y = event.y
    if prev_x:  # если в переменной prev_x есть значение (не None)
        canvas.create_line(x, y, prev_x, prev_y, width=3, fill=color)
    prev_x = x
    prev_y = y
    canvas.bind('<Button-3>', popup)

canvas.bind('<B1-Motion>', draw)
canvas.bind('<Button-3>', popup)

menu = Menu(tearoff=0)
menu.add_command(label='Точка', command=point)
menu.add_command(label='Отрезок', command=segment)
menu.add_command(label='Квадрат', command=square)
menu.add_command(label='Круг', command=cyrcle)


image1 = Image.new('RGB', (800, 570), 'white')
draw_img = ImageDraw.Draw(image1)

Button(root, text='Выбрать цвет', width=11, command=choose_color).grid(row=0, column=1, padx=6)
color_lab = Label(root, bg=color, width=10)
color_lab.grid(row=0, column=2, padx=6)

prev_x = None
prev_y = None

v = IntVar(value=10)
Scale(root, variable=v, from_=1, to=100, orient=HORIZONTAL, command=select,  length = '400').grid(row=0, column=4, padx=6)

Button(root, text= 'Очистить', width=10, command=clear_scene).grid(row=0, column=3)

root.mainloop()