+function ($) {
    //url:提交的url,输入框的value是直接跟在url后面的，jsonFun：json解析方法，selectClickFun：下拉点击事件
    $.fn.inputSearch = function (url, jsonFun, selectClickFun) {
        this.on('blur', function () { $('#__inputSearch4725').css('display', 'none'); });
        this.on('focus', function () { $('#__inputSearch4725').css('display', 'block'); });
        this.on('keyup', function () {
            if (this.value.length == 0) { $('#__inputSearch4725').remove(); return; }
            var cur = $(this);
            $.ajax({
                type: "get",
                url: url + this.value,
                success: function (msg) {
                    var arr = jsonFun(msg);
                    $('#__inputSearch4725').remove();
                    var ul = $('<ul class="inputSearchBox" id="__inputSearch4725" style="position:absolute;left:' + cur.offset().left + 'px;top:' + (cur.offset().top + cur.outerHeight()) + 'px;width:' + cur.outerWidth() + 'px"></ul>');
                    for (var m of arr) {
                        ul.append('<li value="' + m.value + '">' + m.text.replace(cur.val(), function (s) { return '<span>' + s + '</span>'; }) + '</li>');
                    }
                    ul.children().on('mousedown', function () { $('#__inputSearch4725').remove(); cur.val($(this).text()); cur.attr('val', $(this).attr('value')); selectClickFun(); });//绑定事件
                    $('body').append(ul);
                },
                error: function () { alert('请求失败'); }
            })
        })
    }
}(jQuery)