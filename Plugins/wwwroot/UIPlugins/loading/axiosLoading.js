/*
 此文件是 react中用的
 */
import axios from 'axios'
import $ from 'jquery'
import './loading.js'
import './loading.css'
import './loading1.css'


//axios 请求
axios.interceptors.request.use(config => {
    $(document.body).loading();
    return config
}, err => {
    $(document.body).loadingStop();
    return Promise.reject(err)
})
//axios 响应
axios.interceptors.response.use(res => {
    $(document.body).loadingStop();
    return res
}, err => {
    $(document.body).loadingStop();
    return Promise.reject(err)
})