/*
banner图切换（淡入淡出）
bannerFade() 参数：
    interval：切换的时间间隔
    isHoverStop：是否悬浮其上停止计时器
其他：
    .banner-fade-item：项
    .banner-fade-active 显示项
*/
$.fn.bannerFade = function (interval, isHoverStop) {
    var items = this.find('.banner-fade-item');
    var index = 0;
    //执行切换
    function exec() {
        if (index > items.length - 1) { index = 0; }
        items.removeClass('banner-fade-active');
        items.eq(index).addClass('banner-fade-active');
        //按钮切换
        btns.removeClass('banner-fade-btnActive');
        btns.eq(index).addClass('banner-fade-btnActive');
    }
    var timer = setInterval(function () { index++; exec();}, interval);
    //悬浮其上
    if (isHoverStop) {
        this.hover(function () { clearInterval(timer); }, function () { timer = setInterval(function () { index++; exec(); }, interval); });
    }
    //生成按钮
    var btnBox = $('<div class="banner-fade-btnBox"></div>').appendTo(this);
    for (var i = 0; i < items.length; i++) {
        btnBox.append('<div class="banner-fade-btn" index="'+i+'"></div>')
    }
    var btns = btnBox.find('.banner-fade-btn');
    btns.eq(index).addClass('banner-fade-btnActive');
    btnBox.on('click', '.banner-fade-btn', function () {
        index = $(this).attr('index');
        exec();
    });
}