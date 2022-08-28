<?php
@session_start();
error_reporting(0);
include_once('inc/include.php');

$cPlayer = $connec->prepare('SELECT player_id FROM accounts');
$cPlayer->execute();
$countPlayer = $cPlayer->rowCount();

$cPlayerOnline = $connec->prepare('SELECT player_id FROM accounts WHERE online = true');
$cPlayerOnline->execute();
$countPlayerOnline = $cPlayerOnline->rowCount();

if (!empty($_SESSION["uid"])) {
  $stmt = $connec->query("SELECT * FROM accounts WHERE player_id = '".$_SESSION["uid"]."'");
        while ($row = $stmt->fetch()) {
            $_SESSION["player_name"] = $row['player_name'];
            $_SESSION["cash"] = $row['money'];
            $_SESSION["rank"] = $row['rank'];
            $_SESSION["exp"] = $row['exp'];
            $_SESSION["coin"] = $row['kuyraicoin'];
          }
}

?>
<!DOCTYPE html>
<html lang="en">
<head>
  <!-- secured by Farkhost.com -->
  <!-- Required meta tags always come first -->
  <meta charset="utf-8">
  <meta name="viewport" content="width=device-width, initial-scale=1, shrink-to-fit=no">
  <meta http-equiv="x-ua-compatible" content="ie=edge">
  <title>PBKuyraiV2 PVP8x8</title>
  <link rel="icon" href="icon.ico" />
  <!-- Font Awesome -->
  <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.css">
  <!-- Bootstrap core CSS -->
  <link href="css/bootstrap.css" rel="stylesheet">
  <link href="css/bootstrap.min.css" rel="stylesheet">
  <!-- Material Design Bootstrap -->
  <link href="css/mdb.css" rel="stylesheet">
  <link href="css/mdb.min.css" rel="stylesheet">

  <link rel='stylesheet' id='wsl-widget-css'  href='https://mdbootstrap.com/wp-content/plugins/wordpress-social-login/assets/css/style.css?ver=4.9.8' type='text/css' media='all' />
<link rel='stylesheet' id='compiled.css-css'  href='https://mdbootstrap.com/wp-content/themes/mdbootstrap4/css/compiled-4.5.10.min.css?ver=4.5.10' type='text/css' media='all' />

  <link href="https://fonts.googleapis.com/css?family=Mitr" rel="stylesheet">
  <link rel="icon" href="favicon.ico" type="img/ico">
  <link rel="stylesheet" href="//cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/css/toastr.min.css">
  <style>
      /* Required for full background image */
      body {
        font-family: 'Mitr', sans-serif;
    }
    html,
    body,
    header,
    .view {
        height: 100%;
    }

    @media (max-width: 740px) {
        html,
        body,
        header,
        .view {
          height: 1000px;
      }
  }
  @media (min-width: 800px) and (max-width: 850px) {
    html,
    body,
    header,
    .view {
      height: 650px;
  }
}

.top-nav-collapse {
    background-color: #06090f !important;
}

.navbar:not(.top-nav-collapse) {
    background: transparent !important;
}

@media (max-width: 991px) {
    .navbar:not(.top-nav-collapse) {
      background: #06090f !important;
  }
  .bg {
    /* The image used */
    background-image: url("https://mdbootstrap.com/img/Photos/Horizontal/Nature/full page/img(20).jpg");

    /* Full height */
    height: 100%;

    /* Center and scale the image nicely */
    background-position: center;
    background-repeat: no-repeat;
    background-size: cover;
}
}

.rgba-gradient {
    background: -webkit-linear-gradient(45deg, rgba(0, 0, 0, 0.7), rgba(72, 15, 144, 0.4) 100%);
    background: -webkit-gradient(linear, 45deg, from(rgba(0, 0, 0, 0.7), rgba(72, 15, 144, 0.4) 100%)));
background: linear-gradient(to 45deg, rgba(0, 0, 0, 0.7), rgba(72, 15, 144, 0.4) 100%);
}

.card {
    background-color: rgba(126, 123, 215, 0.2);
}

.md-form label {
    color: #ffffff;
}

h6 {
    line-height: 1.7;
}

</style>
</head>

<body>

  <div id="fb-root"></div>
