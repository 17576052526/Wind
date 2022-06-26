/*
         点击当前节点，第一个隐藏的子节点显示
         .click-show 当前节点绑定类样式
         .click-hide 需要点击隐藏的子节点绑定
         */
$(document).on('click', '.click-show', function () {
    var cur = $(this);
    if (cur.attr('isShow') == 'true') { return; }
    cur.attr('isShow', 'true');
    var node = cur.find(':hidden:first');
    node.show();
    function evHide(e) {
        var evCur = $(e.target);
        if (evCur.closest(node).length > 0 && evCur.closest('.click-hide').length == 0) { return; }
        cur.removeAttr('isShow');
        node.hide();
        $(document).off('click', evHide);
    }
    $(document).on('click', evHide);
})