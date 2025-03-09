(function () {
    //鼠标按下
    function mousedown(e) {
        let cur = e.target;
        let th = cur.parentNode;
        let ths = th.parentNode.children;
        let thLast = ths[ths.length - 1];
        let thLastWidth = thLast.offsetWidth;
        let table = th.closest('.table-resize')
        //初始化列宽，因为数据可能是后面才获取的，所以按下才初始化
        if (!table.dataset.tableResizeInit) {
            for (let s of ths) { s.style.width = s.offsetWidth + 'px' }
            table.style.tableLayout = 'fixed';
            thLast.dataset.tableResizeMinWidth = thLastWidth;//最后一列的最小缩小宽度
            table.dataset.tableResizeInit = '1';
        }
        //拖拽
        let x = e.clientX;//记录按下的位置
        let thWidth = th.offsetWidth;
        let thStyle = window.getComputedStyle(th);
        let thLR = parseFloat(thStyle.paddingLeft) + parseFloat(thStyle.paddingRight);
        let thLastMinWidth = thLast.dataset.tableResizeMinWidth;
        let body = document.body;
        body.style.userSelect = 'none';
        body.style.cursor = 'col-resize';
        //鼠标移动
        function mousemove(e) {
            if (thWidth + e.clientX - x > thLR) {//缩小的时候宽度不能小于 padding-left + padding-right
                th.style.width = (thWidth + e.clientX - x) + 'px';
                if (thLastWidth - (e.clientX - x) > thLastMinWidth) {//拖拽时最后一列宽度自适应，但宽度不能小于初始宽度
                    thLast.style.width = (thLastWidth - (e.clientX - x)) + 'px';
                }
            }
        }

        //鼠标松开
        function mouseup() {
            body.style.userSelect = '';
            body.style.cursor = '';
            document.removeEventListener('mousemove', mousemove);
            document.removeEventListener('mouseup', mouseup);
        }
        //绑定鼠标移动和鼠标松开
        document.addEventListener('mousemove', mousemove)
        document.addEventListener('mouseup', mouseup)
    }

    //添加节点，此事件不能删除，因为table可能是后面渲染出来的，事件绑定在th上因为列也有可能是后面渲染出来的
    document.body.addEventListener('mouseenter', function (e) {
        if (e.target.tagName != 'TH' || !e.target.closest('.table-resize') || e.target.classList.contains('table-resize-disable')) { return }

        let cur = e.target;
        if (!cur.querySelector('.table-resize-move')) {
            let children = cur.parentNode.children;
            let index = Array.prototype.indexOf.call(children, cur);
            if (index < children.length - 1) {//最后一个th不能按住拖拽
                cur.style.position = 'relative';
                let node = document.createElement('span')
                node.classList.add('table-resize-move');
                node.style = "cursor:col-resize;width:8px;position:absolute;right:0px;top:0px;bottom:0px;z-index:1;";
                node.onmousedown = mousedown;
                cur.appendChild(node);
            }
        }
    }, true)
})()