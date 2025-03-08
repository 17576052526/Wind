import React, { useState, useEffect } from 'react'
import { BtnPrimary } from './forms'
import { apiUrl, uploadPath } from '../config'
import upload from '../_plugins/upload/upload'


export function UploadImage({ value, name, readOnly, multiple }) {
    let [data, setData] = useState([])
    useEffect(() => setData(value ? value.split('|') : []), [value])

    function up() {
        upload({
            url: apiUrl + uploadPath,
            formData: null,
            multiple: multiple,
            accept: 'image/*',
            headers: null,
            success: function (msg) {
                var msg = JSON.parse(msg);
                if (msg.code == 200) {
                    if (multiple) {
                        setData([...data, ...msg.data.split('|')]);
                    } else {
                        setData([msg.data]);
                    }
                } else {
                    alert(msg.msg)
                }
            },
            progress: function (msg) { e.target.innerHTML = msg == '100%' ? '上传成功' : msg; }
        })
    }
    return (
        <div>
            <div style={{ display: 'flex', flexWrap: 'wrap' }}>
                {data.map((m, i) =>
                    <div style={{ marginRight: '10px', position: 'relative' }}>
                        <img src={apiUrl + m} className="img-show" style={{ width: '80px', height: '80px', objectFit: 'contain', border: '1px solid #eee' }} />
                        {!readOnly && <div style={{ position: 'absolute', right: '0px', top: '-5px', fontSize: '18px', color: '#aaa', fontWeight: 'bold', transform: 'rotate(45deg)', cursor: 'pointer' }} onClick={() => { data.splice(i, 1); setData([...data]) }}>+</div>}
                    </div>
                )}
            </div>
            <input type="hidden" value={data.join('|')} name={name} />
            {!readOnly && <BtnPrimary onClick={up}>图片上传</BtnPrimary>}
        </div>
    )
}

export function UploadFile({ value, name, readOnly, multiple }) {
    let [data, setData] = useState([])
    useEffect(() => setData(value ? value.split('|') : []), [value])

    function up() {
        upload({
            url: apiUrl + uploadPath,
            formData: null,
            multiple: multiple,
            accept: '*/*',
            headers: null,
            success: function (msg) {
                var msg = JSON.parse(msg);
                if (msg.code == 200) {
                    if (multiple) {
                        setData([...data, ...msg.data.split('|')]);
                    } else {
                        setData([msg.data]);
                    }
                } else {
                    alert(msg.msg)
                }
            },
            progress: function (msg) { e.target.innerHTML = msg == '100%' ? '上传成功' : msg; }
        })
    }
    return (
        <div>
            <div style={{ display: 'flex', flexWrap: 'wrap' }}>
                {data.map((m, i) =>
                    <div style={{ marginRight: '15px', position: 'relative' }}>
                        <a href={apiUrl + '/api/common/download?path=' + m}>{m.substring(m.lastIndexOf('/') + 1)}</a>
                        {!readOnly && <div style={{ position: 'absolute', right: '-8px', top: '-8px', fontSize: '18px', color: '#aaa', fontWeight: 'bold', transform: 'rotate(45deg)', cursor: 'pointer' }} onClick={() => { data.splice(i, 1); setData([...data]) }}>+</div>}
                    </div>
                )}
            </div>
            <input type="hidden" value={data.join('|')} name={name} />
            {!readOnly && <BtnPrimary onClick={up}>文件上传</BtnPrimary>}
        </div>
    )
}