<script>(function(d, s, id) {
  var js, fjs = d.getElementsByTagName(s)[0];
  if (d.getElementById(id)) return;
  js = d.createElement(s); js.id = id;
  js.src = 'https://connect.facebook.net/th_TH/sdk.js#xfbml=1&version=v3.1&appId=750875391686504&autoLogAppEvents=1';
  fjs.parentNode.insertBefore(js, fjs);
}(document, 'script', 'facebook-jssdk'));</script>

  <!-- Main navigation -->
  <header>
    <!-- Navbar -->
    <nav class="navbar navbar-expand-lg navbar-dark fixed-top scrolling-navbar" style="border-bottom: 5px solid #067cbd">
      <div class="container">
        <a class="navbar-brand" href="#">
            <strong>PBKuyraiV2</strong>
        </a>
        <button class="navbar-toggler" type="button" data-toggle="collapse" data-target="#navbarSupportedContent-7" aria-controls="navbarSupportedContent-7" aria-expanded="false" aria-label="Toggle navigation">
          <span class="navbar-toggler-icon"></span>
      </button>
      <div class="collapse navbar-collapse" id="navbarSupportedContent-7">
          <ul class="navbar-nav mr-auto">

            <?php
            if (!empty($_SESSION["uid"])) {
              ?>      
          <li class="nav-item">
              <a class="nav-link" href="rank.php" target="_blank"><i class="fa fa-bar-chart" aria-hidden="true"></i>อันดับผู้เล่น</a>
          </li> 
          <li class="nav-item">
              <a class="nav-link" href="clan.php" target="_blank"><i class="fa fa-bar-chart" aria-hidden="true"></i>อันดับแคลน</a>
          </li>           
          <li class="nav-item">
              <a class="nav-link" data-toggle="modal" data-target="#topup"><i class="fa fa-credit-card" aria-hidden="true"></i>เติมเงิน</a>
          </li>
          <li class="nav-item">
              <a class="nav-link" data-toggle="modal" data-target="#itemcode"><i class="fa fa-gift" aria-hidden="true"></i>เติมไอเท็มโค้ด</a>
          </li>
          <li class="nav-item">
            <form class="form-horizontal" method="post" action="share.php">
              <input name="user_id" type="hidden" class="form-control form-control-sm" value="<?php echo $_SESSION["uid"]; ?>">
              <button type="submit" style="margin: 5px auto auto auto;"><i class="fa fa-gift" aria-hidden="true"></i> แชร์รับ 5 KuyraiCoin</button>
            </form>
          </li>
          <?php
      }else{
        ?>
        <li class="nav-item">
              <a class="nav-link" href="rank.php" target="_blank"><i class="fa fa-bar-chart" aria-hidden="true"></i>อันดับผู้เล่น</a>
          </li>     
          <li class="nav-item">
              <a class="nav-link" href="clan.php" target="_blank"><i class="fa fa-bar-chart" aria-hidden="true"></i>อันดับแคลน</a>
          </li>                     
      <li class="nav-item">
              <a class="nav-link" href="https://goo.gl/FmtpGN" target="_blank"><i class="fa fa-download" aria-hidden="true"></i>ดาวน์โหลดเกมส์</a>
          </li>  
      <?php 
  }
  ?>
</ul>
<form class="form-inline">
    <div class="md-form my-0">
      <ul class="navbar-nav mr-auto">
        <?php
        if (!empty($_SESSION["uid"])) {
          ?>
          <li class="nav-item">
              <div class="btn-group">
                <button type="button" class="btn btn-danger">
<i class="fa fa-user-o" aria-hidden="true"></i>
ชื่อตัวละคร : <?php echo $_SESSION["player_name"];?></button>
                <button type="button" class="btn btn-danger dropdown-toggle px-3" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
                    <span class="sr-only">Toggle Dropdown</span>
                </button>
                <div class="dropdown-menu">
                    <a class="dropdown-item" href="#">ยศ : <img src="img/img_rank/<?php echo $_SESSION["rank"];?>.gif")"></a>
                    <a class="dropdown-item" href="#">EXP : <?php echo number_format($_SESSION["exp"]);?></a>
                    <a class="dropdown-item" href="#">แครช : <?php echo number_format($_SESSION["cash"]);?></a>
                    <a class="dropdown-item" href="#">KuyraiCoin : <?php echo number_format($_SESSION["coin"]);?></a>
                    <div class="dropdown-divider"></div>
                    <a class="dropdown-item" data-toggle="modal" data-target="#chang_pass"><i class="fa fa-edit" aria-hidden="true"></i> เปลี่ยนรหัสผ่าน</a>
                    <a class="dropdown-item" href="logout.php"><i class="fa fa-sign-out" aria-hidden="true"></i> ออกจากระบบ</a>
                </div>
            </div>
            
        </li>
        <?php
    }else{
        ?>
        <li class="nav-item">
          <a class="nav-link" data-toggle="modal" data-target="#register"><i class="fa fa-user-plus" aria-hidden="true"></i>สมัครสมาชิก</a>
      </li>
      <li class="nav-item">
          <a class="nav-link" data-toggle="modal" data-target="#login"><i class="fa fa-sign-in" aria-hidden="true"></i>เข้าสู่ระบบ</a>
      </li>
      <?php 
  }
  ?>
