<?php
session_start();
include_once('inc/include.php');

$user = $_POST['username'];
$uid = $_POST['uid'];
$item_code = $_POST['item_code'];
$url = 'index.php';

if($item_code == "")
{
    echo "<script>alert('กรุณากรอกไอเท็มโค้ด');window.location.href='index.php';</script>";
    /*$_SESSION['toastr'] = array(
    'type'      => 'error', // or 'success' or 'info' or 'warning'
    'message' => 'กรุณากรอกไอเท็มโค้ด!!',
    'title'     => 'ผิดพลาด!'
    );
    header('Location: '.$url);
    exit;*/
}
else
{
    $sth = $connec->prepare("SELECT * FROM check_user_itemcode WHERE uid = '".$uid."' AND item_code = '".$item_code."'");
    $sth->execute();
    $result = $sth->fetch(PDO::FETCH_ASSOC);
    if($result){

        echo "<script>alert('คุณได้เติมไอเท็มนี้ไปแล้ว');window.location.href='index.php';</script>";
         /*$_SESSION['toastr'] = array(
    'type'      => 'error', // or 'success' or 'info' or 'warning'
    'message' => 'คุณได้เติมไอเท็มนี้ไปแล้ว !!',
    'title'     => 'ผิดพลาด!'
    );
     header('Location: '.$url);
     exit;*/

    }else{

        $sth2 = $connec->prepare("SELECT * FROM item_code WHERE item_code = '".$item_code."'");
    $sth2->execute();
    $result2 = $sth2->fetch(PDO::FETCH_ASSOC);
    if($result2){

        $stmt = $connec->query("SELECT * FROM item_code WHERE item_code = '".$item_code."'");
        while ($row = $stmt->fetch()) {

        $ISp_Res = $connec->prepare("INSERT INTO check_user_itemcode (uid,item_code) VALUES(?, ?)");
        $ISp_Res->execute(array($uid, $item_code));
        
        if ($row['cash'] != "") {

             $stmt2 = $connec->query("SELECT * FROM accounts WHERE player_id = '".$uid."'");
                while ($row2 = $stmt2->fetch()) {

                    $moneyGet = $row2['money'] = $row2['money'] + $row['cash'];
                    $sql = 'UPDATE accounts '
                    . 'SET money = :mn '
                    . 'WHERE player_id = :p_id';
                    $stmt = $connec->prepare($sql);                                  
                    $stmt->bindParam(':mn', $moneyGet, PDO::PARAM_STR); 
                    $stmt->bindParam(':p_id', $uid, PDO::PARAM_STR);         
                    $stmt->execute();
                    echo "<script>alert('ยินดีด้วยคุณได้รับ ".$row['item_alert']."');window.location.href='index.php';</script>";
                }          
            }else {
                $ISp_Res2 = $connec->prepare("INSERT INTO player_items (owner_id, item_id, item_name, count, category,equip) VALUES(?, ?, ?, ?, ?, ?)");
                $ISp_Res2->execute(array($uid,$row['item_id'],$row['item_name'],$row['item_count'],1,1));
                echo "<script>alert('ยินดีด้วยคุณได้รับ ".$row['item_alert']."');window.location.href='index.php';</script>";
            }
        }
    }else{

        echo "<script>alert('ไม่มีไอเท็มโค้ดนี้ในระบบ');window.location.href='index.php';</script>";
        /*$_SESSION['toastr'] = array(
    'type'      => 'error', // or 'success' or 'info' or 'warning'
    'message' => 'ไม่มีไอเท็มโค้ดนี้ในระบบ !!',
    'title'     => 'ผิดพลาด!'
    );
     header('Location: '.$url);
     exit;*/

            }
 }
}
?>