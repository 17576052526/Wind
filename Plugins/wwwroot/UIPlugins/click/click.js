document.addEventListener('click', function (e) {
    if (!e.target.closest('[click-trigger]')) { return }
    var cur = e.target.closest('[click-trigger]');
    var box = cur.closest('[click]') || cur;

    var mark = box.getAttribute('_click-mark') != 'in' ? 'in' : '';
    box.setAttribute('_click-mark', mark);

    function addClassByNode(node) {
        let inClass = node.getAttribute('click-in');
        let outClass = node.getAttribute('click-out');
        if (inClass || outClass) {
            if (mark) {
                inClass && inClass.trim().split(/\s+/).forEach(m => node.classList.add(m));
                outClass && outClass.trim().split(/\s+/).forEach(m => node.classList.remove(m));
            } else {
                outClass && outClass.trim().split(/\s+/).forEach(m => node.classList.add(m));
                inClass && inClass.trim().split(/\s+/).forEach(m => node.classList.remove(m));
            }
        }
    }

    //递归查找子集，此处不能用querySelectorAll，因为子集中如果有 click 就不能在往里面找了
    function find(parent) {
        for (let child of parent.children) {
            if (child.hasAttribute('click')) { continue; }
            addClassByNode(child);
            find(child);
        }
    }
    find(box)

    //自身
    addClassByNode(box);
});