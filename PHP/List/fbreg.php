<?php
	include('aow-main.inc.php');

// username and password sent from form
$myusername=$_POST['myusername'];
$mypassword1=$_POST['mypassword1'];
$mypassword2=$_POST['mypassword2'];
$myemail1=$_POST['myemail1'];
$myemail2=$_POST['myemail2'];

//TODO: ADD IN CHECKS FOR DUPLICATE UNAME AND EMAIL

// To protect MySQL injection (more detail about MySQL injection)
$myusername = stripslashes($myusername);
$mypassword1 = stripslashes($mypassword1);
$mypassword2 = stripslashes($mypassword2);
$myemail1 = stripslashes($myemail1);
$myemail2 = stripslashes($myemail2);
$myusername = mysql_real_escape_string($myusername);
$mypassword1 = mysql_real_escape_string($mypassword1);
$mypassword2 = mysql_real_escape_string($mypassword2);
$myemail1 = mysql_real_escape_string($myemail1);
$myemail2 = mysql_real_escape_string($myemail2);

if ($mypassword1 != $mypassword2)
{
	die("Password mismatch. <a href=\"fbsim.php\">Back</a>");
}
if ($myemail1 != $myemail2)
{
	die("Email mismatch. <a href=\"fbsim.php\">Back</a>");
}

$phash = hash('sha512',$mypassword1 + "sitekey");

$sql="INSERT INTO Login (uname,passhash,email,access) VALUES('".$myusername."','".$phash."','".$myemail1."',1)";
$db->query($sql);

// Mysql_num_row is counting table row
// If result matched $myusername and $mypassword, table row must be 1 row
echo "You may now log in: <a href=\"fbsim.php\">Back</a>";
?>