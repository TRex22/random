require 'open-uri' 

#Not entirely mine, borrowed some of the regex stuff

last_ip = ""
while 1
	my_ip = (open("http://myip.dk") { |f| /([0-9]{1,3}\.){3}[0-9]{1,3}/.match(f.read).to_a[0] })
	if (my_ip != last_ip)
		last_ip = my_ip
		open("updateURLhere")
		puts "New external IP: " + last_ip.to_s
	else
		puts "No IP Change"
	end
	sleep(60*15)
end