$(document).on('mouseenter', '[hover]', function () {
    $(this).attr('_class', $(this).prop('class'))
    $(this).addClass($(this).attr('hover'))
})
$(document).on('mouseleave', '[hover]', function () {
    $(this).removeClass($(this).attr('hover'))
    $(this).addClass($(this).attr('_class'))
})