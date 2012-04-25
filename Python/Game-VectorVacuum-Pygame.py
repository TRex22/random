import math
import random
import pygame
import time
import os

score = 0
if (os.path.exists("s")):
    f = open("s","r")
    l = f.readline()
    score = int(l)
    f.close()
else:
    f = open("s","w")
    f.write("0\n")
    f.close()

class Edge:
    def __init__(self):
        self.x1, self.y1 = 0.0, 0.0
        self.x2, self.y2 = 0.0, 0.0
        self.v = [self.x2 - self.x1, self.y2 - self.y1]
        self.direction = 0.0
        self.length = 0.0

    def calcdirection(self):
        if (self.v[0] != 0):
            self.direction = math.atan(self.v[1]/self.v[0])
            return 0
        self.direction = math.pi/2
        if (self.v[1] < 0):
            self.direction += math.pi
        
    def calcvector(self):
        self.v = [self.x2 - self.x1, self.y2 - self.y1]

    def calcall(self):
        self.calcvector()
        self.calcdirection()
        
    def mirror(self):
        a = Edge()
        a.x1 -= self.x1
        a.x2 -= self.x2
        a.y1 = self.y1
        a.y2 = self.y2  
        a.calcdirection()
        a.calcvector()
        return a

class Piece:
    def __init__(self,x,y):
        self.edges = []
        self.direction = random.randrange(0,13)*math.pi/6 #random movement direction
        self.x = x
        self.y = y
        self.speed = 30 + 5*random.randrange(0,16)
        self.flag = False
        e = Edge()
        e.y1 = -3
        e.y2 = random.randrange(0,5)
        dist = random.randrange(3,30)
        direction = e.direction + random.randrange(1,6)*math.pi/6
        new_e1 = Edge()
        new_e2 = Edge()
        new_e1.x1, new_e1.y1 = e.x1, e.y1
        new_e2.x1, new_e2.y1 = e.x2, e.y2
        new_x1, new_y1 = e.x1 + dist*math.cos(direction), e.y1 + dist*math.sin(direction)
        new_e1.x2, new_e1.y2 = new_x1, new_y1
        new_e2.x2, new_e2.y2 = new_x1, new_y1
        new_e1.calcall()
        new_e2.calcall()
        self.edges.append(e)
        self.edges.append(new_e1)
        self.edges.append(new_e2)

    def update(self,elapsed):
        self.x += self.speed*math.cos(self.direction)*elapsed
        self.y -= self.speed*math.sin(self.direction)*elapsed

pieces = []

def clean_pieces():
    for p in pieces:
        if (p.x < -250) or (p.x > 750) or (p.y < -250) or (p.y > 750):
            p.flag = True
    i = 0
    while i < len(pieces):
        if pieces[i].flag:
            pieces.remove(pieces[i])
        else:
            i+= 1
        

