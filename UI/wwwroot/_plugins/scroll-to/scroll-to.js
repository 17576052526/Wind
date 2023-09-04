var __scroll_y = document.documentElement.scrollTop || document.body.scrollTop;
function __scrollTo() {
    var scrollTop = document.documentElement.scrollTop || document.body.scrollTop;
    var height = $(window).height();
    $('[scroll-in-add],[scroll-in-remove],[scroll-out-add],[scroll-out-remove]').each(function () {
        var th = $(this);
        if (__scroll_y > scrollTop && th.offset().top + th.height() > scrollTop + height) {
            th.addClass(th.attr('scroll-out-add')).removeClass(th.attr('scroll-out-remove'));
        } else if (th.offset().top < scrollTop + height) {
            th.addClass(th.attr('scroll-in-add')).removeClass(th.attr('scroll-in-remove'));
        }
    })
    __scroll_y = scrollTop;
}
$(__scrollTo);
//减少代码执行频率，滑动过程中也会执行
var __scroll_is;
$(window).on('scroll', function () {
    if (!__scroll_is) {
        __scroll_is = true;
        setTimeout(() => { __scroll_is = false; __scrollTo(); }, 50);
    }
});