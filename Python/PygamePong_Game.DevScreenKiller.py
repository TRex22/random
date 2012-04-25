print "Importing"
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
print "Setting Colours"
c_green = 0,255,0
c_red = 255,0,0
c_blue = 0,0,255
c_white = 255,255,255
c_black = 0,0,0

#----------------------------------------------
#create the screen
print "Creating window"
screen = pygame.display.set_mode(size,False,32)
pygame.display.set_caption("Game.Dev")

#----------------------------------------------
print "Initialising"
#initialise anything you want here
_sprite = pygame.image.load("logo.bmp")
_rect = _sprite.get_rect()
hspeed = 1
vspeed = 1
#end of initialisation

#----------------------------------------------
#main game loop
print "Entering Main Game Loop"
cont = True
while cont:
        start_time = pygame.time.get_ticks()
        #check for someone clicking the exit button
        #exiting the loop in case you want to do stuf before exit
        for event in pygame.event.get():
                if event.type == pygame.QUIT: cont = False;

        #example input
        if pygame.key.get_pressed()[pygame.K_SPACE]:
                _rect.left = 0
                print "SPACE PRESSED"
        
        screen.fill(c_black)

        #left and right
        if hspeed == 1:
                if _rect.width + hspeed + _rect.left < 800:
                        _rect.left += hspeed
                else:
                        hspeed = -1
                        _rect.left += hspeed
        else:
                if _rect.left + hspeed > 0:
                        _rect.left += hspeed
                else:
                        hspeed = 1
                        _rect.left += hspeed
        #up and down
        if vspeed == 1:
                if _rect.top + vspeed + _rect.height < 600:
                        _rect.top += vspeed
                else:
                        vspeed = -1
                        _rect.top += vspeed
        else:
                if _rect.top + vspeed > 0:
                        _rect.top += vspeed
                else:
                        vspeed = 1
                        _rect.top += vspeed
        
        # insert draw code here
        screen.blit(_sprite,_rect)
        #end of draw code
        pygame.display.flip()
        pygame.time.wait(8 - int(round(pygame.time.get_ticks() - start_time)))
        #fps counter
        count += 1
        if pygame.time.get_ticks() - now > 1000.0:
                now = pygame.time.get_ticks()
                last = count
                count = 0
                print "fps:",last
        

        
#----------------------------------------------
print "Exiting"
#any extra closing code here

#end of closing code

#----------------------------------------------
# tidy up before we exit
pygame.quit()
