<?php
session_start();
include_once('inc/include.php');

$user = $_POST['username'];
$pass = $_POST['password'];
$url = 'index.php';

function encripitar($pass){
    $salt = '/x!a@r-$r%an¨.&e&+f*f(f(a)';
    $output = hash_hmac('md5', $pass, $salt);
    return $output;
}

if($user == "")
{
    echo "<script>alert('กรุณากรอกชื่อผู้ใช้งาน');window.location.href='index.php';</script>";
    /*$_SESSION['toastr'] = array(
    'type'      => 'error', // or 'success' or 'info' or 'warning'
    'message' => 'กรุณากรอกชื่อผู้ใช้งาน !!',
    'title'     => 'ผิดพลาด!'
    );
    header('Location: '.$url);
    exit;*/
}
else if($pass == "")
{
    echo "<script>alert('กรุณากรอกรหัสผ่าน');window.location.href='index.php';</script>";
 /*$_SESSION['toastr'] = array(
    'type'      => 'error', // or 'success' or 'info' or 'warning'
    'message' => 'กรุณากรอกรหัสผ่าน !!',
    'title'     => 'ผิดพลาด!'
    );
 header('Location: '.$url);
 exit;*/
}
else
{
    $sth = $connec->prepare("SELECT * FROM accounts WHERE login = '".$user."' AND password = '".encripitar($pass)."'");
    $sth->execute();
    $result = $sth->fetch(PDO::FETCH_ASSOC);
    if($result){

        $stmt = $connec->query("SELECT * FROM accounts WHERE login = '".$user."' AND password = '".encripitar($pass)."'");
        while ($row = $stmt->fetch()) {
            $_SESSION["login"] = $row['login'];
            $_SESSION["uid"] = $row['player_id'];
        }
        echo "<script>alert('เข้าสู่ระบบสำเร็จ');window.location.href='index.php';</script>";
        /*$_SESSION['toastr'] = array(
    'type'      => 'success', // or 'success' or 'info' or 'warning'
    'message' => 'เข้าสู่ระบบสำเร็จ !!',
    'title'     => 'สำเร็จ!'
    );
        header('Location: '.$url);
        exit; */  

    }else{
        echo "<script>alert('ชื่อผู้ใช้งาน หรือ รหัสผ่านนี้  ผิด !!');window.location.href='index.php';</script>";
        /*$_SESSION['toastr'] = array(
    'type'      => 'error', // or 'success' or 'info' or 'warning'
    'message' => 'ชื่อผู้ใช้งาน หรือ รหัสผ่านนี้  ผิด !!',
    'title'     => 'ผิดพลาด!'
    );
        header('Location: '.$url);
        exit; */ 

    }
}
?>