//批量选中
//先选一个，然后按住Ctrl 在选一个，中间的都会自动选中或不选中
+function ($) {
    //记录选中的
    var last_check;
    function check_region() { last_check = this; }
    $(document).on('change', '.check-item', check_region);
    //按键按下事件
    var isKeydown;//keydown事件按住不放会一直执行，所以加个变量判断
    $(document).on('keydown', function (e) {
        if (e.keyCode != 17 || isKeydown) { return; }
        $(document).off('change', check_region);
        isKeydown = true;
        //选中事件
        function change() {
            if (last_check) {
                var cur = $(this);
                var curCheck = cur.prop('checked');
                var item = $('.check-item');
                var lastIndex = item.index(last_check);
                var curIndex = item.index(cur);
                item.each(function (i) {
                    if ((lastIndex < curIndex && lastIndex <= i && curIndex >= i) || (curIndex < lastIndex && curIndex <= i && lastIndex >= i)) {
                        $(this).prop('checked', curCheck);
                    }
                });
            }
        }
        //按键弹开
        function keyup(e) {
            if (e.keyCode != 17) { return; }
            $(document).on('change', '.check-item', check_region);
            isKeydown = null;
            $(document).off({ 'change': change, 'keyup': keyup });
        }
        //绑定事件
        $(document).on('change', '.check-item', change);
        $(document).on('keyup', keyup);
    });
}(jQuery)