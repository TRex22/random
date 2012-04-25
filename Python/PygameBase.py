print("Importing")
import sys, pygame, random
random.seed()
pygame.init()
pygame.font.init()
from pygame.locals import *

#fps counter
last = 0
count = 0
now = pygame.time.get_ticks()

#----------------------------------------------
#screen res
size = width,height = 800,600

#----------------------------------------------
#colors
print("Setting Colours")
c_green = 0,255,0
c_red = 255,0,0
c_blue = 0,0,255
c_white = 255,255,255
c_black = 0,0,0

#----------------------------------------------
#create the screen
print("Creating window")
screen = pygame.display.set_mode(size)
pygame.display.set_caption("My Game Name")

#----------------------------------------------
print("Initialising")
#initialise anything you want here

#end of initialisation

#----------------------------------------------
#main game loop
print("Entering Main Game Loop")
cont = True
while cont:
        start_time = pygame.time.get_ticks()
        #check for someone clicking the exit button
        #exiting the loop in case you want to do stuf before exit
        for event in pygame.event.get():
                if event.type == pygame.QUIT: cont = False;
        
        screen.fill(c_black)
        # insert draw code here

        #end of draw code
        pygame.display.flip()
        pygame.time.wait(8 - int(round(pygame.time.get_ticks() - start_time)))
        #fps counter
        count += 1
        if pygame.time.get_ticks() - now > 1000.0:
                now = pygame.time.get_ticks()
                last = count
                count = 0
                print("fps:",last)
        

        
#----------------------------------------------
print("Exiting")
#any extra closing code here

#end of closing code

#----------------------------------------------
# tidy up before we exit
pygame.quit()
