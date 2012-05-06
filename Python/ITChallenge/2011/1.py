a = input()

for i in range(a):
	b = input()
	count = 0
	while b != 1:
		count += 1
		if (b % 2 != 0):
			b = 3*b + 1
		else:
			b = b/2
	print "Case #"+str(i+1)+":",count
