<?php
	session_start();
	
	//show errors
	error_reporting(E_ALL);
	ini_set('display_errors', '1');
	
	//Includes
	include('list-db.inc.php');
	$c = 'db';
	$db = new $c;
	
	include('list-user.inc.php');
	
	//Is the user logged in?
	$loginc = null;
	$aow_loggedin = false;
	$aow_uname = null;
	$aow_access = -1;
	$aow_id = -1;
	if (isset($_SESSION["lc"]))
	{
		$loginc = $_SESSION["lc"];
		
		if ($loginc != null)
		{
			$aow_loggedin = true;
			$result = $db->query("SELECT * FROM login WHERE lc='".$loginc."'");
			$row = mysql_fetch_array($result);
			$aow_uname = $row['uname'];
			$aow_access = $row['access'];
			$aow_id = $row['idlogin'];
		}
	}
?>