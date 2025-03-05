import axios from 'axios'
import { startLoading, stopLoading } from './loading';


export default function () {


    //axios 添加 loading 效果
    //axios 请求
    axios.interceptors.request.use(config => {
        startLoading(document.body);
        return config
    }, err => {
        stopLoading(document.body);
        return Promise.reject(err)
    })
    //axios 响应
    axios.interceptors.response.use(res => {
        stopLoading(document.body);
        return res
    }, err => {
        stopLoading(document.body);
        return Promise.reject(err)
    })



    return (
        <>
            <input type="button" value="测试startLoading()" onClick={() => startLoading(document.body)} />
            <input type="button" value="测试stopLoading()" style={{ position:'relative',zIndex:'1000000'}} onClick={() => stopLoading(document.body)} />
        </>
    );
}
