import operator

class Caesar:
    def __init__(self,key = 0):
        self.key = key
        self.dict = [" ","A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z"]
        self.words = []

    def encode(self,message,key = 0):
        if (key != 0):
            self.key = key

        nm = ""
        for letter in message:
            nm += self.dict[(operator.indexOf(self.dict,letter) + self.key) % 27]
        return nm

    def decode(self,message, key = 0):
        if (key != 0):
            self.key = key

        om = ""
        for letter in message:
            om += self.dict[(operator.indexOf(self.dict,letter) - self.key) %27]

        return om

    def bruteforce(self,m):
        for i in range(0,27):
            attempt = self.decode(m,i)
            if attempt in self.words:
                return attempt
        return m

c = Caesar(0)

f = open("english.txt")
lines = f.readlines()
t = []
for line in lines:
    t.append(line.strip("\n").strip("'").upper())
c.words = t
f.close()

m = str(raw_input())

print(c.bruteforce(m))


