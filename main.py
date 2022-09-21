# This is a sample Python script.

# Press Shift+F10 to execute it or replace it with your code.
# Press Double Shift to search everywhere for classes, files, tool windows, actions, and settings.
import tkinter
from tkinter import filedialog

from PIL import Image, ImageTk
import numpy as np
import matplotlib.pyplot

class App:
    def __init__(self):
        self.root = tkinter.Tk()
        self.root.title("Анализ R, G, B каналов")
        self.frame = tkinter.Frame(self.root)
        self.frame.grid()


        self.image = Image.open("we.jpeg")
        self.image = self.image.resize((300, 400), Image.Resampling.LANCZOS)
        self.photo = ImageTk.PhotoImage(self.image)


        self.canvas = tkinter.Canvas(self.root, width = self.image.width, height=self.image.height)
        self.c_image = self.canvas.create_image(0, 0, anchor='nw', image=self.photo)
        self.canvas.grid(row=0, column=1)

        self.but = tkinter.Button(self.frame, text="Анализировать R", command=self.analyse_r_event_handler).grid(row=5,
                                                                                                                 column=3)
        self.but = tkinter.Button(self.frame, text="Анализировать G", command=self.analyse_g_event_handler).grid(row=4,
                                                                                                                 column=3)
        self.but = tkinter.Button(self.frame, text="Анализировать B", command=self.analyse_b_event_handler).grid(row=3,
                                                                                                                 column=3)
        self.but = tkinter.Button(self.frame, text="Сменить изображение", command=self.my_event_handler).grid(row=2,
                                                                                                              column=3)
        self.root.mainloop()




    def my_event_handler(self):
        self.filename = filedialog.askopenfilename()
        self.image = Image.open(self.filename)
        self.photo = ImageTk.PhotoImage(self.image)
        self.c_image = self.canvas.create_image(0, 0, anchor='nw', image=self.photo)
        self.canvas.grid(row=1, column=1)

    def analyse(self, a):
        self.image_data = np.asarray(self.image)
        color = {r: 0 for r in range(256)}
        for i in range(len(self.image_data)):
            for j in range(len(self.image_data[0])):
                color[self.image_data[i][j][a]] += 1
        print(color)
        return color

    def analyse_r_event_handler(self):
        color = self.analyse(0)
        matplotlib.pyplot.bar(list(color.keys()), color.values(), color='#f02d63')
        matplotlib.pyplot.show()

    def analyse_g_event_handler(self):
        color = self.analyse(1)
        matplotlib.pyplot.bar(list(color.keys()), color.values(), color='#49ff63')
        matplotlib.pyplot.show()

    def analyse_b_event_handler(self):
        color = self.analyse(2)
        matplotlib.pyplot.bar(list(color.keys()), color.values(), color='#49a4fc')
        matplotlib.pyplot.show()

app= App()

# See PyCharm help at https://www.jetbrains.com/help/pycharm/
