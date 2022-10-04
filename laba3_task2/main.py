from tkinter import *
from colormap import rgb2hex

class MainWindow:
    def __init__(self, window):
        self.root = window
        self.root.title('Line Rasterization')
        self.root.resizable(width=False, height=False)

        self.width = 600 
        self.height = 300 

        canv_1 = Canvas(self.root, width = self.width, height = self.height, bg = "white")
        self.img_1 = PhotoImage(width=self.width, height=self.height)
        canv_1.create_image((self.width / 2, self.height / 2), image=self.img_1, state="normal")
        label_1 = Label(self.root, text="Bresenham's Line Algorithm")

        canv_2 = Canvas(self.root, width = self.width, height = self.height, bg = "white")
        self.img_2 = PhotoImage(width=self.width, height=self.height)
        canv_2.create_image((self.width / 2, self.height / 2), image=self.img_2, state="normal")
        label_2 = Label(self.root, text="Wu's Line Algorithm")

        # в пределах x: -300, 300 y: -150, 150
        self.x_1, self.y_1 = 210, -100
        self.x_2, self.y_2 = -210, 100

        self.alg_br()
        canv_1.pack()
        label_1.pack()

        self.alg_wo()
        canv_2.pack()
        label_2.pack()

        self.root.mainloop()

    def alg_br(self):
        d_x = self.x_2 - self.x_1
        d_y = self.y_2 - self.y_1
        if d_x == 0:
            gr = 1
        else:
            gr = abs(d_y / d_x)
        
        x = self.x_1
        y = self.y_1

        if gr < 1: # x растёт быстрее
            x_finish = self.x_2
            if self.x_1 > self.x_2:
                d_x = self.x_1 - self.x_2
                d_y = self.y_1 - self.y_2
                x = self.x_2
                y = self.y_2
                x_finish = self.x_1

            d = 2 * d_y - d_x

            y_i = 1
            if d_y < 0:
                y_i = -1
                d_y = -d_y
            
            while x <= x_finish:
                self.img_1.put(rgb2hex(0, 0, 0), (self.width//2 + x, self.height//2 - y))
                if d < 0:
                    d += 2 * d_y
                else:
                    y += y_i
                    d += 2 * (d_y - d_x)
                x += 1
                    
        else: # y растёт быстрее
            y_finish = self.y_2

            if self.y_1 > self.y_2:
                d_x = self.x_1 - self.x_2
                d_y = self.y_1 - self.y_2
                x = self.x_2
                y = self.y_2
                y_finish = self.y_1

            d = 2 * d_x - d_y

            x_i = 1
            if d_x < 0:
                x_i = -1
                d_x = -d_x
            
            while y <= y_finish:
                self.img_1.put(rgb2hex(0, 0, 0), (self.width//2 + x, self.height//2 - y))
                if d < 0:
                    d += 2 * d_x
                else:
                    x += x_i
                    d += 2 * (d_x - d_y)
                y += 1

    def alg_wo(self):
        d_x = self.x_2 - self.x_1
        d_y = self.y_2 - self.y_1

        x = self.x_1
        y = self.y_1
        x_finish = self.x_2
        y_finish = self.y_2

        if d_x == 0:
            y = min(self.y_1, self.y_2)
            y_finish = max(self.y_1, self.y_2)

            while y <= y_finish:
                self.img_2.put(rgb2hex(0, 0, 0), (self.width//2 + x, self.height//2 - y))
                y += 1
            return

        if d_y == 0:
            x = min(self.x_1, self.x_2)
            x_finish = max(self.x_1, self.x_2)

            while x <= x_finish:
                self.img_2.put(rgb2hex(0, 0, 0), (self.width//2 + x, self.height//2 - y))
                x += 1
            return

        gr = d_y / d_x
                
        if abs(gr) < 1:

            if self.x_1 > self.x_2:
                d_x = self.x_1 - self.x_2
                d_y = self.y_1 - self.y_2
                gr = d_y / d_x
                x = self.x_2
                y = self.y_2
                x_finish = self.x_1

            self.img_2.put(rgb2hex(0, 0, 0), (self.width//2 + x, self.height//2 - y))
            
            while x <= x_finish - 1:
                val = 1 - abs((y - int(y)))
                c = 255 - int(255 * val)
                self.img_2.put(rgb2hex(c, c, c), (self.width//2 + x, self.height//2 - int(y)))

                val = abs(y - int(y))
                c = 255 - int(255 * abs(val))
                if y > 0:
                    self.img_2.put(rgb2hex(c, c, c), (self.width//2 + x, self.height//2 - (int(y) + 1)))
                else:
                    self.img_2.put(rgb2hex(c, c, c), (self.width//2 + x, self.height//2 - (int(y) - 1)))
                    
                y += gr
                x += 1

        else:

            if self.y_1 > self.y_2:
                d_x = self.x_1 - self.x_2
                d_y = self.y_1 - self.y_2
                x = self.x_2
                y = self.y_2
                y_finish = self.y_1

            gr = d_x / d_y
            self.img_2.put(rgb2hex(0, 0, 0), (self.width//2 + x, self.height//2 - y))

            while y <= y_finish - 1:
                val = 1 - abs((x - int(x)))
                c = 255 - int(255 * val)
                self.img_2.put(rgb2hex(c, c, c), (self.width//2 + int(x), self.height//2 - y))

                val = abs(x - int(x))
                c = 255 - int(255 * abs(val))
                if x > 0:
                    self.img_2.put(rgb2hex(c, c, c), (self.width//2 + int(x) + 1, self.height//2 - y))
                else:
                    self.img_2.put(rgb2hex(c, c, c), (self.width//2 + int(x) - 1, self.height//2 - y))
                    
                x += gr
                y += 1

main_window = MainWindow(Tk())        