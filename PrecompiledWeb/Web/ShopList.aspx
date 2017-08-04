﻿<%@ page language="C#" autoeventwireup="true" inherits="ShopList, App_Web_shoplist.aspx.cdcab7d2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html lang="en">
<head>
    <meta content="Bevis" name="author">
    <meta content="application/xhtml+xml;charset=UTF-8" http-equiv="Content-Type">
    <meta content="no-cache,must-revalidate" http-equiv="Cache-Control">
    <meta content="no-cache" http-equiv="pragma">
    <meta content="0" http-equiv="expires">
    <meta content="telephone=no, address=no" name="format-detection">
    <meta content="width=device-width, initial-scale=1.0, minimum-scale=1.0, maximum-scale=1.0, user-scalable=no" name="viewport">
    <title>BuzzTime Coffee</title>
    <link rel="stylesheet" href="css/commons.css">
    <link rel="stylesheet" href="css/shop.css">
    <%--<script src="lib/vconsole.min.js"></script>--%>
    <script src="http://api.map.baidu.com/api?v=2.0&ak=rMaOcUnbX2a63hgKiceE4ESWgeu0qAG5"></script>
    <script src="lib_cmd/sea.js"></script>
    <script type="text/javascript">
        var APP = {
            urls: {
                getLastAddress: "Ajax/ShopListAjax?method=GetLastAddress",
                getShopList: "Ajax/ShopListAjax?method=GetShopList",
                GetShopListContainOutRange: "Ajax/ShopListAjax?method=GetShopListContainOutRange",
                goodsDetailUrl: "Goods",
                toSelectAddress: "AddRessList"
            }
        }
    </script>
    <script type="text/javascript">
        (function (l) {
            //                l=l||[,"-min"];
            seajs.config({
                base: "./",
                //                    base:"./build",
                map: [
                    [".js", (l && l[1] || "") + ".js?v=20141027"]
                ]
            });
            seajs.use("js_cmd/ShopList");
        })(location.href.match(/de(\-\d+)bug/));
    </script>
</head>
<body>
   <div class="container shopList">
       <header>
           <span class="icon icon_location"></span>
           <a id="locationSelect"  href="AddRessList" class="title text_ellipsis"><span id="currPosi">李女士 上海静安寺</span><i class="icon arrow_title"></i></a>
       </header>
       <section class="wrap_content">
           <ul class="list_shop" id="shopList">
               <%--<li class="shop_item border">
                   <div class="wrap_img"><img style="background-image: url(images/shop.png);"></div>
                   <div class="wrap_info">
                       <div class="title">
                           <h2 class="text_ellipsis">BuzzTime Coffee(吴江路店)</h2>
                           <p>吴江路四季坊街上(食之秘旁)</p>
                       </div>
                       <div class="location"><span class="distence"><1.2km</span><span class="to_the_shop">到店自取<i class="icon arrow_down"></i></span></div>
                   </div>
               </li>
               <li class="shop_item border">
                   <div class="wrap_img"><img style="background-image: url(images/shop.png);"></div>
                   <div class="wrap_info">
                       <div class="title">
                           <h2 class="text_ellipsis">BuzzTime Coffee(吴江路店)</h2>
                           <p>吴江路四季坊街上(食之秘旁)</p>
                       </div>
                       <div class="location"><span class="distence"><1.2km</span><span class="to_the_shop">到店自取<i class="icon arrow_down"></i></span></div>
                   </div>
               </li>--%>
           </ul>
       </section>
       <footer>
           <ul id="tabs" class="wrap_tabs border">
               <li class="on" data-type="0">外卖<i class="icon icon_car"></i></li>
               <li data-type="1">到店自取<i class="icon icon_shop"></i></li>
           </ul>
       </footer>

       <section class="wrap_dialog" id="searchPage">
           <div class="mask"></div>
           <div class="wrap_content">
               <div class="search_page">
                   <div class="title border">
                       <span>当前位置：</span><span id="currLocation" class="curr_location">上海浦东新区东方路135号</span>
                   </div>
                   <div class="wrap_search border">
                       <i class="icon icon_search"></i><input id="search" type="search" class="search" placeholder="搜索小区、写字楼、学校"/><span id="searchBtn" class="search_btn">搜索</span>
                   </div>
                   <div id="scrollCon" class="wrap_result">
                       <ul id="resultList" class="result_list">
                           <!--<li>
                               <div class="result_item border">
                                   <h3>玉兰香苑四期</h3>
                                   <p>上海市浦东新区张东路2281弄1～145号</p>
                               </div>
                           </li>-->
                       </ul>
                   </div>
               </div>
           </div>
       </section>
   </div>
</body>
</html>