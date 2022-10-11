from itertools import cycle
from tkinter import *
from tkinter import colorchooser, messagebox

from PIL import Image, ImageDraw
import numpy as np
from random import randint

from enum import Enum

class ShapeType(Enum):
    point = 0
    segment = 1
    square = 2
    circle = 3

# values - список всех значений, связанных с фигурой
class Shape:
    def __init__(self, type, values, color):
        self.type = type
        self.values = values
        self.color = color

shapes = []

x = 0
y = 0
count_click = 0

root = Tk()
root.title("Ну почти paint")
width = 800
height = 600 

brush_size = 10
color = 'blue'

canvas = Canvas(root,  width=width, height=height, bg = 'white')
canvas.grid(row=1, column=0, columnspan=6, rowspan=5, padx=4, pady=5, sticky=E+W+S+N )

is_redraw = False

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
    draw_img.rectangle((0, 0, width, height), width=0, fill='white')
    
    if not is_redraw:
        shapes.clear()

def popup(event):
    global x, y, prev_x, prev_y
    x = event.x
    y = event.y
    menu.post(event.x_root, event.y_root)
    

def square():
    shapes.append(Shape(ShapeType.square, [x, y, brush_size], color))

    canvas.create_rectangle(x, y, x+brush_size, y+brush_size, fill=color, width=0)
    draw_img.polygon((x, y, x+brush_size, y, x+brush_size, y+brush_size, x, y+brush_size), fill = color)

def point():
    shapes.append(Shape(ShapeType.point, [x, y, 3], color))

    canvas.create_oval(x, y, x+3, y+3, fill=color, width=0)
    draw_img.ellipse((x, y, x+3, y+3), fill=color)

def circle():
    shapes.append(Shape(ShapeType.circle, [x, y, brush_size], color))

    canvas.create_oval(x, y, x+brush_size, y+brush_size, fill=color, width=0)
    draw_img.ellipse((x, y, x+brush_size, y+brush_size), fill=color)

def segment():
    canvas.bind('<Button-3>', click)


def click(event):
    global prev_x, prev_y
    if prev_x == None:
        prev_x = event.x
        prev_y = event.y
        canvas.bind('<Button-3>', popup)
        return
    
    x = event.x
    y = event.y

    shapes.append(Shape(ShapeType.segment, [prev_x, prev_y, x, y, 3], color))

    canvas.create_line(prev_x, prev_y, x, y, width=3, fill=color)
    canvas.bind('<Button-3>', popup)

    prev_x = None
    prev_y = None

canvas.bind('<B1-Motion>', draw)
canvas.bind('<Button-3>', popup)

menu = Menu(tearoff=0)
menu.add_command(label='Точка', command=point)
menu.add_command(label='Отрезок', command=segment)
menu.add_command(label='Квадрат', command=square)
menu.add_command(label='Круг', command=circle)

image1 = Image.new('RGB', (width, height), 'white')
draw_img = ImageDraw.Draw(image1)

Button(root, text='Выбрать цвет', width=11, command=choose_color).grid(row=0, column=1, padx=6)
color_lab = Label(root, bg=color, width=10)
color_lab.grid(row=0, column=2, padx=6)

prev_x = None
prev_y = None

v = IntVar(value=10)
Scale(root, variable=v, from_=1, to=100, orient=HORIZONTAL, command=select, length = '400').grid(row=0, column=4, padx=6)

Button(root, text= 'Очистить', width=10, command=clear_scene).grid(row=0, column=3)

def redraw():
    global x, y, is_redraw
    is_redraw = True
    clear_scene()
    for shape in shapes:
        x = shape.values[0]
        y = shape.values[1]
        if shape.type == ShapeType.point:
            #point()
            brush_size = shape.values[2]
            color = shape.color
            canvas.create_oval(x, y, x + brush_size, y + brush_size, fill=color, width=0)
            draw_img.ellipse((x, y, x + brush_size, y + brush_size), fill=color)
        elif shape.type == ShapeType.segment:
            x_next = shape.values[2]
            y_next = shape.values[3]
            color = shape.color
            canvas.create_line(x, y, x_next, y_next, width=3, fill=color)
        elif shape.type == ShapeType.square:
            #square()
            brush_size = shape.values[2]
            color = shape.color
            canvas.create_rectangle(x, y, x + brush_size, y + brush_size, fill=color, width=0)
            draw_img.polygon((x, y, x + brush_size, y, x + brush_size, y + brush_size, x, y + brush_size), fill = color)
        else:
            #circle()
            brush_size = shape.values[2]
            color = shape.color
            canvas.create_oval(x, y, x + brush_size, y + brush_size, fill=color, width=0)
            draw_img.ellipse((x, y, x + brush_size, y + brush_size), fill=color)

    is_redraw = False


def translate():
    dx = var_x_trans.get()
    dy = var_y_trans.get()

    for shape in shapes:
        if shape.type == ShapeType.circle or shape.type == ShapeType.square:
            coord = np.array([shape.values[0], shape.values[1], 1])
            matrix = np.array([[1, 0, 0], [0, 1, 0], [dx, -dy, 1]])
            res = coord.dot(matrix)
            shape.values[0], shape.values[1] = res[0], res[1]
    
    redraw()



Label(root, text="Полигоны", padx=5, pady=5).grid(row=0, column=7)

Button(root, text="Смещение", padx=5, pady=5, command=translate).grid(row=1, column=6)
var_x_trans = IntVar()
var_x_trans.set(0)
scale_x_trans = Scale(root, variable=var_x_trans, from_=-400, to=400, orient=HORIZONTAL, label='X', length = '150').grid(row=1, column=7)
var_y_trans = IntVar()
var_y_trans.set(0)
scale_y_trans = Scale(root, variable=var_y_trans, from_=-300, to=300, orient=HORIZONTAL, label='Y', length = '150').grid(row=1, column=8)

Button(root, text="Поворот", padx=5, pady=5).grid(row=2, column=6)
var_rot = IntVar()
var_rot.set(0)
scale_rot = Scale(root, variable=var_rot, from_=-360, to=360, orient=HORIZONTAL, label='Градусы', length = '150').grid(row=2, column=7)

Button(root, text="Маштабирование", padx=5, pady=5).grid(row=3, column=6)
var_x_scal = IntVar()
var_x_scal.set(0)
scale_x_scal = Scale(root, variable=var_x_scal, from_=-10, to=10, orient=HORIZONTAL, label='X', length = '150').grid(row=3, column=7)
var_y_scal = IntVar()
var_y_scal.set(0)
scale_y_scal = Scale(root, variable=var_y_scal, from_=-10, to=10, orient=HORIZONTAL, label='Y', length = '150').grid(row=3, column=8)

Label(root, text="Рёбра", padx=5, pady=5).grid(row=4, column=7)

Button(root, text="Поворот на 90°", padx=5, pady=5).grid(row=5, column=6)
Button(root, text="Поворот на -90°", padx=5, pady=5).grid(row=5, column=7)
Button(root, text="Поиск точек пересечения", padx=5, pady=5).grid(row=5, column=8)

root.mainloop()