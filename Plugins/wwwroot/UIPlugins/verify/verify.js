+function ($) {
    //根据节点验证
    function verifyByNode(cur) {
        var verify = cur.attr('verify');
        var msg = cur.attr('msg');
        var val = cur.val();
        var box = cur.next('[msgBox]').children();
        if (!verify) { return true; }//不存在，则算验证通过
        //弹出消息框
        function msgBox(str) {
            if (box.length == 0) {
                var po = $('<span msgBox style="height:100%;position:relative;"><span class="verify-msg"></span></span>').insertAfter(cur);
                box = po.children();
            }
            box.html(str);
            box.show();
        }

        if (verify.indexOf('notnull') != -1 && (val == null || val.length == 0)) { msgBox('不能为空'); return false; }
        if (verify.indexOf('number') != -1) {//数字验证
            if (!/^$|^\d+(\.\d+)?$/.test(val)) {//此判断不能放到上面
                msgBox(msg || '只能输入数字'); return false;
            }
        } else if (verify.indexOf('notnull') == -1 && !new RegExp(verify).test(val)) {//正则表达式验证
            msgBox(msg || '格式不正确'); return false;
        }
        box.hide();
        return true;
    }
    //失去焦点验证
    $(document).on('blur', '[verify]', function () { verifyByNode($(this)) });
    $(document).on('focus', '[verify]', function () { $(this).parent().children('[msgbox]').remove(); });
    //验证方法
    $.fn.verify = function () {
        var isAdopt = true;
        this.each(function () {
            if (!verifyByNode($(this))) { isAdopt = false; }
        });
        return isAdopt;
    }
}(jQuery)