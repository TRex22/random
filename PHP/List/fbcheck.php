<?php
	include('list-main.inc.php');

// username and password sent from form
$myusername=$_POST['myusername'];
$mypassword=$_POST['mypassword'];

// To protect MySQL injection (more detail about MySQL injection)
$myusername = stripslashes($myusername);
$mypassword = stripslashes($mypassword);
$myusername = mysql_real_escape_string($myusername);
$mypassword = mysql_real_escape_string($mypassword);

$phash = hash('sha512',$mypassword + "sitekey");

$sql="SELECT * FROM login WHERE uname='$myusername' and passhash='$phash'";
$result=$db->query($sql);

// Mysql_num_row is counting table row
$count=mysql_num_rows($result);
// If result matched $myusername and $mypassword, table row must be 1 row

if($count==1){
	aow_user_login($db,$myusername);
	header("location:fbsim.php");
} else {
	echo "Wrong Username or Password";
	echo "<br /><a href=\"fbsim.php\">fbsim</a>";
}
?>