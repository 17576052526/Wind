/*
上传文件，参数如下：
    url：上传的url地址，
    accept：<input type="file">标签的 accept属性值 表示可以选的文件类型
    multiple：是否一次可以选多个文件，true 多个，false 单个
    success：上传成功的回调，
    progress：当前已上传的百分比（进度）
*/
+function ($) {
    $.upload = function (url, accept, multiple, success, progress) {
        $('#__upload').remove();
        var form = $('<form id="__upload"></form>').appendTo(document.body);
        var file = $('<input type="file" name="__uploadFile" accept="' + (accept || '') + '" ' + (multiple ? 'multiple="multiple"' : '') + ' style="display:none;" />').appendTo(form).on('change', function () {
            $.ajax({
                type: "post",
                url: url,
                data: new FormData(form[0]),
                contentType: false,
                processData: false,
                success: success,
                error: function () { alert('上传失败，可能原因：\n1.跨域问题\n2.上传的文件太大\n3.url地址错误'); },
                xhr: function () {
                    var xhr = $.ajaxSettings.xhr();
                    xhr.upload.addEventListener('progress', function (e) {
                        progress(Math.floor(100 * e.loaded / e.total));//执行外部函数，并把进度回传回去
                    }, false);
                    return xhr;
                }
            });
        });
        file.click();
    }
}(jQuery)