﻿$.fn.inputSearch = function (settings) {
    var param = $.extend({
        type: 'get',   //ajax请求类型 get，post
        data: null,    //ajax请求参数
        url: null,    //ajax请求路径
        toJson: null,//返回的数据转换成指定格式的json
        dropClick: null//点击下拉触发的事件
    }, settings || {});

    var cur = this;
    var timer = null;
    this.attr('autocomplete', 'off');//关闭浏览器自动填充

    function search() {
        var val = cur.val();
        if (val.length == 0) { $('#__inputSearch4725').remove(); return; }
        $.ajax({
            type: param.type,
            url: param.url + val,
            data: param.data,
            success: function (msg) {
                $('#__inputSearch4725').remove();
                var arr = param.toJson(msg);
                if (arr.length > 0) {
                    var box = $('<ul class="inputSearchBox" id="__inputSearch4725" style="position:absolute;z-index:1;left:' + cur.offset().left + 'px;top:' + (cur.offset().top + cur.outerHeight()) + 'px;min-width:' + cur.outerWidth() + 'px"></ul>').appendTo(document.body);
                    for (var i = 0; i < arr.length; i++) {
                        box.append('<li value="' + arr[i].value + '">' + arr[i].text.replace(val, function (s) { return '<span>' + s + '</span>'; }) + '</li>');
                    }
                    box.on('mousedown', 'li', function () {//此处mousedown 代替click
                        var val = $(this).attr('value');
                        var text = $(this).text();
                        cur.attr('val', val);
                        cur.val(text);
                        param.dropClick(val, text);
                    })
                }
            }
        });
    }
    this.on('keydown', function () {
        clearTimeout(timer);
        timer = setTimeout(search, 300);
    })
    this.on('focus', function () {
        search();
    })
    this.on('click', function (e) {
        e.stopPropagation();
    })
}
//隐藏，此处没用blur 设置隐藏，因为blur有时不触发
$(document).on('click', function () { $('#__inputSearch4725').remove(); })