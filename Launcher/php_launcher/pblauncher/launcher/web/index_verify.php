<?php
@session_start();
$_SESSION['farkhost_verify'] = true;
$_SESSION['farkhost_verify_time'] = time();
header('Location: index.php');
?>