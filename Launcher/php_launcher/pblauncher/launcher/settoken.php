<?php
require_once('web/include.php'); 
function GenerateRandomString($length = 100) 
{
	return substr(str_shuffle(str_repeat($x='0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ/=+', ceil($length/strlen($x)) )),1,$length);
}
$i=0;
while($i<=0)
{
	$Random = GenerateRandomString(100);
	pg_query("UPDATE contas SET token='".$Random."' WHERE id='".$i."'");
	$i++;
}
echo $i;
?>