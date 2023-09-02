var __scroll_x = document.documentElement.scrollTop || document.body.scrollTop;
function scrollAnimate() {
    var scrollTop = document.documentElement.scrollTop || document.body.scrollTop;
    var height = $(window).height();
    $('[scroll-in-add],[scroll-in-remove],[scroll-out-add],[scroll-out-remove]').each(function () {
        var th = $(this);
        if (__scroll_x > scrollTop && th.offset().top + th.height() > scrollTop + height) {
            th.addClass(th.attr('scroll-out-add')).removeClass(th.attr('scroll-out-remove'));
        } else if (th.offset().top < scrollTop + height) {
            th.addClass(th.attr('scroll-in-add')).removeClass(th.attr('scroll-in-remove'));
        }
    })
    __scroll_x = scrollTop;
}
$(scrollAnimate);
$(window).on('scroll', scrollAnimate);