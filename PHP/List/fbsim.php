<?php
	include('list-main.inc.php');
?>
<html>

<head><title>Temporary Log in system</title></head>
<body>

<a href="index.php">Home</a>


<?php if (!$aow_loggedin)
{
?>
	<table width="300" border="0" align="center" cellpadding="0" cellspacing="1" bgcolor="#CCCCCC">
	<tr>
	<form name="form1" method="post" action="fbcheck.php">
	<td>
	<table width="100%" border="0" cellpadding="3" cellspacing="1" bgcolor="#FFFFFF">
	<tr>
	<td colspan="3"><strong>Member Login </strong></td>
	</tr>
	<tr>
	<td width="150">Username</td>
	<td width="6">:</td>
	<td width="294"><input name="myusername" type="text" id="myusername"></td>
	</tr>
	<tr>
	<td>Password</td>
	<td>:</td>
	<td><input name="mypassword" type="password" id="mypassword"></td>
	</tr>
	<tr>
	<td>&nbsp;</td>
	<td>&nbsp;</td>
	<td><input type="submit" name="Submit" value="Login"></td>
	</tr>
	</table>
	</td>
	</form>
	</tr>
	</table>
	
	<table width="450" border="0" align="center" cellpadding="0" cellspacing="1" bgcolor="#CCCCCC">
	<tr>
	<form name="form1" method="post" action="fbreg.php">
	<td>
	<table width="100%" border="0" cellpadding="3" cellspacing="1" bgcolor="#FFFFFF">
	<tr>
	<td colspan="3"><strong>Register</strong></td>
	</tr>
	<tr>
	<td width="150">Username</td>
	<td width="6">:</td>
	<td width="294"><input name="myusername" type="text" id="myusername"></td>
	</tr>
	<tr>
	<td width="150">Email</td>
	<td width="6">:</td>
	<td width="294"><input name="myemail1" type="text" id="myemail1"></td>
	</tr>
	<tr>
	<td width="150">Repeat email:</td>
	<td width="6">:</td>
	<td width="294"><input name="myemail2" type="text" id="myemail2"></td>
	</tr>
	<tr>
	<td>Password</td>
	<td>:</td>
	<td><input name="mypassword1" type="password" id="mypassword1"></td>
	</tr>
	<tr>
	<td>Repeat Password</td>
	<td>:</td>
	<td><input name="mypassword2" type="password" id="mypassword2"></td>
	</tr>
	<tr>
	<td>&nbsp;</td>
	<td>&nbsp;</td>
	<td><input type="submit" name="Submit" value="Register"></td>
	</tr>
	</table>
	</td>
	</form>
	</tr>
	</table>
<?php
}
else
{
?>
	<center>Welcome <?php echo $aow_uname; ?>, <a href="fblogout.php">Log out</a></center>
	<a href="add.php">Add Data</a>
<?php
}
?>
</body>
</html>