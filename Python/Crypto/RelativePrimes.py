def extended_euclid(u,v):
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

def primes(q):
        ans = []
        for i in range(1,q+1):
                t = extended_euclid(27,i)
                if (t[2] == 1):
                        ans.append(i)
        return ans

def mi(a,b):
    ans = extended_euclid(a,b)
    return ans[1]

aa = int(raw_input())

pri = primes(aa)
for p in pri:
    print p
