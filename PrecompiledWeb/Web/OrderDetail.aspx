<%@ page language="C#" autoeventwireup="true" inherits="OrderDetail, App_Web_orderdetail.aspx.cdcab7d2" %>

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
    <link rel="stylesheet" href="css/shop.css?v=2">
    <!--<script src="lib/vconsole.min.js"></script>-->
</head>
<body>
<div class="container orderDetail">
    <header>

        <div class="wrap_status">
            <div class="wrap_img" style="background-image: url(imgs/order_1.png)"></div>
            <span>
                <% if (orderInfo.deliveryType == 0)
                {%>
                <h2>订单已完成</h2>
                <p>预计<%=orderInfo.deliveryMaxTime.Value.ToString("HH:mm")%>之前送达</p>
                <%} else { %>
                <h2>订单已完成</h2>
                <p>请尽快到店自取</p>
                <%} %>
            </span>
        </div>
        <div class="wrap_process">
            <ul>
                <%if(orderInfo.payStatus == 0) { %>
                <li>订单未支付<i class="line"></i></li>
                <%} else { %>
                 <li class="on">订单已支付<i class="line"></i></li>
                <%} %>

                <% if (orderInfo.deliveryType == 0)
                {%>
                    <%if (orderInfo.orderState == 4)
                      {%>
                        <li class="on">配送中<i class="line"></i></li>
                        <li class="on">配送完成<i class="line"></i></li>
                    <%}
                      else if (orderInfo.orderState == 3 || orderInfo.orderState == 5 || orderInfo.orderState == 6 || orderInfo.orderState == 7)
                      { %>
                         <li class="on">配送中<i class="line"></i></li>
                         <li>配送完成<i class="line"></i></li>
                    <%} %>
                    <%else
                        { %>
                         <li>配送中<i class="line"></i></li>
                         <li>配送完成<i class="line"></i></li>
                    <%} %>
                <%} else { %>
                     <%if (orderInfo.orderState == 4)
                       {%>
                        <li  class="on">未自取<i class="line"></i></li>
                        <li  class="on">自取完成<i class="line"></i></li>
                    <%}
                       else
                       {%>
                       <li>未自取<i class="line"></i></li>
                       <li>自取完成<i class="line"></i></li>
                    <%} %>
                <%} %>
            </ul>
        </div>

    </header>
    <section class="wrap_content">
        <div class="order_info border">
            <%--<div class="shop_info border">
            <%=restaurant == null ? string.Empty : restaurant.name%><a href="tel:<%=tel%>" class="icon icon_tel"></a>
            </div>--%>
            <div class="shop_info border"><%=restaurant == null ? string.Empty : restaurant.name%><a href="tel:<%=tel%>" class="icon icon_tel"></a></div>
            <% if (orderInfo != null && orderInfo.distributionId.HasValue && orderInfo.distributionId.Value > 0)
               {%>
            <div class="rider_info border">骑手电话：<%=distributionTel%><a href="tel:<%=distributionTel%>" class="icon icon_tel"></a></div>
            <%} %>
            <ul class="goods_info border">
            <% foreach (var item in orderInfo.listCOrderCommodityRelation)
               {%>
                <li>
                    <span><%=item.chineseName + (string.IsNullOrWhiteSpace(item.chinesePropertyName) ? string.Empty : "(" + item.chinesePropertyName + ")")%></span><span>X<%=item.quantity%>&nbsp;&nbsp;&nbsp;&nbsp;￥<%=item.price%></span>
                </li>
                <%} %>
            </ul>
            <div class="delivery_fee border"><span>配送费</span><span>￥<%=orderInfo.serverFee%></span></div>
            <%if (!string.IsNullOrWhiteSpace(orderInfo.couponName))
              {%>
            <div class="minus_money border"><span><%=orderInfo.couponName%></span><span>￥<%=(orderInfo.payMomey - orderInfo.serverFee - orderInfo.orderMomey)%> </span></div>
            <%} %>
            <div class="total_money"><span>实付 ￥<%=orderInfo.payMomey%></span></div>
        </div>
        <div class="transport_info">
            <div class="delivery_time border"><span>送达时间</span><span><%=orderInfo.deliveryMinTime.Value.ToString("yyyy-MM-dd")%>  <%=orderInfo.deliveryMinTime.Value.ToString("HH:mm")%>-<%=orderInfo.deliveryMaxTime.Value.ToString("HH:mm")%><%=orderInfo.isOutOfTime == true ? "(尽快送达)" : string.Empty %></span></div>
            <div class="delivery_time border"><span>下单时间</span><span><%=orderInfo.orderTime.ToString("yyyy-MM-dd HH:mm:ss")%></span></div>
            <div class="order_tips border"><span>备注</span><span><%=orderInfo.remark%></span></div>
            <div class="delivery_address">
                <span>配送地址</span>
                <div>
                    <%if (orderInfo.address != null){ %>
                    <h2><%=orderInfo.address.name%>&nbsp;<%=orderInfo.address.sex%>&nbsp;&nbsp;<%=orderInfo.address.telephone%></h2>
                    <p><label class="tag"><%=orderInfo.address.label%></label><span><%=orderInfo.address.address + " " + orderInfo.address.houseNumber%></span></p>
                    <%} %>
                </div>
            </div>
        </div>
    </section>
    <footer></footer>
</div>
</body>
</html>