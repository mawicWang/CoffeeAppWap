<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Goods.aspx.cs" Inherits="Goods" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta content="Bevis" name="author" />
    <meta content="application/xhtml+xml;charset=UTF-8" http-equiv="Content-Type"/>
    <meta content="no-cache,must-revalidate" http-equiv="Cache-Control"/>
    <meta content="no-cache" http-equiv="pragma"/>
    <meta content="0" http-equiv="expires"/>
    <meta content="telephone=no, address=no" name="format-detection"/>
    <meta content="width=device-width, initial-scale=1.0, minimum-scale=1.0, maximum-scale=1.0, user-scalable=no" name="viewport"/>
    <title>商品详情</title>
    <link rel="stylesheet" href="css/commons.css"/>
    <link rel="stylesheet" href="css/shop.css"/>
    <link rel="stylesheet" href="css/swiper.min.css">
    <!--<script src="lib/vconsole.min.js"></script>-->
    <script src="lib_cmd/sea.js" type="text/javascript"></script>
    <script type="text/javascript">
        var APP = {
            shopId: 123,
            urls: {
                getLastAddress: "Ajax/ShopListAjax?method=GetLastAddress",
                getShopList: "Ajax/ShopListAjax?method=GetShopList",
                goodsDetailUrl: "Goods",
                toSelectAddress: "AddRessList",
                getGoodsClassify: "Ajax/GoodsAjax?method=GetGoodsClassify",
                getGoodsByClassify: "Ajax/GoodsAjax?method=GetGoodsByClassify",
                toOrderPage: "OrderCreate"
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
            seajs.use("js_cmd/Goods");
        })(location.href.match(/de(\-\d+)bug/));
    </script>
</head>
<body>
<div class="container goods">
    <header>
        <img style="background-image: url(images/banner.png)" alt="">
    </header>
    <section class="wrap_content">
        <div class="wrap_content">
            <div id="wrapItems" class="wrap_items">
                <ul id="itemsList" class="list_items">

                </ul>
            </div>
            <div class="wrap_goods">
                <ul id="goodsList" class="list_goods">
                </ul>
            </div>
        </div>
    </section>
    <footer>
        <div class="wrap_group_btn" style="position: fixed;border-bottom: 0;top: auto;z-index: 30;">
            <div class="wrap_chart">
                <i id="shopCart" class="icon icon_chart"><b id="shopCartNum">0</b></i>
                <div>
                    <span id="currNum" class="numb">0</span>份
                    ¥<span id="currPrice" class="price">0.00</span>
                </div>
            </div>
            <div class="wrap_btns">
                <div id="goOn" class="btn_goon">加入订单</div>
                <div id="toBuy" class="to_buy">去结算</div>
            </div>
        </div>
        <div id="shopCartCon" class="shop_cart">
            <ul id="shopCartList" class="cart_list border">
            </ul>
        </div>
    </footer>

    <section class="wrap_dialog" id="goodSku" ontouchmove="return false;">
        <div class="mask"></div>
        <div class="wrap_content">
            <div class="good_sku">
                <h2 id="goodTitle"></h2>
                <div class="wrap_scroll">
                    <div class="good_scroll">
                        <div class="wrap_header">
                            <div id="headPic" class="wrap_img" style="background-image: url(images/goods_l.png);"></div>
                            <div class="wrap_info">
                                <div class="good_name">
                                    <h3 id="goodName1">Kick Frappe</h3>
                                    <p id="goodName2">Freshly brewed coffee</p>
                                </div>
                                <div class="good_info">
                                    <div class="wrap_price">¥<span id="goodPrice"></span></div>
                                    <div class="wrap_numbr">
                                        <i class="icon icon_minus"></i>
                                        <input id="number" type="text" name="number" value="1" readonly>
                                        <i class="icon icon_add"></i>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="wrap_tabs">
                            <dl id="skuList" class="sku_list">

                            </dl>
                        </div>
                    </div>
                </div>
                <!--<div class="wrap_group_btn">
                    <div class="wrap_chart">
                        <i class="icon icon_chart"><b id="shopCartNum"></b></i>
                        <div>
                            <span id="currNum" class="numb"></span>份
                            ¥<span id="currPrice" class="price"></span>
                        </div>
                    </div>
                    <div class="wrap_btns">
                        <div id="goOn" class="btn_goon">加入订单</div>
                        <div id="toBuy" class="to_buy">去结算</div>
                    </div>
                </div>-->
            </div>
        </div>
    </section>
</div>
</body>
</html>