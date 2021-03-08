/*
 弹框，仅用于消息提示弹框
 msg:要提示的内容
 */
$.alert = function (msg) {
    var box = $('#__alert372');
    if (box.length == 0) { box = $('<div class="alert-box" id="__alert372"></div>').appendTo(document.body); }
    var msgBox = $('<div class="alert">' + msg + '</div><br>').appendTo(box);//<br>用于换行
    function createTime() { return setTimeout(function () { msgBox.addClass('alert-hide'); setTimeout(function () { msgBox.remove() }, 500); }, 5000); }
    var times = createTime();//创建计时器
    msgBox.hover(function () { clearTimeout(times); }, function () { times = createTime(); });
}