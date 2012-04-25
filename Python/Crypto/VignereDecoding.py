import operator

d = [" ","A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z"]

vsquare = []

for i in range(len(d)):
    s = []
    pos = 0
    for j in range(len(d)):
        pos = j + i
        if pos > len(d) - 1:
            pos = pos - len(d)
        s.append(d[pos])
    vsquare.append(s)

def getline(c):
    global vsquare
    for i in range(len(vsquare)):
        print ""



pt = raw_input()
key = raw_input()

s = ""

k = 0
for char in pt:
    pos = (operator.indexOf(d,char) - operator.indexOf(d,key[k])) % 27
    k += 1
    k = k % len(key)
    s += d[pos]

print s
