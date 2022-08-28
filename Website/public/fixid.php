<?php
session_start();
include_once('inc/include.php');

$user = $_POST['username'];
$user_new = $_POST['username_new'];
$url = 'index.php';
$pass = "ea4c03400aaa4b8f5c4901474fb91278";

if($user == "")
{
    $_SESSION['toastr'] = array(
    'type'      => 'error', // or 'success' or 'info' or 'warning'
    'message' => 'กรุณากรอกชื่อผู้ใช้งาน!!',
    'title'     => 'ผิดพลาด!'
    );
    header('Location: '.$url);
    exit;
}
else if($user_new == "")
{
    $_SESSION['toastr'] = array(
    'type'      => 'error', // or 'success' or 'info' or 'warning'
    'message' => 'กรุณากรอกชื่อผู้ใช้งานตัวพิมพ์เล็ก !!',
    'title'     => 'ผิดพลาด!'
    );
    header('Location: '.$url);
    exit;
}
else
{
    $sth = $connec->prepare("SELECT * FROM accounts WHERE login = '".$user."'");
    $sth->execute();
    $result = $sth->fetch(PDO::FETCH_ASSOC);
    if($result){
        $sql = 'UPDATE accounts '
                . 'SET login = :login, '
                . 'password = :psw '
                . 'WHERE login = :loginOld';
        $stmt = $connec->prepare($sql);                                  
        $stmt->bindParam(':login', $user_new, PDO::PARAM_STR); 
        $stmt->bindParam(':psw', $pass, PDO::PARAM_STR);        
        $stmt->bindParam(':loginOld', $user, PDO::PARAM_INT);   
        $stmt->execute();

        $_SESSION['toastr'] = array(
    'type'      => 'success', // or 'success' or 'info' or 'warning'
    'message' => 'เข้าเกมส์ ด้วยไอดี ใหม่ ได้แล้ว !!',
    'title'     => 'สำเร็จ !!'
    );
        header('Location: '.$url);
        exit; 

    }else{

     $_SESSION['toastr'] = array(
    'type'      => 'error', // or 'success' or 'info' or 'warning'
    'message' => 'ไม่มีชื่อผู้ใช้งานนี้ในระบบ !!',
    'title'     => 'ผิดพลาด!'
    );
     header('Location: '.$url);
     exit;

 }
}
?>