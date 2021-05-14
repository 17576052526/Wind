/*
 弹出框最大化或最小化
         .box-zoom 父级框
         .box-zoom-btn 按钮，触发最大化最小化事件
         .box-zoom-btn-active 最大化时按钮样式
         box-zoom-css 属性，最小化时的宽高，此属性标识是否是最大化（初始化不是最大化可以不用管此属性）

         */
$(document).on('click', '.box-zoom-btn', function () {
    var box = $(this).closest('.box-zoom');
    var css = box.attr('box-zoom-css');
    if (css) {//设置最小化
        box.css(JSON.parse(css));
        box.removeAttr('box-zoom-css');
        $(this).removeClass('box-zoom-btn-active');
    } else {//设置最大化
        box.attr('box-zoom-css', JSON.stringify({ 'left': box.css('left'), 'top': box.css('top'), 'width': box.css('width'), 'height': box.css('height'), 'margin-left': box.css('margin-left'), 'margin-top': box.css('margin-top'), 'transform': box.css('transform') }));
        box.css({ 'left': '0px', 'top': '0px', 'width': '100%', 'height': '100%', 'margin-left': '0', 'margin-top': '0', 'transform': 'none' });
        $(this).addClass('box-zoom-btn-active');
    }
})