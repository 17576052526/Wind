import React, { Suspense, lazy } from 'react';
import { HashRouter as Router, Routes, Route, useNavigate, Navigate } from "react-router-dom";//改用 BrowserRouter 路由，只需此行的 HashRouter 改成BrowserRouter，其他都不用改
import axios from 'axios'
import $ from 'jquery'
import common from './common'

/****************************全局设置，不要写在方法里面，因为重新渲染的时候会再次执行****************************/
//设置服务器请求地址，common.apiUrl值 是 src/setupProxy.js 配置的代理服务
axios.defaults.baseURL = common.apiUrl;

//设置token
axios.interceptors.request.use(config => {
    config.headers.Authorization = (common.get('user') || {}).token;
    return config
});

//对返回结果做统一处理
axios.interceptors.response.use(response => {
    let data = response.data;
    if (data.code == 210 || data.code == 401) {//普通错误（不做特殊处理的，直接弹框提示）
        $.alert ? $.alert(data.msg) : alert(data.msg)
        return Promise.reject(response);//标识为失败状态，并且不会进入到axios的回调函数里面
    }
    else if (data.code == 403) {//访问未认证的接口处理
        $.alert ? $.alert(data.msg) : alert(data.msg);
        window.location.href = Router.name == 'HashRouter' ? '#/login' : '/login';//此处用不了 useNavigate()跳转
        return Promise.reject(response);
    }
    return data;//200 和自定义状态码就返回
}, (error) => {
    $.alert ? $.alert(error) : alert(error);//500错误会自动进入此处
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
                    {/*<Route path="/" exact element={<Login />}></Route>*/}{/*设置主页*/}
                    <Route path="/page1" element={<Page1 />}></Route>
                    <Route path="/page2" element={<Page2 />}></Route>
                    <Route path="/login" element={<Login />}></Route>
                    <Route path="/admin/*" element={<Admin />}></Route>
                    <Route path="*" element={<div>404--页面不存在</div>}></Route>
                </Routes>
            </Router>
        </Suspense>
    );
}

export default App;
