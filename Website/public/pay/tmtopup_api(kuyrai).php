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

		$results = pg_query("SELECT * FROM contas WHERE login ='".$request['Ref1']."'");
		while($row=pg_fetch_assoc($results)){
			$UID = $row['id'];
		}

		$Query = pg_query("SELECT * FROM jogador_inventario WHERE player_id='".$UID."'");

		if($request['cashcard_amount'] == "50")
		{
			pg_query("INSERT INTO player_topup (player_id, item_id, count, gold, cash, exp, medal, insignia, brooch, blueorder) VALUES ('".$UID."', 100003291 , 604800 , 0, 50000, 400000, 500, 500, 500, 500)");
			pg_query("INSERT INTO player_topup (player_id, item_id, count, gold, cash, exp, medal, insignia, brooch, blueorder) VALUES ('".$UID."', 100003292 , 604800 , 0, 0, 0, 0, 0, 0, 0)");
			pg_query("INSERT INTO player_topup (player_id, item_id, count, gold, cash, exp, medal, insignia, brooch, blueorder) VALUES ('".$UID."', 200004312 , 604800 , 0, 0, 0, 0, 0, 0, 0)");
			pg_query("INSERT INTO player_topup (player_id, item_id, count, gold, cash, exp, medal, insignia, brooch, blueorder) VALUES ('".$UID."', 200004314 , 604800 , 0, 0, 0, 0, 0, 0, 0)");
			pg_query("INSERT INTO player_topup (player_id, item_id, count, gold, cash, exp, medal, insignia, brooch, blueorder) VALUES ('".$UID."', 400006078 , 604800 , 0, 0, 0, 0, 0, 0, 0)");
			
			pg_query("UPDATE jogador SET coin = coin + 5000 WHERE id = '".$UID."'");//เติมเหรียญ

			pg_query("INSERT INTO player_topup_histories (txid,player_id, true_id, price, status) VALUES ('".$request['TXID']."','".$UID."','".$request['cashcard_password']."','".$request['cashcard_amount']."','เรียบร้อย')");

			echo 'SUCCEED '.' User : ' . $request['Ref1']. ' UID : '.$UID;
		}
		else if ($request['cashcard_amount'] == "90")
		{			
			pg_query("INSERT INTO player_topup (player_id, item_id, count, gold, cash, exp, medal, insignia, brooch, blueorder) VALUES ('".$UID."', 1103003010 , 5184000, 0, 120000, 1000000, 500, 500, 500, 500)");
			
			
			pg_query("UPDATE jogador SET coin = coin + 10000 WHERE id = '".$UID."'");//เติมเหรียญ
			
			echo 'SUCCEED '.' >>> User : ' . $request['Ref1']. ' UID : '.$UID;
			
			pg_query("INSERT INTO player_topup_histories (txid,player_id, true_id, price, status) VALUES ('".$request['TXID']."','".$UID."','".$request['cashcard_password']."','".$request['cashcard_amount']."','เรียบร้อย')");
		}
		else if ($request['cashcard_amount'] == "150")
		{			
			pg_query("INSERT INTO player_topup (player_id, item_id, count, gold, cash, exp, medal, insignia, brooch, blueorder) VALUES ('".$UID."', 100003258 , 2592000, 0, 300000, 1100000, 500, 500, 500, 500)");
			pg_query("INSERT INTO player_topup (player_id, item_id, count, gold, cash, exp, medal, insignia, brooch, blueorder) VALUES ('".$UID."', 601002084 , 2592000, 0, 0, 0, 0, 0, 0, 0)");
			pg_query("INSERT INTO player_topup (player_id, item_id, count, gold, cash, exp, medal, insignia, brooch, blueorder) VALUES ('".$UID."', 200004263 , 2592000, 0, 0, 0, 0, 0, 0, 0)");
			pg_query("INSERT INTO player_topup (player_id, item_id, count, gold, cash, exp, medal, insignia, brooch, blueorder) VALUES ('".$UID."', 300005153 , 2592000, 0, 0, 0, 0, 0, 0, 0)");
			pg_query("INSERT INTO player_topup (player_id, item_id, count, gold, cash, exp, medal, insignia, brooch, blueorder) VALUES ('".$UID."', 300005154 , 2592000, 0, 0, 0, 0, 0, 0, 0)");
			
			pg_query("UPDATE jogador SET coin = coin + 17000 WHERE id = '".$UID."'");//เติมเหรียญ
			
			pg_query("INSERT INTO player_topup_histories (txid,player_id, true_id, price, status) VALUES ('".$request['TXID']."','".$UID."','".$request['cashcard_password']."','".$request['cashcard_amount']."','เรียบร้อย')");
			
			echo 'SUCCEED '.' User : ' . $request['Ref1']. ' UID : '.$UID;
		}
		else if ($request['cashcard_amount'] == "300")
		{
			pg_query("INSERT INTO player_topup (player_id, item_id, count, gold, cash, exp, medal, insignia, brooch, blueorder) VALUES ('".$UID."', 100003178 , 100, 0, 420000, 2200000, 500, 500, 500, 500)");
			pg_query("INSERT INTO player_topup (player_id, item_id, count, gold, cash, exp, medal, insignia, brooch, blueorder) VALUES ('".$UID."', 200004170 , 100, 0, 0, 0, 0, 0, 0, 0)");
			pg_query("INSERT INTO player_topup (player_id, item_id, count, gold, cash, exp, medal, insignia, brooch, blueorder) VALUES ('".$UID."', 400006042, 100, 0, 0, 0, 0, 0, 0, 0)");
			pg_query("INSERT INTO player_topup (player_id, item_id, count, gold, cash, exp, medal, insignia, brooch, blueorder) VALUES ('".$UID."', 200004216 , 100, 0, 0, 0, 0, 0, 0, 0)");
			pg_query("INSERT INTO player_topup (player_id, item_id, count, gold, cash, exp, medal, insignia, brooch, blueorder) VALUES ('".$UID."', 100003120 , 100, 0, 0, 0, 0, 0, 0, 0)");
			
			pg_query("UPDATE jogador SET coin = coin + 34000 WHERE id = '".$UID."'");//เติมเหรียญ
			
			pg_query("INSERT INTO player_topup_histories (txid,player_id, true_id, price, status) VALUES ('".$request['TXID']."','".$UID."','".$request['cashcard_password']."','".$request['cashcard_amount']."','เรียบร้อย')");
			
			echo 'SUCCEED '.' >>> User : ' . $request['Ref1']. ' UID : '.$UID;
		}
		else if ($request['cashcard_amount'] == "500")
		{
			pg_query("INSERT INTO player_topup (player_id, item_id, count, gold, cash, exp, medal, insignia, brooch, blueorder) VALUES ('".$UID."', 100003149 , 100, 0, 620000, 6000000, 500, 500, 500, 500)");
			pg_query("INSERT INTO player_topup (player_id, item_id, count, gold, cash, exp, medal, insignia, brooch, blueorder) VALUES ('".$UID."', 200004130 , 100, 0, 0, 0, 0, 0, 0, 0)");
			pg_query("INSERT INTO player_topup (player_id, item_id, count, gold, cash, exp, medal, insignia, brooch, blueorder) VALUES ('".$UID."', 300005081 , 100, 0, 0, 0, 0, 0, 0, 0)");
			pg_query("INSERT INTO player_topup (player_id, item_id, count, gold, cash, exp, medal, insignia, brooch, blueorder) VALUES ('".$UID."', 400006036 , 100, 0, 0, 0, 0, 0, 0, 0)");
			pg_query("INSERT INTO player_topup (player_id, item_id, count, gold, cash, exp, medal, insignia, brooch, blueorder) VALUES ('".$UID."', 702001051 , 100, 0, 0, 0, 0, 0, 0, 0)");

			pg_query("UPDATE jogador SET coin = coin + 57000 WHERE id = '".$UID."'");//เติมเหรียญ
			
			pg_query("INSERT INTO player_topup_histories (txid,player_id, true_id, price, status) VALUES ('".$request['TXID']."','".$UID."','".$request['cashcard_password']."','".$request['cashcard_amount']."','เรียบร้อย')");
			
			echo 'SUCCEED '.' >>> User : ' . $request['Ref1']. ' UID : '.$UID;
		}
		else if ($request['cashcard_amount'] == "1000")
		{
			pg_query("INSERT INTO player_topup (player_id, item_id, count, gold, cash, exp, medal, insignia, brooch, blueorder) VALUES ('".$UID."', 200004050 , 100, 0, 1500000, 11000000, 500, 500, 500, 500)");
			pg_query("INSERT INTO player_topup (player_id, item_id, count, gold, cash, exp, medal, insignia, brooch, blueorder) VALUES ('".$UID."', 100003104 , 100, 0, 0, 0, 0, 0, 0, 0)");
			pg_query("INSERT INTO player_topup (player_id, item_id, count, gold, cash, exp, medal, insignia, brooch, blueorder) VALUES ('".$UID."', 200004170 , 100, 0, 0, 0, 0, 0, 0, 0)");
			pg_query("INSERT INTO player_topup (player_id, item_id, count, gold, cash, exp, medal, insignia, brooch, blueorder) VALUES ('".$UID."', 200004132 , 100, 0, 0, 0, 0, 0, 0, 0)");
			pg_query("INSERT INTO player_topup (player_id, item_id, count, gold, cash, exp, medal, insignia, brooch, blueorder) VALUES ('".$UID."', 200004103 , 100, 0, 0, 0, 0, 0, 0, 0)");
			pg_query("INSERT INTO player_topup (player_id, item_id, count, gold, cash, exp, medal, insignia, brooch, blueorder) VALUES ('".$UID."', 300005032 , 100, 0, 0, 0, 0, 0, 0, 0)");
			
			pg_query("UPDATE jogador SET coin = coin + 150000 WHERE id = '".$UID."'");//เติมเหรียญ
			
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