</ul>
</div>
</form>
</div>
</div>
</nav>
<!-- Navbar -->
<!-- Full Page Intro -->
    <!-- Content -->
    <div class="container">
<!-- Modal -->
<div class="modal fade" id="topup" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
  <div class="modal-dialog" role="document">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title" id="exampleModalLabel">เติมเงิน</h5>
        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
          <span aria-hidden="true">&times;</span>
      </button>
  </div>
  <div class="modal-body">
      <script type="text/javascript" src='https://www.tmtopup.com/topup/3rdTopup.php?uid=218204'></script>
      <div class="form-group">
          <input name="tmn_password" type="text" id="tmn_password" maxlength="14" class="form-control form-control-sm" placeholder="รหัสบัตร 14 หลัก">
        </div>
        <div class="form-group">
          <input name="ref1" type="text" id="ref1" class="form-control form-control-sm" value="<?php echo $_SESSION["login"]; ?>" readonly>
        </div>
        <div class="form-group">
          <input name="ref2" type="hidden" id="ref2" value="PB-Kuyrai">
        </div>
        <div style="float: right; margin: auto auto 15px auto;"><button type="button" onclick="submit_tmnc()" class="btn btn-success" data-dismiss="modal"><i class="fa fa-credit-card" aria-hidden="true"></i> ยืนยันเติมเงิน</button></div>
		<h3 style="color: red;"> *ออกเกมก่อนเติมทุกครั้ง </h3>

    <!--<img class="d-block w-100" src="img/sl/sl3.png"><hr>
    <img class="d-block w-100" src="img/sl/sl1.png"><hr>
    <img class="d-block w-100" src="img/sl/sl2.png"><hr>-->

        <table class="table table-hover">
          <thead>
            <tr>
              <th scope="col">ราคาบัตร<font color="#ED1C22" style="font-weight: bolder;"> True</font><font color="#F9821A" style="font-weight: bolder;">Money</font></th>
              <th scope="col">แครช</th>
              <th scope="col">EXP</th>
              <th scope="col">KuyraiCoin</th>
            </tr>
          </thead>
          <tbody>
            <tr class="table-active">
              <th scope="row">50</th>
              <td>36,000</td>
              <td>120,000</td>
              <td>50</td>
            </tr>
            <tr class="table-active">
              <th scope="row">90</th>
              <td>90,000 </td>
              <td>250,000</td>
              <td>90</td>
            </tr>
            <tr class="table-active">
              <th scope="row">150</th>
              <td>156,000</td>
              <td>450,000</td>
              <td>150</td>
            </tr>
            <tr class="table-active">
              <th scope="row">300</th>
              <td>324,000</td>
              <td>900,000</td>
              <td>300</td>
            </tr>
            <tr class="table-active">
              <th scope="row">500</th>
              <td>660,000</td>
              <td>1,500,000</td>
              <td>500</td>
            </tr>
            <tr class="table-active">
              <th scope="row">1,000</th>
              <td>1,320,000</td>
              <td>4,000,000</td>
              <td>1,000</td>
            </tr>
          </tbody>
        </table>

</div>
</form>
</div>
</div>
</div>
</div>

