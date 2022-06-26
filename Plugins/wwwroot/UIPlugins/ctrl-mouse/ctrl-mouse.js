/*
 * 按下ctrl+鼠标左键拖拽
 * 参数一：按住的节点，参数二：接收的节点，参数三：接收事件
 */
$.ctrlMouse = function (downNode, upNode, upEvent) {
    var isCtrl = false;
    var downObj;
    $(document).on('keydown', function (e) { if (e.keyCode == 17) { isCtrl = true; } });
    $(document).on('keyup', function (e) { if (e.keyCode == 17) { isCtrl = false; } });
    $(document).on('mousedown', downNode, function () {
        if (isCtrl) {
            downObj = this;
            $(downNode).css('cursor', 'move');
            $(document.body).css({ 'user-select': 'none', 'cursor': 'move' });
            $(upNode).css('cursor', 'alias');
            //释放资源
            function mouseup() {
                if (downObj) {
                    downObj = null;
                    $(downNode).css('cursor', '');
                    $(document.body).css({ 'user-select': '', 'cursor': '' });
                    $(upNode).css('cursor', '');
                    //释放事件
                    $(document).off('mouseup', mouseup);
                    $(document).off('mouseup', upNodeMouseup);
                }
            }
            function upNodeMouseup() {
                if (downObj) {
                    upEvent(downObj, this);
                }
            }
            //绑定
            $(document).on('mouseup', mouseup);
            $(document).on('mouseup', upNode, upNodeMouseup);
        }
    });
}