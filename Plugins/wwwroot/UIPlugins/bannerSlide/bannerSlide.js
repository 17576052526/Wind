(function ($) {
    $.fn.bannerSlide = function (settings) {
        var param = $.extend({
            interval: 5000,   //间隔5秒切换一次
            speed: 500,    //0.5秒切换完成
            isHoverStop: false,    //悬浮其上停止计时器，移动端一定要是false
            callback: null   //动画完成后的回调，参数一：当前banner索引
        }, settings || {});

        var ul = this.find('ul');
        var ulLi = ul.children('li');
        var ol = this.find('ol');
        var box = this.find('.bannerSlideBox');
        var width = box[0].getBoundingClientRect().width;//获取精确宽度（含小数位）
        //左右各加一个banner
        ul.prepend(ulLi.eq(ulLi.length - 1).clone());
        ul.append(ulLi.eq(0).clone());
        ulLi = ul.children('li');//重新取值
        var index = 1;//当前索引
        var length = ulLi.length;//li数量，显示的banner数量需要减2
        //设置ul、li宽度
        ul.width(length + '00%');
        ulLi.width(100 / length + '%');
        //生成按钮
        for (var i = 0; i < length - 2; i++) { ol.append('<li index="' + (i + 1) + '"></li>'); }
        ol.css("margin-left", -ol.width() / 2);
        var olLi = ol.children('li');
        olLi.eq(0).addClass('bannerSlideCurBtn');
        //绑定按钮单击事件
        olLi.on('click', function () { clearInterval(tims); tims = setInterval(function () { change(index + 1); }, param.interval); change(parseInt($(this).attr('index'))); });
        //设置显示第一个
        box.scrollLeft(width);
        //计时器
        var tims = setInterval(function () { change(index + 1); }, param.interval);
        //悬浮其上停止计时器
        if (param.isHoverStop) { this.hover(function () { clearInterval(tims); }, function () { tims = setInterval(function () { change(index + 1); }, param.interval); }); }
        //浏览器缩放
        $(window).on('resize', function () { width = box[0].getBoundingClientRect().width; box.scrollLeft(width * index); });
        //执行动画
        function change(i) {
            box.stop(true, true).animate({ scrollLeft: width * i }, param.speed, function () {
                if (i == 1 || i == length - 2) {
                    box.scrollLeft(width * i);
                }
                if (param.callback) { param.callback(i); }
            });
            i = i == length - 1 ? 1 : i == 0 ? length - 2 : i;
            //切换按钮
            olLi.removeClass('bannerSlideCurBtn');
            olLi.eq(i - 1).addClass('bannerSlideCurBtn');
            index = i;
        }
        //移动端手指滑动
        var position = {}; //触点位置
        var scroLeft;
        var isLeftRight;//true左右滑动，false上下滑动
        //手指按下
        box.get(0).addEventListener('touchstart', function (e) {
            var touches = e.changedTouches[0];
            position.x = touches.clientX;
            position.y = touches.clientY;
            box.stop();
            scroLeft = box.scrollLeft();
            isLeftRight = null;
        }, false);
        //手指移动
        box.get(0).addEventListener('touchmove', function (e) {
            var touches = e.changedTouches[0];
            var x = touches.clientX - position.x;
            var y = touches.clientY - position.y;
            if (isLeftRight == null) {  //isLeftRight取值
                if (Math.abs(x) > 10 || Math.abs(y) > 10) { //由前面滑动的10px判断左右还是上下滑动
                    isLeftRight = Math.abs(x) > Math.abs(y);
                    if (isLeftRight) { clearInterval(tims); }
                }
            } else if (isLeftRight) {//左右滑动
                e.preventDefault();
                if (scroLeft - x > width * (length - 1)) {//滑动到最右端
                    scroLeft = scroLeft % width;
                } else if (scroLeft - x < 0) {//滑动到最左端
                    scroLeft = (scroLeft % width) + width * (length - 2);
                }
                box.scrollLeft(scroLeft - x);
            }
        }, false);
        //手指抬起
        box.get(0).addEventListener('touchend', function (e) {
            if (isLeftRight) {
                var touches = e.changedTouches[0];
                var x = touches.clientX - position.x;//滑动的距离
                tims = setInterval(function () { change(index + 1); }, param.interval);
                var haif = width / 2 > 200 ? 200 : width / 2;
                if (x > haif) {
                    change(index - 1);
                } else if (x < -haif) {
                    change(index + 1);
                } else {
                    box.animate({ scrollLeft: width * index }, 300, function () { if (param.callback) { param.callback(index); } });
                }
            }
        }, false);
    }
})(jQuery);