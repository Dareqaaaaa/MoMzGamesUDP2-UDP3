<?php

function GenerateRandomString($length = 100) 
{
	return substr(str_shuffle(str_repeat($x='0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ/=+', ceil($length/strlen($x)) )),1,$length);
}
$Random = GenerateRandomString(100);

error_reporting(0);
session_start();
require_once('web/include.php'); 

if($_GET["login"] != "" || $_GET["password"] != "")
{
	$Query = pg_query("SELECT * FROM contas WHERE login LIKE '".$_GET["login"]."' AND senha LIKE '".$_GET["password"]."';");
	$Result = pg_num_rows($Query);
	$Query2 = pg_query("SELECT * FROM contas WHERE bans='1' AND login LIKE '".$_GET["login"]."';");
	$Ban = pg_num_rows($Query2);
	$Query3 = pg_query("SELECT * FROM contas WHERE active = 'f' AND login LIKE '".$_GET["login"]."';");
	$Active = pg_num_rows($Query3);
	$Query4 = pg_query("SELECT token FROM contas WHERE login LIKE '".$_GET["login"]."' AND senha LIKE '".$_GET["password"]."';");
	while($row = pg_fetch_assoc($Query4))
	{
		$Token = $row[token];
	}
	
	if($Result != 1)
	{
		echo "<script>window.history.back()</script>";
		header('Location: fail.php');
	}
	else
	{
		if (($Ban == 1) && ($Active))
		{
			header('Location: activeban.php');
		}
		else if ($Ban == 1)
		{
			header('Location: ban.php');
		}
		else if ($Active)
		{
			header('Location: active.php');
		}
		else
		{
			if ($Token == null)
			{
				header('Location: fail.php');
			}
			else
			{
				echo $Token;	
			}	
		}	
	}
}
else
{
	echo "<script>window.history.back()</script>";
	header('Location: fail.php');
}
?>