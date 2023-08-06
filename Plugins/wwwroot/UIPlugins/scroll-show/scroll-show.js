function scrollShow() {
    var scrollTop = document.documentElement.scrollTop;
    var height = $(window).height();
    $('.scroll-show').each(function () {
        if (scrollTop + height > $(this).offset().top) {
            $(this).removeClass('scroll-show').addClass($(this).attr('addClass'));
        }
    })
}
$(scrollShow);
$(window).on('scroll', scrollShow);