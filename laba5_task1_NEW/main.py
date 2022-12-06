from my_turtle import my_turtle
from tkinter import *
from colormap import rgb2hex
import numpy as np

def translate(x, y, dx, dy):
    coordinates = np.array([x, y, 1])
    matrix = np.array([[1, 0, 0], [0, 1, 0], [dx, dy, 1]]) 
    result = coordinates.dot(matrix)
    return result[0], result[1]

def scale(x, y, k):
    coordinates = np.array([x, y, 1])
    matrix = np.array([[k, 0, 0], [0, k, 0], [0, 0, 1]]) 
    result = coordinates.dot(matrix)
    return result[0], result[1]


class MainWindow:
    def __init__(self, window, turtle_result):
        self.root = window
        self.root.title('Fractals')
        self.width, self.height = width, height
        self.root.resizable(width=False, height=False)

        self.turtle_result = turtle_result

        self.canv = Canvas(self.root, width = self.width, height = self.height, bg = "white")
        self.img = PhotoImage(width=self.width, height=self.height)
        self.canv.grid(row=1, column=0)

        self.draw()

        self.canv.pack()
        self.root.mainloop()

    def draw(self):
        result = self.turtle_result[:]
        x_max, x_min, y_max, y_min = 0, 0, 0, 0

        for line in result:
            if line[0] > x_max:
                x_max = line[0]
            elif line[0] < x_min:
                x_min = line[0]

            if line[2] > x_max:
                x_max = line[2]
            elif line[2] < x_min:
                x_min = line[2]

            if line[1] > y_max:
                y_max = line[1]
            elif line[1] < y_min:
                y_min = line[1]

            if line[3] > y_max:
                y_max = line[3]
            elif line[3] < y_min:
                y_min = line[3]

        k = 0
        x_diff = x_max + abs(x_min)
        y_diff = y_max + abs(y_min)
        if x_diff > self.width:
            k = self.width / x_diff 
        
        if y_diff > self.height:
            k = min(k, self.height / y_diff)

        if k > 0:
            k -= 0.01
            for line in result:
                x_first, y_first, x_second, y_second = line[0], line[1], line[2], line[3]

                x_first, y_first = scale(x_first, y_first, k)
                x_second, y_second = scale(x_second, y_second, k)

                line[0], line[1], line[2], line[3] = x_first, y_first, x_second, y_second

            x_max, x_min, y_max, y_min = 0, 0, 0, 0

            for line in result:
                if line[0] > x_max:
                    x_max = line[0]
                elif line[0] < x_min:
                    x_min = line[0]

                if line[2] > x_max:
                    x_max = line[2]
                elif line[2] < x_min:
                    x_min = line[2]

                if line[1] > y_max:
                    y_max = line[1]
                elif line[1] < y_min:
                    y_min = line[1]

                if line[3] > y_max:
                    y_max = line[3]
                elif line[3] < y_min:
                    y_min = line[3]

        x_center = (x_max + x_min) / 2
        y_center = (y_max + y_min) / 2

        x_center = self.width//2 - x_center
        y_center = self.height//2 - y_center

        for line in result:
            x_first, y_first, x_second, y_second = line[0], line[1], line[2], line[3]

            x_first, y_first = translate(x_first, y_first, x_center, y_center)
            x_second, y_second = translate(x_second, y_second, x_center, y_center)

            line[0], line[1], line[2], line[3] = x_first, y_first, x_second, y_second

                
        for line in self.turtle_result:
            r, g, b = line[4]
            self.canv.create_line(line[0], line[1], line[2], line[3], fill=rgb2hex(r, g, b), width=line[5]) 

# user's section

generation = 5

file = 'koch_curve.txt' # 4 or 5
#file = 'sierpinski_triangle.txt' # 6 MAX - more is not interesting
#file = 'gosper_curve.txt' # 5 MAX
#file = 'dragon_curve.txt' # 12
#file = 'tree.txt'# 5
#file = 'tree_color.txt'# 10

width, height = 1000, 700 
t = my_turtle(file, generation, width, height)

main_window = MainWindow(Tk(), t.turtle_result)  
