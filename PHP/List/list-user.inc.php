<?php
//AoW user handling
function aow_user_login($db,$myusername)
{
	$loginc = sha1($myusername + "sitekey" + time());

	$sql="UPDATE login SET lc='".$loginc."' WHERE uname='".$myusername."'";
	$db->query($sql) or die(mysql_error());

	session_start();
	$_SESSION['lc'] = $loginc;
}

function aow_user_logout($db)
{
	if (isset($_SESSION['lc']))
	{
		$loginc = $_SESSION['lc'];
		//session_start();
		$_SESSION['lc'] = null;
		unset($_SESSION['lc']);
		session_destroy();

		// Connect to server and select databse.
			
		$sql = "UPDATE login SET lc='' WHERE lc='".$loginc."'";
		$db->query($sql);
	}
}

?>