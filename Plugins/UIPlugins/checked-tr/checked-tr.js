/*
    复选框选中父级<tr>添加类样式
        .check-item 触发事件的复选框
        .checked-tr 选中时添加在<tr>上的类样式
*/
//js修改checked属性也能触发change事件（这段代码容易引发未知的bug，一般不用）
/*
$.propHooks.checked = {
    set: function (elem, value, name) {
        if (elem[name] != value) {
            elem[name] = value;
            $(elem).trigger("change");
        }
    }
};
*/
//复选框选中，tr添加类样式
$(document).on('change', '.check-item', function () {
    if ($(this).prop('checked')) {
        $(this).closest('tr').addClass('checked-tr');
    }
    else {
        $(this).closest('tr').removeClass('checked-tr');
    }
});