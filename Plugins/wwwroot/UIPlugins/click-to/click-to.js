$(document).on('click', '[click-to]', function () {
    var str = $(this).attr('click-to')
    if ($(this).attr('_click-to-is')) {
        $(this).removeAttr('_click-to-is')
        var obj = str ? eval(str) : $(this).find('[out-add],[out-remove]')
        obj.each(function () {
            $(this).addClass($(this).attr('out-add'))
            $(this).removeClass($(this).attr('out-remove'))
        })
    } else {
        $(this).attr('_click-to-is', '1')
        var obj = str ? eval(str) : $(this).find('[in-add],[in-remove]')
        obj.each(function () {
            $(this).addClass($(this).attr('in-add'))
            $(this).removeClass($(this).attr('in-remove'))
        })
    }
    
})