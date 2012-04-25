import copy
a = [["P",[]],["Q",["R","P"]],["R",["S"]],["S",["Q"]]]

#gv(graph) => string containing a list of the vertices
def gv(g):
    g1 = copy.deepcopy(g)
    t = ""
    if (len(g1) > 0):
        t += g1[0][0]
        g1 = g1[1:]
        return t + gv(g1)
    return ""

#ml([], vertice_string) => list containing the vertices
def ml(g,s):
    if len(s) > 0:
        g += s[0]
        return ml(g,s[1:])
    return g

#dfs(Graph, Vertice_list) => list of all paths
def dfs(g,vl):
    print("")

#path(graph,vertice,maxdepth) => path list
def path(g,v,maxdepth):
    t = []
    if (maxdepth > 1):
        maxdepth -= 1
        return t + path(g,v,maxdepth)
    return t

#is_cyclic(path_list) => true if is cyclic, otherwise false
def is_cyclic(l):
    if (len(l) > 2):
        return l[0] == l[len(l)-1]
    return False


#DEBUG: check that the vertices are listed correctly
vertices = gv(a)
vertice_list = ml([],vertices)
#
#print(vertices) ##check that the vertices are gotten correctly
#print(vertice_list) ##check that the list is created correctly
#DEBUG END

#DEBUG: check that the path creates a path of maxdepth
p1 = path(a,"A",1)
p2 = path(a,"A",2)
#
print(p1)
print(p2)
#DEBUG END