class Ship:
    def __init__(self):
        self.edges = []
        self.used = []
        self.ap = []

    #create the initial edge
    def part_a(self,length):
        e = Edge()
        e.y2 = length
        e.calcall()
        self.edges.append(e)

    #generate a new shape on a random vector "iterations" times
    def part_b(self,iterations,m):
        while iterations > 0:
            iterations -= 1
            selected_vector = self.edges[random.randrange(0,len(self.edges))] #pick a random edge
            while (selected_vector in self.used):
                selected_vector = self.edges[random.randrange(0,len(self.edges))]
            self.used.append(selected_vector)
            
            if (random.randrange(0,2,1) >= 1): #triangle or rectangle
                #triangle
                dist = random.randrange(5,m) + iterations
                direction = selected_vector.direction + random.randrange(1,6)*math.pi/6
                new_e1 = Edge()
                new_e2 = Edge()
                new_e1.x1, new_e1.y1 = selected_vector.x1, selected_vector.y1
                new_e2.x1, new_e2.y1 = selected_vector.x2, selected_vector.y2
                new_x1, new_y1 = selected_vector.x1 + dist*math.cos(direction), selected_vector.y1 + dist*math.sin(direction)
                new_e1.x2, new_e1.y2 = new_x1, new_y1
                new_e2.x2, new_e2.y2 = new_x1, new_y1
                new_e1.calcall()
                new_e2.calcall()
                self.edges.append(new_e1)
                self.edges.append(new_e2)
            else: #rectangle
                dist = random.randrange(5,m)
                direction = selected_vector.direction - math.pi/2
                new_e1 = Edge()
                new_e2 = Edge()
                new_e3 = Edge()
                new_e1.x1, new_e1.y1 = selected_vector.x1, selected_vector.y1
                new_e2.x1, new_e2.y1 = selected_vector.x2, selected_vector.y2
                new_e1.x2, new_e1.y2 = selected_vector.x1 + dist*math.cos(direction), selected_vector.y1 + dist*math.sin(direction)
                new_e2.x2, new_e2.y2 = selected_vector.x2 + dist*math.cos(direction), selected_vector.y2 + dist*math.sin(direction)
                new_e3.x1, new_e3.y1, new_e3.x2, new_e3.y2 = new_e1.x2, new_e1.y2, new_e2.x2, new_e2.y2
                new_e1.calcall()
                new_e2.calcall()
                new_e3.calcall()
                self.edges.append(new_e1)
                self.edges.append(new_e2)
                self.edges.append(new_e3)

    def part_c(self): #mirror
        for i in range(len(self.edges)):
            self.edges.append(self.edges[i].mirror())
        for i in range(len(self.ap)):
            n = -self.ap[i][0], self.ap[i][1]
            self.ap.append(n)
            

    def part_d(self): #for enemy ships, make a collision rectangle
        minx = 0
        miny = 0
        maxx = 0
        maxy = 0
        for e in self.edges:
            if e.x1 > maxx:
                maxx = e.x1
            if e.x1 < minx:
                minx = e.x1
            if e.y1 > maxy:
                maxy = e.y1
            if e.y1 < miny:
                miny = e.y1
            if e.x2 > maxx:
                maxx = e.x2
            if e.x2 < minx:
                minx = e.x2
            if e.y2 > maxy:
                maxy = e.y2
            if e.y2 < miny:
                miny = e.y2
        self.c1 = minx, miny
        self.c2 = maxx, maxy

    def part_b_a(self,iterations): #for enemy ships create attachment points
        for i in range(iterations):
            ape = self.edges[random.randrange(0,len(self.edges))]
            ap = ape.x1, ape.y1
            self.ap.append(ap)
        
            

    def p(self):
        print self.edges

def NewEnemy(boss,mini):
        global scalar
        a = 7
        b = 10
        m = 15
        if (mini):
            a -= 3
            b -= 5
            m -= 5
        if (boss):
            b += 10
            m += 80
        s = Ship()
        s.part_a(a)
        s.part_b(b,m)
        if (boss):
            s.part_b_a(random.randrange(3,5) + scalar)
        elif not (mini):
            s.part_b_a(random.randrange(1,3) + scalar)
        else:
            s.part_b_a(1 + scalar)
        s.part_c()
        s.part_d()
        return s

