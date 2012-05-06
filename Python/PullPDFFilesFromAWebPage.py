import httplib2, urllib
from BeautifulSoup import BeautifulSoup, SoupStrainer

#toget = raw_input('URL:')
toget = 'http://acmicpc-live-archive.uva.es/nuevoportal/downloads.php'
#matching = raw_input('MATCHING:')
matching = 'pdf'

http = httplib2.Http()
status, response = http.request(toget)

links = []

for link in BeautifulSoup(response, parseOnlyThese=SoupStrainer('a')):
    if link.has_key('href') and matching in link['href']:
        links.append(link['href'])

flipped = toget[::-1]
flag = True
while flag:
    if len(flipped) > 0:
        if flipped[0] != "/":
            flipped = flipped[1:]
        else:
            flag = False
    else:
        flag = False

if len(flipped) > 0 and flipped != "http://":
    toget = flipped[::-1]

for l in links:
    if not "http://" in l:
	l = toget + l
    urllib.urlretrieve(l, l.split('/')[-1])
    print "downloaded:",l
