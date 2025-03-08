/*
 ˵�����������ṹ�������ز��Һ�Ľ��
 tree��json ���ṹ
 where�����ҵ�����
 ע�⣺children���ӽڵ�����
 ʾ����let tree=findTree(json, (m) => m.name == 'aa223');
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
�����޸����ṹ�ֶ�
ʾ����findTreeBatUpdate(tree,'isOpen',1);  ȫ��չ��

*/
export function findTreeBatUpdate(tree, field, value) {
    if (!tree) { return }
    for (let m of tree) {
        m[field] = value;
        findTreeBatUpdate(m.children, field, value)
    }
}