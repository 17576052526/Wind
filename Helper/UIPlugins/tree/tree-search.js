//树结构搜索插件  .tree-search 输入框，.tree 树的项，.tree-box 子级的父级，tree-active 打开，注意：不能在节点用style 属性设置display，如果要设置显示隐藏请用类样式设置
$(document).on('focus', '.tree-search', function () {
    function search(str) {
        $('.tree').each(function () {
            var $this = $(this);
            var box = $this.children('.tree-box');
            var t_che = $this.children(':not(".tree-box"):contains("' + str + '")');
            if (t_che.length > 0) {
                $this.css('display', '').removeClass('tree-active');
                box.css('display', '');
                box.children().css('display', '');
            }
            var che = box.children(':contains("' + str + '")');
            if (che.length > 0) {
                $this.css('display', '').addClass('tree-active');
                box.css('display', '');
                che.css('display', '');
                box.children().not(che).css('display', 'none');
            }
            if (che.length == 0 && t_che.length == 0) {
                $this.css('display', 'none');
            }
        });
    }
    var cur = $(this);
    function keyup() {
        var val = cur.val();
        search(val);
        if (val.length == 0) {
            $('.tree').removeClass('tree-active');
        }
    }
    //清除
    function blur() { cur.off({ 'blur': blur, 'keyup': keyup }); }
    cur.on({ 'blur': blur, 'keyup': keyup });
    //输入框选中文本，IE11不支持
    this.setSelectionRange(0, this.value.length);
});