<?php
require_once('include.php');
?>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<link href='https://fonts.googleapis.com/css?family=Kanit&subset=thai,latin' rel='stylesheet' type='text/css'>
<meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
<meta http-equiv="X-UA-Compatible" content="IE=edge">
<meta http-equiv="pragma" content="no-cache">
<meta http-equiv="cache-control" content="no-cache">
<meta name="keywords" content="Project Thailand">
<meta name="description" content="Welcome to Project Thailand official web-site!">
<title>News | Project Thailand</title>	
<script async src="//www.google-analytics.com/analytics.js"></script>
<script>
//<![CDATA[
  (function(i,s,o,g,r,a,m){i['GoogleAnalyticsObject']=r;i[r]=i[r]||function(){
  (i[r].q=i[r].q||[]).push(arguments)},i[r].l=1*new Date();a=s.createElement(o),
  m=s.getElementsByTagName(o)[0];a.async=1;a.src=g;m.parentNode.insertBefore(a,m)
  })(window,document,'script','//www.google-analytics.com/analytics.js','ga');

  ga('create', 'UA-46870417-2', 'auto');
  ga('send', 'pageview');
//]]>
</script>
<style type="text/css">
	html,body{width:100%;height:100%;font-family:Arial,dotum,돋움;}
	html{overflow:hidden;}
	body { }
	body,div,ul,li{margin:0;padding:0; list-style:none;}
	#launcherwap {background:url(images/launcher_bg_new.gif) no-repeat; width:445px; height:152px; overflow:hidden;}
	#launcherwap .ltit {clear:both; height:22px; text-align:center; font-size:14px; font-weight:bold; color:#a9e6ff; padding:0 0 0 0; margin:0 0 8px 0; font-family: Tahoma;}
	#launcherwap .ltit a {}
	#launcherwap .ltit a:hover {}
	#launcherwap .bbsL {float:left; width:356px; height:24px; padding:0 0 0 7px;font-size:12px; color:#ffffff; }
	#launcherwap .bbsL a {display:inline-block; width:340px; font-size:12px; color:#ffffff; text-overflow:ellipsis; white-space:nowrap; overflow:hidden; font-family: Tahoma; text-decoration:none;}
	#launcherwap .bbsL a:hover { color:#a9e6ff; text-decoration:underline;}
	#launcherwap .bbsR {float:left; width:82px; height:24px; text-align:center; font-size:12px; color:#ffffff;}
</style>
</head>
<body style="overflow:hidden" oncontextmenu="return false" onselectstart="return false" ondragstart="return false">
<ul id="launcherwap">
	<li class="ltit"><a>ประกาศ</a></li>
	<?php 
    $query = pg_query("SELECT * FROM newsweb ORDER BY id DESC;") or die(mysql_error()); 
	while($array = pg_fetch_object($query))
	{
    ?>
      <tr>
		<?php
		$date = new DateTime($array->date);
		?>
			<?php
			if ($array->url == "#")
			{
			?>
				<li class="bbsL">* <a href="#"><?php echo $array->msg;?></a></li>
			<?php
			}
			else
			{
			?>
				<li class="bbsL">* <a href="<?php echo $array->url;?>" target="_blank"><?php echo $array->msg;?></a></li>
			<?php
			}
			?>
			<li class="bbsR"><?php echo $date->format('d.M.Y')?></li>
      </tr>
      <?php 
	  }
	  ?>  
</ul>
</body>
</html>