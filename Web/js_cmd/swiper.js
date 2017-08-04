define(function(require,exports,module){
    var $ = require("lib_cmd/zepto.js"),
        swiper = require("lib_cmd/swiper.min.js");

    $(function(){
        var mySwiper = new Swiper('.swiper-container', {
            // Optional parameters
//            direction: 'vertical',
            loop: true,

            // If we need pagination
            pagination: '.swiper-pagination',

        })
    });

})
