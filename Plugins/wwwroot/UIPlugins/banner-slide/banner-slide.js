$.fn.bannerSlide = function (settings) {
    var param = $.extend({
        interval: 5000,   //间隔5秒切换一次
        speed: 500,    //0.5秒切换完成
        isHoverStop: false,    //鼠标悬浮其上停止计时器
        isSwitchStop: true,    //手动切换后是否停止计时器
        callback: null   //动画完成后的回调，参数一：当前banner对象
    }, settings || {});

    var box = this.children().eq(0);
    var li = box.children();
    //左右各加一个
    box.prepend(li.eq(li.length - 1).clone());
    box.append(li.eq(0).clone());
    //设置第一个显示
    box.css('transform', 'translateX(-100%)');
    var index = 1;
    //设置过渡效果
    setTimeout(function () { box.css('transition', 'all ' + (param.speed / 1000) + 's'); }, 100);
    //生成按钮
    var btnBox = $('<div class="banner-slide-btnBox"></div>').appendTo(this);
    for (var i = 0; i < li.length; i++) {
        btnBox.append('<div class="banner-slide-btn"></div>')
    }
    btnBox.children().eq(0).addClass('banner-slide-btnActive');
    btnBox.on('click', '.banner-slide-btn', function () {
        index = $(this).index() + 1;
        exec();
        if (param.isSwitchStop) { clearInterval(time) }
    });
    //执行
    function exec() {
        box.css('transform', 'translateX(-' + index + '00%)');
        //动画完成后在执行判断
        setTimeout(function () {
            if (index >= li.length + 1) {
                index = 1;
                box.css('transition-duration', '0s');
                box.css('transform', 'translateX(-' + index + '00%)');
                setTimeout(function () { box.css('transition-duration', (param.speed / 1000) + 's'); }, 100);
            } else if (index <= 0) {
                index = li.length;
                box.css('transition-duration', '0s');
                box.css('transform', 'translateX(-' + index + '00%)');
                setTimeout(function () { box.css('transition-duration', (param.speed / 1000) + 's'); }, 100);
            }
            //按钮选中
            btnBox.children().removeClass('banner-slide-btnActive');
            btnBox.children().eq(index - 1).addClass('banner-slide-btnActive');
            //动画完成后执行的事件
            if (param.callback) {
                param.callback(li.eq(index));
            }
        }, param.speed);
    }
    //开始计时器
    var time = setInterval(function () {
        index++;
        exec();
    }, param.interval)
    //悬浮其上停止计时器
    if (param.isHoverStop) {
        box.on('mouseenter', function () { clearInterval(time) });
        box.on('mouseleave', function () {
            time = setInterval(function () {
                index++;
                exec();
            }, param.interval);
        });
    }
    //上一个
    function previous() {
        index--;
        exec();
        if (param.isSwitchStop) { clearInterval(time) }
    }
    //下一个
    function next() {
        index++;
        exec();
        if (param.isSwitchStop) { clearInterval(time) }
    }
    //移动端左右滑动事件
    var startX, moveX, width = this.width()
    document.addEventListener('touchstart', function (event) {
        startX = event.touches[0].pageX
        clearInterval(time);//停止计时器
        box.css('transition-duration', '0s');//停止过渡效果
    })
    document.addEventListener('touchmove', function (event) {
        moveX = event.touches[0].pageX - startX
        box.css('transform', 'translateX(-' + (width * index - moveX) + 'px)');//移动
    })
    document.addEventListener('touchend', function (event) {
        //重新启动计时器
        time = setInterval(function () {
            index++;
            exec();
        }, param.interval)
        //启动过渡效果
        box.css('transition-duration', (param.speed / 1000) + 's');

        if (moveX < -80) {
            next();
        } else if (moveX > 80) {
            previous();
        } else {
            box.css('transform', 'translateX(-' + index + '00%)');//还原
        }
    })

    return [previous, next];//上一个，下一个
}