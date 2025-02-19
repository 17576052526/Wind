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
                inClass && inClass.split(' ').forEach(m => node.classList.add(m));
                outClass && outClass.split(' ').forEach(m => node.classList.remove(m));
            } else {
                outClass && outClass.split(' ').forEach(m => node.classList.add(m));
                inClass && inClass.split(' ').forEach(m => node.classList.remove(m));
            }
        }
    }

    //�ݹ�����Ӽ����˴�������querySelectorAll����Ϊ�Ӽ�������� hover �Ͳ���������������
    function find(parent) {
        for (let child of parent.children) {
            if (child.hasAttribute('hover')) { continue; }
            addClassByNode(child);
            find(child);
        }
    }
    find(box)

    //����
    addClassByNode(box);
}

document.addEventListener('mouseenter', __hover9175, true);
document.addEventListener('mouseleave', __hover9175, true);