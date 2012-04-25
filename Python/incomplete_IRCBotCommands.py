import time

class Commands:
    def __init__(self):
        self.availablecommands = []
        for s in dir(self):
            if "handle_" in s:
                self.availablecommands.append(s)
        self.comslist = ""
        for s in self.availablecommands:
            if (len(self.comslist) > 0):
                self.comslist += " "
            self.comslist += s.split("_")[1]
        self.message = "NONE - " + time.ctime()

        self.dict = {}
        f = open("dict.txt")
        while 1:
            line = f.readline()
            if not line: break
            _first = line.split()[0]
            line = line[len(_first):]
            self.dict[_first] = line
        f.close()

    def save(self):
        os.remove("dict.txt")
        f = open("dict.txt","w")
        for key in self.dict.keys():
            f.write(key + self.ict[key])
        f.close()

    def handle_help(self,msg,bot):
        bot.privmsg(msg.target,"Commands: " + self.comslist)

    def handle_operators(self,msg,bot):
        bot.privmsg(msg.target,"Allowed command prefixes: " + bot._ops)

    def handle_nick(self,msg,bot):
        _split = msg.message.split()
        if (len(_split) > 1) and (msg.frm in bot.owners):
            bot.changenick(_split[1])
            bot.privmsg(msg.target,"Changing nick to " + _split[1])

    def handle_quit(self,msg,bot):
        if (msg.frm in bot.owners):
            bot.privmsg(bot.channel,"Bye bye")
            bot.sendquit()

    def handle_eval(self,msg,bot):
        if (msg.frm in bot.owners):
            _split = msg.message.split()[0]
            _temp_msg = msg.message[len(_split)+1:]
            ans = eval(_temp_msg)
            bot.privmsg(msg.target,"ANS: " + str(ans))

    def handle_uptime(self,msg,bot):
        current = time.time()
        totalsecs = int(round(current - bot.starttime))
        l = [86400,3600,60]
        ol = []
        for num in l:
            n = totalsecs // num
            totalsecs -= n*num
            ol.append(n)
        ol.append(totalsecs)
        _string = "Uptime: Days - " + str(ol[0]) + ", Hours - " + str(ol[1]) + ", Minutes - " + str(ol[2]) + ", Seconds - " + str(ol[3])
        bot.privmsg(msg.target,_string)

    
    def handle_setmsg(self,msg,bot):
        l = len(msg.message.split()[0])
        _temp = msg.message[l:]
        if (msg.frm in bot.owners):
            self.message = _temp + " - " + time.ctime()

    def handle_message(self,msg,bot):
        bot.privmsg(msg.target,self.message)
            
    def handle_url(self,msg,bot):
        bot.privmsg(msg.target,"my-url")

    def handle_learn(self,msg,bot):
        if (msg.frm in bot.owners):
            _msg = msg.message[6:].lower().strip()
            _first = _msg.split()[0]
            _msg = _msg[len(_first):]
            self.dict[_first] = _msg
            bot.privmsg(msg.target,"Got it")

    def handle_list(self,msg,bot):
        _split = msg.message.split()
        _ans = "Sorry, I dont know of that"
        if (len(_split) > 1):
            _check = _split[1].lower()
            if (_check in self.dict.keys()):
                _ans = _check + self.dict[_check]
        bot.privmsg(msg.target,_ans)
