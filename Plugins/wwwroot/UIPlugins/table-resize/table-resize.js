/*
 表头调整宽度
 */
+function ($) {
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
            //记录列宽，（不读取第一行，其他全用第一行的列宽，因为存在跨列的情况）
            var arr = []
            table.find('tr').each(function (i) {
                arr[i] = [];
                $(this).children().each(function (j) {
                    arr[i][j] = $(this).css('width');
                });
            });
            //写入列宽
            table.find('tr').each(function (i) {
                $(this).children().each(function (j) {
                    var obj = $(this);
                    obj.css('width', arr[i][j]);
                    obj.children('.table-resize-item').css('width', obj.width())
                });
            });
            //最后一列不设宽度
            table.find('tr').find('th:last,td:last').css('width', '');
        }
    })
}(jQuery)