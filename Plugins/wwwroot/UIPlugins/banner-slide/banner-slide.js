//dom 查找的节点，interval 切换的间隔，speed 切换的速度，switchStop 手动切换后暂停时间
window.bannerSlide = function ({ dom, interval, speed, switchStop }) {
    var cur = document.querySelector(dom);
    var box = cur.querySelector('.banner-slide-box');
    var items = box.children;
    var length = items.length;
    //左右各加一个
    box.insertBefore(items[length - 1].cloneNode(true), items[0]);
    box.appendChild(items[1].cloneNode(true));
    //设置第一个显示
    box.style.transform = 'translateX(-100%)';
    //设置过渡效果
    box.style.transition = 'all ' + speed + 'ms';
    //生成按钮
    var btnBox = document.createElement('div');
    btnBox.className = "banner-slide-btnBox";
    for (let i = 0; i < length; i++) {
        var btn = document.createElement('div');
        btn.className = "banner-slide-btn";
        btnBox.appendChild(btn);
        btn.onclick = function (e) {
            exec(i);
            handSwitch();
        }
    }
    btnBox.children[0].classList.add('banner-slide-btnActive');
    cur.appendChild(btnBox);
    //第一个添加类样式（动画）
    items[1].querySelectorAll('[animate]').forEach(function (e) { e.className += ' ' + e.getAttribute('animate'); });
    //手动切换事件
    function handSwitch() {
        clearInterval(time);
        setTimeout(() => setTime(), switchStop);
    }
    //切换
    var index = 0;
    function exec(i) {
        box.style.transform = 'translateX(-' + (i + 1) + '00%)';
        index = i >= length ? 0 : i < 0 ? length - 1 : i;
        //按钮选中
        btnBox.querySelector('.banner-slide-btnActive').classList.remove('banner-slide-btnActive');
        btnBox.children[index].classList.add('banner-slide-btnActive');
        //切换完成后
        setTimeout(function () {
            if (i >= length || i < 0) {
                box.style.transition = 'none';
                box.style.transform = 'translateX(-' + (index + 1) + '00%)';
                setTimeout(() => box.style.transition = 'all ' + speed + 'ms', 100);
            }
            //动画类样式添加
            box.querySelectorAll('[animate]').forEach(e => e.className = e.className.replace(' ' + e.getAttribute('animate'), ''))
            items[index + 1].querySelectorAll('[animate]').forEach(function (e) { e.className += ' ' + e.getAttribute('animate'); });
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
    var startX, moveX, width = cur.offsetWidth
    cur.addEventListener('touchstart', function (e) {
        startX = e.touches[0].pageX;
        clearInterval(time);//停止计时器
        box.style.transition = 'none';//停止过渡效果
    })
    cur.addEventListener('touchmove', function (e) {
        moveX = e.touches[0].pageX - startX;
        box.style.transform = 'translateX(-' + (width * (index + 1) - moveX) + 'px)';//移动
    })
    cur.addEventListener('touchend', function (e) {
        //重新启动计时器
        setTime();
        //启动过渡效果
        box.style.transition = 'all ' + speed + 'ms'

        if (moveX < -80) {
            next();
        } else if (moveX > 80) {
            previous();
        } else {
            box.style.transform = 'translateX(-' + (index + 1) + '00%)';//还原
        }
    })



    return [previous, next, (a) => a ? setTime() : clearInterval(time)];
}