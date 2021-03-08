+function ($) {
    $.moveNode = function (moveNode,toNodes, completeFun) {
        //鼠标按下
        $(document).on('mousedown', moveNode, function () {
            toNodes = $(toNodes);//放在里面
            let th = $(this);
            let cur = th.closest('.move-box');
            let tonode = null;
            let position = null;
            cur.addClass('move-node-move');
            //鼠标移动
            function mousemove(e) {
                //计算所有节点的位置区域，如果有用到move-box插件，必须先等其设置了absolute在计算位置
                if (position == null) {
                    position = [];
                    toNodes.each(function () {
                        let m = $(this);
                        if (!m.is(cur)) {
                            let offset = m.offset();
                            position.push({ node: m, x1: offset.left, x2: offset.left + m.outerWidth(), y1: offset.top, y2: offset.top + m.outerHeight() });
                        }
                    });
                }
                let x = e.clientX;
                let y = e.clientY;
                for (let i = 0; i < position.length; i++) {
                    let m = position[i];
                    if (x >= m.x1 && x <= m.x2 && y >= m.y1 && y <= m.y2) {//悬浮到某个块
                        if (!m.node.is(tonode)) {//是从别处移动到当前块
                            if (tonode) { tonode.removeClass('move-node-hover'); }//tonode!=null表示从另一块移动的当前块
                            tonode = m.node;
                            tonode.addClass('move-node-hover');
                            th.css('cursor', 'alias');
                            $(document.body).css('cursor', 'alias');
                        }
                        return;//找到了就不执行后面的
                    }
                }
                //执行到此处说明没有悬浮在任何一个块上，tonode!=null表示从另一块移动到空白处
                if (tonode) {
                    tonode.removeClass('move-node-hover');
                    tonode = null;
                    th.css('cursor', '');
                    $(document.body).css('cursor', '');
                }
            }
            //鼠标松开
            function mouseup() {
                if (tonode) {
                    tonode.after(cur);
                    tonode.removeClass('move-node-hover');
                }
                th.css('cursor', '');
                $(document.body).css('cursor', '');
                cur.removeClass('move-node-move');
                cur.css('position', '');//如果有用move-box插件配合使用，则需要这句
                $(document).off({ 'mousemove': mousemove, 'mouseup': mouseup });
                completeFun(cur, tonode);//执行完成，执行回调，并传移动节点和悬浮其上节点
            }
            $(document).on({ 'mousemove': mousemove, 'mouseup': mouseup });
        })
    }
}(jQuery)