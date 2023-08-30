$(document).on('mouseenter', '[hover-to]', function () {
    var str = $(this).attr('hover-to')
    var obj = str ? eval(str) : $(this).find('[in-add],[in-remove]')
    obj.addClass(obj.attr('in-add'))
    obj.removeClass(obj.attr('in-remove'))
})
$(document).on('mouseleave', '[hover-to]', function () {
    var str = $(this).attr('hover-to')
    var obj = str ? eval(str) : $(this).find('[out-add],[out-remove]')
    obj.addClass(obj.attr('out-add'))
    obj.removeClass(obj.attr('out-remove'))
})