from itertools import cycle
from tkinter import *
from tkinter import colorchooser, messagebox

from PIL import Image, ImageDraw
import numpy as np
from random import randint
import math

from enum import Enum

class ShapeType(Enum):
    point = 0
    segment = 1
    triangle = 2
    square = 3

class Shape:
    def __init__(self, type, coordinates, color):
        self.type = type
        self.coordinates = coordinates
        self.color = color

shapes = []

x = 0
y = 0
count_click = 0

root = Tk()
root.title("Задание 4")
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

def triangle():
    x_1 = x + brush_size
    y_1 = y
    x_2 = x + (x_1 - x)/2
    y_2 =  y - brush_size * 1.73205080757 / 2
    shapes.append(Shape(ShapeType.triangle, [x, y, x_1, y, x_2, y_2], color))

    canvas.create_polygon(x, y, x_1, y_1, x_2, y_2, fill=color, width=0)
    draw_img.polygon((x, y, x_1, y_1, x_2, y_2), fill = color)
    
def square():
    x_1 = x + brush_size
    y_1 = y
    x_2 = x_1
    y_2 = y + brush_size
    x_3 = x
    y_3 = y_2
    shapes.append(Shape(ShapeType.square, [x, y, x_1, y_1, x_2, y_2, x_3, y_3], color))

    canvas.create_polygon(x, y, x_1, y_1, x_2, y_2, x_3, y_3, fill=color, width=0)
    draw_img.polygon((x, y, x_1, y_1, x_2, y_2, x_3, y_3), fill = color)

def point():
    x_next = x + 3
    y_next = y + 3
    shapes.append(Shape(ShapeType.point, [x, y, x_next, y_next], color))

    canvas.create_oval(x, y, x_next, y_next, fill=color, width=0)
    draw_img.ellipse((x, y, x_next, y_next), fill=color)

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

    shapes.append(Shape(ShapeType.segment, [prev_x, prev_y, x, y], color))

    canvas.create_line(prev_x, prev_y, x, y, width=3, fill=color)
    canvas.bind('<Button-3>', popup)

    prev_x = None
    prev_y = None

canvas.bind('<B1-Motion>', draw)
canvas.bind('<Button-3>', popup)

menu = Menu(tearoff=0)
menu.add_command(label='Точка', command=point)
menu.add_command(label='Отрезок', command=segment)
menu.add_command(label='Треугольник', command=triangle)
menu.add_command(label='Квадрат', command=square)

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
        if shape.type == ShapeType.point:
            canvas.create_oval(shape.coordinates, fill=shape.color, width=0)
            draw_img.ellipse(shape.coordinates, fill=shape.color)
        elif shape.type == ShapeType.segment:
            canvas.create_line(shape.coordinates, width=3, fill=shape.color)
        elif shape.type == ShapeType.segment:
            canvas.create_line(shape.coordinates, width=3, fill=color)
        elif shape.type == ShapeType.triangle:
            canvas.create_polygon(shape.coordinates, fill=shape.color, width=0)
            draw_img.polygon(shape.coordinates, fill = shape.color)
        else:
            canvas.create_polygon(shape.coordinates, fill=shape.color, width=0)
            draw_img.polygon(shape.coordinates, fill = shape.color)
       
    var_x_trans.set(0)
    var_y_trans.set(0)
    var_rot.set(0)
    var_x_scal.set(1)
    var_y_scal.set(1)

    is_redraw = False

def coord_mult(shape, matrix):
    n = len(shape.coordinates)
    i = 0
    while i < n:
        coord = np.array([shape.coordinates[i], shape.coordinates[i + 1], 1])
        res = coord.dot(matrix)
        shape.coordinates[i], shape.coordinates[i + 1] = res[0], res[1]
        i += 2

