/*
 调整左右两边宽度，或上下高度
    .x-resize 调整左右宽度
    .y-resize 调整上下高度
 */
document.addEventListener('mousedown', function (e) {
    if (!e.target.closest('.x-resize,.y-resize')) { return }
    var cur = e.target.closest('.x-resize,.y-resize');
    var previous = cur.previousElementSibling;
    var next = cur.nextElementSibling;

    if (cur.classList.contains('x-resize')) {//左右调整
        var x = e.clientX;
        var previousWidth = parseFloat(window.getComputedStyle(previous).width);
        var nextWidth = parseFloat(window.getComputedStyle(next).width);
        previous.style.width = previousWidth + 'px';
        next.style.width = nextWidth + 'px';
        var mousemove = function (e) {
            previous.style.width = previousWidth + (e.clientX - x) + 'px';
            next.style.width = nextWidth - (e.clientX - x) + 'px';
        }
    } else {//上下调整
        var y = e.clientY;
        var previousHeight = parseFloat(window.getComputedStyle(previous).height);
        var nextHeight = parseFloat(window.getComputedStyle(next).height);
        previous.style.height = previousHeight + 'px';
        next.style.height = nextHeight + 'px';
        var mousemove = function (e) {
            previous.style.height = previousHeight + (e.clientY - y) + 'px';
            next.style.height = nextHeight - (e.clientY - y) + 'px';
        }
    }

    previous.style.flexGrow = '0';
    previous.style.flexBasis = 'auto';
    next.style.flexGrow = '0';
    next.style.flexBasis = 'auto';
    document.body.style.cursor = window.getComputedStyle(cur).cursor;
    document.body.style.userSelect = 'none';

    function mouseup() {
        document.body.style.cursor = '';
        document.body.style.userSelect = '';
        document.removeEventListener('mousemove', mousemove)
        document.removeEventListener('mouseup', mouseup)
    }
    document.addEventListener('mousemove', mousemove)
    document.addEventListener('mouseup', mouseup)
});