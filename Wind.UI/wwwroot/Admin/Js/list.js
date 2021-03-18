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