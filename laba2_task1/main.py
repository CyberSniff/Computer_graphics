from tkinter import *
from tkinter import ttk
from tkinter import filedialog
from turtle import color
from PIL import Image, ImageTk
import numpy as np
import matplotlib.pyplot as plt

class MainWindow:
    def __init__(self, window):
        self.root = window
        self.root.title('RGB to Grayscale Conversion')
        self.root.resizable(width=False, height=False)

        self.canvas_image = Canvas(self.root, width=300, height=300)
        self.canvas_image.grid(row=0, column=0)
        self.label_image = Label(self.root, text="Original Image")
        self.label_image.grid(row=1, column=0)

        self.canvas_image_changed_1 = Canvas(self.root, width=300, height=300) 
        self.canvas_image_changed_1.grid(row=0, column=1)
        self.label_image_changed_1 = Label(self.root, text="NTSC RGB Method")
        self.label_image_changed_1.grid(row=1, column=1)

        self.canvas_image_changed_2 = Canvas(self.root, width=300, height=300)
        self.canvas_image_changed_2.grid(row=0, column=2)
        self.label_image_changed_2 = Label(self.root, text="sRGB Method")
        self.label_image_changed_2.grid(row=1, column=2)

        self.canvas_image_difference = Canvas(self.root, width=300, height=300)
        self.canvas_image_difference.grid(row=2, column=0)
        self.label_image_difference = Label(self.root, text="Difference")
        self.label_image_difference.grid(row=3, column=0)

        self.canvas_histogram_1 = Canvas(self.root, width=300, height=300)
        self.canvas_histogram_1.grid(row=2, column=1)
        self.label_image_histogram_1 = Label(self.root, text="NTSC RGB Histogram")
        self.label_image_histogram_1.grid(row=3, column=1)

        self.canvas_histogram_2 = Canvas(self.root, width=300, height=300)
        self.canvas_histogram_2.grid(row=2, column=2)
        self.label_image_histogram_2 = Label(self.root, text="sRGB Histogram")
        self.label_image_histogram_2.grid(row=3, column=2)

        self.button = Button(self.root, text="Choose Image", command=self.open)
        self.button.grid(row=4, column=1, padx=5, pady=5)

        self.root.mainloop()
    
    def open(self):
        file = filedialog.askopenfilename(initialdir=r"C:\Users\София\Desktop\The Seventh Semester\Компьютерная Графика\laba2_task1")
        image = Image.open(file)
        
        self.points = np.array(image)
        
        image_resized = image.resize((300, 300), Image.ANTIALIAS)
        self.photo_image = ImageTk.PhotoImage(image_resized)
        self.canvas_image.create_image(0, 0, image=self.photo_image, anchor=NW)

        self.change_1()
        self.change_2()
        self.difference()
        self.histogram_1()
        self.histogram_2()
    
    def change_1(self):
        self.result_1 = np.zeros_like(self.points)
        for i in range(0, self.points.shape[0]):
            for j in range(0, self.points.shape[1]):
                pixel = self.points[i, j]
                y = pixel[0] * 0.299 + pixel[1] * 0.587 + pixel[2] * 0.114
                self.result_1[i, j, 0] = self.result_1[i, j, 1] = self.result_1[i, j, 2] = y

        image_new = Image.fromarray(self.result_1)
        image_resized = image_new.resize((300, 300), Image.ANTIALIAS)
        self.photo_image_changed_1 = ImageTk.PhotoImage(image_resized)
        self.canvas_image_changed_1.create_image(0, 0, image=self.photo_image_changed_1, anchor=NW)        
    
    def change_2(self):
        self.result_2 = np.zeros_like(self.points)
        for i in range(0, self.points.shape[0]):
            for j in range(0, self.points.shape[1]):
                pixel = self.points[i, j]
                y = pixel[0] * 0.2126 + pixel[1] * 0.7152 + pixel[2] * 0.0722
                self.result_2[i, j, 0] = self.result_2[i, j, 1] =  self.result_2[i, j, 2] = y
                
        image_new = Image.fromarray(self.result_2)
        image_resized = image_new.resize((300, 300), Image.ANTIALIAS)
        self.photo_image_changed_2 = ImageTk.PhotoImage(image_resized)
        self.canvas_image_changed_2.create_image(0, 0, image=self.photo_image_changed_2, anchor=NW)  

    def difference(self):
        self.result_difference = np.zeros_like(self.points)
        for i in range(0, self.points.shape[0]):
            for j in range(0, self.points.shape[1]):
                y =  abs(self.result_1[i, j, 0] - self.result_2[i, j, 0])
                self.result_difference[i, j, 0] = self.result_difference[i, j, 1] = self.result_difference[i, j, 2] = y
        
        image_new = Image.fromarray(self.result_difference)
        image_resized = image_new.resize((300, 300), Image.ANTIALIAS)
        self.photo_image_difference = ImageTk.PhotoImage(image_resized)
        self.canvas_image_difference.create_image(0, 0, image=self.photo_image_difference, anchor=NW) 

    def histogram_1(self):
        fig1 = plt.gcf()
        x = np.arange(256, dtype=int)
        y = np.zeros_like(x, dtype=int)
        for i in range(0, self.result_1.shape[0]):
            for j in range(0, self.result_1.shape[1]):
                y[self.result_1[i,j,0]] += 1

        plt.bar(x, y, color='black')
        fig1.savefig('histogram_1.png', dpi=100)

        image = Image.open("histogram_1.png")
        image_resized = image.resize((300, 300), Image.ANTIALIAS)
        self.photo_histogram_1 = ImageTk.PhotoImage(image_resized)
        self.canvas_histogram_1.create_image(0, 0, image=self.photo_histogram_1, anchor=NW) 

    def histogram_2(self):
        fig1 = plt.gcf()
        x = np.arange(256, dtype=int)
        y = np.zeros_like(x, dtype=int)
        for i in range(0, self.result_2.shape[0]):
            for j in range(0, self.result_2.shape[1]):
                y[self.result_2[i,j,0]] += 1

        plt.bar(x, y, color='black')
        fig1.savefig('histogram_2.png', dpi=100)

        image = Image.open("histogram_2.png")
        image_resized = image.resize((300, 300), Image.ANTIALIAS)
        self.photo_histogram_2 = ImageTk.PhotoImage(image_resized)
        self.canvas_histogram_2.create_image(0, 0, image=self.photo_histogram_2, anchor=NW)

main_window = MainWindow(Tk())