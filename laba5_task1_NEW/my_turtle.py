from random import randint
import math
import numpy as np

class my_turtle:
    def __init__(self, file, generation, width, height):
        self.turtle_result = []
        self.x, self.y = 0, 0
        self.file, self.generation, self.width, self.height = file, generation, width, height
        self.parse_file()
        self.get_generation_axiom()
        self.get_direction()
        self.draw()

    def parse_file(self):
        self.is_random = False
        self.is_color = False

        self.states_stack = []

        with open(self.file) as file_open:
            lines = file_open.readlines()

        basis = lines[0].split()
        self.atom = basis[0]

        self.angle_rotate = 0
        if basis[1] == 'random':
            self.is_random = True
        else:
            self.angle_rotate = float(basis[1])

        self.angle = int(basis[2])

        #gen = self.generation if self.generation > 0 else 1
        self.length = 30 #max(30 / gen, 5)
        self.thickness = 3
        self.color = [153, 0, 255]

        if 'color' in basis:
            self.color = [102, 51, 0]
            self.is_color = True
            self.length = 70
            self.thickness = 5
            
        self.axiom = lines[1]

        self.rules = {}
        for i in range(2, len(lines)):
            key = lines[i].split()[0]
            value = lines[i].split()[1]
            self.rules[key] = value

    def get_generation_axiom(self):
        for g in range(self.generation):
            result = ''
            for symbol in self.axiom:
                if symbol in self.rules.keys():
                    result += self.rules[symbol]
                else:
                    result += symbol
            self.axiom = result

    def get_direction(self):
        second_x, second_y = 0, 0
        
        if self.angle == 0:         # east
            second_x = self.length
        elif self.angle == 90:     # north
            second_y = -self.length
        elif self.angle == 180:    # west
            second_x = -self.length
        else:                       # south
            second_y = self.length

        self.last_second_x, self.last_second_y = 0, 0
        self.second_x, self.second_y = second_x, second_y
        self.angle = 0

    def draw(self):
        for symbol in self.axiom:
            if symbol == self.atom:
                self.forward()

            elif symbol == '-':
                if self.is_random:
                    self.angle_rotate = randint(0, 45)
                self.left()

            elif symbol == '+':
                if self.is_random:
                    self.angle_rotate = randint(0, 45)
                self.right()

            elif symbol == '[':
               self.states_stack.append([self.last_second_x, self.last_second_y, self.angle, self.length, self.color[:], self.thickness])

            elif symbol == ']':
                self.last_second_x, self.last_second_y, self.angle, self.length, self.color, self.thickness = self.states_stack.pop()

            elif symbol == '@':
                self.length -= 5
                self.color[1] += 10
                self.color[1] = min(self.color[1], 255)
                self.thickness -= 0.5
                self.thickness = max(self.thickness, 0.5)
                
            else:
                ...

    def forward(self):
        first_x, first_y = self.last_second_x, self.last_second_y
        second_x, second_y = self.second_x, self.second_y

        sin = math.sin(np.deg2rad(self.angle))
        cos = math.cos(np.deg2rad(self.angle))
        
        matrix = np.array([[cos, sin, 0], [-sin, cos, 0], [0, 0, 1]])
        coordinates = np.array([second_x, second_y, 1])
        res = coordinates.dot(matrix)

        matrix = np.array([[1, 0, 0], [0, 1, 0], [first_x, first_y, 1]])
        result = res.dot(matrix)
        second_x, second_y = result[0], result[1]

        self.turtle_result.append([first_x, first_y, second_x, second_y, self.color[:], self.thickness])
        self.last_second_x, self.last_second_y = second_x, second_y

    def left(self):
        self.angle -= self.angle_rotate

    def right(self):
        self.angle += self.angle_rotate



