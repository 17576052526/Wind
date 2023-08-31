$.fn.bannerSlide = function (settings) {
    var param = $.extend({
        interval: 5000,   //间隔5秒切换一次
        speed: 500,    //0.5秒切换完成
        switchStop: 30000,    //手动切换后暂停多久重新启动计时器
    }, settings || {});

    var box = this.children().eq(0);
    var li = box.children();
    //左右各加一个
    box.prepend(li.eq(li.length - 1).clone());
    box.append(li.eq(0).clone());
    //第一个添加类样式（动画）
    li.eq(0).find('[animate]').each(function () {
        var delay = $(this).attr('delay');
        if (delay) {
            setTimeout(() => $(this).addClass($(this).attr('animate')), parseFloat(delay) * 1000);
        } else {
            $(this).addClass($(this).attr('animate'));
        }
    });
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
        ztTime();//手动切换后暂停一段时间重新启动计时器
    });
    //暂停一段时间后重新启动计时器
    function ztTime() {
        clearInterval(time);
        setTimeout(() => setTime(), param.switchStop);
    }
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
            //其他的去掉类样式
            li.find('[animate]').each(function () {
                $(this).removeClass($(this).attr('animate'));
            });
            //切换完后子节点可以添加类样式（动画）
            li.eq(index - 1).find('[animate]').each(function () {
                var delay = $(this).attr('delay');
                if (delay) {
                    setTimeout(() => $(this).addClass($(this).attr('animate')), parseFloat(delay) * 1000);
                } else {
                    $(this).addClass($(this).attr('animate'));
                }
            });
        }, param.speed);
    }
    //计时器
    var time;
    function setTime() {
        clearInterval(time);
        time = setInterval(function () {
            index++;
            exec();
        }, param.interval)
    }
    setTime();
    //上一个
    function previous() {
        index--;
        exec();
        ztTime();//手动切换后暂停一段时间重新启动计时器
    }
    //下一个
    function next() {
        index++;
        exec();
        ztTime();//手动切换后暂停一段时间重新启动计时器
    }
    //移动端左右滑动事件
    var startX, moveX, width = this.width()
    this.get(0).addEventListener('touchstart', function (event) {
        startX = event.touches[0].pageX
        clearInterval(time);//停止计时器
        box.css('transition-duration', '0s');//停止过渡效果
    })
    this.get(0).addEventListener('touchmove', function (event) {
        moveX = event.touches[0].pageX - startX
        box.css('transform', 'translateX(-' + (width * index - moveX) + 'px)');//移动
    })
    this.get(0).addEventListener('touchend', function (event) {
        //重新启动计时器
        setTime();
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

    return [previous, next, (a) => a ? setTime() : clearInterval(time)];//上一个，下一个
}