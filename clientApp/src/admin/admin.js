import React, { useEffect, Suspense, lazy } from 'react';
import { Routes, Route, useNavigate, useLocation } from "react-router-dom";
import axios from 'axios'
import $ from 'jquery'
import './importShare'
import './css/admin.css'

let Test_Main = lazy(() => import('./shared/Test_Main'))
let Index = lazy(() => import('./index'))

export default function () {
    let navigate = useNavigate();
    let location = useLocation();

    useEffect(() => {

    }, []);

    //注销
    function cancel() {
        $.confirm('确定注销？', async () => {
            let msg = await axios.get('/api/common/cancel');
            if (msg.code == 1) {
                navigate('/login')
            }
        })
    }

    return (
        <div className="flex-column h-100">
            {/*头部*/}
            <div className="flex" style={{ height: '50px', backgroundColor: '#fff' }} >
                <div className="logo">WindAdmin</div>
                <div className="flex-1 flex" style={{ borderBottom: '1px solid #DEE2E6' }} >
                    <div className="flex-1 flex-center">
                        <span className="head-nav-item mlr-15 font-16"><i className="icon-reorder"></i></span>
                        <span className="head-nav-item mlr-15">首页</span>
                        <span className="head-nav-item mlr-15">联系人</span>
                    </div>
                    <div className="flex-center">
                        <span className="head-nav-item mlr-10">
                            <i className="icon-envelope"></i>
                            <span className="head-nav-mark">12</span>
                        </span>
                        <span className="head-nav-item mlr-10">
                            <i className="icon-bell"></i>
                            <span className="head-nav-mark">8</span>
                        </span>
                        <span className="head-nav-item mlr-10"><i className="icon-user"></i>管理员</span>
                        <span className="head-nav-item mlr-10" onClick={() => cancel()}><i className="icon-off"></i>注销</span>
                    </div>
                </div>
            </div>
            {/*******/}
            <div className="flex-1 flex">
                {/*左边栏目*/}
                <div className="left-body">
                    <div className="flex">
                        <input type="text" className="flex-1 search-text" placeholder="搜索" />
                        <div className="search-btn"><i className="icon-search"></i></div>
                    </div>
                    <ul className="nav">
                        <li className="tree-active">
                            <div className="nav-switch icon-folder_open">一级菜单</div>
                            <ol>
                                <li><a className={"nav-btn icon-circle_blank cursor-pointer " + (location.pathname == '/admin' && 'nav-btn-active')} onClick={() => navigate("")}>首页</a></li>
                                <li><a className={"nav-btn icon-circle_blank cursor-pointer " + (location.pathname == '/admin/test_main' && 'nav-btn-active')} onClick={() => navigate("test_main")}>测试页</a></li>
                                <li><a className="nav-btn icon-circle_blank" href="#">编辑页</a></li>
                            </ol>
                        </li>
                        <li className="tree-active">
                            <div className="nav-switch icon-folder_open">一级菜单</div>
                            <ol>
                                <li><a className="nav-btn icon-circle_blank" href="#">编辑页</a></li>
                                <li><a className="nav-btn icon-circle_blank" href="#">编辑页</a></li>
                                <li><a className="nav-btn icon-circle_blank" href="#">编辑页</a></li>
                            </ol>
                        </li>
                        <li>
                            <div className="nav-switch icon-folder_open">一级菜单</div>
                        </li>
                        <li>
                            <div className="nav-btn icon-folder_open">一级菜单</div>
                        </li>
                    </ul>
                </div>
                {/*右边块*/}
                <div className="flex-1 p-7.5">
                    <Suspense fallback="">
                        <Routes>
                            <Route path="" exact element={<Index />}></Route>{/*设置主页*/}
                            <Route path="Test_Main" element={<Test_Main />}></Route>
                        </Routes>
                    </Suspense>
                </div>
            </div>
        </div>
    );
}
