//鼠标移动div
+function ($) {
    $(document).on('mousedown', '.move-box-mouse', function (e) {
        var box = $(this).closest('.move-box');
        //当前元素是否是正常文档流还是定位
        var x, y;
        if (box.css('position') == 'static') {
            //获取节点位置
            var offset = box.offset();
            x = e.clientX - offset.left;
            y = e.clientY - offset.top;
            box.css({ 'position': 'absolute', 'left': offset.left, 'top': offset.top });
        } else {
            x = e.clientX - parseInt(box.css('left'));
            y = e.clientY - parseInt(box.css('top'));
        }
        //设置body
        $(document.body).css({ 'user-select': 'none', 'cursor': 'move' });
        //鼠标移动
        function mousemove(e) { box.css({ 'left': e.clientX - x, 'top': e.clientY - y }); }
        //鼠标松开
        function mouseup(e) {
            $(document.body).css({ 'user-select': '', 'cursor': '' });
            $(document).off({ 'mousemove': mousemove, 'mouseup': mouseup });
        }
        $(document).on({ 'mousemove': mousemove, 'mouseup': mouseup });
    })
}(jQuery)