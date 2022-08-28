<?php
try{$dbuser = 'postgres';
$dbpass = '123456';
$host = '103.233.195.56';
$dbname='postgres';
$connec = new PDO("pgsql:host=$host;dbname=$dbname", $dbuser, $dbpass);
}
catch (PDOException $e)
{
	echo "Error : " . $e->getMessage() . "<br/>";
	die();
} 
?>