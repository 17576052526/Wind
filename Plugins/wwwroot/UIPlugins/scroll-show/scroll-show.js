function scrollShow() {
    var scrollTop = document.documentElement.scrollTop;
    var height = $(window).height();
    $('[scroll-animate]').each(function () {
        var th = $(this);
        if (scrollTop + height > th.offset().top) {
            var delay = th.attr('delay');
            if (delay) {
                setTimeout(() => th.addClass(th.attr('scroll-animate')).removeAttr('scroll-animate'), parseFloat(delay) * 1000)
            } else {
                th.addClass(th.attr('scroll-animate')).removeAttr('scroll-animate');
            }
        }
    })
}
$(scrollShow);
$(window).on('scroll', scrollShow);