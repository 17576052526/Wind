document.addEventListener('click', function (e) {
    if (!e.target.closest('[click-to]')) { return }
    var cur = e.target.closest('[click-to]')
    var box = cur.closest('[class-box]')
    if (!box) { throw '缺少 class-box属性'; }
    var mark = box.getAttribute('__click-to-mark') != 'in' ? 'in' : 'out';
    box.setAttribute('__click-to-mark', mark);
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
})

