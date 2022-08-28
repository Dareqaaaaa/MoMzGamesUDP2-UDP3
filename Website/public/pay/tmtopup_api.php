<?php
require_once('AES.php');
require_once('include.php');

// กำหนด API Passkey
define('API_PASSKEY', 'pbkuyrai-ss2');
require_once('AES.php');

if($_SERVER['REMOTE_ADDR'] == '203.146.127.115' && isset($_GET['request']))
{
	$aes = new Crypt_AES();
	$aes->setKey(API_PASSKEY);
	$_GET['request'] = base64_decode(strtr($_GET['request'], '-_,', '+/='));
	$_GET['request'] = $aes->decrypt($_GET['request']);
	// $request['cashcard_amount']
	if($_GET['request'] != false)
	{
		parse_str($_GET['request'],$request);
		$request['Ref1'] = base64_decode($request['Ref1']);

		$results = pg_query("SELECT * FROM accounts WHERE login ='".$request['Ref1']."'");
		while($row=pg_fetch_assoc($results)){
			$UID = $row['player_id'];
		}
		if($request['cashcard_amount'] == "50")
		{
			pg_query("UPDATE accounts SET money = money + 36000 WHERE player_id = '".$UID."'");
			pg_query("UPDATE accounts SET kuyraicoin = kuyraicoin + 50 WHERE player_id = '".$UID."'");
			pg_query("UPDATE accounts SET exp = exp + 120000 WHERE player_id = '".$UID."'");
			pg_query("INSERT INTO player_topup_histories (txid,player_id, true_id, price, status) VALUES ('".$request['TXID']."','".$UID."','".$request['cashcard_password']."','".$request['cashcard_amount']."','เรียบร้อย')");
			echo 'SUCCEED '.' User : ' . $request['Ref1']. ' UID : '.$UID;
		}
		else if ($request['cashcard_amount'] == "90")
		{			
			pg_query("UPDATE accounts SET money = money + 90000 WHERE player_id = '".$UID."'");
			pg_query("UPDATE accounts SET kuyraicoin = kuyraicoin + 90 WHERE player_id = '".$UID."'");
			pg_query("UPDATE accounts SET exp = exp + 250000 WHERE player_id = '".$UID."'");
			pg_query("INSERT INTO player_topup_histories (txid,player_id, true_id, price, status) VALUES ('".$request['TXID']."','".$UID."','".$request['cashcard_password']."','".$request['cashcard_amount']."','เรียบร้อย')");
			echo 'SUCCEED '.' >>> User : ' . $request['Ref1']. ' UID : '.$UID;
		}
		else if ($request['cashcard_amount'] == "150")
		{			
			
			pg_query("UPDATE accounts SET money = money + 156000 WHERE player_id = '".$UID."'");
			pg_query("UPDATE accounts SET kuyraicoin = kuyraicoin + 150 WHERE player_id = '".$UID."'");
			pg_query("UPDATE accounts SET exp = exp + 450000 WHERE player_id = '".$UID."'");		
			pg_query("INSERT INTO player_topup_histories (txid,player_id, true_id, price, status) VALUES ('".$request['TXID']."','".$UID."','".$request['cashcard_password']."','".$request['cashcard_amount']."','เรียบร้อย')");
			echo 'SUCCEED '.' User : ' . $request['Ref1']. ' UID : '.$UID;
		}
		else if ($request['cashcard_amount'] == "300")
		{
			pg_query("UPDATE accounts SET money = money + 324000 WHERE player_id = '".$UID."'");
			pg_query("UPDATE accounts SET kuyraicoin = kuyraicoin + 300 WHERE player_id = '".$UID."'");
			pg_query("UPDATE accounts SET exp = exp + 900000 WHERE player_id = '".$UID."'");

			pg_query("INSERT INTO player_items (owner_id, item_id, item_name, count, category, equip) VALUES ('".$UID."', 100003157 , AUG A3 CHAMPION , 1296000, 1, 1)");
			pg_query("INSERT INTO player_items (owner_id, item_id, item_name, count, category, equip) VALUES ('".$UID."', 200004142 , Kriss S.V Champion , 1296000, 1, 1)");
			pg_query("INSERT INTO player_items (owner_id, item_id, item_name, count, category, equip) VALUES ('".$UID."', 300005084 , CHEYTAC_M200_CHAMPION , 1296000, 1, 1)");


			pg_query("INSERT INTO player_topup_histories (txid,player_id, true_id, price, status) VALUES ('".$request['TXID']."','".$UID."','".$request['cashcard_password']."','".$request['cashcard_amount']."','เรียบร้อย')");
			echo 'SUCCEED '.' >>> User : ' . $request['Ref1']. ' UID : '.$UID;
		}
		else if ($request['cashcard_amount'] == "500")
		{
			pg_query("UPDATE accounts SET money = money + 660000 WHERE player_id = '".$UID."'");
			pg_query("UPDATE accounts SET kuyraicoin = kuyraicoin + 500 WHERE player_id = '".$UID."'");
			pg_query("UPDATE accounts SET exp = exp + 1500000 WHERE player_id = '".$UID."'");

			pg_query("INSERT INTO player_items (owner_id, item_id, item_name, count, category, equip) VALUES ('".$UID."', 100003157 , AUG A3 CHAMPION , 2592000, 1, 1)");
			pg_query("INSERT INTO player_items (owner_id, item_id, item_name, count, category, equip) VALUES ('".$UID."', 200004142 , Kriss S.V Champion , 2592000, 1, 1)");
			pg_query("INSERT INTO player_items (owner_id, item_id, item_name, count, category, equip) VALUES ('".$UID."', 300005084 , CHEYTAC_M200_CHAMPION , 2592000, 1, 1)");

			pg_query("INSERT INTO player_topup_histories (txid,player_id, true_id, price, status) VALUES ('".$request['TXID']."','".$UID."','".$request['cashcard_password']."','".$request['cashcard_amount']."','เรียบร้อย')");
			echo 'SUCCEED '.' >>> User : ' . $request['Ref1']. ' UID : '.$UID;
		}
		else if ($request['cashcard_amount'] == "1000")
		{
			pg_query("UPDATE accounts SET money = money + 1320000 WHERE player_id = '".$UID."'");
			pg_query("UPDATE accounts SET kuyraicoin = kuyraicoin + 1000 WHERE player_id = '".$UID."'");
			pg_query("UPDATE accounts SET exp = exp + 4000000 WHERE player_id = '".$UID."'");
			pg_query("INSERT INTO player_topup_histories (txid,player_id, true_id, price, status) VALUES ('".$request['TXID']."','".$UID."','".$request['cashcard_password']."','".$request['cashcard_amount']."','เรียบร้อย')");
			echo 'SUCCEED '.' >>> User : ' . $request['Ref1']. ' UID : '.$UID;
		}
	}
	else
	{		
		echo 'ERROR|INVALID_PASSKEY';		
	}
}
else
{
	echo 'ERROR|ACCESS_DENIED';
}
?>
