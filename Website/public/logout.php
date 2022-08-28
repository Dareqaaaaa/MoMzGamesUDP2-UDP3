<?php
header('Location:index.php');
session_start();
unset($_SESSION['player_name']);
unset($_SESSION['uid']);
unset($_SESSION['cash']);
unset($_SESSION['rank']);
unset($_SESSION['login']);
unset($_SESSION['exp']);
unset($_SESSION['kuyraicoin']);
session_destroy();
echo "<script>window.history.back()</script>";
?>