class Enemy:
    def __init__(self,x,y,behaviour,firerate,bullettype,ship,health = 1):
        self.x = x
        self.y = y
        self.behaviour = behaviour
        self.firerate = firerate
        self.next = firerate
        self.bullettype = bullettype
        self.s = ship
        self.health = health
        self.dir = True

    def update(self,elapsed):
        global bullets, score, acuumulator, ai
        #shooting
        self.next -= elapsed
        if (self.next <= 0):
            self.next = self.firerate
            for p in self.s.ap:
                bullets.append(Bullet(self.x + p[0],self.y + p[1],1,self.bullettype))
        i = 0
        flag = True
        while i < len(bullets):
            if (bullets[i].owner == 0):
                if (bullets[i].x < self.s.c2[0] + self.x) and (bullets[i].x > self.s.c1[0] + self.x) and (bullets[i].y < self.s.c2[1] + self.y) and (bullets[i].y > self.s.c1[1] + self.y):
                        flag = False
                        self.health -= 1
                        if not ai:
                            score += 1
                            acuumulator += 1
            if flag:
                i += 1
            else:
                bullets.remove(bullets[i])
                flag = True
        #movement
        if self.behaviour == 0: #behaviour zero is just random movement, no pattern
            if (random.randrange(0,100,1) >= 99):
                self.dir = not self.dir
            if (self.dir):
                if (self.x > 0):
                    self.x -= 45*elapsed
                else:
                    self.dir = not self.dir
            else:
                if (self.x < 500):
                    self.x += 45*elapsed
                else:
                    self.dir = not self.dir
            self.y += random.randrange(2,4,1)*0.1 #should make the ships move down at slightly different speeds
            if (self.y > 500): #delete the enemy if it moves off the bottom of the screen
                self.health = 0
        elif self.behaviour == 1:
            if (random.randrange(0,100,1) >= 99):
                self.dir = not self.dir
            if (self.dir):
                if (self.x > 0):
                    self.x -= 0.5
                else:
                    self.dir = not self.dir
            else:
                if (self.x < 500):
                    self.x += 0.5
                else:
                    self.dir = not self.dir
            if (self.y < self.s.c2[1]-self.s.c1[1]+50):
                self.y += 60*elapsed #should make the ships move down at slightly different speeds


enemies = []
def clean_enemies():
    global enemies, pp
    i = 0
    while i < len(enemies):
        for q in pieces:
            c1 = enemies[i].s.c1
            c2 = enemies[i].s.c2
            if (q.x > c1[0]+ enemies[i].x) and (q.x < c2[0] + enemies[i].x) and (q.y > c1[1] + enemies[i].y) and (q.y < c2[1] + enemies[i].y):
                enemies[i].health -= 1
                q.flag = True
        if enemies[i].health <= 0:
            a = random.randrange(0,21)
            if (a <= 14):
                pickups.append(Pickup(enemies[i].x,enemies[i].y,random.randrange(0,pp+1)))
            for q in range(random.randrange(1,len(enemies[i].s.edges))):
                pieces.append(Piece(enemies[i].x,enemies[i].y))
            enemies.remove(enemies[i])
        else:
            i += 1
        
#e = Enemy(250,0,0,0.5,0,NewEnemy(False,False),3)
#enemies.append(e)

class Bullet:
    def __init__(self,x,y,owner,behaviour,direction = 3*math.pi/2): #3*math.pi/2 = down
        self.x = x
        self.y = y
        self.ox = x
        self.oy = y
        self.owner = owner
        self.behaviour = behaviour
        self.direction = direction
        self.count = 0
        if (behaviour != 0):
            self.randomiser = random.randrange(0,2)
            if (self.randomiser == 0):
                self.randomiser = -1

    def update(self,elapsed):
        if (self.behaviour == 0): #single bullet in direction
            self.x += elapsed*180*math.cos(self.direction)
            self.y -= elapsed*180*math.sin(self.direction)
        if (self.behaviour == 1): #single bullet spiralling outwards
            self.count += elapsed
            self.x = self.ox + self.count*90*math.cos(self.count*1.5*self.randomiser)
            self.y = self.oy + self.count*90*math.sin(self.count*1.5*self.randomiser)
        if (self.behaviour == 2):
            self.count += elapsed
            self.y = self.oy - self.count*180*math.sin(self.direction)
            self.x = self.ox + math.cos(self.count*6)*10
            
bullets = []

class Pickup:
    def __init__(self,x,y,t):
        self.t = t
        self.x = x
        self.y = y
        self.flag = False
        self.l = "N"
        if (t == 0):
            self.l = "h"
        if (t == 1):
            self.l = "H"
        if (t == 2):
            self.l = "S"
        if (t == 3):
            self.l = "D"
        if (t == 4):
            self.l = "T"
        if (t == 5):
            self.l = "c"
        if (t == 6):
            self.l = "C"
        if (t == 7):
            self.l = "p"
        if (t == 8):
            self.l = "P"
        if (t == 9):
            self.l = "M"
        if (t == 10):
            self.l = "Z"

    def update(self,elapsed):
        self.y += 45*elapsed

