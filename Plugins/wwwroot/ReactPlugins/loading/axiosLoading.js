import axios from 'axios'
import { startLoading, stopLoading } from './loading'

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