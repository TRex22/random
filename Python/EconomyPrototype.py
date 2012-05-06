import math
import random

test_days = input("number of days:")
test_size = input("population size:")
showsold = input("show sold lines (1/0):")
showrestock = input("show business restocks (1/0):")

class Business:
    def __init__(self,Name,ID,Sector,Requires,Outputs,Capital,Bank,UnitCost,Markup,Rent,Owner,MinStock = 0):
        self.name = Name
        l = 30 - len(Name)
        self.name += l*" "
        self.id = ID
        self.sector = Sector
        self.requires = Requires
        self.outputs = Outputs
        self.capital = Capital
        #self.min = MinDaily
        self.unitcost = UnitCost
        self.markup = Markup #1.00 = 100%, 0.15 = 15%, etc
        self.bank = Bank
        self.minstock = MinStock
        self.rent = Rent
        self.owner = Owner
        self.supplier = []
        self.loga = 0
        self.logb = 0
        

    def Purchase(self,Units):
        if self.sector == 3 or self.sector == 4:
            return -1 #if it is a service we dont care, if it is a residence we dont care
        sale_cost = Units * self.unitcost
        capital_sold = Units / (1.0 + self.markup)
        while (self.capital < capital_sold*self.unitcost):
        	arg = self.Restock()
        	if (showrestock == 1):
        		print "\t\tRESTOCK\t\t",self.name,"\t",arg,"\t",self.capital
        if (showsold == 1):
        	print "\t\t\tSOLD:\t\t",self.name,"\t",self.capital,"\t",capital_sold*self.unitcost
        if (self.capital >= capital_sold*self.unitcost):
        	self.capital -= capital_sold*self.unitcost
        	self.bank += sale_cost
        	self.loga += sale_cost
        	return sale_cost
        return -1 #if you cannot make this sale because you are out of stock then you cannot sell it

    def Service(self,Units):
        if self.sector != 3:
            return -1
        self.bank += self.unitcost * Units
        return 0
        

    def StockTake(self):
        if self.sector == 3 or self.sector == 4:
            return -1 #if it is a service/residence we dont care
        units_left = self.capital / (self.unitcost / (1.0 + self.markup))
        if (units_left < self.minstock):
            return self.minstock - units_left
        return -1

    def Rent(self):
        self.bank -= self.rent
        self.logb += self.rent
        
        #tax
        #self.bank = self.bank * 0.975
        
        return self.rent
        
    def Restock(self):
		if not self.sector in [3,4] and len(self.supplier) > 0:
			sup = self.supplier[random.randint(0,len(self.supplier)-1)]
			am = sup.Purchase(500) #get 100 units
			self.bank -= am
			self.logb += am
			self.capital += am
			return am
		else:
			self.capital += 1000*self.unitcost
			return 1000*self.unitcost
		return -1
    
    def PrintLog(self):
    	print "--SUMMARY--\t",self.name,self.id,"\t\tIN:",self.loga,"\tOUT:",self.logb,"\tCapital:",self.capital,"\tBank:",self.bank
    	self.loga = 0
    	self.logb = 0
    	
    def Event(self):
    	if (random.randint(0,1000) > 999):
    		tmp = random.randint(0,1)
    		if tmp == 0:
    			am = random.randint(1,3)*30000
    			print "--EVENT--\t\t",self.name,self.id,"got a Government subsidy for:",am
    			self.bank += am
    		elif tmp == 1:
    			self.bank = self.bank * 0.75
    			print "--EVENT--\t\t",self.name,"had a tax audit and lost 25% bank balance"

resources = [
    "wood",
    "plastic",
    "produce",
    "steel",
    "furniture",
    "packaging",
    "groceries",
    "tools"
    ]
consumers = [
	"wood",
	"plastic",
	"produce",
	"steel"
	]
businesses = []
#init all businesses here

def add_b_w(b,i):
	b.append(Business("Woodcutter",i,1,[],["wood"],75000.0+random.randint(-50000,50000),50000.0+random.randint(-25000,25000),50.0+random.randint(-10,75),0.0,15000.0+random.randint(0,25000),0))
def add_b_p(b,i):
	b.append(Business("Plastics Maker",i,1,[],["plastic"],75000.0+random.randint(-50000,50000),50000.0+random.randint(-25000,25000),50.0+random.randint(-10,75),0.0,15000.0+random.randint(0,25000),0))
def add_b_pr(b,i):
	b.append(Business("Farmland",i,1,[],["produce"],75000.0+random.randint(-50000,50000),50000.0+random.randint(-25000,25000),50.0+random.randint(-10,75),0.0,15000.0+random.randint(0,25000),0))
