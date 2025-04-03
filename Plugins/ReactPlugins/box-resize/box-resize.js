/*
调整浮动框大小，框上设置 .box-resize 类样式即可
*/

document.addEventListener('mouseover', function (e) {
    if (!e.target.closest('.box-resize')) { return; }
    var box = e.target.closest('.box-resize');
    if (box.getAttribute('__boxResizeInit')) { return; }
    box.setAttribute('__boxResizeInit', '1');
    //添加节点
    box.insertAdjacentHTML("beforeend", '<span class="box-resize-top" style="position:absolute;top:0px;left:0px;width:100%;height:9px;cursor:n-resize;"></span>');
    box.insertAdjacentHTML("beforeend", '<span class="box-resize-right" style="position:absolute;right:0px;top:0px;width:9px;height:100%;cursor:e-resize;"></span>');
    box.insertAdjacentHTML("beforeend", '<span class="box-resize-bottom" style="position:absolute;bottom:0px;left:0px;width:100%;height:9px;cursor:s-resize;"></span>');
    box.insertAdjacentHTML("beforeend", '<span class="box-resize-left" style="position:absolute;left:0px;top:0px;width:9px;height:100%;cursor:w-resize;"></span>');
    box.insertAdjacentHTML("beforeend", '<span class="box-resize-top box-resize-left" style="position:absolute;top:0px;left:0px;width:9px;height:9px;cursor:nw-resize;"></span>');
    box.insertAdjacentHTML("beforeend", '<span class="box-resize-top box-resize-right" style="position:absolute;top:0px;right:0px;width:9px;height:9px;cursor:ne-resize;"></span>');
    box.insertAdjacentHTML("beforeend", '<span class="box-resize-bottom box-resize-left" style="position:absolute;bottom:0px;left:0px;width:9px;height:9px;cursor:sw-resize;"></span>');
    box.insertAdjacentHTML("beforeend", '<span class="box-resize-bottom box-resize-right" style="position:absolute;bottom:0px;right:0px;width:9px;height:9px;cursor:se-resize;"></span>');

    //绑定事件
    box.addEventListener('mousedown', function (e) {
        if (!e.target.closest('.box-resize-top,.box-resize-right,.box-resize-bottom,.box-resize-left')) { return }
        var cur = e.target.closest('.box-resize-top,.box-resize-right,.box-resize-bottom,.box-resize-left');
        var x = e.clientX, y = e.clientY;
        var boxStyle = window.getComputedStyle(box);
        var top = parseFloat(boxStyle.top);
        var left = parseFloat(boxStyle.left);
        var width = parseFloat(boxStyle.width);
        var height = parseFloat(boxStyle.height);
        box.style.transform = boxStyle.transform;//translate 如果是百分比，则转换成固定值，因为百分比会受宽高影响
        var side = box.getBoundingClientRect();//获取位置
        var windowWidth = window.innerWidth;//获取浏览器宽度
        var windowHeight = window.innerHeight;//获取浏览器高度

        function mousemove(e) {
            if (cur.classList.contains('box-resize-top')) {
                var curY = e.clientY < y - side.top ? y - side.top : e.clientY;
                box.style.height = height - (curY - y) + 'px';
                box.style.top = top + (curY - y) + 'px';
            }
            if (cur.classList.contains('box-resize-right')) {
                var curX = e.clientX > windowWidth - (side.left + side.width - x) ? windowWidth - (side.left + side.width - x) : e.clientX;
                box.style.width = width + (curX - x) + 'px';
            }
            if (cur.classList.contains('box-resize-bottom')) {
                var curY = e.clientY > windowHeight - (side.top + side.height - y) ? windowHeight - (side.top + side.height - y) : e.clientY;
                box.style.height = height + (curY - y) + 'px';
            }
            if (cur.classList.contains('box-resize-left')) {
                var curX = e.clientX < x - side.left ? x - side.left : e.clientX;
                box.style.width = width - (curX - x) + 'px';
                box.style.left = left + (curX - x) + 'px';
            }
        }
        document.body.style.userSelect = 'none';
        document.body.style.cursor = window.getComputedStyle(cur).cursor;
        function mouseup() {
            document.body.style.userSelect = '';
            document.body.style.cursor = '';
            document.removeEventListener('mousemove', mousemove);
            document.removeEventListener('mouseup', mouseup);
        }
        document.addEventListener('mousemove', mousemove);
        document.addEventListener('mouseup', mouseup);
    })
})