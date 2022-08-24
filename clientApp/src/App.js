import React, { Suspense, lazy } from 'react';
import { HashRouter as Router, Routes, Route, useNavigate } from "react-router-dom";
import axios from 'axios'
import $ from 'jquery'
import common from './common'

/****************************全局设置，不要写在方法里面，因为重新渲染的时候会再次执行****************************/
//设置服务器请求地址，/api 是 src/setupProxy.js 配置的代理服务
axios.defaults.baseURL = common.apiUrl;

//设置token
axios.interceptors.request.use(config => {
    config.headers.Authorization = common.getUser() && common.getUser().token;
    return config
});

//对返回结果做统一处理
axios.interceptors.response.use(response => {
    let data = response.data;
    if (data.code == -1) { $.alert ? $.alert(data.msg) : alert(data.msg) }//普通错误（不做特殊处理的，直接弹框提示）
    else if (data.code == 403) { $.alert ? $.alert(data.msg) : alert(data.msg); }//访问未认证的接口处理
    return data;
}, (error) => {
    $.alert ? $.alert(error) : alert(error);
    return Promise.reject(error);
});
/****************************************************************************************************************/


let Page1 = lazy(() => import('./admin/page1'))
let Page2 = lazy(() => import('./admin/page2'))
let Login = lazy(() => import('./admin/login'))
let Admin = lazy(() => import('./admin/admin'))

function App() {

    return (
        <Suspense fallback="">
            <Router>
                <Routes>
                    {/*<Route path="/page1/*" element={<Page1 />}></Route>*/}{/*二级路由写法，父路由要加/*  ，子路由<Route path="Test_Main" element={<Test_Main />}></Route> */}
                    {/*<Route path="/" element={<Navigate to="/page2" />}></Route>*/}{/*重定向*/}
                    {/*<Route path="/" exact component={Page1}></Route>*/}{/*设置主页*/}
                    <Route path="/page1" element={<Page1 />}></Route>
                    <Route path="/page2" element={<Page2 />}></Route>
                    <Route path="/login" element={<Login />}></Route>
                    <Route path="/admin/*" element={<Admin />}></Route>
                </Routes>
            </Router>
        </Suspense>
    );
}

export default App;
