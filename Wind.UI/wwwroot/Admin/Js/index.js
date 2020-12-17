//树结构显示隐藏
$(document).on('click', '.tree-switch', function () {
    var box = $(this).closest('.tree');
    var child = box.children('.tree-box');
    if (box.hasClass('tree-active')) {
        //当前隐藏
        child.css('display', 'block');
        box.removeClass('tree-active');
        child.slideUp(300);
    } else {
        //其他显示的隐藏
        var active = box.parent().children('.tree-active');
        active.children('.tree-box').css('display', 'block').slideUp(300);
        active.removeClass('tree-active');
        //当前显示
        child.slideDown(300);
        box.addClass('tree-active');
    }
});