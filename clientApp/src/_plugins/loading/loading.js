/* eslint-disable *///此处不是注释，react当中不校验代码，不加此句react当中会报错
/*
加载超过一定时间出现 loading图
    $(box).loading(interval)：经过 interval（毫秒）时间后，在box节点内出现loading图
    $(box).loadingStop()：停止 box里面的 loading图显示
*/
var _loading_timer = [];
$.fn.loading = function (interval) {
    var cur = this;
    _loading_timer.push(setTimeout(function () {
        cur.append('<div class="loading-box"><div class="loading1"><span></span><span></span><span></span><span></span><span></span></div></div>');
        if (cur.css('position') == 'static') { cur.css('position', 'relative') }
    }, interval || 1500));
}
$.fn.loadingStop = function () {
    this.children('.loading-box').eq(0).remove();
    if (_loading_timer.length > 0) { clearTimeout(_loading_timer[0]); _loading_timer.splice(0, 1); }
}