<!-- Modal -->
<div class="modal fade" id="itemcode" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
  <div class="modal-dialog" role="document">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title" id="exampleModalLabel">เติมไอเท็มโค้ด</h5>
        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
          <span aria-hidden="true">&times;</span>
      </button>
  </div>
  <div class="modal-body">
    <form class="form-horizontal" method="post" action="item_code_topup.php">
    <div class="form-group">
          <input name="username" type="text" class="form-control form-control-sm" value="<?php echo $_SESSION["login"]; ?>" readonly>
          <input name="uid" type="hidden" class="form-control form-control-sm" value="<?php echo $_SESSION["uid"]; ?>" readonly>
        </div>
      <div class="form-group">
          <input name="item_code" type="text" class="form-control form-control-sm" placeholder="รหัสไอเท็มโค้ด">
        </div>       
        <button type="button" class="btn btn-outline-danger waves-effect" data-dismiss="modal">ปิด</button>
        <button type="submit" class="btn btn-outline-primary waves-effect">เติมไอเท็มโค้ด</button>
    </form>
</div>
</div>
</div>
</div>
</div>


<!-- Modal -->
<div class="modal fade" id="login" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
  <div class="modal-dialog" role="document">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title" id="exampleModalLabel">เข้าสู่ระบบ</h5>
        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
          <span aria-hidden="true">&times;</span>
      </button>
  </div>
  <div class="modal-body">
    <form class="form-horizontal" method="post" action="check_login.php">
      <div class="md-form">
        <i class="fa fa-user-circle-o prefix"></i>
        <input type="text" name="username" id="inputValidationEx" class="form-control validate">
        <label for="form2" class="active" style="color: red;">ชื่อผู้ใช้งาน</label>
    </div>
    <div class="md-form">
        <i class="fa fa-lock prefix"></i>
        <input type="password" name="password" id="inputValidationEx" class="form-control validate">
        <label for="form3" class="active"  style="color: red;">รหัสผ่าน</label>
    </div>
</div>
<div class="modal-footer">
    <button type="button" class="btn btn-outline-danger waves-effect" data-dismiss="modal">ปิด</button>
    <button type="submit" class="btn btn-outline-primary waves-effect">เข้าสู่ระบบ</button>
</form>
</div>
</div>
</div>
</div>

<!-- Modal -->
<div class="modal fade" id="chang_pass" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
  <div class="modal-dialog" role="document">
    <div class="modal-content">
      <div class="modal-header">
        <h5 class="modal-title" id="exampleModalLabel">แก้ไขรหัสผ่าน</h5>
        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
          <span aria-hidden="true">&times;</span>
      </button>
  </div>
  <div class="modal-body">
    <form class="form-horizontal" method="post" action="chang_pass.php">
      <div class="md-form">
        <i class="fa fa-lock prefix"></i>
        <input type="password" name="pass_old" id="inputValidationEx" class="form-control validate">
        <label for="form2" class="active" style="color: red;">รหัสผ่านเดิม</label>
    </div>
    <div class="md-form">
        <i class="fa fa-lock prefix"></i>
        <input type="password" name="pass_new" id="inputValidationEx" class="form-control validate">
        <label for="form3" class="active"  style="color: red;" min="4">รหัสผ่านใหม่ (*ตัวพิมพ์เล็กเท่านั้น!!)</label>
    </div>
</div>
<div class="modal-footer">
    <button type="button" class="btn btn-outline-danger waves-effect" data-dismiss="modal">ปิด</button>
    <button type="submit" class="btn btn-outline-primary waves-effect">เปลี่ยนรหัสผ่าน</button>
</form>
</div>
</div>
</div>
</div>



<!-- Full Height Modal Right -->
<div class="modal fade right" id="register" tabindex="-1" role="dialog" aria-labelledby="myModalLabel" aria-hidden="true" style="opacity: 0.7;
filter: alpha(opacity=70);">

<!-- Add class .modal-full-height and then add class .modal-right (or other classes from list above) to set a position to the modal -->
<div class="modal-dialog modal-full-height modal-right" role="document">


  <div class="modal-content">
    <div class="modal-header">
      <h4 class="modal-title w-100" id="myModalLabel">สมัครสมาชิก</h4>
      <button type="button" class="close" data-dismiss="modal" aria-label="Close">
        <span aria-hidden="true">&times;</span>
    </button>
