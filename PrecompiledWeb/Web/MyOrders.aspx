<%@ page language="C#" autoeventwireup="true" inherits="MyOrders, App_Web_myorders.aspx.cdcab7d2" %>

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
    <script src="lib_cmd/sea.js"  type="text/javascript"></script>
    <script type="text/javascript">
        var APP = {
            shopId: "",
            urls: {
                deleteOrder: "Ajax/GoodsAjax?method=DeleteOrder"
            }
        }
    </script>
    <script type ="text/javascript">
        (function (l) {
            //                l=l||[,"-min"];
            seajs.config({
                base: "./",
                //                    base:"./build",
                map: [
                    [".js", (l && l[1] || "") + ".js?v=20141027"]
                ]
            });
            seajs.use("js_cmd/myOrders");
        })(location.href.match(/de(\-\d+)bug/));
    </script>
</head>
<body>
<div class="container orders">
    <header>
    </header>
    <section class="wrap_content">
        <ul class="order_list">
            <%listOrderInfo.ForEach(item =>
              {%>
            <li>
                <a href="OrderDetail?id=<%=item.Id%>">
                    <div class="wrap_img" style="background-image: url(<%=string.IsNullOrWhiteSpace(item.ResImgUrl) ? "images/good.png" : item.ResImgUrl%>)"></div>
                    <div class="wrap_info">
                        <h2><%=item.ResName%></h2>
                        <p>订单时间：<%=item.OrderTime.ToString("yyyy-MM-dd HH:mm:ss")%></p>
                        <div><span><%=item.DeliveryType%></span><span><%=item.PayStatus == 0 ? "未支付" : "已支付"%></span></div>
                    </div>
                    <div class="icon icon_delete"><span hidden><%=item.Id%></span></div>
                </a>
            </li>
            <%});%>
        </ul>
    </section>
    <footer></footer>

</div>
</body>
</html>
