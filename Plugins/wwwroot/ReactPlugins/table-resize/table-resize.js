import './table-resize.css'

/*
拖拽调整表格列宽
使用说明：
1.<table>上加class="table-resize"，
2.第一行的 th或td 上加 class="table-resize-disable" 标记该列不拖拽

*/

//自适应列宽
function tableResizeAuto(table) {
    table.style.tableLayout = 'auto';
    var cells = table.querySelector('tr').querySelectorAll('th,td');
    var fWidth = 0;
    var mix = [];
    var zWidth = 0;
    cells.forEach(th => {
        var width = parseFloat(window.getComputedStyle(th).width);
        if (th.style.width) {
            if (parseFloat(th.style.width) < width) {
                fWidth += width - parseFloat(th.style.width);
            }
        } else {
            mix.push(th);
            zWidth += width;
        }
    });
    mix.forEach((th, i) => {
        var width = parseFloat(window.getComputedStyle(th).width);
        th.style.width = width + (fWidth * (width / zWidth)) + 'px';
    });
    table.style.tableLayout = 'fixed';
}
document.addEventListener('DOMNodeInserted', function (e) {
    if (!e.target.closest || !e.target.closest('.table-resize') || e.target.parentNode.tagName != 'TBODY') { return }
    var table = e.target.closest('.table-resize');
    if (table.getAttribute('__tableResizeWidthInit')) { return }
    table.setAttribute('__tableResizeWidthInit', '1');
    setTimeout(() => tableResizeAuto(table), 100);
})


document.addEventListener('mouseover', function (e) {
    if (!e.target.closest('.table-resize')) { return }
    var table = e.target.closest('.table-resize');
    if (table.getAttribute('__tableResizeInit')) { return }
    table.setAttribute('__tableResizeInit', '1');
    //添加节点
    var cells = table.querySelector('tr').querySelectorAll('th,td');
    var span = '<span class="table-resize-move" style="cursor:col-resize;width:8px;position:absolute;right:0px;top:0px;bottom:0px;"></span>';
    cells.forEach((th) => {
        //.table-resize-disable 不拖拽
        if (!th.classList.contains('table-resize-disable')) {
            th.style.position = 'relative';
            th.insertAdjacentHTML("beforeend", span);
        }
    });
    //鼠标按下
    table.addEventListener('mousedown', function (e) {
        if (!e.target.closest('.table-resize-move')) { return }
        //调整列宽
        var cur = e.target.closest('th,td');
        var style = window.getComputedStyle(cur);
        var width = parseFloat(style.width);
        var paddingLeft = parseFloat(style.paddingLeft);
        var paddingRight = parseFloat(style.paddingRight);
        var last = cells[cells.length - 1];
        var lastWidth = parseFloat(window.getComputedStyle(last).width);
        var x = e.clientX;
        function mousemove(e) {
            var side = e.clientX - x < -(width - paddingLeft - paddingRight) ? -(width - paddingLeft - paddingRight) : e.clientX - x;
            last.style.width = lastWidth - side + 'px';
            cur.style.width = width + side + 'px';
        }
        //解除绑定
        document.body.style.userSelect = 'none';
        document.body.style.cursor = 'col-resize';
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