pickups = []
p = Pickup(250,250,2)
pickups.append(p)
p = Pickup(250,260,2)
pickups.append(p)
pp = 10

def clean_pickups():
    global s_r, health, s_t, ai, s_level
    for p in pickups:
        d = math.sqrt((p.x - s_pos[0])**2 + (p.y - s_pos[1])**2)
        if (d <= s_r+10):
            if not ai:
                if p.t == 1:
                    health += 10
                if p.t == 0:
                    health += 4
            if p.t == 2:
                if (s_t == 0):
                    s_level += 1
                else:
                    s_t = 0
                    s_level = 1
            if p.t == 3:
                if (s_t == 1):
                    s_level += 1
                else:
                    s_t = 1
                    s_level = 1
            if p.t == 4:
                if (s_t == 2):
                    s_level += 1
                else:
                    s_t = 2
                    s_level = 1
            if p.t == 5:
                if (s_t == 3):
                    s_level += 1
                else:
                    s_t = 3
                    s_level = 1
            if p.t == 6:
                if (s_t == 4):
                    s_level += 1
                else:
                    s_level = 1
                    s_t = 4
            if p.t == 7:
                if (s_t == 5):
                    s_level += 1
                else:
                    s_t = 5
            if p.t == 8:
                if (s_t == 6):
                    s_level += 1
                else:
                    s_t = 6
                    s_level = 1
            if p.t == 9:
                if (s_t == 7):
                    s_level += 1
                else:
                    s_t = 7
                    s_level = 1
            if p.t == 10:
                for q in pieces:
                    if not (p.x-q.x == 0):
                        q.direction = math.atan((p.y-q.y)/(p.x-q.x))
                    q.speed += 15
            p.flag = True
    i = 0
    for p in pickups:
        if p.y > 500:
            p.flag = True
    while i < len(pickups):
        if (pickups[i].flag):
            pickups.remove(pickups[i])
        else:
            i += 1

