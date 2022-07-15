/*
上传文件，参数如下：
    url：上传的url地址，
    accept：<input type="file">标签的 accept属性值 表示可以选的文件类型
    multiple：是否一次可以选多个文件，true 多个，false 单个
    success：上传成功的回调，
    progress：当前已上传的百分比（进度）
    data：body参数，格式：aa=11&bb=22
*/
+function ($) {
    $.upload = function (settings) {
        var param = $.extend({
            url: null,//上传接口地址
            accept: null,//<input type="file">标签的 accept属性值 表示可以选的文件类型
            multiple: null,//一次是否可以选多个文件
            success: null,//上传成功的回调函数
            progress: null,//当前已上传的百分比（进度）的回调函数
            data: ''//body参数
        }, settings || {});
        $('#__upload').remove();
        var form = $('<form id="__upload"></form>').appendTo(document.body);
        var file = $('<input type="file" name="__uploadFile" accept="' + (param.accept || '') + '" ' + (param.multiple ? 'multiple="multiple"' : '') + ' style="display:none;" />').appendTo(form).on('change', function () {
            var formData = new FormData(form[0]);
            param.data.split('&').forEach(function (m) {
                var i = m.indexOf('=');
                if (i != -1) {
                    formData.append(m.substr(0, i), m.substr(i + 1));
                }
            });
            $.ajax({
                type: "post",
                url: param.url,
                data: formData,
                contentType: false,
                processData: false,
                success: param.success,
                error: function () { alert('上传失败，可能原因：\n1.跨域问题\n2.上传的文件太大\n3.上传接口地址错误'); },
                xhr: function () {
                    var xhr = $.ajaxSettings.xhr();
                    param.progress && xhr.upload.addEventListener('progress', function (e) {
                        param.progress(Math.floor(100 * e.loaded / e.total));//执行外部函数，并把进度回传回去
                    }, false);
                    return xhr;
                }
            });
        });
        file.click();
    }
}(jQuery)

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
+function ($) {
    //加载
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
    //追加显示的图片 obj:按钮对象，data:要追加的图片地址
    function addImg(box, data) {
        data && data.split('|').forEach(function (m) {
            box.append('<a href="' + m + '" target="_blank"><img src="' + m + '" /><span class="upload-img-remove"></span></a>');
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
            accept: cur.attr('accept') || "image/gif,image/jpeg,image/jpg,image/png,image/svg",
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
        cur.closest('a').remove();//此代码位置不能改
        imgToVal(obj);
    })
}(jQuery)