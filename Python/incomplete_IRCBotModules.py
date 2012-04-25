import re
import mechanize


class Modules:
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

        #urls regex
        #borrowed and adapted from:
        self._url_regex = re.compile('(http[s]?://|www)(?:[a-zA-Z]|[0-9]|[$-_@.&+]|[!*\(\),]|(?:%[0-9a-fA-F][0-9a-fA-F]))+')
        

    def handle_urls(self,msg,bot):
        _split = msg.message.split()
        for word in _split:
            if not (self._url_regex.match(word) == None):
                try:
                    if not ("http://" in word):
                        word = "http://" + word
                    br = mechanize.Browser()
                    br.open(word)
                    if (br.viewing_html()):
                        bot.privmsg(msg.target,msg.frm + "'s URL: " + str(br.title()) + " ( " + word + " ) ")
                    else:
                        bot.privmsg(msg.target,msg.frm + "'s URL: No title ( " + word + " ) ")
                except:
                    print " -- Tried to parse an invalid url:", sys.exc_info()[0]
