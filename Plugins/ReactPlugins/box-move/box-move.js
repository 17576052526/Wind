import './box-move.css'

/*
 ����ƶ�div
    .box-move ��Ҫ�ƶ��Ŀ�
    .box-move-switch ��갴�µĿ�
 */
document.addEventListener('mousedown', function (e) {
    if (!e.target.closest('.box-move-switch')) { return }
    var cur = e.target.closest('.box-move-switch');
    var box = cur.closest('.box-move');
    var boxStyle = window.getComputedStyle(box);
    if (boxStyle.position == 'static') { box.style.position = 'absolute'; }
    var x = e.clientX, y = e.clientY, left = parseFloat(boxStyle.left), top = parseFloat(boxStyle.top);
    var topSide = box.getBoundingClientRect().top;//��ȡλ��
    //�ƶ�
    function mousemove(e) {
        box.style.left = (e.clientX - x + left) + 'px';
        //�����ƶ����ܳ����߽磨��Ȼ�ƶ���������
        box.style.top = ((e.clientY < y - topSide ? y - topSide : e.clientY) - y + top) + 'px';
    }
    document.body.style.userSelect = 'none';
    document.body.style.cursor = 'move';
    //����
    function mouseup(e) {
        document.body.style.userSelect = '';
        document.body.style.cursor = '';
        document.removeEventListener('mousemove', mousemove);
        document.removeEventListener('mouseup', mouseup);
    }
    document.addEventListener('mousemove', mousemove);
    document.addEventListener('mouseup', mouseup);
});