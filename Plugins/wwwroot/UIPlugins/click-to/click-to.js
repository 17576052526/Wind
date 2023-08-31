$(document).on('click', '[click-to]', function () {
    var str = $(this).attr('click-to')
    var obj = str ? eval(str) : $(this).find('[add],[remove]')
    obj.each(function () {
        $(this).addClass($(this).attr('add'))
        $(this).removeClass($(this).attr('remove'))
    })
})