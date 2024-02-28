document.addEventListener('click', function (e) {
    if (!e.target.closest('.tree-switch')) { return }
    var box = e.target.closest('.tree');
    var child = box.querySelector('.tree-box');
    if (box.classList.contains('tree-active')) {
        child.style.height = window.getComputedStyle(child).height;//auto 转换为实际高度，css动画不支持auto高度
        setTimeout(() => { box.classList.remove('tree-active'); child.style.height = '0px'; }, 10);//延迟执行，css动画才会生效
    } else {
        box.classList.add('tree-active');
        child.style.height = 'auto';//设置为auto，是为了要获取高度
        var height = window.getComputedStyle(child).height;
        child.style.height = '0px';//因为前面设置为auto了，现在要设置回去
        setTimeout(() => child.style.height = height, 10);//延迟执行，css动画才会生效
    }
})