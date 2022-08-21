/* eslint-disable *///此处不是注释，react当中不校验代码，不加此句react当中会报错
/*
 表头调整宽度
 */
//鼠标按下
$(document).on('mousedown', '.table-resize-move', function (e) {
    var th = $(this).parent();
    var x = e.clientX - th.width();//记录按下的位置
    var resizeBoxs = th.closest('table').find('tr').find('th:eq(' + th.index() + '),td:eq(' + th.index() + ')');
    $(document.body).css({ 'user-select': 'none', 'cursor': 'col-resize' });
    //鼠标移动
    function mousemove(e) {
        resizeBoxs.css('width', e.clientX - x);//不要用width()设置
        resizeBoxs.children('.table-resize-item').css('width', e.clientX - x);
    }
    //鼠标松开
    function mouseup(e) {
        $(document.body).css({ 'user-select': '', 'cursor': '' });
        $(document).off({ 'mousemove': mousemove, 'mouseup': mouseup });
    }
    $(document).on({ 'mousemove': mousemove, 'mouseup': mouseup });
});
//添加节点
$(document).on('mouseenter', '.table-resize', function () {
    var cur = $(this);
    if (cur.children('.table-resize-move').length == 0) {
        cur.css('position', 'relative');
        cur.append('<span class="table-resize-move" style="cursor:col-resize;width:8px;position:absolute;right:0px;top:0px;bottom:0px;z-index:1;"></span>');
    }
});
//初始化列宽（不能在悬浮其上的时候初始化列宽）
$(document).on('mousedown', '.table-resize', function () {
    //初始化每一列的列宽（如果不设置，在拉动时，如果有剩余宽度会平分给所有的列，其他列也会跟着宽度发生变化），最后一列宽度自适应，剩余宽度给最后一列
    var headTr = $(this).closest('tr');
    var table = headTr.closest('table');
    if (!table.is('[table-resize-init]')) {
        table.attr('table-resize-init', '');
        //设置第一行列宽
        headTr.children().each(function () {
            var obj = $(this);
            var child = obj.children('.table-resize-item');
            obj.css('width', obj.css('width'));
            child.css('width', child.css('width'))
        })
        //最后一列不设宽度
        table.find('tr').find('th:last,td:last').css('width', '');
    }
})