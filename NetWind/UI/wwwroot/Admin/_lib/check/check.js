+function ($) {
    //绑定全选
    $(document).on('change', '.check-box .check-all', function () {
        var cur = $(this);
        cur.closest('.check-box').find('.check-item').prop('checked', cur.prop('checked'));
    });
    //所有的都选择就勾上全选，否则去掉全选
    $(document).on('change', '.check-box .check-item', function () {
        var cur = $(this);
        var checked = cur.prop('checked');
        var box = cur.closest('.check-box');
        if (checked && box.find('.check-item:not(:checked)').length == 0) {
            box.find('.check-all').prop('checked', true);
        } else if (!checked) {
            box.find('.check-all').prop('checked', false);
        }
    });
}(jQuery)