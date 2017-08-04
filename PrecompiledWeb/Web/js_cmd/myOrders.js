define(function(require,exports,module){
    var $ = require("lib_cmd/zepto.js"),
        myDialog = require("lib_cmd/myDialog.js");

    $(function(){
        $(".icon_delete").on("click",function(){
            var self=$(this),
                orderId=self.find("span").html();
            console.log(orderId);
            var deleteConfirm = confirm("确定删除订单记录吗？",{
                callBack:function(evt){
                    var that = this,ele = null;
                    if(evt && (ele = evt.target) && ("BUTTON" == ele.tagName) && $(ele).hasClass("_btn_confirm")){
                        var _l=window.loading();
                        $.ajax({
                            url:APP.urls.deleteOrder,
                            type:"POST",
                            dataType:"json",
                            data:{OrderId:orderId},
                            success:function(res){
                                _l.destroy();
                                var data=res.Data;
                                if(res.Status!==0){
                                    tip('删除订单记录失败，请稍后再试。', { classes: "otip", t: 2000 });
                                    return null
                                }
                                location.reload();
                            }
                        })
                        that.close();
                    }
                    return that;
                }
            });
        })
    })

})
