from tkinter import *
from tkinter import ttk
from tkinter import filedialog
from PIL import Image, ImageTk
import numpy as np
import matplotlib.pyplot as plt
import math

class MainWindow:
    def __init__(self, window):
        self.root = window
        self.root.title('RGB to HSV')
        self.root.resizable(width=False, height=False)

        self.button_open = Button(self.root, text="Choose Image", command=self.open, padx=5, pady=5)
        self.button_open.grid(row=0)

        self.button_change = Button(self.root, text="Change Image", command=self.change, padx=5, pady=5)
        self.button_change.grid(row=1)

        self.button_save = Button(self.root, text="Save Image", command=self.save, padx=5, pady=5)
        self.button_save.grid(row=2)

        self.canvas_image = Canvas(self.root, width=400, height=400)
        self.canvas_image.grid(row=3)

        self.var_h = IntVar()
        self.var_h.set(0)
        self.scale_h = Scale(self.root, variable=self.var_h, from_=0, to=360, orient=HORIZONTAL, label='Hue', length = '400').grid(row=4)

        self.var_s = IntVar()
        self.var_s.set(0)
        self.scale_s = Scale(self.root, variable=self.var_s, from_=-100, to=100, orient=HORIZONTAL, label='Saturation', length = '400').grid(row=5)

        self.var_v = IntVar()
        self.var_v.set(0)
        self.scale_v = Scale(self.root, variable=self.var_v, from_=-100, to=100, orient=HORIZONTAL, label='Value or Brightness', length = '400').grid(row=6)

        self.root.mainloop()

    def open(self):
        self.file = filedialog.askopenfilename(initialdir=r"C:\Users\София\Desktop\The Seventh Semester\Компьютерная Графика\laba2_task3")

        image = Image.open(self.file)
        self.points = np.array(image)
        
        image_resized = image.resize((400, 400), Image.ANTIALIAS)
        self.photo_image = ImageTk.PhotoImage(image_resized)
        self.canvas_image.create_image(0, 0, image=self.photo_image, anchor=NW)

        self.var_h.set(0)
        self.var_s.set(0)
        self.var_v.set(0)

        self.rgb_to_hsv()

    def change(self):
        self.image_data_new = np.zeros(self.points.shape)
        h = self.var_h.get()
        s = self.var_s.get()
        v = self.var_v.get()

        for i in range(len(self.points)):
            for j in range(len(self.points[0])):
                if h == 0:
                    h_new = self.image_data[i][j][0]
                else:
                    h_new = (self.image_data[i][j][0] + h) % 360
                
                if s == 0:
                    s_new = self.image_data[i][j][1]
                else:
                    s_new = self.image_data[i][j][1] + s
                    s_new = max(min(s_new, 100), 0)

                if v == 0:
                    v_new = self.image_data[i][j][2]
                else:
                    v_new = self.image_data[i][j][2] + v
                    v_new = max(min(v_new, 100), 0)

                self.image_data_new[i][j][0] = h_new
                self.image_data_new[i][j][1] = s_new
                self.image_data_new[i][j][2] = v_new
        
        self.hsv_to_rgb()
        self.show_picture()

    def save(self):
        filename = "rgb_to_hsv.jpg"
        self.image_new.save(filename)

    def rgb_to_hsv(self):
        self.image_data = np.zeros(self.points.shape)

        for i in range(len(self.points)):
            for j in range(len(self.points[0])):
                r = self.points[i][j][0] / 255
                g = self.points[i][j][1] / 255
                b = self.points[i][j][2] / 255

                n_max = max(r, g, b)
                n_min = min(r, g, b)

                if n_max == n_min:
                    h = 0
                elif n_max == r:
                    if g >= b:
                        h = 60 * (g - b) / (n_max - n_min)
                    else:
                        h = 60 * (g - b) / (n_max - n_min) + 360
                elif n_max == g:
                    h = 60 * (b - r) / (n_max - n_min) + 120
                else:
                    h = 60 * (r - g) / (n_max - n_min) + 240   

                if n_max == 0:
                    s = 0
                else:
                    s = 1 - n_min / n_max

                v = n_max

                self.image_data[i][j][0] = int(h)
                self.image_data[i][j][1] = int(s * 100)
                self.image_data[i][j][2] = int(v * 100)
        
    def hsv_to_rgb(self):
        self.image_data_new2 = np.zeros(self.points.shape)

        for i in range(len(self.points)):
            for j in range(len(self.points[0])):
                h = self.image_data_new[i][j][0]
                s = self.image_data_new[i][j][1] / 100
                v = self.image_data_new[i][j][2] / 100

                h_i = math.floor(h / 60) % 6
                f = h / 60 - math.floor(h / 60)
                p = v * (1 - s)
                q = v * (1 - f * s)
                t = v * (1 - (1 - f) * s)

                match h_i:
                    case 0:
                        r = v; g = t; b = p
                    case 1:
                        r = q; g = v; b = p
                    case 2:
                        r = p; g = v; b = t
                    case 3:
                        r = p; g = q; b = v
                    case 4:
                        r = t; g = p; b = v
                    case 5:
                        r = v; g = p; b = q
                    case _:
                        raise ValueError('h_i value error')

                self.image_data_new2[i][j][0] = int(round(r * 255))
                self.image_data_new2[i][j][1] = int(round(g * 255))
                self.image_data_new2[i][j][2] = int(round(b * 255))

    def show_picture(self):
        self.image_new = Image.fromarray(self.image_data_new2.astype(np.uint8))
        image_resized = self.image_new.resize((400, 400), Image.ANTIALIAS)
        self.photo_image = ImageTk.PhotoImage(image_resized)
        self.canvas_image.create_image(0, 0, image=self.photo_image, anchor=NW) 

main_window = MainWindow(Tk())        