<?php
session_start();
include_once('inc/include.php');

if (!empty($_SESSION["uid"])) {

   $item_id = $_POST['item_id']; 
   $uid = $_SESSION["uid"];

                  $strShop = "SELECT * FROM web_item_shop WHERE item_id = '".$item_id."'";
                  foreach ($connec->query($strShop) as $row) 
                  {

                  $strAcc = "SELECT * FROM accounts WHERE player_id = '".$uid."'";
                  foreach ($connec->query($strAcc) as $row2) 
                  {
                    if ($row2['kuyraicoin'] >= $row['price']) {

                    $_SESSION["coin"] = $_SESSION["coin"] - $row['price'];
                    $ISp_Res2 = $connec->prepare("INSERT INTO player_items (owner_id, item_id, item_name, count, category,equip) VALUES(?, ?, ?, ?, ?, ?)");
                    $ISp_Res2->execute(array($uid,$row['item_id'],$row['item_name'],$row['count'],$row['category'],1));

                    $sql = 'UPDATE accounts '
                    . 'SET kuyraicoin = :mn '
                    . 'WHERE player_id = :p_id';
                    $stmt = $connec->prepare($sql);                                  
                    $stmt->bindParam(':mn', $_SESSION["coin"], PDO::PARAM_STR); 
                    $stmt->bindParam(':p_id', $uid, PDO::PARAM_STR);         
                    $stmt->execute();
                    echo "<script>alert('คุณได้ซื้อไอเท็มเรียบร้อย!!');window.location.href='index.php';</script>";

                  }else{
                       echo "<script>alert('KuyraiCoin ของคุณไม่เพียงพอ!!');window.location.href='index.php';</script>";
                  }  
                }                                 
              }
}else{
    echo "<script>alert('กรุณาเข้าสู่ระบบ!!');window.location.href='index.php';</script>";
}
?>