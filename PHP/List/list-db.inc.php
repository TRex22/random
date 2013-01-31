<?php
class db {
	public $db_host, $db_username, $db_password, $db_name, $db_qcount, $db_con, $db_result;
	function __construct() {
        $this->db_host="localhost"; // Host name
		$this->db_username="root"; // Mysql username
		$this->db_password=""; // Mysql password
		$this->db_name="aow"; // Database name
		$this->db_qcount = 0;
		
		$this->db_con = mysql_connect($this->db_host,$this->db_username,$this->db_password) or die("Unable to connect to the MySQL database: ".mysql_error());
		mysql_select_db($this->db_name) or die ("Unable to select the MySQL table: ".mysql_error());
    }
	
	function query($sql)
	{
		$this->db_qcount = $this->db_qcount + 1;
		$this->db_result = mysql_query($sql,$this->db_con) or die("Unable to execute MySQL query: ".mysql_error());
		return $this->db_result;
	}
	
	function queries()
	{
		return $this->db_qcount;
	}
	
	function close()
	{
		mysql_close($this->db_con);
	}
}
?>