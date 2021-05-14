//调整box大小
+function ($) {
    $(document).on('mouseenter', '.box-resize', function () {
        var cur = $(this);
        if (!cur.is('[box-resize-init]')) {
            cur.attr('box-resize-init', '');
            cur
                .append('<span class="box-resize-top" style="position:absolute;top:0px;left:0px;width:100%;height:9px;cursor:n-resize;"></span>')
                .append('<span class="box-resize-right" style="position:absolute;right:0px;top:0px;width:9px;height:100%;cursor:e-resize;"></span>')
                .append('<span class="box-resize-bottom" style="position:absolute;bottom:0px;left:0px;width:100%;height:9px;cursor:s-resize;"></span>')
                .append('<span class="box-resize-left" style="position:absolute;left:0px;top:0px;width:9px;height:100%;cursor:w-resize;"></span>')
                .append('<span class="box-resize-top box-resize-left" style="position:absolute;top:0px;left:0px;width:9px;height:9px;cursor:nw-resize;"></span>')
                .append('<span class="box-resize-top box-resize-right" style="position:absolute;top:0px;right:0px;width:9px;height:9px;cursor:ne-resize;"></span>')
                .append('<span class="box-resize-bottom box-resize-left" style="position:absolute;bottom:0px;left:0px;width:9px;height:9px;cursor:sw-resize;"></span>')
                .append('<span class="box-resize-bottom box-resize-right" style="position:absolute;bottom:0px;right:0px;width:9px;height:9px;cursor:se-resize;"></span>');
            //去除 transform 上的 translateX，translateY 值设置到 left和top上
            var t = cur.css('transform');
            if (t != 'none') {
                t = t.match(/\(.+\)/)[0];
                t = t.substr(1, t.length - 2);
                var arr = t.split(',');
                cur.css('left', parseFloat(cur.css('left')) + parseFloat(arr[4]));
                cur.css('top', parseFloat(cur.css('top')) + parseFloat(arr[5]));
                cur.css({ 'transform': 'none' });
            }
        }
    })
    //鼠标按下
    $(document).on('mousedown', '.box-resize-top,.box-resize-right,.box-resize-bottom,.box-resize-left', function (e) {
        var x = e.clientX, y = e.clientY;
        var cur = $(this);
        var box = cur.parent();
        $(document.body).css({ 'user-select': 'none', 'cursor': cur.css('cursor') });

        var clas = cur.attr('class');
        var isTop = clas.indexOf('box-resize-top') != -1;
        var isRight = clas.indexOf('box-resize-right') != -1;
        var isBottom = clas.indexOf('box-resize-bottom') != -1;
        var isLeft = clas.indexOf('box-resize-left') != -1;
        var isPosition = box.css('position') == 'absolute' || box.css('position') == 'fixed';

        var top = (isTop && isPosition) && parseInt(box.css('top'));
        var width = (isRight || isLeft) && box.outerWidth();
        var height = (isBottom || isTop) && box.outerHeight();
        var left = (isLeft && isPosition) && parseInt(box.css('left'));

        function mousemove(e) {
            if (isTop) {
                box.css('height', height - (e.clientY - y));
                if (isPosition) { box.css('top', top + (e.clientY - y)) }
            }
            if (isRight) {
                box.css('width', width + (e.clientX - x));
            }
            if (isBottom) {
                box.css('height', height + (e.clientY - y));
            }
            if (isLeft) {
                box.css('width', width - (e.clientX - x));
                if (isPosition) { box.css('left', left + (e.clientX - x)) }
            }
        }
        function mouseup() {
            $(document.body).css({ 'user-select': '', 'cursor': '' });
            $(document).off({ 'mousemove': mousemove, 'mouseup': mouseup });
        }
        $(document).on({ 'mousemove': mousemove, 'mouseup': mouseup });
    });
}(jQuery)