def add_b_s(b,i):
	b.append(Business("Forge",i,1,[],["steel"],75000.0+random.randint(-50000,50000),50000.0+random.randint(-25000,25000),50.0+random.randint(-10,75),0.0,15000.0+random.randint(0,25000),0))
def add_b_w_f(b,i):
	b.append(Business("Furniture Store",i,2,["wood"],["furniture"],75000.0+random.randint(-50000,50000),50000.0+random.randint(-25000,25000),70.0+random.randint(-10,75),0.0,15000.0+random.randint(0,25000),100))
def add_b_p_p(b,i):
	b.append(Business("Packaging Manu",i,2,["plastic"],["packaging"],75000.0+random.randint(-50000,50000),50000.0+random.randint(-25000,25000),70.0+random.randint(-10,75),0.0,15000.0+random.randint(0,25000),100))
def add_b_pr_g(b,i):
	b.append(Business("Grocery Store",i,2,["produce"],["groceries"],75000.0+random.randint(-50000,50000),50000.0+random.randint(-25000,25000),70.0+random.randint(-10,75),0.0,15000.0+random.randint(0,25000),100))
def add_b_s_t(b,i):
	b.append(Business("Tools Manu",i,2,["steel"],["tools"],75000.0+random.randint(-50000,50000),50000.0+random.randint(-25000,25000),70.0+random.randint(-10,75),0.0,15000.0+random.randint(0,25000),100))

add_b_w(businesses,0)
add_b_p(businesses,1)
add_b_pr(businesses,2)
add_b_s(businesses,3)
add_b_w_f(businesses,4)
add_b_p_p(businesses,5)
add_b_pr_g(businesses,6)
add_b_s_t(businesses,7)

for i in range(test_size/5):
	q = random.randint(0,7)
	if q == 0:
		add_b_w(businesses,i+8)
	elif q == 1:
		add_b_p(businesses,i+8)
	elif q == 2:
		add_b_pr(businesses,i+8)
	elif q == 3:
		add_b_s(businesses,i+8)
	elif q == 4:
		add_b_w_f(businesses,i+8)
	elif q == 5:
		add_b_p_p(businesses,i+8)
	elif q == 6:
		add_b_pr_g(businesses,i+8)
	elif q == 7:
		add_b_s_t(businesses,i+8)
		

#create a graph to link all the dependencies!
for bus in businesses:
	for req in bus.requires:
		for bus2 in businesses:
			if req in bus2.outputs:
				bus.supplier.append(bus2)

#supply demand curve
sd_curve = {}
sd_curve["wood"] = 50.0
sd_curve["plastic"] = 50.0
sd_curve["produce"] = 50.0
sd_curve["steel"] = 50.0
sd_curve["furniture"] = 50.0
sd_curve["packaging"] = 50.0
sd_curve["groceries"] = 50.0
sd_curve["tools"] = 50.0

#make a list to go with our supply demand system
sd_list = {}
for bus in businesses:
	if not bus.outputs[0] in sd_list.keys():
		sd_list[bus.outputs[0]] = []
	sd_list[bus.outputs[0]].append(bus)

def update_curve(c):
	for k in c.keys():
		if (c[k] <= 25.0):
			c[k] += random.random()*15
		elif (c[k] >= 75.0):
			c[k] -= random.random()*15
		else:
			c[k] += random.random()*30 - 15
	return c

def print_dict(d):
	s = ""
	for k in d.keys():
		if (len(s) > 0):
			s += " "
		s += str(k) + ":" + str(d[k])
	print s

mcount = 1
flag = 0
for b in businesses:
	b.PrintLog()

for i in range(test_days):
	print "--DAY--\t\t",i,"\t\t--DAY--"
	sd_curve = update_curve(sd_curve)
	mcount += 1
	#update the businesses 
	for bus in businesses:
		bus.Event()
		if (mcount == 30):
			bus.Rent()
			am = bus.StockTake()
			if (bus.bank <= 0):
				flag += 1
				print "--------------NOTICE-------------\t\t",bus.name,"\t",bus.id,"\t","went bankrupt on day",i
				bus.bank += 50000.0
			bus.PrintLog()
		for j in sd_curve.keys():
			if not j in consumers:
				ptot = random.randint(1,int((sd_curve[j]/100)*test_size))/3
				for k in range(ptot):
					nam = random.randint(1,3)
					nb = sd_list[j][random.randint(0,len(sd_list[j])-1)]
					nb.Purchase(nam)
	if (mcount == 30):
		mcount = 1
	#print_dict(sd_curve)
print "--Bankrupt count--\t\t",flag,"\t\t--Bankrupt count--"

tcash = 0
tcap = 0
for b in businesses:
	b.PrintLog()
	tcash += b.bank
	tcap += b.capital
print "----WORLD BANK----\t\t",tcash
print "----WORLD CAPITAL----\t\t",tcap
