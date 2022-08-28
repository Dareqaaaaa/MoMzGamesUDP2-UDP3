<?php
session_start();
include_once('inc/include.php');

$pass_old = $_POST['pass_old'];
$pass_new = $_POST['pass_new'];
$url = 'index.php';

function encripitar($pass){
    $salt = '/x!a@r-$r%an¨.&e&+f*f(f(a)';
    $output = hash_hmac('md5', $pass, $salt);
    return $output;
}
if($pass_old == "")
{
    echo "<script>alert('กรุณากรอกรหัสผ่านเดิม');window.location.href='index.php';</script>";
    /*$_SESSION['toastr'] = array(
    'type'      => 'error', // or 'success' or 'info' or 'warning'
    'message' => 'กรุณากรอกรหัสผ่านเดิม !!',
    'title'     => 'ผิดพลาด!'
    );
    header('Location: '.$url);
    exit;*/
}
else if($pass_new == "")
{
    echo "<script>alert('กรุณากรอกรหัสผ่านใหม่');window.location.href='index.php';</script>";
   /*$_SESSION['toastr'] = array(
    'type'      => 'error', // or 'success' or 'info' or 'warning'
    'message' => 'กรุณากรอกรหัสผ่านใหม่ !!',
    'title'     => 'ผิดพลาด!'
    );
   header('Location: '.$url);
   exit;*/
}
else
{
    $sth = $connec->prepare("SELECT * FROM accounts WHERE player_id = '".$_SESSION["uid"]."' AND password = '".encripitar($pass_old)."'");
    $sth->execute();
    $result = $sth->fetch(PDO::FETCH_ASSOC);
    if($result){

     $sql = 'UPDATE accounts '
     . 'SET password = :psw '
     . 'WHERE player_id = :p_id';
     $stmt = $connec->prepare($sql);                                  
     $stmt->bindParam(':psw', encripitar($pass_new), PDO::PARAM_STR); 
     $stmt->bindParam(':p_id', $_SESSION["uid"], PDO::PARAM_STR);         
     $stmt->execute();

     echo "<script>alert('เปลี่ยนรหัสผ่าน สำเร็จ');window.location.href='index.php';</script>";
    /* $_SESSION['toastr'] = array(
    'type'      => 'success', // or 'success' or 'info' or 'warning'
    'message' => 'เปลี่ยนรหัสผ่าน สำเร็จ !!',
    'title'     => 'สำเร็จ!'
    );
     header('Location: '.$url);
     exit;  */ 

 }else{

    echo "<script>alert('รหัสผ่านเดิม  ผิด');window.location.href='index.php';</script>";
    /*$_SESSION['toastr'] = array(
    'type'      => 'error', // or 'success' or 'info' or 'warning'
    'message' => 'รหัสผ่านเดิม  ผิด !!',
    'title'     => 'ผิดพลาด!'
    );
    header('Location: '.$url);
    exit;  */

}
}
?>