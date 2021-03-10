//暂无代码
/*
    树结构显示隐藏
        .tree 每一个项（项的类样式）
        .tree-switch 触发显示隐藏的按钮
        .tree-box 子项的父级
        .tree-active 激活（显示）的样式
*/
$(document).on('click', '.tree-switch', function () {
    var box = $(this).closest('.tree');
    var child = box.children('.tree-box');
    if (box.hasClass('tree-active')) {
        //当前隐藏
        child.css('display', 'block');
        box.removeClass('tree-active');
        child.slideUp(300);
    } else {
        //其他显示的隐藏
        var active = box.parent().children('.tree-active');
        active.children('.tree-box').css('display', 'block').slideUp(300);
        active.removeClass('tree-active');
        //当前显示
        child.slideDown(300);
        box.addClass('tree-active');
    }
});
/*
 树结构搜索插件
 .tree-search 搜索输入框
 .tree 树的项
 .tree-box 子级的父级.
 .tree-active 打开子节点
 注意：不能在节点用 style设置display，如果要设置 display 用类样式设置
 */
$(document).on('focus', '.tree-search', function () {
    function search(str) {
        $('.tree').each(function () {
            var $this = $(this);
            var box = $this.children('.tree-box');
            var che_title = $this.children(':not(".tree-box"):contains("' + str + '")');
            var che_chil = box.children(':contains("' + str + '")');
            if (che_title.length > 0 || che_chil.length > 0) {
                if (che_title.length > 0) {
                    $this.css('display', '').removeClass('tree-active');
                    box.css('display', '');
                    box.children().css('display', '');
                }
                if (che_chil.length > 0) {
                    $this.css('display', '').addClass('tree-active');
                    box.css('display', '');
                    che_chil.css('display', '');
                    box.children().not(che_chil).css('display', 'none');
                }
            } else {
                $this.css('display', 'none');
            }
        });
    }
    var cur = $(this);
    var timer = null;
    function keydown() {
        clearTimeout(timer);
        timer = setTimeout(function () {
            var val = cur.val();
            search(val);
            if (val.length == 0) {
                $('.tree').removeClass('tree-active');
            }
        }, 300)
    }
    //清除
    function blur() { cur.off({ 'blur': blur, 'keydown': keydown }); }
    cur.on({ 'blur': blur, 'keydown': keydown });
    //输入框选中文本，IE11不支持
    this.setSelectionRange(0, this.value.length);
});