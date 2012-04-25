import operator

class Caesar:
    def __init__(self,key = 0):
        self.key = key
        self.dict = [" ","A","B","C","D","E","F","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z"]

    def encode(self,message,key = 0):
        if (key != 0):
            self.key = key

        nm = ""
        for letter in message:
            nm += self.dict[(operator.indexOf(self.dict,letter) + self.key) % 26]
        return nm

    def decode(self,message, key = 0):
        if (key != 0):
            self.key = key

        om = ""
        for letter in message:
            om += self.dict[(operator.indexOf(self.dict,letter) - self.key) %26]

        return om

c = Caesar(3)
m = "CAKE"
a = c.encode(m)
print a
print c.decode(a)
