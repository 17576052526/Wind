function __hover9175(e) {
    if (!e.target.hasAttribute || !e.target.hasAttribute('hover-trigger')) { return }
    var cur = e.target;
    var box = cur.closest('[hover]') || cur;
    var mark = e.type == 'mouseenter' ? 'in' : '';

    function addClassByNode(node) {
        let inClass = node.getAttribute('hover-in');
        let outClass = node.getAttribute('hover-out');
        if (inClass || outClass) {
            if (mark) {
                inClass && node.classList.add(inClass)
                outClass && node.classList.remove(outClass)
            } else {
                outClass && node.classList.add(outClass)
                inClass && node.classList.remove(inClass)
            }
        }
    }

    //递归查找子集，此处不能用querySelectorAll，因为子集中如果有 hover 就不能在往里面找了
    function find(parent) {
        for (let child of parent.children) {
            if (child.hasAttribute('hover')) { continue; }
            addClassByNode(child);
            find(child);
        }
    }
    find(box)

    //自身
    addClassByNode(box);
}

document.addEventListener('mouseenter', __hover9175, true);
document.addEventListener('mouseleave', __hover9175, true);