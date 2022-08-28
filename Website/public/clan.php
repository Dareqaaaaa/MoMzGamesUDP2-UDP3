<?php
@session_start();
include_once('inc/include.php');
error_reporting(0);
?>
<!DOCTYPE html>   
<html lang="en">   
<head>   
<meta charset="utf-8">   
<title>จัดอันดับแคลน</title>  
<link rel="icon" href="icon.ico" /> 
<meta name="description" content="Bootstrap.">  
<link href="https://fonts.googleapis.com/css?family=Mitr" rel="stylesheet">
<link href="http://maxcdn.bootstrapcdn.com/bootstrap/3.2.0/css/bootstrap.min.css" rel="stylesheet">   
<script src="http://ajax.googleapis.com/ajax/libs/jquery/1.7.1/jquery.min.js"></script>
<link rel="stylesheet" href="http://cdn.datatables.net/1.10.2/css/jquery.dataTables.min.css"></style>
<script type="text/javascript" src="http://cdn.datatables.net/1.10.2/js/jquery.dataTables.min.js"></script>
<script type="text/javascript" src="http://maxcdn.bootstrapcdn.com/bootstrap/3.2.0/js/bootstrap.min.js"></script>
 <style type="text/css">
    body {
     font-family: 'Mitr', sans-serif;
   }

   }
 </style>
</head>  
<body style="margin:20px auto">  
<div class="container">
<div class="row header" style="text-align:center;color:green">
<h3>จัดอันดับแคลน</h3>
</div>
<table id="myTable" class="table table-dark" >  
        <thead>  
          <tr>  
            <th>อันดับ</th>  
            <th>ยศ</th>  
            <th>ชื่อแคลน</th>  
            <th>EXP</th> 
            <th>หัวหน้าแคลน</th>
            <th>สมาชิก</th>  
            <th>แข่งทั้งหมด</th>   
            <th>ชนะ</th>      
            <th>แพ้</th> 
            <th>ผู้เล่นฆ่าเยอะที่สุดของแคลน</th> 
            <th>ฆ่า</th>      
          </tr>  
        </thead>  
        <tbody>  

        <?php
                  $strRank = "SELECT * FROM clan_data order by clan_exp desc limit 2000";
                  $num = 1;
                  foreach ($connec->query($strRank) as $row) 
                  {
                    $sqlP = "SELECT * FROM accounts WHERE player_id = '".$row['owner_id']."'";
                    foreach ($connec->query($sqlP) as $row2) 
                    {
                      $namePlayer = $row2['player_name'];    

                      $cPlayer = $connec->prepare("SELECT clan_id FROM accounts WHERE clan_id = '".$row['clan_id']."'");
                      $cPlayer->execute();
                      $countPlayer = $cPlayer->rowCount();  
 
                      $b_Kill = explode("-", $row['best_kills']);  
                      $b_player =  $b_Kill[0];  
                      $b_kill =  $b_Kill[1]; 

                      $best_Kill = "SELECT * FROM accounts WHERE player_id = '".$b_player."'";
                    foreach ($connec->query($best_Kill) as $row3) 
                    {
                      $namePlayer2 = $row3['player_name'];       
                    ?>                               
          <tr>             
            <td><?php echo $num;?></td>  
            <td><img src="img/img_clan/<?php echo pg_escape_string($row['clan_rank']); ?>.jpg")"></td> 
            <td><?php echo pg_escape_string($row['clan_name']); ?></td>  
            <td><?php echo number_format($row['clan_exp']); ?></td> 
            <td><?php echo pg_escape_string($namePlayer); ?></td>  
            <td><?php echo $countPlayer."/".$row['max_players'];?></td> 
            <td><?php echo number_format($row['partidas']); ?></td> 
            <td><?php echo number_format($row['vitorias']); ?></td> 
            <td><?php echo number_format($row['derrotas']); ?></td> 
            <td><?php echo $namePlayer2; ?></td> 
            <td><?php echo $b_kill; ?></td>     
          </tr>         
          <?php
                $num++;
              }
                }
              }
                ?>
        </tbody>  
      </table>  
    </div>
</body>  
<script>
$(document).ready(function(){
    $('#myTable').dataTable();
});
</script>
</html>  