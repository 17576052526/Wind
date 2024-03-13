/*
 树结构搜索插件
 .tree 树的项
 .tree-box 子级的父级.
 .tree-active 打开子节点
 注意：不能在节点用 style设置display，如果要设置 display 用类样式设置
 */
window.treeSearch = function (dom, str) {
    dom.querySelectorAll('.tree').forEach(root => {
        var box = root.querySelector('.tree-box');
        for (let m of box.children) { m.style.display = '' }//子级都设置为显示，不设的innerText拿不到
        var exist = root.innerText.includes(str);
        var boxExist = exist && box.innerText.includes(str);
        root.style.display = exist ? '' : 'none';
        box.style.height = '';//去掉 tree.js 中设置的高度
        if (boxExist) {
            root.classList.add('tree-active');
            //遍历 tree-box的子集
            for (let s of box.children) {
                if (!s.classList.contains('tree')) {
                    s.style.display = s.innerText.includes(str) ? '' : 'none';
                }
            }
        } else {
            root.classList.remove('tree-active');
        }
    })
}