</div>
<div class="modal-body" style="color: red;">

    <div class="card-body">
      <!--Header-->
      <div class="text-center">
        <h3 class="white-text">
          <i class="fa fa-user white-text"></i> <div style="color: grey;">กรอกข้อมูล</div></h3>
          <hr class="hr-light">
      </div>
      <!--Body-->
      
      <!-- Material input -->
      <form class="form-horizontal" method="post" action="save_reg.php">
          <div class="md-form">
            <i class="fa fa-user-circle-o prefix"></i>
            <input type="text" name="username" id="inputValidationEx" class="form-control validate">
            <label for="form2" class="active" style="color: red;" min="4">ไอดี (*ตัวพิมพ์เล็กเท่านั้น!!)</label>
        </div>

        <div class="md-form">
            <i class="fa fa-envelope prefix"></i>
            <input type="email" name="email" id="inputValidationEx" class="form-control validate">
            <label for="inputValidationEx" data-error="ไม่ถูกต้อง" data-success="right"  style="color: red;">อีเมล</label>
        </div>

        <div class="md-form">
            <i class="fa fa-lock prefix"></i>
            <input type="password" name="password" id="inputValidationEx" class="form-control validate">
            <label for="form3" class="active"  style="color: red;" min="4">รหัสผ่าน (*ตัวพิมพ์เล็กเท่านั้น!!)</label>
        </div>

        <div class="md-form">
            <i class="fa fa-lock prefix"></i>
            <input type="password" name="re-password" id="inputValidationEx" class="form-control validate">
            <label for="form4" class="active"  style="color: red;" min="4">ยืนยันรหัสผ่าน</label>
        </div>
    </div>

</div>
<div class="modal-footer justify-content-center">
  <button type="button" class="btn btn-outline-danger waves-effect" data-dismiss="modal">ปิด</button>
  <button type="submit" class="btn btn-outline-primary waves-effect">สมัครสมาชิก</button>
</form>
</div>
</div>
<!-- Full Height Modal Right -->

</div>
<!-- Content -->
</div>
<!-- Mask & flexbox options-->
</div>
<!-- Full Page Intro -->

<div style="
background: url('img/pb/bgweb.png') no-repeat center center fixed;
    -webkit-background-size: cover;
    -moz-background-size: cover;
    -o-background-size: cover;
    background-size: cover;
  color:#fff;
    background-color:#333;">
    <div class="mask pattern-2 flex-center waves-effect waves-light">
<div class="container-fluid" style="width: 90%;">  
<div class="row">
  <!-- col-md-3 -->
    <div class="col-md-3">
<hr>
<hr>
<hr>
<hr>
      <div class="card">
    <div class="card-header white-text" style="border-bottom: 5px solid #067cbd; background-color: #06090f;"><i class="fa fa-server" aria-hidden="true"></i> สถานะเซิฟเวอร์</div>
    <div class="card-body">

      <div class="card text-white bg-dark">
    <div class="card-body" style="background-color: #05090e;">
      <div class="row"> 

        <div class="col-md-6">
          <i class="fa fa-users"></i> ไอดีทั้งหมด
        </div>

        <div class="col-md-6">
          <font color='green' style="color: #FF1493;"><?php echo number_format($countPlayer); ?></font>
        </div>

        <div class="col-md-6">
          <i class="fa fa-server"></i> เซิฟเวอร์
        </div>

        <div class="col-md-6">
          <font color='green' style="color: #66FF33;">ออนไลน์</font>
        </div>

        <div class="col-md-6">
          <i class="fa fa-star"></i> Exp Bonus
        </div>

        <div class="col-md-6">
          <font color='yellow' style="color: #FFFF00;">+10,000%</font>
        </div>

        <div class="col-md-6">
          <i class="fa fa-star"></i> Point Bonus
        </div>

        <div class="col-md-6">
          <font color='yellow' style="color: #FFFF00;">+3,000%</font>
        </div>

        <div class="col-md-6">
          <i class="fa fa-users"></i> ผู้เล่นออนไลน์
        </div>

        <div class="col-md-6">
          <font color='green' style="color: #00FFFF;"><?php echo number_format($countPlayerOnline + 0); ?></font>
        </div>

      </div> 
    </div>
  </div>  

    </div>
</div>

