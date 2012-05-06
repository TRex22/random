a = input()

def iii(a1, a2):
	c = 0
	for ele in a1:
		if ele in a2:
			c += 1
	return c

for RAWR in range(a):
	g = []
	num = input()
	for RAWR2 in range(num):
		b = raw_input().split()
		b = b[1:]
		g.append(b)
	l = len(g)
	pairs = 0
	count = 0
	for i in range(l-1):
		for j in range(i+1,l):
			pairs += 1
			count += iii(g[i],g[j])
			
	print "Case #"+str(RAWR+1)+":","%.4f" % (float(count)/pairs)
