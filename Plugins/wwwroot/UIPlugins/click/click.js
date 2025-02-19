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
                if (inClass) { node.className = node.className + ' ' + inClass; }
                if (outClass) { node.className = node.className.replace(' ' + outClass, ''); }
            } else {
                if (outClass) { node.className = node.className + ' ' + outClass; }
                if (inClass) { node.className = node.className.replace(' ' + inClass, ''); }
            }
        }
    }

    //�ݹ�����Ӽ����˴�������querySelectorAll����Ϊ�Ӽ�������� click �Ͳ���������������
    function find(parent) {
        for (let child of parent.children) {
            if (child.hasAttribute('click')) { continue; }
            addClassByNode(child);
            find(child);
        }
    }
    find(box)

    //����
    addClassByNode(box);
});