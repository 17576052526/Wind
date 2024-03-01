var __previousScroll = document.documentElement.scrollTop || document.body.scrollTop;
function __scrollTo() {
    var scrollTop = document.documentElement.scrollTop || document.body.scrollTop;
    var height = document.documentElement.clientHeight;
    document.querySelectorAll('[scroll-in-add],[scroll-in-remove],[scroll-out-add],[scroll-out-remove]').forEach(cur => {
        var obj = cur.getBoundingClientRect();
        var top = obj.top + scrollTop;//绝对位置
        if (__previousScroll > scrollTop && top + obj.height > scrollTop + height) {
            var outAdd = cur.getAttribute('scroll-out-add');
            var outRemove = cur.getAttribute('scroll-out-remove');
            outAdd && outAdd.split(' ').forEach(s => cur.classList.add(s));
            outRemove && outRemove.split(' ').forEach(s => cur.classList.remove(s));
        } else if (top < scrollTop + height) {
            var inAdd = cur.getAttribute('scroll-in-add');
            var inRemove = cur.getAttribute('scroll-in-remove');
            inAdd && inAdd.split(' ').forEach(s => cur.classList.add(s));
            inRemove && inRemove.split(' ').forEach(s => cur.classList.remove(s));
        }
    })
    __previousScroll = scrollTop;
}
window.addEventListener('load', __scrollTo);
//减少代码执行频率
var __scroll_is;
window.addEventListener('scroll', function () {
    if (!__scroll_is) {
        __scroll_is = true;
        setTimeout(() => { __scroll_is = false; __scrollTo(); },50);
    }
})