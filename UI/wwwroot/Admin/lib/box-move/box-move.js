/* eslint-disable *///此处不是注释，react当中不校验代码，不加此句react当中会报错
/*
 鼠标移动div
    .box-move 是要移动的框
    .box-move-switch 鼠标按下的块
 */
$(document).on('mousedown', '.box-move-switch', function (e) {
    var box = $(this).closest('.box-move');
    //当前元素是否是正常文档流还是定位
    var x, y;
    if (box.css('position') == 'static') {
        //获取节点位置
        var offset = box.offset();
        x = e.clientX - offset.left;
        y = e.clientY - offset.top;
        box.css({ 'position': 'absolute', 'left': offset.left, 'top': offset.top });
    } else {
        //去除 transform 上的 translateX，translateY 值设置到 left和top上，是为了 e.clientY - y > 0 这个判断有效
        var t = box.css('transform');
        if (t != 'none') {
            t = t.match(/\(.+\)/)[0];
            t = t.substr(1, t.length - 2);
            var arr = t.split(',');
            box.css('left', parseFloat(box.css('left')) + parseFloat(arr[4]));
            box.css('top', parseFloat(box.css('top')) + parseFloat(arr[5]));
            box.css({ 'transform': 'none' });
        }
        x = e.clientX - parseInt(box.css('left'));
        y = e.clientY - parseInt(box.css('top'));
    }
    //设置body
    $(document.body).css({ 'user-select': 'none', 'cursor': 'move' });
    //鼠标移动
    function mousemove(e) { box.css({ 'left': e.clientX - x, 'top': e.clientY - y > 0 ? e.clientY - y : 0 }); }//往上移动不能超出边界（不然移动不回来）
    //加入透明遮罩层，在有iframe的时候要加入
    var zzc = $('<div style="width:100vw;height:100vh;position:fixed;top:0;left:0;z-index:99999;"></div>').appendTo(document.body);
    //鼠标松开
    function mouseup(e) {
        zzc.remove();//删除透明遮罩层
        $(document.body).css({ 'user-select': '', 'cursor': '' });
        $(document).off({ 'mousemove': mousemove, 'mouseup': mouseup });
    }
    $(document).on({ 'mousemove': mousemove, 'mouseup': mouseup });
});