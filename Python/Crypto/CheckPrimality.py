check = int(raw_input())

count = 0

for i in range(2,check/2):
    if (check/i == check/(i*1.0)):
        count += 1

if count > 0:
    print "False"
else:
    print "True"
