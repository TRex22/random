import operator

class Affine:
    def __init__(self,a,k):
        self.a = a
        self.k = k
        self.dict = [" ","A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z"]
        self.words = []

    def encode(self,message,a = 0, k = 0):
        if not (a == 0):
            self.a = a
        if not (k == 0):
            self.k = k

        nm = ""
        for letter in message:
            nm += self.dict[(operator.indexOf(self.dict,letter)*self.a + self.k) % 27]

        return nm


    def extended_euclid(self,u,v):
        u1 = 1
        u2 = 0
        u3 = u
        v1 = 0
        v2 = 1
        v3 = v

        while v3 != 0:
            q = u3/v3
            t1 = u1 - q*v1
            t2 = u2 - q*v2
            t3 = u3 - q*v3
            u1 = v1
            u2 = v2
            u3 = v3
            v1 = t1
            v2 = t2
            v3 = t3

        return u1, u2, u3

    def primes(self,q):
        ans = []
        for i in range(1,q+1):
            t = self.extended_euclid(27,i)
            if (t[2] == 1):
                ans.append(i)
        return ans

    def mi(self,a=0):
        if not (a == 0):
            self.a = a
        ans = self.extended_euclid(27,self.a)
        return ans[1]

    def decode(self,message,a = 0, k = 0):
        if not (a == 0):
            self.a = a
        if not (k == 0):
            self.k = k

        om = ""
        inv = self.mi()
        for letter in message:
            aa = inv * (operator.indexOf(self.dict,letter)-self.k)
            om += self.dict[aa % 27]

        return om

    def bruteforce(self,m):
        ans = []
        p = self.primes(27)
        for prime in p:
            for i in range(1,27+1):
                attempt = self.decode(m,prime,i)
                ans.append(attempt)
                if attempt in self.words:
                    ans.append((prime, i, attempt))
                spl = attempt.split(" ")
                for j in spl:
                    if j in self.words:
                        ans.append((prime, i, attempt))
        return ans


a = Affine(0,0)

f = open("english.txt")
lines = f.readlines()
t = []
for line in lines:
    t.append(line.strip("\n").strip("'").upper())
a.words = t
f.close()

m = str(raw_input())

print(a.bruteforce(m)[0])