def translate():
    d_x = var_x_trans.get()
    d_y = var_y_trans.get()

    for shape in shapes:
        if shape.type == ShapeType.triangle or shape.type == ShapeType.square:
            matrix = np.array([[1, 0, 0], [0, 1, 0], [d_x, -d_y, 1]])
            coord_mult(shape, matrix)
    
    redraw()

def find_center(shape):
    i = 0
    n = len(shape.coordinates)
    x_res = 0
    y_res = 0

    while i < n:
        x_res += shape.coordinates[i]
        y_res += shape.coordinates[i + 1]
        i += 2

    n /= 2
    x_res /= n
    y_res /= n
    return x_res, y_res

def rotate():
    degree = var_rot.get()
    sin = math.sin(np.deg2rad(degree))
    cos = math.cos(np.deg2rad(degree))

    for shape in shapes:
        if shape.type == ShapeType.triangle or shape.type == ShapeType.square:
                x_center, y_center = find_center(shape)
                matrix = np.array([[1, 0, 0], [0, 1, 0], [-x_center, -y_center, 1]])
                coord_mult(shape, matrix)

                matrix = np.array([[cos, sin, 0], [-sin, cos, 0], [0, 0, 1]])
                coord_mult(shape, matrix)

                matrix = np.array([[1, 0, 0], [0, 1, 0], [x_center, y_center, 1]])
                coord_mult(shape, matrix)
    redraw()

def scale():
    k_x = var_x_scal.get()
    k_y = var_y_scal.get()

    for shape in shapes:
        if shape.type == ShapeType.triangle or shape.type == ShapeType.square:
                x_center, y_center = find_center(shape)
                matrix = np.array([[1, 0, 0], [0, 1, 0], [-x_center, -y_center, 1]])
                coord_mult(shape, matrix)

                matrix = np.array([[k_x, 0, 0], [0, k_y, 0], [0, 0, 1]])
                coord_mult(shape, matrix)

                matrix = np.array([[1, 0, 0], [0, 1, 0], [x_center, y_center, 1]])
                coord_mult(shape, matrix)
    redraw()


Label(root, text="Полигоны", padx=5, pady=5).grid(row=0, column=7)

Button(root, text="Смещение", padx=5, pady=5, command=translate).grid(row=1, column=6)
var_x_trans = IntVar()
var_x_trans.set(0)
scale_x_trans = Scale(root, variable=var_x_trans, from_=-400, to=400, orient=HORIZONTAL, label='X', length = '150').grid(row=1, column=7)
var_y_trans = IntVar()
var_y_trans.set(0)
scale_y_trans = Scale(root, variable=var_y_trans, from_=-300, to=300, orient=HORIZONTAL, label='Y', length = '150').grid(row=1, column=8)

Button(root, text="Поворот", padx=5, pady=5, command=rotate).grid(row=2, column=6)
var_rot = IntVar()
var_rot.set(0)
scale_rot = Scale(root, variable=var_rot, from_=-360, to=360, orient=HORIZONTAL, label='Градусы', length = '150').grid(row=2, column=7)

Button(root, text="Маштабирование", padx=5, pady=5, command=scale).grid(row=3, column=6)
var_x_scal = DoubleVar()
var_x_scal.set(1)
scale_x_scal = Scale(root, variable=var_x_scal, from_=0.1, to=10, resolution = 0.1, orient=HORIZONTAL, label='X', length = '150').grid(row=3, column=7)
var_y_scal = DoubleVar()
var_y_scal.set(1)
scale_y_scal = Scale(root, variable=var_y_scal, from_=0.1, to=10, resolution = 0.1, orient=HORIZONTAL, label='Y', length = '150').grid(row=3, column=8)

Label(root, text="Рёбра", padx=5, pady=5).grid(row=4, column=7)

Button(root, text="Поворот на 90°", padx=5, pady=5).grid(row=5, column=6)
Button(root, text="Поворот на -90°", padx=5, pady=5).grid(row=5, column=7)
Button(root, text="Поиск точек пересечения", padx=5, pady=5).grid(row=5, column=8)

root.mainloop()