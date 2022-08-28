<?php
@session_start();
include_once('inc/include.php');
error_reporting(0);
?>
<!DOCTYPE html>   
<html lang="en">   
<head>   
<meta charset="utf-8">   
<title>จัดอันดับผู้เล่น</title>  
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
<h3>จัดอันดับผู้เล่น</h3>
</div>
<table id="myTable" class="table table-dark" >  
        <thead>  
          <tr>  
            <th>อันดับ</th>  
            <th>ยศ</th>  
            <th>ชื่อ</th>  
            <th>EXP</th>
            <th>ชนะ</th> 
            <th>แพ้</th> 
            <th>Win Rate</th> 
            <th>ฆ่า</th>   
            <th>ตาย</th> 
            <th>KDR</th> 
            <th>ยิงหัว</th>       
            <th>สถานะ</th>                
          </tr>  
        </thead>  
        <tbody>  

        <?php
                  $strRank = "SELECT * FROM accounts order by exp desc limit 2000";
                  $num = 1;
                  foreach ($connec->query($strRank) as $row) 
                  {
                    $rank_killdeath   =     round($row['kills_count'] / ($row['kills_count']+$row['deaths_count']) * 100)."%";
                    $rank_winrate   =   round($row['fights_win'] / ($row['fights_win']+$row['fights_lost']) * 100)."%";
                    ?>                               
          <tr>             
            <td><?php echo $num;?></td>  
            <td><img src="img/img_rank/<?php echo pg_escape_string($row['rank']); ?>.gif")"></td> 
            <td><?php echo pg_escape_string($row['player_name']); ?></td>  
            <td><?php echo number_format($row['exp']); ?></td>  
            <td><?php echo number_format($row['fights_win']); ?></td>
            <td><?php echo number_format($row['fights_lost']); ?></td>
            <td><?php echo $rank_winrate; ?></td>  
            <td><?php echo number_format($row['kills_count']); ?></td>
            <td><?php echo number_format($row['deaths_count']); ?></td> 
            <td><?php echo $rank_killdeath; ?></td>  
            <td><?php echo number_format($row['headshots_count']); ?></td> 
            <td>
              <?php if($row['online'] == true){ ?>
                <span class="badge badge-pill badge-success" style="background-color: green;">Online</span>
            <?php }else { ?>
                <span class="badge badge-pill badge-success" style="background-color: red;">Offline</span>
            </td>     
          </tr>         
          <?php
      }
                $num++;
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