/* eslint-disable *///此处不是注释，react当中不校验代码，不加此句react当中会报错
/*
上传文件，参数如下：
    url：上传的url地址，
    accept：<input type="file">标签的 accept属性值 表示可以选的文件类型
    multiple：是否一次可以选多个文件，true 多个，false 单个
    success：上传成功的回调，
    progress：当前已上传的百分比（进度）
    headers：http 请求头
    data：body参数，格式：aa=11&bb=22
*/
$.upload = function (settings) {
    var param = $.extend({
        url: null,//上传接口地址
        accept: null,//<input type="file">标签的 accept属性值 表示可以选的文件类型
        multiple: null,//一次是否可以选多个文件
        success: null,//上传成功的回调函数
        progress: null,//当前已上传的百分比（进度）的回调函数
        headers: null,//http 请求头
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
            headers: param.headers,
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
