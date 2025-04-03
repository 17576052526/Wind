import './box-move.css'

/*
 鼠标移动div
    .box-move 是要移动的框
    .box-move-switch 鼠标按下的块
 */
document.addEventListener('mousedown', function (e) {
    if (!e.target.closest('.box-move-switch')) { return }
    var cur = e.target.closest('.box-move-switch');
    var box = cur.closest('.box-move');
    var boxStyle = window.getComputedStyle(box);
    if (boxStyle.position == 'static') { box.style.position = 'absolute'; }
    var x = e.clientX, y = e.clientY, left = parseFloat(boxStyle.left), top = parseFloat(boxStyle.top);
    var topSide = box.getBoundingClientRect().top;//获取位置
    //移动
    function mousemove(e) {
        box.style.left = (e.clientX - x + left) + 'px';
        //往上移动不能超出边界（不然移动不回来）
        box.style.top = ((e.clientY < y - topSide ? y - topSide : e.clientY) - y + top) + 'px';
    }
    document.body.style.userSelect = 'none';
    document.body.style.cursor = 'move';
    //弹开
    function mouseup(e) {
        document.body.style.userSelect = '';
        document.body.style.cursor = '';
        document.removeEventListener('mousemove', mousemove);
        document.removeEventListener('mouseup', mouseup);
    }
    document.addEventListener('mousemove', mousemove);
    document.addEventListener('mouseup', mouseup);
});