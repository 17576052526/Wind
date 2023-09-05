/* eslint-disable *///此处不是注释，react当中不校验代码，不加此句react当中会报错
/*
 弹框，仅用于消息提示弹框
 msg:要提示的内容
 */
$.alert = function (msg) {
    var box = $('#__alert372');
    if (box.length == 0) { box = $('<div class="alert-box" id="__alert372"></div>').appendTo(document.body); }
    var msgBox = $('<div class="alert"><div style="max-height:75vh;overflow:auto;">' + msg + '</div></div><br>').appendTo(box);//<br>用于换行
    $('<div class="alert-remove">+</div>').appendTo(msgBox.children()).on('click', function () { $(this).closest('.alert').remove() });
    function createTime() { return setTimeout(function () { msgBox.addClass('alert-hide'); setTimeout(function () { msgBox.remove() }, 500); }, 5000); }
    var times = createTime();//创建计时器
    msgBox.hover(function () { clearTimeout(times); }, function () { times = createTime(); });
}