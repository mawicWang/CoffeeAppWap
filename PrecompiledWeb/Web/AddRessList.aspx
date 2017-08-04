<%@ page language="C#" autoeventwireup="true" inherits="AddRessList, App_Web_addresslist.aspx.cdcab7d2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta content="Bevis" name="author" />
    <meta content="application/xhtml+xml;charset=UTF-8" http-equiv="Content-Type" />
    <meta content="no-cache,must-revalidate" http-equiv="Cache-Control" />
    <meta content="no-cache" http-equiv="pragma" />
    <meta content="0" http-equiv="expires"/>
    <meta content="telephone=no, address=no" name="format-detection"/>
    <meta content="width=device-width, initial-scale=1.0, minimum-scale=1.0, maximum-scale=1.0, user-scalable=no" name="viewport"/>
    <title>�ջ���ַ</title>
    <link rel="stylesheet" href="css/commons.css" type="text/css" />
    <!--<link rel="stylesheet" href="css/addressList.css">-->
    <link rel="stylesheet" href="css/shop.css" type="text/css" />
    <script src="http://api.map.baidu.com/api?v=2.0&ak=rMaOcUnbX2a63hgKiceE4ESWgeu0qAG5" type="text/javascript"></script>
    <script src="lib_cmd/sea.js"  type="text/javascript"></script>
    <script type="text/javascript">
        var APP = {
            urls: {
                shopListUrl: "ShopList",
                getAddressList: "Ajax/AddRessListAjax?method=GetAddressList",
                submitAddress: "Ajax/AddRessListAjax?method=SubmitAddress",
                deleteAddress: "Ajax/AddRessListAjax?method=DeleteAddress"
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
            seajs.use("js_cmd/AddRessList");
        })(location.href.match(/de(\-\d+)bug/));
    </script>
</head>
<body>
<div class="container addressList">
    <header>

    </header>
    <section class="wrap_content">
        <ul class="list_address" id="addressList">
            <li class="address_item border">
                <a href="#">
                    <h3>��÷÷&nbsp;Ůʿ&nbsp;&nbsp;15012341234</h3>
                    <p><label class="tag">��</label>�Ϻ��ж���·122��</p>
                </a>
                <i class="icon icon_edit"></i>
            </li>
        </ul>
    </section>
    <footer>
        <div id="addAddress" class="wrap_footer border">
            <i class="icon icon_address"></i>������ַ
        </div>
    </footer>

    <section class="wrap_dialog " id="addressPage">
        <div class="mask"></div>
        <div class="wrap_content">
            <div class="address_page">
                <form id="addressForm" action="data/msg.json">
                    <ul class="address_form">
                        <li class="border">
                            <span class="item_header">��ϵ��</span>
                            <div class="wrap_input">
                                <input id="name" type="text" name="name" placeholder="��������" required>
                                <input id="addressId" type="hidden" name="addressId">
                            </div>
                        </li>
                        <li class="border">
                            <span class="item_header"></span>
                            <div class="">
                                <label><input id="sex1" type="radio" name="sex" value="����" class="hidden"><i class="icon icon_circle"></i><span>����</span></label>
                                <label><input id="sex2" type="radio" name="sex" value="Ůʿ" class="hidden"><i class="icon icon_circle"></i><span>Ůʿ</span></label>
                            </div>
                        </li>
                        <li class="border">
                            <span class="item_header">��ϵ�绰</span><div class="wrap_input"><input id="telephone" type="tel" name="telephone" placeholder="�����ֻ���"></div>
                        </li>
                        <li class="border">
                            <span class="item_header">����</span><div><input id="city" class="hidden" type="text" name="city" value="�Ϻ�">�Ϻ�</div>
                        </li>
                        <li class="border wrap_address">
                            <span class="item_header">�ջ���ַ</span>
                            <div class="wrap_input ">
                                <div id="toSearchPage" class="border arrow">
                                    <span id="addressShow" class="c-97">С��</span>
                                    <input id="address" type="text" class="hidden" name="address">
                                    <input id="latitude" name="latitude" type="hidden">
                                    <input id="longitude" name="longitude" type="hidden">
                                </div>
                                <div class="wrap_input"><input id="houseNumber" type="text" name="houseNumber" placeholder="��ϸ��ַ�������ƺŵȣ�"></div>
                            </div>
                        </li>
                        <li class="border">
                            <span class="item_header">��ǩ</span>
                            <div>
                                <label><input id="tag1" type="radio" name="tag" value="��" class="hidden"><span class="tag">��</span></label>
                                <label><input id="tag2" type="radio" name="tag" value="��˾" class="hidden"><span class="tag">��˾</span></label>
                            </div>
                        </li>
                    </ul>
                    <div id="submitBtn" class="wrap_submit">ȷ��</div>
                </form>
            </div>
        </div>
    </section>

    <section class="wrap_dialog" id="searchPage">
        <div class="mask"></div>
        <div class="wrap_content">
            <div class="search_page">
                <div class="title border">
                    <span>��ǰλ�ã�</span><span id="currLocation" class="curr_location"></span>
                </div>
                <div class="wrap_search border">
                    <i class="icon icon_search"></i><input id="search" type="search" class="search" placeholder="����С����д��¥��ѧУ"/>
                </div>
                <div id="scrollCon" class="wrap_result">
                    <ul id="resultList" class="result_list">
                        <!--<li>
                            <div class="result_item border">
                                <h3>������Է����</h3>
                                <p>�Ϻ����ֶ������Ŷ�·2281Ū1��145��</p>
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