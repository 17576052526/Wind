/*
选项卡 iframe 版
    .tab-iframe 父级框
    .tab-iframe-item 触发单击的项
    .tab-iframe-content <iframe> 的父级框
    .tab-iframe-active 当前激活的项
    href 属性 写在 .tab-iframe-item 上，iframe指向的地址
*/
$(document).on('click', '.tab-iframe-item', function () {
    var cur = $(this);
    var box = cur.closest('.tab-iframe');
    var content = box.find('.tab-iframe-content');
    var href = cur.attr('href');
    //隐藏
    box.find('.tab-iframe-item').removeClass('tab-iframe-active');
    content.children().css('display', 'none');
    //显示
    cur.addClass('tab-iframe-active');
    var curIframe = content.children('[src="' + href + '"]');
    if (curIframe.length == 0) {
        content.append('<iframe style="width:100%;height:100%;border:0;display:block;" src="' + href + '"></iframe>');
    } else {
        curIframe.css('display', 'block');
    }
});