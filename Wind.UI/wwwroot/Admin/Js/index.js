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
//树结构搜索插件
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