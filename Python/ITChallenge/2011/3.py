#a = raw_input()

num = input()

def addone():
	global t, qq
	t[qq-1] += 1
	carry = 0
	c = 0
	ss = ""
	for j in range(qq):
		t[qq-1-j] += carry
		if (t[qq-1-j] > 1):
			t[qq-1-j] = 0
			carry = 1
		else:
			carry = 0
			c += 1
		ss += str(t[qq-1-j])
	return ss

def makestring():
	s = ""
	ends = ""
	global t, b, l, qq
	for i in range(len(t)):
		if (t[i] == 1):
			s += b[i] + "\n"
			if (len(s.rstrip()) > l): #just in case the current line is too
				return ""
			ends += s
			s = ""
		else:
			s += b[i] + " "
	st = s + b[qq]
	if (len(st.rstrip()) > l):
		return ""
	else:
		ends += st
	return ends

def calcscore(s):
	global l
	score = 0
	test = s.split("\n")
	for i in range(len(test) -1 ):
		score += (l - len(test[i].strip()))**2
	return score

for jjj in range(num):
	a = raw_input()
	b = a.split(" ")
	l = int(b[0])
	b = b[1:]
	c = len(b)-1
	
	pp = 1

	t = []
	for i in range(c):
		t.append(i%2)
	qq = len(t)

	q = {}

	ls = 99999999
	lo = ""
	while not (addone() == qqq):
		out = makestring()
		if (out != ""):
			gg = calcscore(out)
			if (gg < ls):
				lo = out
				ls = gg

	print "Case #"+str(jjj+1)+":",ls
