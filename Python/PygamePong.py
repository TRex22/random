import sys, pygame, random
random.seed()
pygame.init()
pygame.font.init()

#collision check
def collide_2D(rect1,rect2):
        return 

size = width,height = 800,600
#colors
c_green = 0,255,0
c_red = 255,0,0
c_blue = 0,0,255
c_white = 255,255,255
c_black = 0,0,0

screen = pygame.display.set_mode(size)
cont = True

pygame.display.set_caption("Example 1 - pong")

ball_sprite = pygame.image.load("Ball.bmp")
paddle1_sprite = pygame.image.load("Player1.bmp")
paddle2_sprite = pygame.image.load("Player2.bmp")

ball_rect = ball_sprite.get_rect()
paddle1_rect = paddle1_sprite.get_rect()
paddle2_rect = paddle2_sprite.get_rect()

hspeed = 1
vspeed = 1

#player1 keys
A_pressed = False
D_pressed = False
#player2 keys
LEFT_pressed = False
RIGHT_pressed = False

while cont:
        #handle events (eg. program quit)
        for event in pygame.event.get():
                if event.type == pygame.QUIT: cont = False;

        #handle keyboard input
        for key in pygame.key.get_pressed():
                if key == K_a:
                        A_pressed = True
                else:
                        A_pressed = False
                if key == K_d:
                        D_pressed = True
                else:
                        D_pressed = False
                if key == K_LEFT:
                        LEFT_pressed = True
                else:
                        LEFT_pressed = False
                if key == K_RIGHT:
                        RIGHT_pressed = True
                else:
                        RIGHT_pressed = False
                


        #control code
        #player1
        


        #automatic ball code
        #left and right
        if hspeed == 1:
                if ball_rect.width + hspeed + ball_rect.left < 800:
                        ball_rect.left += hspeed
                else:
                        hspeed = -1
                        ball_rect.left += hspeed
        else:
                if ball_rect.left + hspeed > 0:
                        ball_rect.left += hspeed
                else:
                        hspeed = 1
                        ball_rect.left += hspeed
        #up and down
        if vspeed == 1:
                if ball_rect.top + vspeed + ball_rect.height < 600:
                        ball_rect.top += vspeed
                else:
                        vspeed = -1
                        ball_rect.top += vspeed
        else:
                if ball_rect.top + vspeed > 0:
                        ball_rect.top += vspeed
                else:
                        vspeed = 1
                        ball_rect.top += vspeed

        #draw code
        screen.fill(c_black)
        screen.blit(ball_sprite,ball_rect)
        screen.blit(paddle1_sprite,paddle1_rect)
        screen.blit(paddle2_sprite,paddle2_rect)
        pygame.display.flip()


pygame.quit()
