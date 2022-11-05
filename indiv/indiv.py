from delauney import Graph, Point, Edge, Triangle
import random
import sys
import pygame

graph = Graph()


pygame.init()
screen = pygame.display.set_mode([1024, 768])
screen.fill((0, 0, 0))
pygame.display.set_caption('Триангуляция Делоне')
def add_point():
    while graph.addPoint(Point(x, y)) is False:
        print("Невозможно добавить точку")
    pygame.draw.circle(screen, (255, 255, 255), (x, y), 3)



    pygame.display.update()



def delaunay():

    graph.generateDelaunayMesh()
    for p in graph._points:
       pygame.draw.circle(screen, (255, 255, 255), p.pos(), 3)

    for e in graph._edges:
        pygame.draw.line(screen, (0, 255, 0), e._a.pos(), e._b.pos())

    pygame.display.update()


while True:
    events = pygame.event.get()
    for e in events:
        if e.type == pygame.QUIT:
            sys.exit()
        if e.type == pygame.MOUSEBUTTONDOWN:
            x, y = e.pos
            print(x, y)
            add_point()
            # Change the current color of the input box.
        if e.type == pygame.KEYDOWN:
            delaunay()




