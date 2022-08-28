<?php
session_start();
include_once('inc/include.php');

$user = $_POST['username'];
$pass = $_POST['password'];
$repass = $_POST['re-password'];
$mail = $_POST['email'];
$url = 'index.php';

function encripitar($pass){
    $salt = '/x!a@r-$r%an¨.&e&+f*f(f(a)';
    $output = hash_hmac('md5', $pass, $salt);
    return $output;
}

function utf8_strlen($str)

    {
      $c = strlen($str);

      $l = 0;
      for ($i = 0; $i < $c; ++$i)
      {
         if ((ord($str[$i]) & 0xC0) != 0x80)
         {
            ++$l;
         }
      }
      return $l;
    }


if($user == "")
{
    echo "<script>alert('กรุณากรอกชื่อผู้ใช้งาน');window.location.href='index.php';</script>";
    /*$_SESSION['toastr'] = array(
    'type'      => 'error', // or 'success' or 'info' or 'warning'
    'message' => 'กรุณากรอกชื่อผู้ใช้งาน!!',
    'title'     => 'ผิดพลาด!'
    );
    header('Location: '.$url);
    exit;*/
}
else if (utf8_strlen($user) <= 4) 
{
    echo "<script>alert('ไอดี ต้องมีมากกว่า 4 ตัวอักษร');window.location.href='index.php';</script>";
}
else if($pass == "")
{
    echo "<script>alert('กรุณากรอกรหัสผ่าน');window.location.href='index.php';</script>";
    /*$_SESSION['toastr'] = array(
    'type'      => 'error', // or 'success' or 'info' or 'warning'
    'message' => 'กรุณากรอกรหัสผ่าน!!',
    'title'     => 'ผิดพลาด!'
    );
    header('Location: '.$url);
    exit;*/
}
else if($repass == "")
{
    echo "<script>alert('กรุณากรอกรหัสผ่านยืนยัน');window.location.href='index.php';</script>";
    /*$_SESSION['toastr'] = array(
    'type'      => 'error', // or 'success' or 'info' or 'warning'
    'message' => 'กรุณากรอกรหัสผ่านยืนยัน!!',
    'title'     => 'ผิดพลาด!'
    );
    header('Location: '.$url);
    exit;*/
}
else if($pass != $repass)
{
    echo "<script>alert('รหัสผ่านไม่ตรงกัน');window.location.href='index.php';</script>";
    /*$_SESSION['toastr'] = array(
    'type'      => 'error', // or 'success' or 'info' or 'warning'
    'message' => 'รหัสผ่านไม่ตรงกัน!!',
    'title'     => 'ผิดพลาด!'
    );
    header('Location: '.$url);
    exit;  */    
}
else if (utf8_strlen($pass) <= 4) 
{
    echo "<script>alert('รหัสผ่าน ต้องมีมากกว่า 4 ตัวอักษร');window.location.href='index.php';</script>";
}
else
{

    $sth = $connec->prepare("SELECT * FROM accounts WHERE login = '".$user."'");
    $sth->execute();
    $result = $sth->fetch(PDO::FETCH_ASSOC);
    if($result){
        echo "<script>alert('มีชื่อผู้ใช้งานนี้ในระบบแล้ว');window.location.href='index.php';</script>";
       /* $_SESSION['toastr'] = array(
    'type'      => 'error', // or 'success' or 'info' or 'warning'
    'message' => 'มีชื่อผู้ใช้งานนี้ในระบบแล้ว!!',
    'title'     => 'ผิดพลาด!'
    );
        header('Location: '.$url);
        exit; */  
    }else{
        $ISp_Res = $connec->prepare("INSERT INTO accounts (login,password,email) VALUES(?, ?, ?)");
        $ISp_Res->execute(array($user, encripitar($pass), $mail));

        echo "<script>alert('สมัครสมาชิกเสร็จเรียบร้อย');window.location.href='index.php';</script>";

        /*$_SESSION['toastr'] = array(
    'type'      => 'success', // or 'success' or 'info' or 'warning'
    'message' => 'สมัครสมาชิกเสร็จเรียบร้อย!!',
    'title'     => 'สำเร็จ!'
    );
        header('Location: '.$url);
        exit;  */  
    }
}
?>