from tkinter import *
from sympy import Lambda, symbols
from colormap import rgb2hex

class MainWindow:
    def __init__(self, window):
        self.root = window
        self.root.title('Triangle Rasterization')
        self.root.resizable(width=False, height=False)

        self.width = 400 
        self.height = 400 

        canv = Canvas(self.root, width = self.width, height = self.height, bg = "white")
        self.img = PhotoImage(width=self.width, height=self.height)
        canv.create_image((self.width / 2, self.height / 2), image=self.img, state="normal")

        self.draw_triangle()
        canv.pack()
        self.root.mainloop()

    def draw_triangle(self):
        # в пределах 200, -200
        point_1 = [-140, 145]
        point_2 = [150, 147]
        point_3 = [-140, 14]
        
        color_1 = [255, 102, 0] # оранжевый
        color_2 = [204, 0, 204] # фиолетовый
        color_3 = [0, 255, 204] # бирюзовый

        # сортируем по убыванию y
        if (point_1[1] < point_2[1]):
            point_1, point_2 = point_2, point_1
            color_1, color_2 = color_2, color_1
        if (point_2[1] < point_3[1]):
            point_2, point_3 = point_3, point_2
            color_2, color_3 = color_3, color_2
        if (point_1[1] < point_3[1]):
            point_1, point_3 = point_3, point_1
            color_1, color_3 = color_3, color_1

        x_1, y_1 = point_1
        x_2, y_2 = point_2
        x_3, y_3 = point_3

        y = symbols('y')

        k_1 = (y_1 - y_2) / (x_1 - x_2)
        b_1 = y_2 - k_1 * x_2
        eq_1 = Lambda((y), (y - b_1) / k_1)
        
        if x_1 == x_3:
            eq_2 = Lambda((y), x_1)
            k_2 = 0
        else:
            k_2 = (y_1 - y_3) / (x_1 - x_3)
            b_2 = y_3 - k_2 * x_3
            eq_2 = Lambda((y), (y - b_2) / k_2)
        
        k_3 = (y_2 - y_3) / (x_2 - x_3)
        b_3 = y_3 - k_3 * x_3
        eq_3 = Lambda((y), (y - b_3) / k_3)

        y_cur = y_1
        x_side_1 = x_side_2 = 0

        while y_cur >= y_3:
            if y_cur >= y_2:
                if y_cur == y_1 and k_1 == 0: # p1, p2 - прямая
                    x_side_1 = x_1
                    x_side_2 = x_2
                    color_side_1 = color_1
                    color_side_2 = color_2
                
                else:
                    x_side_1 = eq_1(y_cur)
                    x_side_2 = eq_2(y_cur)
                    
                    if y_cur == y_1 and y_1 != y_2: # точка - вершина p1
                        color_side_1 = color_side_2 = color_1
                    elif y_cur == y_2 and y_2 != y_1: # точка - вершина p2
                        color_side_1 = color_2
                        color_side_2 = self.calculate_color(y_1, y_3, y_cur, color_1, color_3)
                    else:
                        color_side_1 = self.calculate_color(y_1, y_2, y_cur, color_1, color_2)
                        color_side_2 = self.calculate_color(y_1, y_3, y_cur, color_1, color_3)
                    
            else:
                if y_cur == y_3 and k_3 == 0: # p2, p3 - прямая
                    x_side_1 = x_2
                    x_side_2 = x_3
                    color_side_1 = color_2
                    color_side_2 = color_3

                else:
                    x_side_1 = eq_2(y_cur)
                    x_side_2 = eq_3(y_cur)

                    if y_cur == y_3 and y_3 != y_2: # точка - вершина p3
                        color_side_1 = color_side_2 = color_3
                    else:
                        color_side_1 = self.calculate_color(y_1, y_3, y_cur, color_1, color_3)
                        color_side_2 = self.calculate_color(y_2, y_3, y_cur, color_2, color_3)

            # сортируем по возрастанию x 
            if x_side_1 > x_side_2:
                x_side_1, x_side_2 = x_side_2, x_side_1
                color_side_1, color_side_2 = color_side_2, color_side_1

            x_cur = int(x_side_1)

            while x_cur <= x_side_2:
                r, g, b = self.calculate_color(x_side_1, x_side_2, x_cur, color_side_1, color_side_2)
                self.img.put(rgb2hex(r, g, b), (self.width//2 + x_cur, self.height//2 - y_cur))
                x_cur += 1
            y_cur -= 1

    def calculate_color(self, point_1, point_2, point_cur, color_1, color_2):
        if point_1 == point_2:
            return color_1
        d = abs(point_1 - point_cur) / abs(point_1 - point_2)
        r_1, g_1, b_1 = color_1 
        r_2, g_2, b_2 = color_2

        r_new = g_new = b_new = 0

        if r_1 < r_2:
            r_new = r_1 + int(d * abs(r_1 - r_2))
        else:
            r_new = r_1 - int(d * abs(r_1 - r_2))

        if g_1 < g_2:
            g_new = g_1 + int(d * abs(g_1 - g_2))
        else:
            g_new = g_1 - int(d * abs(g_1 - g_2))

        if b_1 < b_2:
            b_new = b_1 + int(d * abs(b_1 - b_2))
        else:
            b_new = b_1 - int(d * abs(b_1 - b_2))

        return r_new, g_new, b_new

main_window = MainWindow(Tk())        