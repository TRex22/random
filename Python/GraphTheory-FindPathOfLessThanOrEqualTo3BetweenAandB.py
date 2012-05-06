#recursive check for a path between A and B of length <= 3
#Ernest Loveland
import copy

#Get the list of vertices you can go to from a given vertice
#returns blank if we for some arb reason ask for a vertice's list that
#   doesnt exist
def findlist(G,V):
    G1 = copy.deepcopy(G)
    if (G1[0][0] == V):
        return G1[0][1]
    elif len(G1) > 0:
        return findlist(G[1:],V)
    else:
        return []

#begin the recursive search by getting the places you can go to from the A
#   vertice, returns the final result
def search(G, A, B):
    L = findlist(G,A)
    return gothrough(G, L, B, 2) #the 2 is what we want the maximum length to
                                 #   be less 1 (so for l <= 3 we use 2)

#go through each vertex in the list of possible paths, if C == 0 we have
#   reached our max depth, so we return a false (prevents infinite loops)
#   L is the current vertice list, if the list is empty we return false as there
#   was no path. We check if the first item in the list is the B we are looking
#   for, if it is check will return True, if it isnt, we run gothrough on the
#   tail of the list.
def gothrough(G, L, B, C):
    if (C == 0):
        return False
    elif len(L) > 0:
        a = check(G, L[0], C, B)
        if (a):
            return True
        return gothrough(G,L[1:],B,C)
    else:
        return False

#takes in the current vertice we are checking, the length of our path so far
#   and the B we are looking for. If the B is in L we return true as we are
#   <= our max length (as gothrough will never call check after the maximum
#   length of path). If our B isnt in L we run gothrough with C-1 as our list
#   has gotten longer by 1, still looking for original B
def check(G, V, C, B):
    L = findlist(G,V)
    if (B in L):
        return True
    else:
        return gothrough(G, L, B, C-1)

#sample input 1: same graph as given in COMS1 Lab test graph {P,Q,R,S} but named
#   differently, has the same structure. Returns true as there is a path
#   between A and C that is length <= 3.
#G = [["A",["B"]],["B",["A","C","D"]],["C",["B","D"]],["D",["C","B"]]]
#A = "A"
#B = "C"
#sample input 2: a graph {A,B,C,D,E} where A leads to B, B to C, etc only
#   you cannot go from A to E, and each vertice only goes to the next vertice
#   (will work directed and undirected, this is just directed). returns false
#   as the length from A to E is 4 so it never gets there.
#G = [["A",["B"]],["B",["C"]],["C",["D"]],["D",["E"]],["E",[]]]
#A = "A"
#B = "E"
#sample input 3: as above but undirected:
G = [["A",["B"]],["B",["A","C"]],["C",["B","D"]],["D",["C","E"]],["E",["D"]]]
A = "A"
B = "E"

print search(G, A, B)
