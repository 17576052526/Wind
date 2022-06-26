/*
    tab 页
         target=main 标识可以触发打开tab页事件
         .tab-pager tab页的按钮父级框
         .tab-pager-active 当前选中的样式
         .tab-pager-content iframe 的父级框
         .tab-pager-remove 删除按钮
         .tab-pager-item 按钮项
*/
$(document).on('click', '[target=main]', function () {
    var cur = $(this);
    var href = cur.attr('href');

    var box = $('.tab-pager');
    var content = $('.tab-pager-content');

    box.children('.tab-pager-active').removeClass('tab-pager-active');//移除激活类样式
    content.children().css('display', 'none');//隐藏iframe

    var curTabBtn = box.children('[href="' + href + '"]');
    if (curTabBtn.length == 0) {
        box.append('<a href="' + href + '" target="main" class="tab-pager-item tab-pager-active" title="' + cur.text() + '">' + cur.html() + '<i class="tab-pager-remove"></i></a>');
        content.append('<iframe style="width:100%;height:100%;border:0;display:block;" src="' + href + '"></iframe>');
    } else {
        curTabBtn.addClass('tab-pager-active');
        var curIframe = content.children('[src="' + href + '"]');
        if (curIframe.length == 0) {
            content.append('<iframe style="width:100%;height:100%;border:0;display:block;" src="' + href + '"></iframe>');
        } else {
            curIframe.css('display', 'block');
        }
    }
    return false;
});
//删除事件
$(document).on('click', '.tab-pager-remove', function () {
    var itemBtn = $(this).closest('.tab-pager-item');
    var href = itemBtn.attr('href');
    //显示标签
    var box = itemBtn.closest('.tab-pager');
    var index = itemBtn.index();
    if (index == box.children().length - 1) {
        box.children().eq(index - 1).click();
    } else {
        box.children().eq(index + 1).click();
    }
    //删除标签
    itemBtn.remove();
    $('.tab-pager-content').children('[src="' + href + '"]').remove();
    return false;
});