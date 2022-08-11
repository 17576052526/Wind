import React, { Suspense, lazy } from 'react';
import { HashRouter as Router, Routes, Route, Navigate } from "react-router-dom";
import axios from 'axios'
import common from './common'

let Page1 = lazy(() => import('./components/page1'))
let Page2 = lazy(() => import('./components/page2'))
let Login = lazy(() => import('./components/login'))

function App() {
    //设置服务器请求地址，/api 是 src/setupProxy.js 配置的代理服务
    axios.defaults.baseURL = common.baseURL;

    //设置token
    axios.interceptors.request.use(config => {
        config.headers.Authorization = (common.getSessionStorage("loginMsg") || {}).token;
        return config
    });

    //对返回结果做统一处理
    axios.interceptors.response.use(response => {
        let data = response.data;
        if (data.code == -1) { alert(data.msg) }//普通错误（不做特殊处理的，直接弹框提示）
        return data;
    }, (error) => {
        alert("服务器异常");
        return Promise.reject(error);
    });

    return (
        <Suspense fallback="">
            <Router>
                <Routes>
                    {/*<Route path="/" element={<Navigate to="/page2" />}></Route>*/}{/*重定向*/}
                    {/*<Route path="/" exact component={Page1}></Route>*/}{/*设置主页*/}
                    <Route path="/page1" element={<Page1 />}></Route>
                    <Route path="/page2" element={<Page2 />}></Route>
                    <Route path="/login" element={<Login />}></Route>
                </Routes>
            </Router>
        </Suspense>
    );
}

export default App;