<div class="card">
    <div class="card-header white-text" style="border-bottom: 5px solid #067cbd; background-color: #06090f;"><i class="fa fa-download" aria-hidden="true"></i> ดาวน์โหลดตัวเกมส์</div>
    <div class="card-body">
      <center>
        <a href="https://goo.gl/FmtpGN" class="btn btn-danger btn-rounded" target="_blank"> &nbsp;&nbsp;&nbsp;&nbsp;<i class="fa fa-download" aria-hidden="true"></i> ดาวน์โหลดตัวเกมส์-ตัวเต็ม&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</a>

        <a href="https://download.microsoft.com/download/1/B/E/1BE39E79-7E39-46A3-96FF-047F95396215/dotNetFx40_Full_setup.exe" class="btn btn-danger btn-rounded" target="_blank"><i class="fa fa-download" aria-hidden="true"></i> แก้เออเร่อ .net framework 4.0</a>

      </center>
      
    </div>
  </div>

<div class="card">
    <div class="card-header white-text" style="border-bottom: 5px solid #067cbd; background-color: #06090f;"><i class="fa fa-facebook-official" aria-hidden="true"></i> แฟนเพจ</div>
    <div class="card-body">

        <div id="fb-root"></div>
<script>(function(d, s, id) {
  var js, fjs = d.getElementsByTagName(s)[0];
  if (d.getElementById(id)) return;
  js = d.createElement(s); js.id = id;
  js.src = 'https://connect.facebook.net/th_TH/sdk.js#xfbml=1&version=v3.1&appId=750875391686504&autoLogAppEvents=1';
  fjs.parentNode.insertBefore(js, fjs);
}(document, 'script', 'facebook-jssdk'));</script>

<div class="fb-page" data-href="https://www.facebook.com/pbkuyrai/" data-tabs="timeline" data-small-header="true" data-adapt-container-width="true" data-hide-cover="false" data-show-facepile="true"><blockquote cite="https://www.facebook.com/pbkuyrai/" class="fb-xfbml-parse-ignore"><a href="https://www.facebook.com/pbkuyrai/">PB KuyRai</a></blockquote></div>

      </div>
    </div>

    </div>
    <!--end col-md-3 -->

    <!-- col-md-6 -->
    <div class="col-md-6">
<hr>
<hr>
<hr>
<hr>

<div class="card">
    <div class="card-header white-text" style="border-bottom: 5px solid #067cbd; background-color: #06090f;"><i class="fa fa-shopping-cart" aria-hidden="true"></i> PBKuyrai-Shop</div>
    <div class="card-body">