def player_shoot(t):
    global s_pos, bullets
    if (t == 0):
        bullets.append(Bullet(s_pos[0],s_pos[1],0,0,math.pi/2))
        for i in range(s_level-1):
            bullets.append(Bullet(s_pos[0],s_pos[1],0,0,math.pi/2+((i+1)*math.pi/20)))
            bullets.append(Bullet(s_pos[0],s_pos[1],0,0,math.pi/2-((i+1)*math.pi/20)))
    if (t == 1):
        bullets.append(Bullet(s_pos[0],s_pos[1],0,0,math.pi/2+0.3))
        bullets.append(Bullet(s_pos[0],s_pos[1],0,0,math.pi/2-0.3))
        for i in range(s_level-1):
            bullets.append(Bullet(s_pos[0],s_pos[1],0,0,math.pi/2+0.3+((i+1)*math.pi/20)))
            bullets.append(Bullet(s_pos[0],s_pos[1],0,0,math.pi/2-0.3-((i+1)*math.pi/20)))
            bullets.append(Bullet(s_pos[0],s_pos[1],0,0,math.pi/2+0.3+((i+1)*math.pi/20)+math.pi))
            bullets.append(Bullet(s_pos[0],s_pos[1],0,0,math.pi/2-0.3-((i+1)*math.pi/20)+math.pi))
    if (t == 2):
        bullets.append(Bullet(s_pos[0],s_pos[1],0,0,math.pi/2))
        bullets.append(Bullet(s_pos[0],s_pos[1],0,0,math.pi/2+0.4))
        bullets.append(Bullet(s_pos[0],s_pos[1],0,0,math.pi/2-0.4))
        for i in range(s_level-1):
            bullets.append(Bullet(s_pos[0],s_pos[1],0,0,math.pi/2+((i+1)*math.pi/20)))
            bullets.append(Bullet(s_pos[0],s_pos[1],0,0,math.pi/2-((i+1)*math.pi/20)))
    if (t == 3):
        bullets.append(Bullet(s_pos[0],s_pos[1],0,1,math.pi/2))
        for i in range(s_level-1):
            bullets.append(Bullet(s_pos[0],s_pos[1],0,0,math.pi/2+((i+1)*math.pi/20)))
            bullets.append(Bullet(s_pos[0],s_pos[1],0,0,math.pi/2-((i+1)*math.pi/20)))
    if (t == 4):
        bullets.append(Bullet(s_pos[0],s_pos[1],0,1,math.pi/2))
        bullets.append(Bullet(s_pos[0],s_pos[1],0,1,math.pi/2))
        for i in range(s_level-1):
            bullets.append(Bullet(s_pos[0],s_pos[1],0,0,math.pi/2+((i+1)*math.pi/20)))
            bullets.append(Bullet(s_pos[0],s_pos[1],0,0,math.pi/2-((i+1)*math.pi/20)))
    if (t == 5):
        bullets.append(Bullet(s_pos[0],s_pos[1],0,2,math.pi/2))
        for i in range(s_level-1):
            bullets.append(Bullet(s_pos[0],s_pos[1],0,2,math.pi/2))
    if (t == 6):
        bullets.append(Bullet(s_pos[0],s_pos[1],0,2,math.pi/2+0.3))
        bullets.append(Bullet(s_pos[0],s_pos[1],0,2,math.pi/2-0.3))
        for i in range(s_level-1):
            bullets.append(Bullet(s_pos[0],s_pos[1],0,2,math.pi/2+0.3))
            bullets.append(Bullet(s_pos[0],s_pos[1],0,2,math.pi/2-0.3))
    if (t == 7):
        bullets.append(Bullet(s_pos[0],s_pos[1],0,2,math.pi/2))
        bullets.append(Bullet(s_pos[0],s_pos[1],0,0,math.pi/2+0.4))
        bullets.append(Bullet(s_pos[0],s_pos[1],0,0,math.pi/2-0.4))
        for i in range(s_level-1):
            bullets.append(Bullet(s_pos[0],s_pos[1],0,2,math.pi/2))
            bullets.append(Bullet(s_pos[0],s_pos[1],0,0,math.pi/2+((i+1)*math.pi/20)))
            bullets.append(Bullet(s_pos[0],s_pos[1],0,0,math.pi/2-((i+1)*math.pi/20)))
            

def clear_dead():
    global bullets
    i = 0
    while i < len(bullets):
        if (bullets[i].x < -250) or (bullets[i].x > 750) or (bullets[i].y < -250) or (bullets[i].y > 750):
            bullets.remove(bullets[i])
        else:
            i += 1

def newship():
    global s, score, acuumulator
    s = Ship()
    s.part_a(random.randrange(3,4))
    s.part_b(random.randrange(3,9),15)
    s.part_c()
    score -= acuumulator
    acuumulator = 0

def collision_player():
    global s, s_r, bullets, s_pos, pause, enemies, health
    i = 0
    flag = True
    flag2 = False
    while i < len(bullets):
        if bullets[i].owner == 1:
            d = math.sqrt((bullets[i].x - s_pos[0])**2 + (bullets[i].y - s_pos[1])**2)
            if (d <= s_r):
                flag = False
                flag2 = True
        if flag:
            i += 1
        else:
            flag = True
            bullets.remove(bullets[i])
    i = 0
    while i < len(enemies):
        c1 = enemies[i].s.c1
        c2 = enemies[i].s.c2
        if (s_pos[0] > c1[0]+ enemies[i].x) and (s_pos[0] < c2[0] + enemies[i].x) and (s_pos[1] > c1[1] + enemies[i].y) and (s_pos[1] < c2[1] + enemies[i].y):
            bbb = e.health
            e.health = 0
            health -= bbb
            flag2 = True
        i += 1
    return flag2
                
s = Ship()
s.part_a(random.randrange(3,4))
s.part_b(random.randrange(3,9),15)
s.part_c()
s_pos = [250,250+125]
s_r = 7 #25 pixel collision radius
s_t = 0
s_next = 0.2
s_level = 1
#s.p()


