import upload from './upload'

/*
        上传文件
        url：ajax请求地址
        formData：FormData对象
            form 转 FormData    var formData = new FormData(document.getElementById('form'))
            formData添加参数    formData.append('key','value');
            file添加到FormData  for (let m of file.files) { formData.append('file', m) }
        multiple：是否可以选择多文件，true 多文件，false或null 单文件
        accept：可以选择的文件类型  image/* 表示可以选择所有图片
        headers：请求头，json对象格式
        success：回调函数
        progress：上传进度回调函数
        */
export default function () {
    return (
        <>
            <input type="button" value="上传图片" onClick={(e) => upload({
                url: '/server/api/common/Upload',
                formData: null,
                multiple: true,
                accept: 'image/*',
                headers: { token: 'a1111' },
                success: function (msg) { alert('上传成功，消息：' + msg) },
                progress: function (msg) { e.target.value = msg == '100%' ? '上传成功' : msg; }
            })} />

            <input type="button" value="上传文件" onClick={(e) => upload({
                url: '/server/api/common/Upload',
                formData: null,
                multiple: false,
                accept: '*/*',
                headers: { token: 'a1111' },
                success: function (msg) { alert('上传成功，消息：' + msg) },
                progress: function (msg) { e.target.value = msg == '100%' ? '上传成功' : msg; }
            })} />
        </>
    );
}
