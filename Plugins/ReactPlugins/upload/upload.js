
export default function ({ url, formData, multiple, accept, headers, success, progress }) {
    var file = document.createElement('input');
    file.type = 'file';
    if (multiple) { file.multiple = 'multiple' }
    if (accept) { file.accept = accept }
    file.onchange = function () {
        if (!formData) { formData = new FormData() }
        for (let m of file.files) { formData.append('__file', m) }
        //ajax上传
        var xhr = new XMLHttpRequest();
        xhr.open('POST', url);
        if (headers) {
            for (let k in headers) {
                xhr.setRequestHeader(k, headers[k]);
            }
        }
        xhr.onload = function () {
            if (xhr.status === 200) {
                success && success(xhr.responseText);
            } else {
                alert('上传失败！错误信息：' + xhr.responseText);
            }
        }
        if (progress) {
            xhr.upload.onprogress = function (e) {
                if (e.lengthComputable) {
                    progress(Math.round(e.loaded / e.total * 100) + '%');
                }
            }
        }
        xhr.send(formData);
    }
    file.click();
}