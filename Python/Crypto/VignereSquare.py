a = raw_input()

ch = []
while a != "#":
    ch.append(a)
    a = raw_input()

for i in range(len(ch)):
    s = ""
    pos = 0
    for j in range(len(ch)):
        pos = j + i
        if pos > len(ch) - 1:
            pos = pos - len(ch)
        if (len(s) > 0):
            s += " "
        s += ch[pos]
    print s
        
