document.addEventListener('mouseenter', function (e) {
    if (!e.target.hasAttribute || !e.target.hasAttribute('hover')) { return }
    var cur = e.target;
    cur.setAttribute('__hover-class', cur.className);
    cur.getAttribute('hover').split(' ').forEach(s => cur.classList.add(s));
}, true);
document.addEventListener('mouseleave', function (e) {
    if (!e.target.hasAttribute || !e.target.hasAttribute('hover')) { return }
    var cur = e.target;
    cur.className = cur.getAttribute('__hover-class');
}, true);