//暂无代码
+function ($) {
    //选项卡
    $(document).on('click', '.tab-item', function () {
        var cur = $(this);
        var box = cur.closest('.tab');
        var items = box.find('.tab-item');
        var contents = box.find('.tab-content');
        //选中样式
        items.removeClass('tab-active');
        cur.addClass('tab-active');
        //对应内容显示
        contents.hide();//隐藏
        contents.eq(items.index(cur)).show();//不要用css('display','block')设置
    });
}(jQuery)