/* eslint-disable *///此处不是注释，react当中不校验代码，不加此句react当中会报错
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