<!-- Classic tabs -->
<div class="classic-tabs mx-2" style="background-color: #06090f;">

  <!-- Nav tabs -->
  <ul class="nav tabs" role="tablist" style="background-color: #070911; border-bottom: 5px solid #067cbd;">
    <li class="nav-item">
      <a class="nav-link waves-light active" data-toggle="tab" href="#panel1001" role="tab">ปืน</a>
    </li>
    <li class="nav-item">
      <a class="nav-link waves-light" data-toggle="tab" href="#panel1002" role="tab">ตัวละคร/หน้ากาก</a>
    </li>
    <li class="nav-item">
      <a class="nav-link waves-light" data-toggle="tab" href="#panel1003" role="tab">ไอเท็ม/กล่องสุ่ม</a>
    </li>
  </ul>
  <div class="tab-content border-right border-bottom border-left rounded-bottom">
    <!--Panel 1-->
    <div class="tab-pane fade in show active" id="panel1001" role="tabpanel">
      <div class="row">
        <?php
                  $strShop = "SELECT * FROM web_item_shop WHERE category = '1' AND open_shop = '1' order by tag_id";
                  foreach ($connec->query($strShop) as $row) 
                  {
                    $count = $row['count'] / 24 / 60 / 60;
                    ?>  

      <div class="col-md-4">
        <div class="card mb-2">
          <form class="form-horizontal" method="post" action="buy_item.php">
            <input type="hidden" name="item_id" value="<?php echo $row['item_id'];?>">
          <img class="card-img-top" src="<?php echo $row['img_url'];?>"
            alt="Card image cap">
          <div class="card-body">
            <p class="card-text" style="color: #ffffff;">
              ชื่อ : <font color="#66ff31"><?php echo $row['item_name'];?></font><br>
              ราคา : <font color="#ff1493"><?php echo $row['price'];?></font> KuyraiCoin<br>
              ได้รับ : <font color="#ff1493"><?php echo $count; ?></font> วัน
            </p>
            <hr>
            <center><button type="submit" class="btn btn-outline-danger btn-rounded waves-effect">ซื้อ</button></center>
          </form>
          </div>
        </div>
      </div>

      <?php
    }
    ?>
      </div>
    </div>
    <!--/.Panel 1-->
    <!--Panel 2-->
    <div class="tab-pane fade" id="panel1002" role="tabpanel">
      <div class="row">
        <?php
                  $strShop = "SELECT * FROM web_item_shop WHERE category = '2' AND open_shop = '1' order by tag_id";
                  foreach ($connec->query($strShop) as $row) 
                  {
                    $count = $row['count'] / 24 / 60 / 60;
                    ?>  

      <div class="col-md-4">
        <div class="card mb-2">
          <form class="form-horizontal" method="post" action="buy_item.php">
            <input type="hidden" name="item_id" value="<?php echo $row['item_id'];?>">
          <img class="card-img-top" src="<?php echo $row['img_url'];?>"
            alt="Card image cap">
          <div class="card-body">
            <p class="card-text" style="color: #3190e0;">
              ชื่อ : <font color="#66ff31"><?php echo $row['item_name'];?></font><br>
              ราคา : <font color="#ff1493"><?php echo $row['price'];?></font> KuyraiCoin<br>
              ได้รับ : <font color="#ff1493"><?php echo $count; ?></font> วัน
            </p>
            <hr>
            <center><button type="submit" class="btn btn-outline-danger btn-rounded waves-effect">ซื้อ</button></center>
          </form>
          </div>
        </div>
      </div>

      <?php
    }
    ?>
      </div>
    </div>
    <!--/.Panel 2-->
    <!--Panel 3-->
    <div class="tab-pane fade" id="panel1003" role="tabpanel">
      <div class="row">
        <?php
                  $strShop = "SELECT * FROM web_item_shop WHERE category = '3' AND open_shop = '1' order by tag_id";
                  foreach ($connec->query($strShop) as $row) 
                  {
                    $count = $row['count'] / 24 / 60 / 60;
                    if ($row['count'] <= 86400) {
                      $count = $row['count'];
                    }
                    ?>  

      <div class="col-md-4">
        <div class="card mb-2">
          <form class="form-horizontal" method="post" action="buy_item.php">
            <input type="hidden" name="item_id" value="<?php echo $row['item_id'];?>">
          <img class="card-img-top" src="<?php echo $row['img_url'];?>"
            alt="Card image cap">
          <div class="card-body">
            <p class="card-text" style="color: #3190e0;">
              ชื่อ : <font color="#66ff31"><?php echo $row['item_name'];?></font><br>
              ราคา : <font color="#ff1493"><?php echo $row['price'];?></font> KuyraiCoin<br>
              ได้รับ : <font color="#ff1493"><?php echo $count; ?></font> วัน/ชิ้น
            </p>
            <hr>
            <center><button type="submit" class="btn btn-outline-danger btn-rounded waves-effect">ซื้อ</button></center>
          </form>
          </div>
        </div>
      </div>

      <?php
    }
    ?>
      </div>
    </div>
    <!--/.Panel 3-->
  </div>

</div>
<!-- Classic tabs -->

      </div>
    </div>


      <div class="card">
    <div class="card-header white-text" style="border-bottom: 5px solid #067cbd; background-color: #06090f;"><i class="fa fa-newspaper-o" aria-hidden="true"></i> ข่าวสารอัพเดท</div>
    <div class="card-body">
     
      <hr>

      <div class="view overlay zoom">
    <img src="img/pb/promote.jpg" class="img-fluid " alt="zoom" style="width: 100%">
    <div class="mask flex-center waves-effect waves-light">
    </div>
</div>
<hr>

        <a href="https://goo.gl/n4aymz" target="_blank"><div class="view overlay zoom">
    <img src="img/fix/fix1.png" class="img-fluid " alt="zoom" style="width: 100%">
    <div class="mask flex-center waves-effect waves-light">
    </div>
</div></a>
  <hr>

  <div class="view overlay zoom">
    <img src="img/fix/config.png" class="img-fluid " alt="zoom" style="width: 100%">
    <div class="mask flex-center waves-effect waves-light">
    </div>
</div>
<hr>

<div class="view overlay zoom">
    <img src="img/fix/rank.jpg" class="img-fluid " alt="zoom" style="width: 100%">
    <div class="mask flex-center waves-effect waves-light">
    </div>
</div>

    </div>
</div>
      
        
    </div>
    <!--end col-md-6 -->

    <!-- col-md-3 -->
    <div class="col-md-3">
