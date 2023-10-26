/* eslint-disable *///此处不是注释，react当中不校验代码，不加此句react当中会报错
/*
图片上传参数：
        class ="upload-img"（必填）： 绑定图片上传点击事件
        url（必填）：图片上传接口地址
        hidden-name：隐藏域的 name，存在值会自动生成隐藏域，用于form提交图片地址，如果无form提交可不理会此属性
        multiple：是否可以允许上传多张图片，true：允许，默认值（null值）：不允许
        data：页面加载时显示已上传的图片
        upload-param：上传图片时body参数，格式：aa=11&bb=22，如果没有则不管此参数
        accept：可选文件类型，默认值：image/gif,image/jpeg,image/jpg,image/png,image/svg
获取图片地址：
        $(按钮对象).attr('data') 获取图片地址
*/
//加载
$(function () {
    $('.upload-img').each(function () {
        var cur = $(this);
        var data = cur.attr('data');
        //生成box
        var boxId = 'imgBox' + Math.floor(Math.random() * 1000000);
        cur.attr('boxId', boxId);
        var box = $('<div class="upload-img-box" id="' + boxId + '"></div>').insertBefore(cur);
        //生成隐藏域
        var hidName = cur.attr('hidden-name');
        if (hidName) {
            var hiddenId = 'hidden' + Math.floor(Math.random() * 1000000);
            cur.attr('hiddenId', hiddenId);
            $('<input type="hidden" name="' + hidName + '" id="' + hiddenId + '" value="' + data + '"  />').insertBefore(cur)
        }
        addImg(box, data);
    })
})
//追加显示的图片 obj:按钮对象，data:要追加的图片地址
function addImg(box, data) {
    data && data.split('|').forEach(function (m) {
        box.append('<div href="' + m + '" style="cursor:pointer"><img src="' + m + '" class="img-show" /><span class="upload-img-remove"></span></div>');
    })
}
//根据显示的图片重构值
function imgToVal(obj) {
    var box = $('#' + obj.attr('boxId'));
    var val = '';
    box.children().each(function () {
        val += '|' + $(this).attr('href');
    })
    val = val.substr(1);
    obj.attr('data', val);
    var hiddenid = obj.attr('hiddenId');
    if (hiddenid) {
        $('#' + hiddenid).val(val);
    }
}
//上传按钮点击事件
$(document).on('click', '.upload-img', function () {
    var cur = $(this);
    $.upload({
        url: cur.attr('url'),
        accept: cur.attr('accept') || "image/gif,image/jpeg,image/jpg,image/png,image/svg,image/webp",
        multiple: cur.attr('multiple'),
        data: cur.attr('upload-param'),
        success: function (msg) {
            var box = $('#' + cur.attr('boxId'));
            if (!cur.attr('multiple')) { box.empty(); }
            addImg(box, msg);
            imgToVal(cur);
        }
    });
});
//移除图片
$(document).on('click', '.upload-img-remove', function (e) {
    e.preventDefault();
    var cur = $(this);
    var obj = $('.upload-img[boxId=' + cur.closest('.upload-img-box').attr('id') + ']')
    cur.closest('div').remove();//此代码位置不能改
    imgToVal(obj);
})