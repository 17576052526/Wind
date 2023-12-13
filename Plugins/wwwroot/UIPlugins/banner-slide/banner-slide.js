//interval 切换的间隔，speed 切换的速度，switchStop 手动切换后暂停时间
$.fn.bannerSlide = function (interval, speed, switchStop) {
    var box = this.children('.banner-slide-box');
    var items = box.children();
    //左右各加一个
    box.prepend(items.eq(items.length - 1).clone());
    box.append(items.eq(0).clone());
    //设置第一个显示
    box.css('transform', 'translateX(-100%)');
    //设置过渡效果
    setTimeout(function () { box.css('transition', 'all ' + speed + 'ms'); }, 100);
    //生成按钮
    var btnBox = $('<div class="banner-slide-btnBox"></div>').appendTo(this);
    for (var i = 0; i < items.length; i++) {
        btnBox.append('<div class="banner-slide-btn"></div>')
    }
    btnBox.children().eq(0).addClass('banner-slide-btnActive');
    btnBox.on('click', '.banner-slide-btn', function () {
        exec($(this).index() + 1);
        handSwitch();
    });
    //第一个添加类样式（动画）
    items.eq(0).find('[animate]').each(function () { $(this).addClass($(this).attr('animate')) });
    //手动切换事件
    function handSwitch() {
        clearInterval(time);
        setTimeout(() => setTime(), switchStop);
    }
    //切换
    var index = 1;
    function exec(i) {
        box.css('transform', 'translateX(-' + i + '00%)');
        index = i >= items.length + 1 ? 1 : i <= 0 ? items.length : i;
        //动画完成后在执行判断
        setTimeout(function () {
            if (i >= items.length + 1 || i <= 0) {
                box.css({ 'transition-duration': '0ms', 'transform': 'translateX(-' + index + '00%)' });
                setTimeout(function () { box.css('transition-duration', speed + 'ms'); }, 100);
            }
            //按钮选中
            btnBox.children().removeClass('banner-slide-btnActive');
            btnBox.children().eq(index - 1).addClass('banner-slide-btnActive');
            //动画显示
            items.find('[animate]').each(function () { $(this).removeClass($(this).attr('animate')); });
            items.eq(index - 1).find('[animate]').each(function () { $(this).addClass($(this).attr('animate')); });
        }, speed);
    }
    //计时器
    var time;
    function setTime() {
        clearInterval(time);
        time = setInterval(function () { exec(index + 1) }, interval);
    }
    setTime();
    //上一个
    function previous() {
        exec(index - 1);
        handSwitch();
    }
    //下一个
    function next() {
        exec(index + 1);
        handSwitch();
    }
    //移动端左右滑动事件
    var startX, moveX, width = this.width()
    this.on('touchstart', function (e) {
        startX = e.touches[0].pageX;
        clearInterval(time);//停止计时器
        box.css('transition-duration', '0ms');//停止过渡效果
    })
    this.on('touchmove', function (e) {
        moveX = e.touches[0].pageX - startX;
        box.css('transform', 'translateX(-' + (width * index - moveX) + 'px)');//移动
    })
    this.on('touchend', function (e) {
        //重新启动计时器
        setTime();
        //启动过渡效果
        box.css('transition-duration', speed + 'ms');

        if (moveX < -80) {
            next();
        } else if (moveX > 80) {
            previous();
        } else {
            box.css('transform', 'translateX(-' + index + '00%)');//还原
        }
    })

    return [previous, next, (a) => a ? setTime() : clearInterval(time)];
}

$(function () {
    //启动方法，可以在启动方法里面写扩展
    function start(th) {
        let [previous, next, startOrPause] = th.bannerSlide(parseInt(th.attr('interval')), parseInt(th.attr('speed')), parseInt(th.attr('switchStop')));

    }
    //同时支持数据后端渲染，和ajax渲染
    $('.banner-slide').each(function () {
        let th = $(this);
        if (th.children('.banner-slide-box').children().length > 0) {
            start(th);
        } else {
            let time = setInterval(function () {
                if (th.children('.banner-slide-box').children().length > 0) {
                    start(th);
                    clearInterval(time);
                }
            }, 1000)
        }
    });
})