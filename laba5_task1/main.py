import turtle
from random import randint

# user's section

generation = 5

#file = 'koch_curve.txt'
#file = 'sierpinski_triangle.txt'
#file = 'gosper_curve.txt'
#file = 'dragon_curve.txt'
file = 'tree.txt'
#file = 'tree_color.txt'

# file parsing

is_random = False
is_color = False

states_stack = []

with open(file) as file_open:
    lines = file_open.readlines()

basis = lines[0].split()

atom = basis[0]

angle = 0
if basis[1] == 'random':
    is_random = True
else:
    angle = float(basis[1])

direction = int(basis[2])

gen = generation if generation > 0 else 1
length = max(30 / gen, 5)
thickness = 3
color = [0.6, 0.0, 1.0]

if 'color' in basis:
    color = [0.4, 0.2, 0.0]
    is_color = True
    length = 70
    thickness = 5
    
axiom = lines[1]

rules = {}
for i in range(2, len(lines)):
    key = lines[i].split()[0]
    value = lines[i].split()[1]
    rules[key] = value

# window setup

width, height = 800, 600
window = turtle.Screen()
window.setup(width, height)
window.screensize(gen * width, gen * height)
window.bgcolor('white')
window.delay(0)

start_x, start_y = 0, 0
 
if direction == 0: # east
    start_x = -width//2 + 10
elif direction == 90: # north
    start_y = -height//2 + 10
elif direction == 180: # west
    start_x = width//2 - 10
else: # south
    start_y = height//2 - 10

# brush setup    
brush = turtle.Turtle()

brush.speed(0)
brush.setheading(direction)
brush.penup()

brush.goto(start_x, start_y)
brush.pendown()

# functions

def get_axiom_gen(axiom, generation):
    axiom = axiom
    for g in range(generation):
        result = ''
        for symbol in axiom:
            if symbol in rules.keys():
                result += rules[symbol]
            else:
                result += symbol
        axiom = result
    return axiom

def draw(axiom):
    global length, color, thickness, angle

    for symbol in axiom:
        brush.color(color)
        brush.pensize(thickness)

        if symbol == atom:
            brush.forward(length)

        elif symbol == '-':
            if is_random:
                angle = randint(0, 45)
            brush.left(angle)

        elif symbol == '+':
            if is_random:
                angle = randint(0, 45)
            brush.right(angle)

        elif symbol == '[':
            states_stack.append([brush.position(), brush.heading(), length, color[:], thickness])

        elif symbol == ']':
            p, h, length, color, thickness = states_stack.pop()
            brush.penup()
            brush.setposition(p)
            brush.setheading(h)
            brush.pendown()

        elif symbol == '@':
            length -= 5
            color[1] += 0.05
            color[1] = min(color[1], 1.0)
            thickness -= 0.5
            thickness = max(thickness, 0.5)
            
        else:
            ...

#drawing

axiom = get_axiom_gen(axiom, generation)
draw(axiom)
turtle.Screen().exitonclick()
