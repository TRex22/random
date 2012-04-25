import socket
import thread
import time
import sys
import re
import commands
import random
import time
import modules

class IRCBot:
    class IRCMessage:
        def __init__(self, rawdata, mesreg, bot):
            try:
                self.raw = rawdata
                self.regexmatch = mesreg.match(str(rawdata))
                self.friendly = ""
                if not (self.regexmatch == None):
                    self.l = []
                    for g in self.regexmatch.groups():
                        self.l.append(g)
                        self.friendly += " " + str(g)
                self.type = "None"
                if not (self.l[0] == None):
                    self.type = str(self.l[1])
                    if ("@" in self.l[0]):
                        _split = self.l[0].split('@')
                        _split2 = _split[0].split("!")
                        self.rname = _split2[1]
                        self.frm = _split2[0]
                        self.ident = _split[1]
                    else:
                        self.frm = self.l[0]
                        self.ident = self.l[0]
                    if (self.type in ['NOTICE','PRIVMSG']):
                        self.target = self.l[2]
                        if (self.target == bot.nick):
                            self.target = self.frm #hacky way to make the bot reply to privmsgs via privmsg
                        self.message = str(self.l[3])
                    else:
                        self.target = ""
                        self.message = ""
                    if (self.type in ['JOIN']):
                        self.target = self.l[2]
            except:
                print " -- IRCMessage creation error:", sys.exc_info()[0]
                
    
    def __init__(self, _server, _port, _nick, _channel, _owner, _allowed_ops):
        self.starttime = time.time()
        self.commands = commands.Commands()
        self.mods = modules.Modules()
        self.server = _server
        self.port = _port
        self.nick = _nick
        self.channel = _channel
        self.owners = []
        self.owners.append(_owner)
        self._ops = _allowed_ops
        self.ops = []
        for character in _allowed_ops:
            self.ops.append(character)
        self.quit = False
        self.restart = False
        s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        self.sock = s
        print " -- Connecting to server:",self.server
        contup = (self.server, self.port)
        self.sock.connect(contup)
        print " -- Connected, sending user info"
        self.sock.send("NICK " + self.nick + "\r\n")
        self.sock.send("USER " + self.nick + " " + self.nick + " " + self.nick + " :" + self.nick + "\r\n")
        self.msg = ""
        print " -- Initialising message handler"
        self._mesreg = re.compile("^(?:[:@]([^\\s]+) )?([^\\s]+)(?: ((?:[^:\\s][^\\s]* ?)*))?(?: ?:(.*))?$")
        print " -- Starting listen thread"
        self._thread = thread.start_new(self.receiveloop, (None,))
        self.curnick = _nick

    def sendraw(self,raw):
        self.sock.send(raw + "\r\n")

    def receiveloop(self, _a):
        while True:
            try:
                if (self.quit):
                    break;
                self.msg += self.sock.recv(1024)
                _temp_handle = self.msg + " "
                msgs = _temp_handle.split("\r\n")
                if (msgs[len(msgs)-1] == " "):
                    msgs.pop()
                _temp_l = []
                while (len(msgs) >= 1):
                    _temp_msg = msgs.pop()
                    if ("PING" in _temp_msg):
                        _temp_spl = _temp_msg.split()
                        self.sendraw("PONG " + _temp_spl[1])
                    self.msg = self.msg[len(_temp_msg)+4:]
                    #handle message here
                    thread.start_new(self.handlemessage,(_temp_msg,))
            except:
                print " -- Unexpected error:", sys.exc_info()[0]

    def handlemessage(self,raw):
        try:
            _msg = self.IRCMessage(raw,self._mesreg,self)
            if (_msg.type in ["PRIVMSG","NOTICE"]) and (len(_msg.message) > 0):
                if (_msg.message[0] in self.ops):
                    _split = _msg.message.split(" ")
                    _command = _split[0][1:]
                    if ("handle_" + _command in self.commands.availablecommands):
                        eval("self.commands.handle_"+_command+"(_msg,self)")
                    else:
                        privmsg(_msg.frm,"The command " + _command + " does not exist, to get a list of commands use the 'help' command")
            elif (_msg.type == "376"):
                print " -- Joining bot channel"
                self.join(self.channel)
                self.sendraw("MODE dont_trip +B dont_trip")
            elif (_msg.type == "433"):
                _newnick = self.curnick + str(random.randrange(1000,9999))
                self.changenick(_newnick)
                print " -- Nick collision, changing nicks to",_newnick
            elif (_msg.type == "JOIN") and (_msg.l[3] == self.channel):
                self.notice(_msg.frm,"Welcome to " + self.channel + ", " + self.commands.message)
            if (_msg.type == "PRIVMSG"):
                #modules are run only on privmsgs
                for mod in self.mods.availablecommands:
                    eval("self.mods."+mod+"(_msg,self)")
        except:
            print " -- Mesage handling error:", sys.exc_info()[0]
        

    def join(self,channel):
        self.sendraw("JOIN " + channel)
        
    def part(self,channel):
        self.sendraw("PART " + channel)

    def privmsg(self,target,message):
        _mes_col = "4"
        self.sendraw("PRIVMSG " + target + " " + _mes_col + message)
    
    def sendquit(self):
        self.sendraw("QUIT ")# + message)
        self.quit = True
        self._thread.close()

    def notice(self,target,message):
        self.sendraw("NOTICE " + target + " " + message)

    def beginrestart(self):
        self.restart = True
        self.sendquit()

    def changenick(self,nick):
        self.sendraw("NICK " + nick)
        self.curnick = nick
