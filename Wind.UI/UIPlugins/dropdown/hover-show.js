/*
         悬浮在当前节点上，第一个隐藏的子节点显示
         .hover-show 当前节点绑定类样式
         .click-hide 需要点击隐藏的子节点绑定
         */
$(document).on('mouseenter', '.hover-show', function () {
    var node = $(this).find(':hidden:first');
    node.show();
    function ev() {
        node.hide();
        node.off('click', ev);
        $(document).off('mouseleave', ev);
    }
    node.on('click', '.click-hide', ev);
    $(document).on('mouseleave', '.hover-show', ev);
});