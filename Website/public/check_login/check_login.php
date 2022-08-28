<?php
session_start();
include_once('../inc/include.php');

$user = $_POST['username'];
$pass = $_POST['password'];
$hwid = $_POST['hwid'];

function encripitar($pass){
    $salt = '/x!a@r-$r%an¨.&e&+f*f(f(a)';
    $output = hash_hmac('md5', $pass, $salt);
    return $output;
}
    $sth = $connec->prepare("SELECT * FROM accounts WHERE login = '".$user."' AND password = '".encripitar($pass)."'");
    $sth->execute();
    $result = $sth->fetch(PDO::FETCH_ASSOC);
    if($result){

                    $sql = 'UPDATE accounts '
                    . 'SET hwid = :mn '
                    . 'WHERE login = :p_id';
                    $stmt = $connec->prepare($sql);                                  
                    $stmt->bindParam(':mn', $hwid, PDO::PARAM_STR); 
                    $stmt->bindParam(':p_id', $user, PDO::PARAM_STR);         
                    $stmt->execute();

                    $stmt2 = $connec->query("SELECT * FROM accounts WHERE login = '".$user."'");
                    while ($row = $stmt2->fetch()) {
                        $hwid_user = $row['hwid'];
                    }

                    $sth = $connec->prepare("SELECT * FROM ban_hwid WHERE hwid = '".$hwid_user."'");
                    $sth->execute();
                    $result = $sth->fetch(PDO::FETCH_ASSOC);
                    if($result){
                        echo "เครื่องของคุณโดนแบน"; 
                    }else{
                        echo "เข้าสู่ระบบสำเร็จ"; 
                    }

                    
    }else{
        echo "ชื่อผู้ใช้งาน หรือ รหัสผ่านไม่ถูกต้อง"; 
	}
?>