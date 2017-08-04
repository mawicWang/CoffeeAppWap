<%@ page language="C#" autoeventwireup="true" inherits="OrderResult, App_Web_orderresult.aspx.cdcab7d2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta content="Bevis" name="author">
    <meta content="application/xhtml+xml;charset=UTF-8" http-equiv="Content-Type">
    <meta content="no-cache,must-revalidate" http-equiv="Cache-Control">
    <meta content="no-cache" http-equiv="pragma">
    <meta content="0" http-equiv="expires">
    <meta content="telephone=no, address=no" name="format-detection">
    <meta content="width=device-width, initial-scale=1.0, minimum-scale=1.0, maximum-scale=1.0, user-scalable=no" name="viewport">
    <title>商品详情</title>
    <link rel="stylesheet" href="css/commons.css">
    <link rel="stylesheet" href="css/shop.css">
    <!--<script src="lib/vconsole.min.js"></script>-->
</head>
<body>
<div class="container order_result">
    <header>
        <div class="wrap_tips">
            <div class="wrap_img"></div>
            <h2>下单成功</h2>
        </div>
        <div class="wrap_btns">
            <div><a href="ShopList">返回首页</a></div>
            <div><a href="MyOrders">我的订单</a></div>
        </div>
    </header>
    <section class="wrap_content">
        <div class="wrap_line">
            <p><i class="icon icon_heart"></i>你可能会喜欢</p>
        </div>
        <ul class="goods_list">
            <%listCommodityInfo.ForEach(item => { %>
                <li>
                    <div class="wrap_img" style="background-image: url(<%=item.picPath%>);"></div>
                    <div class="wrap_info">
                        <h3><%=item.chineseName%></h3>
                        <p><%=item.englishName%></p>
                    </div>
                </li>
            <%  });%>
        </ul>
    </section>
    <footer></footer>

</div>
</body>
</html>
