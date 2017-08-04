<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OrderResult.aspx.cs" Inherits="OrderResult" %>

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
    <script src="http://res.wx.qq.com/open/js/jweixin-1.2.0.js"></script>
    <script>
        (function () {
            wx.config({
                debug: false, // 开启调试模式,调用的所有api的返回值会在客户端alert出来，若要查看传入的参数，可以在pc端打开，参数信息会通过log打出，仅在pc端时才会打印。
                appId: '<%=appId%>',
                timestamp: <%=timestamp%>, //@常老板，服务端填入
                nonceStr: '<%=nonceStr%>', //@常老板，服务端填入
                signature: '<%=signature%>',//@常老板，服务端填入
                jsApiList: [
                    'onMenuShareTimeline',
                    'onMenuShareAppMessage'
                ] // 必填，需要使用的JS接口列表，所有JS接口列表见附录2
            });
            wx.ready(function(){
                initShare();
            });
        })();

        function initShare() {
            wx.onMenuShareTimeline({
                title: 'BuzztimeCoffee', //@常老板，待定，分享标题
                link: '<%=urllink%>', //@常老板，待定，分享链接
                imgUrl: 'http://demo.open.weixin.qq.com/jssdk/images/p2166127561.jpg', //@常老板，待定，分享图标
                trigger: function (res) {
                    // 不要尝试在trigger中使用ajax异步请求修改本次分享的内容，因为客户端分享操作是一个同步操作，这时候使用ajax的回包会还没有返回
                    // alert('用户点击分享到朋友圈');
                },
                success: function (res) {
                    //alert('已分享');
                },
                cancel: function (res) {
                    //alert('已取消');
                },
                fail: function (res) {
                    //alert(JSON.stringify(res));
                }
            });
            console.log('已注册获取“分享到朋友圈”状态事件');
            wx.onMenuShareAppMessage({
                title: 'BuzztimeCoffee', //@常老板，待定，分享标题
                desc: '我刚刚在BuzztimeCoffee下了个订单', //@常老板，待定，分享描述
                link: '<%=urllink%>', //@常老板，待定，分享链接
                imgUrl: 'http://demo.open.weixin.qq.com/jssdk/images/p2166127561.jpg', //@常老板，待定，分享图标
                trigger: function (res) {
                    // 不要尝试在trigger中使用ajax异步请求修改本次分享的内容，因为客户端分享操作是一个同步操作，这时候使用ajax的回包会还没有返回
                    //alert('用户点击发送给朋友');
                },
                success: function (res) {
                    //alert('已分享');
                },
                cancel: function (res) {
                    //alert('已取消');
                },
                fail: function (res) {
                    //alert(JSON.stringify(res));
                }
            });
            console.log('已注册获取“发送给朋友”状态事件');
        }

        var _system={
            $:function(id){return document.getElementById(id);},
            _client:function(){
                return {w:document.documentElement.scrollWidth,h:document.documentElement.scrollHeight,bw:document.documentElement.clientWidth,bh:document.documentElement.clientHeight};
            },
            _scroll:function(){
                return {x:document.documentElement.scrollLeft?document.documentElement.scrollLeft:document.body.scrollLeft,y:document.documentElement.scrollTop?document.documentElement.scrollTop:document.body.scrollTop};
            },
            _cover:function(show){
                if(show){
                    this.$("cover").style.display="block";
                    this.$("cover").style.width=(this._client().bw>this._client().w?this._client().bw:this._client().w)+"px";
                    this.$("cover").style.height=(this._client().bh>this._client().h?this._client().bh:this._client().h)+"px";
                }else{
                    this.$("cover").style.display="none";
                }
            },
            _guide:function(click){
                this._cover(true);
                this.$("guide").style.display="block";
                this.$("guide").style.top=(_system._scroll().y+5)+"px";
                window.onresize=function(){_system._cover(true);_system.$("guide").style.top=(_system._scroll().y+5)+"px";};
                if(click){_system.$("cover").onclick=function(){
                    _system._cover();
                    _system.$("guide").style.display="none";
                    _system.$("cover").onclick=null;
                    window.onresize=null;
                };}
            },
            _zero:function(n){
                return n<0?0:n;
            }
        }


    </script>
    <style type="text/css">
        #cover{display:none;position:absolute;left:0;top:0;z-index:18888;background-color:#000000;opacity:0.7;}
        #guide{display:none;position:absolute;right:18px;top:5px;z-index:19999;}
        #guide img{width:260px;height:180px;padding-top: 0;}
    </style>
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
           <%-- <div><a href="javascript:_system._guide(true)">分享</a></div>--%>
            <div><a href="javascript:_system._guide(true)">去分享</a></div>
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
<div id="cover"></div>
<div id="guide"><img src="images/guide1.png"></div>
</body>
</html>
