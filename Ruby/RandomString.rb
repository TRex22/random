def random_string( len )
    chars = ("a".."z").to_a + ("A".."Z").to_a + ("1".."9").to_a
    randstr = ""
    1.upto(len) { |i| randstr << chars[rand(chars.size-1)] }
    return randstr
end

(1..100).each do |aaa|
	puts random_string(64)
end
gets