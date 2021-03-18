//批量删除
function checkDelete() {
    var check = $('#form [name="checkID"]:checked');
    if (check.length == 0) {
        alert('请选择删除的行');
    } else {
        if (confirm('确定删除？')) {
            $('#form').submit();
        }
    }
}
//排序，加载
$(function () {
    var orderby = queryString('orderby');
    if (orderby != null && orderby.length > 0) {
        $('#searchForm').append('<input type="hidden" name="orderby" id="__hiddenOrderby" value="' + orderby + '" />');
        //加倒叙排序类样式
        var arr = orderby.split(/\++/);
        if (arr.length > 1 && arr[1].toLowerCase() == 'desc') {
            $('.orderby[orderby="' + arr[0] + '"]').addClass('orderby-desc')
        } else {
            $('.orderby[orderby="' + arr[0] + '"]').addClass('orderby-asc')
        }
    }
})
//排序
$(document).on('click', '[orderby]', function () {
    var colName = $(this).attr('orderby');
    var hidden = $('#__hiddenOrderby');
    if (hidden.length == 0) {
        $('#searchForm').append('<input type="hidden" name="orderby" id="__hiddenOrderby" value="' + colName + '" />');
    } else {
        hidden.val(hidden.val() == colName ? colName + ' desc' : colName);
    }
    $('#searchForm').submit();
})