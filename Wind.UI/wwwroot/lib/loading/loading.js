﻿/*
加载超过一定时间出现 loading图
    $(box).loading(interval)：经过 interval（毫秒）时间后，在box节点内出现loading图
    $(box).loadingStop()：停止 box里面的 loading图显示
*/
+function ($) {
    var timer;
    $.fn.loading = function (interval) {
        var cur = this;
        timer = setTimeout(function () {
            cur.append('<div class="fixed-center-box"><div class="loading1"><span></span><span></span><span></span><span></span><span></span></div></div>');
            if (cur.css('position') == 'static') { cur.css('position', 'relative') }
        }, interval || 2000);
    }
    $.fn.loadingStop = function () {
        this.children('.fixed-center-box').remove();
        clearTimeout(timer);
    }
}(jQuery)