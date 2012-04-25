require "socket"
require "thread"
require "time"
require "active_record"

class EBot
	attr_accessor :s, :uname, :server, :port, :connected, :lt, :last, :now, :db, :chan_list, :joined, :join_list, :auth_code, :auth_fail, :auth_list, :irc_markup, :pass
	
	#give the bot it's inital settings
	def initialize(_name, _server, _port, _pass)
		@connected = false
		@uname = _name
		@server = _server
		@port = _port
		@last = Time.new
		@now = Time.new
		@db = false
		@joined = false
		
		@pass = _pass
		
		@auth_code = ""#random_string(12)
		@auth_fail = 0
		@auth_list = ["edg3","Supremacy"]
		
		@irc_markup = ["7,0%0,7%8,7%7,4%1,4%4,1%0,1","4,1%1,4%7,4%8,7%0,7%7,0%"]
	end
	
	#run a connect, will also be used in reconnecting
	def connect()
		begin
			@connected = true
			@s = TCPSocket.new(@server, @port)
		rescue Exception => e
			puts "#{e.message} - #{e.backtrace.inspect} \n\n"
			puts "An Exception was raised. (Code: 00006)"
		end
	end
	
	def send_udata()
		if @connected
			@s.puts "NICK #{@uname} \r\n"
			@s.puts "USER #{@uname} #{@uname} #{@uname} : #{@uname} \r\n"
		end
	end
	
	def send_raw(mes)
		if @connected
			@s.puts "#{mes} \r\n"
		end
	end
	
	def join(channel)
		if @connected
			send_raw("JOIN #{channel}")
		end
	end
	
	def listen()
		while a = @s.gets
			if a.include? "PING"
				b = a.split(" :")
				@s.puts "PONG :#{b[1]} \r\n"
				#puts "PONG\r\n"
				if @joined == false
					db_get_chanlist()
					
					#subscriptions = Irc_Subscription.find(:all, :conditions => "status=1 {:status => 1 , })
					#subscriptions.each do |subs|
					@join_list.each do |joinee|
						join(joinee)
					end
					
					send_raw("PRIVMSG NickServ :Identify #{@pass}")
					
					auth_new()
					@joined = true
				end
			end
			
			@now = Time.new
			if @now.to_f > @last.to_f + 3600 #3600 = hour
				#send an update
				@last = @now
				news_send(@chan_list,0) #send to every subscribed channel
			end
			
			begin
				#puts a
				if /\:(.+)\:(.+)/ =~ a
					part_a = $1
					part_b = $2
				if /\!(news|help|shampoomesideways)/ =~ part_b
					part_c = $1
					#bot commands here
					/(.+)\!(.+)/ =~ part_a
					if part_c == "news"
						news_send($1)
					elsif part_c == "help"
						send_raw("PRIVMSG #{$1} :#{@irc_markup[0]} - Visit the website: http://www.clanclash.za.net - #{@irc_markup[1]}")
					elsif part_c == "shampoomesideways"
						send_raw("QUIT")
					end
				elsif /\?(.+)/ =~ part_b
					part_d = $1
					#puts part_d
					if /(#{@auth_code}) (.+)/ =~ part_d
						part_e = $2
						@auth_fail = 0
						
						if part_e.include? "quit"
							send_raw("QUIT")
						elsif part_e.include? "force"
							news_send(@chan_list,0)
						elsif /eval (.+)/ =~ part_e
							ans = eval($1)
							print ans
							/(.+)#(.+)/ =~ part_a
							ch = "\##{$2}"
							send_raw("PRIVMSG #{ch} : eval: #{ans}")
						end
						
						auth_new()
					else
						@auth_fail = @auth_fail + 1
						if @auth_fail == 10
							auth_new()
							puts a
							@auth_fail = 0
						end
					end
				end
				end
			rescue Exception => e
				puts "#{e.message} - #{e.backtrace.inspect} \n\n"
				puts "An Exception was raised. (Code: 00005)"
			end
		end
	end
	
	#send each news item
	def news_send(targets,opt = nil)
	#refreshes the chanlists
		db_get_chanlist()
	
	#joins/rejoins all the channels
		@join_list.each do |joinee|
			join(joinee)
		end
		
		news = db_list()
		
		begin
			if opt == nil
				if news.length > 0
					targets = targets.split()
					targets.each do |target|
						news.each do |newsitem|
							send_raw("PRIVMSG #{target} :#{newsitem[1]}")
							sleep 1 #to prevent spam/flood triggers
						end
					end
				end
			else
				if news.length > 0
					targets = targets.split()
					targets.each do |target|
						n_t = target.split("@")
						news.each do |newsitem|
							if n_t[0].to_s.strip() == newsitem[0].to_s.strip()
								send_raw("PRIVMSG #{n_t[1]} :#{newsitem[1]}")
								sleep 1 #to prevent spam/flood triggers
							end
						end
					end
				end	
			end
		rescue Exception => e
			puts "#{e.message} - #{e.backtrace.inspect} \n\n"
			puts "An Exception was raised. (Code: 00004)"
		end
	end

	
	#db specific stuff after this
	def db_connect(host,db,user,pass)
		begin
			ActiveRecord::Base.establish_connection(  
				:adapter => "mysql",  
				:host => "#{host}",  
				:database => "#{db}",
				:username => "#{user}",
				:password => "#{pass}"
			)
			@db = true
		rescue Exception => e
			puts "#{e.message} - #{e.backtrace.inspect} \n\n"
			puts "An Exception was raised. (Code: 00001)"
		end
	end
	
	class Irc_Notification < ActiveRecord::Base  
	end
	class Irc_Category < ActiveRecord::Base
	end
	class Irc_Subscription < ActiveRecord::Base
	end

	
	def db_get_chanlist()
	
		begin
			ActiveRecord::Base.connection.reconnect!
		rescue	Exception => e
		end
		
		tmp_str = ""
		tmp_cha = []
		subscriptions = Irc_Subscription.find(:all, :conditions => {:status => 1 })
		subscriptions.each do |subs|
			if tmp_str.length > 0
				tmp_str = tmp_str + " "
			end
			tmp_str = tmp_str + "#{subs.irc_category_id}@#{subs.channel}"
			tmp_cha << subs.channel
		end
		
		@join_list = tmp_cha.uniq
		@chan_list = tmp_str
	end
	
	def db_list(opt = nil)
		if (@db)
			begin
				
				begin
					ActiveRecord::Base.connection.reconnect!
				rescue	Exception => e
				end
			
				tmp_arr = Array.new
				count = 0
				
				notifications = Irc_Notification.find(:all)
				tmp_cat = Irc_Category.find(:all)
				
				categories = ["NONE"]
				
				tmp_cat.each do |cc|
					categories << cc.title
				end
				
				notifications.each do |nn|
					
					c = nn.irc_category_id
					if c >= categories.length
						c = 0
					end
					if nn.updated_at == nn.created_at
						qq = "(Created: #{nn.created_at.strftime("%I:%M%p - %m/%d/%Y")})"
					else
						qq = "(Updated: #{nn.updated_at.strftime("%I:%M%p - %m/%d/%Y")})"
					end
					
					s = "#{@irc_markup[0]} - #{categories[c]} (\##{nn.id}) - #{nn.body} - #{qq} - #{@irc_markup[1]}"
					tmp_arr[count] = [nn.irc_category_id,s]
					count = count + 1
				end
			rescue Exception => e
				puts "#{e.message} - #{e.backtrace.inspect} \n\n"
				puts "An Exception was raised. (Code: 00000)"
				tmp_arr = ["#{@irc_markup[0]} - An Exception was raised. (Code: 00000) - #{@irc_markup[1]}"]
			end
			
			tmp_arr #return
		else
			nil #return
		end
	end
	
	def auth_new()
		@auth_code = random_string(4)
		puts @auth_code
		@auth_list.each do |t|
			send_raw("NOTICE #{t} : New auth code for admin commands: #{@auth_code}")
		end
	end
	
	def random_string( len )
		chars = ("a".."z").to_a + ("A".."Z").to_a + ("1".."9").to_a
		randstr = ""
		1.upto(len) { |i| randstr << chars[rand(chars.size-1)] }
		randstr
	end
end


bot = EBot.new("[cc]announce","za.shadowfire.org",6667, "3fPNbL5rdXvlvfHtfE8JOWLv")
bot.db_connect("apocalypse-gfx.com","demigod_gaming","demigod_ircbot","qTk*s?qR$T}c")
bot.connect()
bot.send_udata()
bot.listen()