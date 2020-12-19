//树结构显示隐藏
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
//左边显示隐藏 .left-switch 开关按钮  #left 显示隐藏的块
$(document).on('click','.left-switch', function () {
    var cur = $('#left');
    if (cur.css('display') == 'none') {
        cur.css('display', 'block');
    } else {
        cur.css('display', 'none');
    }
});
//左边搜索框搜索事件  .tree-search 输入框，.tree 树的项，.tree-box 子级的父级，tree-active 打开，注意：不能在节点用style 属性设置display，如果要设置显示隐藏请用类样式设置
$(document).on('focus', '.tree-search', function () {
    function search(str) {
        $('.tree').each(function () {
            var $this = $(this);
            var box = $this.children('.tree-box');
            var che = box.children(':contains("' + str + '")');
            if (che.length > 0) {
                $this.css('display', '').addClass('tree-active');
                che.css('display', '');
                box.children().not(che).css('display', 'none');
            } else {
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