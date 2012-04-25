def enc(inp, key, count):
	ans = ''
	for i in inp:
		new = ((ord(i) - ord('A')) + int(key) + 1) % int(count)
		ans = ans + chr(new + ord('A') -1 )
	return ans

inp = raw_input()
l = []
count = 0
while inp != '#':
	l += inp.upper()
	count += 1
	inp = raw_input()
v = []
for i in range(count):
	j = l
	v.append(j)
	l = []
	for i in range(count):
		l.append(enc(j[i], 1, count))

a = ''
for i in range(len(v)):
	p = ''
	for j in range(count):
		if v[i][j] == '@':
			a = ' '
			p += a
		else: p += v[i][j]
	print p

