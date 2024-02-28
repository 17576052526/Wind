import { useState, useEffect } from 'react';
import common from '../../common';
import upload from '../upload'

/**
 * 图片上传，说明：
 * 1.此控件是根据系统架构和业务来的，并不是所有项目通用的控件
 * 2.按钮样式和按钮value，都在里面写死，外面不传，因为这是给后台用的，所有上传按钮都应该保持统一样式
 * 3.参数说明：
 *      multiple：true 多图上传，false 单图上传
 *      data：图片，多张图片用 | 隔开
 *      getData：函数，用于外部获取图片地址
 *      isFormal：true 上传到正式文件夹，false 上传到临时文件夹
 * 
 * 4.外部调用示例：<UploadImg multiple={true} data="/upload/20240223/1708667097282946.jpg|/upload/20240223/1708667097804681.png" getData={(data) => console.log(data)} isFormal={true}></UploadImg>
 */
export default function ({ multiple, data, getData, isFormal = true }) {
    let [list, setList] = useState([]);
    useEffect(() => {
        data && setList(data.split('|'));
    }, []);
    useEffect(() => {
        getData && getData(list.join('|'));
    }, [list]);
    return (
        <>
            <div style={{ display: 'flex', flexWrap: 'wrap' }}>
                {list.map((m,i) =>
                    <div style={{ marginRight: '10px', position: 'relative' }}>
                        <img src={common.apiUrl + m} className="img-show" style={{ width: '80px', height: '80px', objectFit: 'contain', border: '1px solid #eee' }} />
                        <div style={{ position: 'absolute', right: '0px', top: '-5px', fontSize: '18px', color: '#aaa', fontWeight: 'bold', transform: 'rotate(45deg)', cursor: 'pointer' }} onClick={() => { list.splice(i, 1); setList([...list]) }}>+</div>
                    </div>
                )}
            </div>
            <button type="button" onClick={(e) => upload({
                url: common.apiUrl + '/api/common/' + (isFormal ? 'Upload' : 'UploadCache'),
                formData: null,
                multiple: multiple,
                accept: 'image/*',
                headers: null,
                success: function (msg) {
                    var msg = JSON.parse(msg);
                    if (msg.code == 200) {
                        if (multiple) {
                            list.push(msg.data);
                            setList([...list]);
                        } else {
                            setList(msg.data.split('|'));
                        }
                    } else {
                        alert(msg.msg)
                    }
                },
                progress: function (msg) { e.target.innerHTML = msg == '100%' ? '上传成功' : msg; }
            })}>图片上传</button>
        </>
    );
}
