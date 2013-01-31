<?php
	include('list-db.inc');
	
	$c = 'db';
	$db = new $c;
	
	$q = "CREATE TABLE `login` (
		`idlogin` int(10) unsigned NOT NULL AUTO_INCREMENT,
		`uname` varchar(45) NOT NULL,
		`passhash` varchar(150) NOT NULL,
		`email` varchar(150) NOT NULL,
		`access` int(10) unsigned NOT NULL DEFAULT '1',
		`lc` varchar(100) NOT NULL,
		PRIMARY KEY (`idlogin`)
		)";
	
	$db->query($q);
	
	
	$db->close();
?>