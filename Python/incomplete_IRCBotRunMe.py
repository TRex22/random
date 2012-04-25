import ircbot
import time

Restarting = True

while Restarting:
    Running = True
    time.sleep(5)
    bot = ircbot.IRCBot("someserver",someport,"dont_trip","#thechannel","theadmin","!.?$ (allowed command prefixes)")
    time.sleep(5)
    while Running:
        if bot.quit:
            Running = False
        time.sleep(5)
    if (bot.restart):
        Restarting = True
        print "Restarting bot"
    else:
        Restarting = False
print "Good night"
