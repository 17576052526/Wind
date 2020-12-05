/*
 按键弹开显示
 .key-show 当前节点绑定类样式
 .click-hide 需要点击隐藏的子节点绑定
 */
$(document).on('keyup', '.key-show', function () {
    var cur = $(this);
    var node = cur.find(':hidden:first');
    node.show();
    function ev(e) { e.stopPropagation(); }
    function evHide() {
        node.hide();
        cur.off('click', evHide);
        cur.off('click', ev);
        $(document).off('click', evHide);
    }
    cur.on('click', '.click-hide', evHide);//给node 的子节点 .click-hide 绑定事件
    cur.on('click', ev);
    $(document).on('click', evHide);
})