a = input()

def intersects(a1,a2):
	c = 0
	for ele in a1:
		if ele in a2:
			c += 1
	return c

for RAWR in range(a):
	sizes = {}
	g = []
	count = 0
	lastsize = 0
	first = True
	num = input()
	for RAWR2 in range(num):
		aa = raw_input().split()
		bb = int(aa[0])
		lastit = 0
		if (bb > lastit-(num/10)) or first:
			if bb in sizes.keys():
				g[sizes[bb]].append(aa[1:])
			else:
				sizes[bb] = len(g)
				g.append([])
				g[sizes[bb]].append(aa[1:])
			first = False
			lastit = bb
	
	k = sizes.keys()
	k.sort()
	kk = k.pop()
	aaa = g[sizes[kk]]
	first = True
	while ((len(k) > 0) or first):
		bbb = len(aaa)
		for i in range(bbb-1):
			for j in range(i+1,bbb):
				c = intersects(aaa[i],aaa[j])
				if (c > count):
					count = c

		if (len(k) > 0):
			arp = len(k)-1
			for i in range(arp):
				if (k[arp-i] < count):
					k.remove(k[arp-i])
			jj = k.pop()
			hh = g[sizes[jj]]
			for i in hh:
				aaa.append(i)
		first = False
		

	print "Case #"+str(RAWR+1)+":",count
