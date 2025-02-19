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
                inClass && node.classList.add(inClass)
                outClass && node.classList.remove(outClass)
            } else {
                outClass && node.classList.add(outClass)
                inClass && node.classList.remove(inClass)
            }
        }
    }

    //�ݹ�����Ӽ����˴�������querySelectorAll����Ϊ�Ӽ�������� click �Ͳ���������������
    function find(box) {
        for (let child of box.children) {
            if (child.hasAttribute('click')) { continue; }
            addClassByNode(child);
            find(child);
        }
    }
    find(box)

    //����
    addClassByNode(box);
});