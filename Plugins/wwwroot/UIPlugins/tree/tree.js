/*
    树结构显示隐藏
        .tree 每一个项（项的类样式）
        .tree-switch 触发显示隐藏的按钮
        .tree-box 子项的父级
        .tree-active 激活（显示）的样式
*/
$(document).on('click', '.tree-switch', function () {
    var box = $(this).closest('.tree');
    var child = box.children('.tree-box');
    if (box.hasClass('tree-active')) {
        //当前隐藏
        child.css('display', 'block');
        box.removeClass('tree-active');
        child.slideUp(300);
    } else {
        //当前显示
        child.slideDown(300);
        box.addClass('tree-active');
    }
});