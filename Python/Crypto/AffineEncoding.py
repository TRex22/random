import operator

class Affine:
	def __init__(self,a,k):
		self.a = a
		self.k = k
		self.dict = [" ","A","B","C","D","E","F","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","V","W","X","Y","Z"]
		self.words = []

	def encode(self,message,a = 0, k = 0):
		if not (a == 0):
			self.a = a
		if not (k == 0):
			self.k = k

		nm = ""
		for letter in message:
			nm += self.dict[(operator.indexOf(self.dict,letter)*self.a + self.k) % 26]

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
			t = self.extended_euclid(26,i)
			if (t[2] == 1):
				ans.append(i)
		return ans

	def mi(self,a=0):
		if not (a == 0):
			self.a = a
		ans = self.extended_euclid(26,self.a)
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
			om += self.dict[aa % 26]

		return om

	def bruteforce(self,m):
		p = self.primes(26)
		for prime in p:
			for i in range(0,26+1):
				attempt = self.decode(m,prime,i)
				if attempt in self.words:
					return prime, i, attempt
		return 0, 0, m

a = Affine(0,0)
i = int(raw_input())
p = a.primes(i)
print len(p)
