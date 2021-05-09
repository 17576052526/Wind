/*
 * 按下ctrl+鼠标左键拖拽
 * 参数一：按住的节点，参数二：接收的节点，参数三：接收事件
 */
$.ctrlMouse = function (downNode, upNode, upEvent) {
    $(document).on('keydown', function (e) {
        if (e.keyCode == 17) {//按下ctrl
            function keyup(e) {
                if (e.keyCode == 17) {
                    $(document).off('keyup', keyup);//释放弹开事件
                    $(downNode).off('mousedown', downNodeMousedown);//释放节点按下事件
                }
            }
            function downNodeMousedown() {
                var downObj = this;
                function mouseup() {
                    $(document).off('mouseup', mouseup);//释放
                    $(upNode).off('mouseup', upNodeMouseup);//释放
                    $(downNode).css('cursor', '');
                    $(document.body).css({ 'user-select': '', 'cursor': '' });
                    $(upNode).css('cursor', '');
                }
                function upNodeMouseup() { upEvent(downObj, this); }
                $(document).on('mouseup', mouseup);//绑定
                $(upNode).on('mouseup', upNodeMouseup);//绑定
                $(downNode).css('cursor', 'move');
                $(document.body).css({ 'user-select': 'none', 'cursor': 'move' });
                $(upNode).css('cursor', 'alias');
            }
            $(document).on('keyup', keyup);//绑定弹开事件
            $(downNode).on('mousedown', downNodeMousedown);//绑定节点按下事件
        }
    });
}