/*
    回车触发节点单击事件
    .enter-click  回车触发当前节点单击事件，如果有多个节点绑定该类样式则是触发最后一个单击事件
*/
$(document).on('keyup', function (e) {
    if (e.keyCode == 13) {
        $('.enter-click:last').click();
    }
});