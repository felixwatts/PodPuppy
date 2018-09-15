<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Strict//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>PodPuppy - Free Podcast Receiver</title>
    <link rel="stylesheet" type="text/css" href="stylesheet.css" />
</head>
<body>  
  <?php

  $msg = "From\r\n====\r\n\r\n". $_POST["email"] . "\r\n\r\nDescription\r\n===========\r\n\r\n" . $_POST["desc"] . "\r\n\r\n" . $_POST["report"];
  
  mail("felix@fwatts.info", "PodPuppy Bug Report", $msg);
  
  ?>
</body>
</html>
