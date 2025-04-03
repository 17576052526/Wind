(function () {
    function _hover(e) {
        if (!e.target.hasAttribute || !e.target.hasAttribute('hover-trigger')) { return }
        var cur = e.target;
        var box = cur.closest('[hover]') || cur;
        var mark = e.type == 'mouseenter' ? 'in' : '';

        function addClassByNode(node) {
            let inClass = node.getAttribute('hover-in');
            let outClass = node.getAttribute('hover-out');
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
    document.addEventListener('mouseenter', _hover, true);
    document.addEventListener('mouseleave', _hover, true);
})();