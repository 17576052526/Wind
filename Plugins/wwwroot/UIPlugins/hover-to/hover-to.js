$(document).on('mouseenter', '[hover-to]', function () {
    var str = $(this).attr('hover-to')
    var obj = str ? eval(str) : $(this).find('[in-add],[in-remove]')
    obj.each(function () {
        $(this).addClass($(this).attr('in-add'))
        $(this).removeClass($(this).attr('in-remove'))
    })
})
$(document).on('mouseleave', '[hover-to]', function () {
    var str = $(this).attr('hover-to')
    var obj = str ? eval(str) : $(this).find('[out-add],[out-remove]')
    obj.each(function () {
        $(this).addClass($(this).attr('out-add'))
        $(this).removeClass($(this).attr('out-remove'))
    })
})