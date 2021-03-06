﻿/*
 表头调整宽度
 */
+function ($) {
    //鼠标按下
    $(document).on('mousedown', '.table-resize-move', function (e) {
        var th = $(this).parent();
        var x = e.clientX - th.width();//记录按下的位置
        var resizeBoxs = th.closest('table').find('tr').find('th:eq(' + th.index() + ')>.table-resize-item,td:eq(' + th.index() + ')>.table-resize-item');
        $(document.body).css({ 'user-select': 'none', 'cursor': 'col-resize' });
        //鼠标移动
        function mousemove(e) {
            resizeBoxs.css('width', e.clientX - x);//不要用width()设置
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
}(jQuery)