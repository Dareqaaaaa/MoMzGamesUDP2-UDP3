<?php
session_start();
include_once('inc/include.php');
date_default_timezone_set("Asia/Bangkok");
$date_now = date("Y-m-d");

$userID = $_POST['user_id'];

    $sth = $connec->prepare("SELECT * FROM share_facebook WHERE uid = '".$userID."' AND datenow = '".$date_now."'");
    $sth->execute();
    $result = $sth->fetch(PDO::FETCH_ASSOC);
    if($result){       
        echo "<script>alert('วันนี้คุณได้แชร์ไปแล้ว กรุณาแชร์ใหม่อีกครั้งในวันพรุ่งนี้');window.location.href='index.php';</script>";
    }else{
        $ISp_Res = $connec->prepare("INSERT INTO share_facebook (uid,datenow) VALUES(?, ?)");
        $ISp_Res->execute(array($userID, $date_now));

        /*$ISp_Res2 = $connec->prepare("INSERT INTO player_items (owner_id, item_id, item_name, count, category,equip) VALUES(?, ?, ?, ?, ?, ?)");
        $ISp_Res2->execute(array($userID,400006047,'Cerberus',100,1,1));*/

                $stmt2 = $connec->query("SELECT * FROM accounts WHERE player_id = '".$userID."'");
                while ($row2 = $stmt2->fetch()) {

                    $moneyGet = $row2['kuyraicoin'] = $row2['kuyraicoin'] + 5;
                    $sql = 'UPDATE accounts '
                    . 'SET kuyraicoin = :mn '
                    . 'WHERE player_id = :p_id';
                    $stmt = $connec->prepare($sql);                                  
                    $stmt->bindParam(':mn', $moneyGet, PDO::PARAM_STR); 
                    $stmt->bindParam(':p_id', $userID, PDO::PARAM_STR);         
                    $stmt->execute();
                }

                /*header('Location: https://www.facebook.com/sharer/sharer.php?u=https%3A%2F%2Fwww.facebook.com%2F2376638292562375%2Fvideos%2F448541042300422%2F&amp;src=sdkpreparse');*/

                header('Location: https://www.facebook.com/sharer/sharer.php?u=https%3A%2F%2Fwww.facebook.com%2Fpbkuyrai%2Fphotos%2Fa.2395213640704840%2F2700963166796551%2F%3Ftype%3D3%26theater&amp;src=sdkpreparse');
                   }
?>