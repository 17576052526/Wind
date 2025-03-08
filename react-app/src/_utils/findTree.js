/*
 说明：查找树结构，并返回查找后的结果
 tree：json 树结构
 where：查找的条件
 注意：children是子节点名称
 示例：let tree=findTree(json, (m) => m.name == 'aa223');
 */
export function findTree(tree, where) {
    let arr = [];
    for (let m of tree) {
        let model = null;
        let children = m.children && findTree(m.children, where)
        if (children && children.length > 0) {
            model = { ...m };
            model.children = children;
            arr.push(model)
        } else if (where(m)) {
            model = { ...m };
            model.children = null;
            arr.push(model)
        }
    }
    return arr;
}

/*
批量修改树结构字段
示例：findTreeBatUpdate(tree,'isOpen',1);  全部展开

*/
export function findTreeBatUpdate(tree, field, value) {
    if (!tree) { return }
    for (let m of tree) {
        m[field] = value;
        findTreeBatUpdate(m.children, field, value)
    }
}