#b = Bullet(250,250,0,1)
#bullets.append(b)

pygame.init()
size = 500,500
screen = pygame.display.set_mode(size)

spawn_time = 5
spawn_counter = 5

gfont = pygame.font.Font("SECRCODE.TTF",24)

bb = 2

run = True
pause = False
pause_time = -1
start_time = 0
end_time = 0
ai = False
ai_flag = True
elapsed = 0
start_time = time.time()
health = 20
#score = 0
spawn_delay = 1
acuumulator = 0
scalar = 0
while run:
    end_time = start_time
    start_time = time.time()
    elapsed = start_time - end_time
    #game logic
    keys = pygame.key.get_pressed()
    if not (pause): #only update if we arent paused
        s_next -= elapsed
        if (s_next <= 0):
            player_shoot(s_t)
            s_next = 0.2

        spawn_delay -= elapsed

        if len(enemies) < 1:
            spawn_time = 0

        #spawn_time = 1 #debug to stop spawning
        if spawn_time > 0:
            spawn_time -= elapsed
        elif len(enemies) == 0:
            scalar = score / 250
            spawn_time = 9
            spawn_counter -= 1
            if (spawn_counter == 0):
                e = NewEnemy(False,True)
                b = Enemy(250,0,1,0.3,random.randrange(0,bb),NewEnemy(True,False),100 + scalar*50)
                c = Enemy(125,0,1,1,random.randrange(0,bb+1),e,2)
                d = Enemy(375,0,1,1,random.randrange(0,bb+1),e,2)
                enemies.append(b)
                enemies.append(c)
                enemies.append(d)
                spawn_counter = 5
            else:
                if (spawn_counter == 4):
                    acuumulator = 0
                em = False
                if (random.randrange(1,2) == 2):
                    em = True
                ecount = random.randrange(1,6)
                ehealth = 15 - ecount
                eship = NewEnemy(False,em)
                for i in range(ecount):
                    ee = Enemy(random.randrange(1,499),0,1,0.5,random.randrange(0,bb+1),eship,ehealth + scalar*10)
                    enemies.append(ee)
                    
        if not ai:
            if keys[pygame.K_LEFT]:
                if s_pos[0] > 0:
                    s_pos[0] -= 250*elapsed
            if keys[pygame.K_RIGHT]:
                if s_pos[0] < 500:
                    s_pos[0] += 250*elapsed
            if keys[pygame.K_UP]:
                if s_pos[1] > 0:
                    s_pos[1] -= 250*elapsed
            if keys[pygame.K_DOWN]:
                if s_pos[1] < 500:
                    s_pos[1] += 250*elapsed
            if keys[pygame.K_z]:
                if (spawn_delay <= 0):
                    b = Enemy(250,0,1,0.7,0,NewEnemy(True,False),100)
                    enemies.append(b)
                    spawn_delay = 1
            
        else:
            #ai
            if (len(pickups) == 0):
                if s_pos[1] < 400:
                    s_pos[1] += 90*elapsed
                elif s_pos[1] > 450:
                    s_pos[1] -= 90*elapsed
                if ai_flag:
                    if (s_pos[0] > 25):
                        s_pos[0] -= 90*elapsed
                    else:
                        ai_flag = not ai_flag
                else:
                    if (s_pos[0] < 475):
                        s_pos[0] += 45*elapsed
                    else:
                        ai_flag = not ai_flag
            else: #chase the pickup
                if s_pos[0] > pickups[0].x:
                    s_pos[0] -= 90*elapsed
                elif s_pos[0] < pickups[0].x:
                    s_pos[0] += 90*elapsed
                if s_pos[1] > pickups[0].y:
                    s_pos[1] -= 90*elapsed
                elif s_pos[1] < pickups[0].y:
                    s_pos[1] += 90*elapsed
                    
                
            if keys[pygame.K_SPACE]:
                ai = False #let a player drop in whenever they want

        if (len(bullets) > 0):
            for b in bullets:
                b.update(elapsed)
        if (len(enemies) > 0):
            for e in enemies:
                e.update(elapsed)
        if (len(pickups) > 0):
            for p in pickups:
                p.update(elapsed)
        if (len(pieces) > 0):
            for p in pieces:
                p.update(elapsed)
        clean_pickups()
        clean_enemies()
        clean_pieces()
        clear_dead()
    else:
        if keys[pygame.K_SPACE]:
            ai = False
            pause = False
            pause_time = -1 #continue play as the next player is ready
        

    if (collision_player()):
        if not (ai):
            health -= 1
        if (health <= 0):
            pause = True
            pause_time = 3.5 #pause for 7 seconds
            ai = True #go to ai if a player doesnt drop in
            health = 20
            newship()
            spawn_counter = 5 #reset spawn count, score will be added on boss levels

    if (pause_time > 0) and pause:
        pause_time -= elapsed
    elif pause:
        pause = False
        pause_time = -1

    #drawing
    screen.fill((0,0,0))

    #draw our ship
    for e in s.edges:
        pygame.draw.aaline(screen,(0,0,255),(s_pos[0] + e.x1, s_pos[1] + e.y1),(s_pos[0] + e.x2, s_pos[1] + e.y2))

    #draw bullets
    if (len(bullets) > 0):
        for b in bullets:
            if (b.owner == 0):
                bc = (0,0,255)
            else:
                bc = (0,255,0)
            pygame.draw.circle(screen,bc,(b.x,b.y),3)

    #draw enemies
    if (len(enemies) > 0):
        for e in enemies:
            for edge in e.s.edges:
                pygame.draw.aaline(screen,(255,0,0),(e.x + edge.x1, e.y + edge.y1),(e.x + edge.x2, e.y + edge.y2))
            sss = str(e.health)
            if (len(sss) > 0):
                surface = gfont.render(sss,True,(255,255,0))
                surface_rect = surface.get_rect()
                new_rect = pygame.Rect(e.x-surface_rect.width/2,e.y-e.s.c2[1]-surface_rect.height,surface_rect.width, surface_rect.height)
                screen.blit(surface,new_rect)

    #draw pieces
    if (len(pieces) > 0):
        for p in pieces:
            for edge in p.edges:
                pygame.draw.aaline(screen,(255,0,0),(p.x + edge.x1, p.y + edge.y1),(p.x + edge.x2, p.y + edge.y2))

    #draw pickups
    if (len(pickups) > 0):
        for p in pickups:
            surface = gfont.render(p.l,True,(255,255,0))
            surface_rect = surface.get_rect()
            new_rect = pygame.Rect(p.x-surface_rect.width/2,p.y-surface_rect.height/2,surface_rect.width, surface_rect.height)
            screen.blit(surface,new_rect)

    surface = gfont.render("SCORE: " + str(score),True,(255,255,0))
    surface_rect = surface.get_rect()
    screen.blit(surface,surface_rect)
    surface = gfont.render(str(health),True,(255,255,0))
    surface_rect = surface.get_rect()
    new_rect = pygame.Rect(s_pos[0]-surface_rect.width/2,s_pos[1]+15,surface_rect.width, surface_rect.height)
    screen.blit(surface,new_rect)

    if (pause):
        surface = gfont.render("DEATH IN VACUUM! <spacebar> TO PLAY",True,(255,255,0))
        surface_rect = surface.get_rect()
        new_rect = pygame.Rect(250-surface_rect.width/2,500-surface_rect.height,surface_rect.width, surface_rect.height)
        screen.blit(surface,new_rect)
    elif (ai):
        surface = gfont.render("AUTOPILOT ENGAGED! <spacebar> TO PLAY",True,(255,255,0))
        surface_rect = surface.get_rect()
        new_rect = pygame.Rect(250-surface_rect.width/2,500-surface_rect.height,surface_rect.width, surface_rect.height)
        screen.blit(surface,new_rect)
    
    pygame.display.flip()
    for event in pygame.event.get():
        if event.type == pygame.QUIT:
            run = False

os.remove("s")
f = open("s","w")
f.write(str(score))
f.write("\n")

pygame.quit()
