/*
 确认框插件
         参数一：内容
         参数二：点击确定执行的方法
         参数三：标题
         参数四：点击取消执行的方法
         */
$.confirm = function (content, yesFunc, title, noFunc) {
    //关闭弹出框
    function close() {
        $('#confirm_394,#confirm_815').remove()
    }
    //添加遮罩层
    $(document.body).append('<div style="position:fixed;left:0px;top:0px;z-index:100; width:100%;height:100%;" id="confirm_394"></div>');
    //添加 box框
    var box = $('<div class="box box-move confirm" id="confirm_815">\
            <div class="box-body">'+ content + '</div>\
        </div>').appendTo(document.body);
    var boxFoot = $('<div class="box-foot"></div>').appendTo(box);
    $('<input type="button" value="确定" class="btn-primary" />').appendTo(boxFoot).on('click', function () {
        close();
        yesFunc && yesFunc();
    });
    $('<input type="button" value="取消" class="btn-default" style="margin-left:15px;" />').appendTo(boxFoot).on('click', function () {
        close();
        noFunc && noFunc();
    });
    //关闭弹出框react 中不能直接在dome中加onclick,所以改用jquery绑定
    var boxHead = $('<div class="box-head box-move-switch" style="display:flex;justify-content:space-between;"><div>' + (title || '消息提示') + '</div></div>').prependTo(box)
    $('<span class="confirm-remove">+</span>').appendTo(boxHead).on('click', close)
}