<hr>
<hr>
<hr>
<hr>
<div class="card">
    <div class="card-header white-text" style="border-bottom: 5px solid #067cbd; background-color: #06090f;"><i class="fa fa-bar-chart" aria-hidden="true"></i> Top 5 ผู้เล่น</div>
    <div class="card-body">
        <table id="myTable" class="table table-dark" style="background-color: #05090e;">  
        <thead>  
          <tr>  
            <th>อันดับ</th>  
            <th>ยศ</th>  
            <th>ชื่อ</th>  
            <th>KDR</th>                 
          </tr>  
        </thead>  
        <tbody>  

        <?php
                  $strRank = "SELECT * FROM accounts order by exp desc limit 5";
                  $num = 1;
                  foreach ($connec->query($strRank) as $row) 
                  {
                    $rank_killdeath   =     round($row['kills_count'] / ($row['kills_count']+$row['deaths_count']) * 100)."%";
                    ?>                               
          <tr>             
            <td><?php echo $num;?></td>  
            <td><img src="img/img_rank/<?php echo pg_escape_string($row['rank']); ?>.gif")"></td> 
            <td><?php echo pg_escape_string($row['player_name']); ?></td>    
            <td><?php echo $rank_killdeath; ?></td>       
          </tr>         
          <?php
                $num++;
              }
                ?>
        </tbody>  
      </table> 
    </div>
</div>


      <div class="card">
    <div class="card-header white-text" style="border-bottom: 5px solid #067cbd; background-color: #06090f;"><i class="fa fa-bar-chart" aria-hidden="true"></i> Top 5 แคลน</div>
    <div class="card-body">
          <table id="myTable" class="table table-dark" style="background-color: #05090e;">
          <thead>  
          <tr>  
            <th>อันดับ</th>  
            <th>ยศ</th>  
            <th>ชื่อ</th>  
            <th>EXP</th>                 
          </tr>  
        </thead>  
        <tbody>  

        <?php
                  $strClan = "SELECT * FROM clan_data order by clan_exp desc limit 5";
                  $num = 1;
                  foreach ($connec->query($strClan) as $row) 
                  {
                    ?>                               
          <tr>             
            <td><?php echo $num;?></td>  
            <td><img src="img/img_clan/<?php echo pg_escape_string($row['clan_rank']); ?>.jpg")"></td> 
            <td><?php echo pg_escape_string($row['clan_name']); ?></td>    
            <td><?php echo pg_escape_string($row['clan_exp']); ?></td>       
          </tr>         
          <?php
                $num++;
              }
                ?>
        </tbody>  
      </table>

    </div>
</div>

    </div>
    <!--end col-md-3 -->
  </div>

  </div>
  </div>
</div>
  <!--end cortuner -->

</header>
<!-- Main navigation -->

<!-- SCRIPTS -->
<!-- JQuery -->
<script type="text/javascript" src="js/jquery-3.3.1.min.js"></script>
<!-- Tooltips -->
<script type="text/javascript" src="js/popper.min.js"></script>
<!-- Bootstrap core JavaScript -->
<script type="text/javascript" src="js/bootstrap.min.js"></script>
<!-- MDB core JavaScript -->
<script type="text/javascript" src="js/mdb.min.js"></script>
<script>
  new WOW().init();
</script>
<script type="text/javascript" src="//cdnjs.cloudflare.com/ajax/libs/toastr.js/latest/js/toastr.min.js"></script>
<script>
   @if(Session::has('message'))
   var type="{{Session::get('alert-type','info')}}"
   switch(type){
    case 'info':
    toastr.info("{{ Session::get('message') }}");
    break;
    case 'success':
    toastr.success("{{ Session::get('message') }}");
    break;
    case 'warning':
    toastr.warning("{{ Session::get('message') }}");
    break;
    case 'error':
    toastr.error("{{ Session::get('message') }}");
    break;
}
@endif
</script>
<script>
    $(function(){
        <?php
        // toastr output & session reset
        session_start();
        if(isset($_SESSION['toastr']))
        {
            echo 'toastr.'.$_SESSION['toastr']['type'].'("'.$_SESSION['toastr']['message'].'", "'.$_SESSION['toastr']['title'].'")';
            unset($_SESSION['toastr']);
        }
        ?>          
    });
</script>
</body>

</html>