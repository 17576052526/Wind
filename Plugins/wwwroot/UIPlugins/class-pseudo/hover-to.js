function __hoverTo(e) {
    if (!e.target.hasAttribute || !e.target.hasAttribute('hover-to')) { return }
    var cur = e.target;
    var box = cur.closest('[class-box]');
    if (!box) { throw '缺少 class-box属性'; }
    var mark = e.type == 'mouseenter' ? 'in' : 'out';
    //递归查找子级，如果遇到 class-box就不在往里面找了
    function find(root) {
        for (let m of root.children) {
            if (m.hasAttribute('class-box')) { continue; }
            if (m.hasAttribute(mark + '-add')) { m.getAttribute(mark + '-add').split(' ').forEach(s => m.classList.add(s)) }
            if (m.hasAttribute(mark + '-remove')) { m.getAttribute(mark + '-remove').split(' ').forEach(s => m.classList.remove(s)) }
            find(m);
        }
    }
    find(box);
    //自身
    if (box.hasAttribute(mark + '-add')) { box.getAttribute(mark + '-add').split(' ').forEach(s => box.classList.add(s)) }
    if (box.hasAttribute(mark + '-remove')) { box.getAttribute(mark + '-remove').split(' ').forEach(s => box.classList.remove(s)) }
}
document.addEventListener('mouseenter', __hoverTo, true);
document.addEventListener('mouseleave', __hoverTo, true);
