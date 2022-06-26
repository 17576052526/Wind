/*
表格树（此插件不渲染数据）
    .table-tree：标识当前表格是表格树
    .table-tree-switch：标识显示隐藏的按钮
    .table-tree-active：加此样式显示子级
    treekey属性：用来标识层级关系，父子级|隔开标识，例如父级是：aaa，那么子级就是：aaa|11，子级的子级 aaa|11|222
*/
$('.table-tree').each(function () {
    var cur = $(this);
    cur.find('tr[treekey]').each(function () {
        var key = $(this).attr('treekey');
        var length = key.split('|').length - 1;
        var btn = $(this).find('.table-tree-switch');
        btn.css('margin-left', (length * 20) + 'px');//设置缩进
        //显示隐藏
        if (!$(this).hasClass('table-tree-active')) {
            cur.find('[treekey^="' + key + '|"]').hide();
        }
        //事件绑定
        btn.on('click', function () {
            var curNode = $(this);
            var tr = curNode.closest('tr');
            var k = tr.attr('treekey');
            var chil = cur.find('[treekey^="' + k + '|"]');
            if (tr.hasClass('table-tree-active')) {
                chil.hide();
                chil.removeClass('table-tree-active');
                tr.removeClass('table-tree-active');
            } else {
                chil.each(function () {
                    if ($(this).attr('treekey').substr(k.length).lastIndexOf('|') == 0) {
                        $(this).show();
                    }
                });
                tr.addClass('table-tree-active');
            }
        });
    });
});