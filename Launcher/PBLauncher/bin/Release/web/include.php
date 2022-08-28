<?php
error_reporting(0);
session_start();

   $host        = "host=127.0.0.1";
   $port        = "port=5432";
   $dbname      = "dbname=postgres";
   $credentials = "user=postgres password=123456";
   $db = pg_connect( "$host $port $dbname $credentials");
   if(!$db)
   {
      echo "Error: ไม่สามารถเชื่อมต่อกับฐานข้อมูล\n";
   }
?>