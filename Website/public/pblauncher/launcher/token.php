<?php

function GenerateRandomString($length = 100) 
{
	return substr(str_shuffle(str_repeat($x='0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ/=+', ceil($length/strlen($x)) )),1,$length);
}
$Random = GenerateRandomString(100);

echo $Random;
?>