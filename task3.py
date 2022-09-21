import math
import tkinter
from tkinter import CENTER, filedialog

import PIL
from PIL import Image, ImageTk
import numpy as np
import array
import matplotlib.pyplot

class App:
    def __init__(self):
        self.root = tkinter.Tk()
        self.root.title("Преобразование изображение из RGB в HSV")
        self.frame = tkinter.Frame(self.root)
        self.frame.grid()

        self.var_h = tkinter.IntVar()
        scale_h = tkinter.Scale(self.root, variable=self.var_h, from_=0, to=360, orient=tkinter.HORIZONTAL, label='H', length = '400').grid(row=2,  column=0)

        self.var_s = tkinter.DoubleVar()
        scale_s = tkinter.Scale(self.root, variable=self.var_s, from_=0, to=100, orient=tkinter.HORIZONTAL, label='S', length = '400').grid(row=4,
                                                                                                            column=0)

        self.var_v = tkinter.DoubleVar()
        scale_v = tkinter.Scale(self.root, variable=self.var_v, from_=0, to=100, orient=tkinter.HORIZONTAL, label='V', length = '400').grid(row=6,
                                                                                                            column=0)

        self.image = Image.open("we.jpeg")
        self.image = self.image.resize((300, 400), Image.Resampling.LANCZOS)
        self.photo = ImageTk.PhotoImage(self.image)

        # вставляем кнопку
        # Добавим изображение
        self.canvas = tkinter.Canvas(self.root, height=400, width=400)
        self.c_image = self.canvas.create_image(0, 0, anchor='nw', image=self.photo)
        self.canvas.grid(row=1, column=0)

        self.but = tkinter.Button(self.frame, text="rgb => hsv", command=self.analyse_r_event_handler).grid(row=5,
                                                                                                                 column=3)

        self.but = tkinter.Button(self.frame, text="Сменить изображение", command=self.my_event_handler).grid(row=2,
                                                                                                              column=3)
        self.but3 = tkinter.Button(self.frame, text="Изменить изображение", command=self.my_event_handler()).grid(row=4,
                                                                                                              column=3)


        self.root.mainloop()



    def my_event_handler(self):
        self.image = Image.open("we.jpeg")
        self.image = self.image.resize((300, 400), Image.ANTIALIAS)
        self.photo = ImageTk.PhotoImage(self.image)
        self.c_image = self.canvas.create_image(0, 0, anchor='nw', image=self.photo)
        self.canvas.grid(row=1, column=0)


    def rgb_to_hsv(self):
        self.image_data = np.asarray(self.image)
        self.image_data_new = np.zeros(self.image_data.shape)
        for i in range(len(self.image_data)):
            for j in range(len(self.image_data[0])):
                r = self.image_data[i][j][0]/255
                g = self.image_data[i][j][1]/255
                b = self.image_data[i][j][2]/255
                n_max = max(r, g, b)
                n_min = min(r, g, b)
                if (n_max == r):
                    if (g>=b):
                        h = round((60*((g-b)/(n_max-n_min))), 0)
                    else:
                        h = round((60 * ((g - b) / (n_max - n_min)))+360, 0)
                elif (n_max == g):
                    h = round((60 * ((g - b) / (n_max - n_min))) + 120, 0)
                else:
                    h = round((60 * ((g - b) / (n_max - n_min))) + 240, 0)
                if (n_max == 0):
                    s = 0
                else:
                    s = round(1 - (n_min/n_max), 2)
                v = round(n_max, 2)
                opt_array = [h, s, v]
                self.image_data_new[i][j] = opt_array
                #print(self.image_data[i][j], "==", self.image_data_new[i][j])
        #Image.fromarray(self.image_data_new).show()

        #self.image = ImageTk.PhotoImage(Image.fromarray(self.image_data_new))

    def change_picture(self):
        self.image_data_new1 = np.zeros(self.image_data.shape)
        # self.image_data_new = np.asarray(self.image)
        for i in range(len(self.image_data_new)):
            for j in range(len(self.image_data_new[0])):
                h_c = min(self.image_data_new[i][j][0] + self.var_h.get(), 360)
                s_c = min(self.image_data_new[i][j][1] + (self.var_s.get()/100), 1)
                v_c = min(self.image_data_new[i][j][2] + (self.var_v.get()/100), 1)
                opt_array = [h_c,  s_c, v_c]
                print(opt_array)
                self.image_data_new1[i][j] = opt_array

    def hsv_to_rgb(self):
        self.image_data_new2 = np.zeros(self.image_data.shape)
        #self.image_data_new = np.asarray(self.image)
        for i in range(len(self.image_data_new)):
            for j in range(len(self.image_data_new[0])):
                #h = self.image_data_new[i][j][0]
                h =1
                s = self.image_data_new[i][j][1]
                v = self.image_data_new[i][j][2]
                hf =  int(math.floor(h/60))
                h_i = int(hf % 6)
                f = h/60 - hf
                p = v*(1-s)
                q = v*(1-f*s)
                t = v*(1-(1-f)*s)
                if h_i== 0:
                        r = v
                        g = t
                        b = p
                elif h_i == 1:
                        r = q
                        g = v
                        b = p
                elif h_i == 2:
                        r = p
                        g = v
                        b = t
                elif h_i == 3:
                        r = p
                        g = q
                        b = v
                elif h_i == 4:
                        r = t
                        g = p
                        b = v
                elif h_i == 5:
                        r = v
                        g = p
                        b = q
                else:
                    print("все плохо")
                opt_array = [int(r*256), int(g*256), int(b*256)]
                self.image_data_new2[i][j] = opt_array
#        PIL.Image.fromarray(self.image_data_new2).open()
        self.image = ImageTk.PhotoImage(Image.fromarray(self.image_data_new2, 'RGB'))
        self.canvas.create_image(0, 0, anchor=tkinter.NW, image=self.image)

    def analyse(self, a):
        self.image_data = np.asarray(self.image)
        color = {r: 0 for r in range(256)}
        for i in range(len(self.image_data)):
            for j in range(len(self.image_data[0])):
                color[self.image_data[i][j][a]] += 1
        return color

    def save_file(self):
        PIL.Image.fromarray(self.data).save(filedialog.asksaveasfilename())

    def analyse_r_event_handler(self):
        color = self.analyse(0)
        self.rgb_to_hsv()
        self.hsv_to_rgb()
        self.change_picture()


app= App()
