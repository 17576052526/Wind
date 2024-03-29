import React, { useEffect, Suspense, lazy } from 'react';
import { Routes, Route, useNavigate, useLocation } from "react-router-dom";
import axios from 'axios'
import $ from 'jquery'
import common, { useStates } from '../common'
import './importShare'
import './_css/admin.css'

let Test_Main = lazy(() => import('./pages/Test_Main'))
let Index = lazy(() => import('./index'))

export default function () {
    let [state, setState] = useStates({
        //定义状态
        isLeftShow: 1,//左边块是否显示 ，存在值则显示
    });

    let navigate = useNavigate();
    let location = useLocation();

    useEffect(() => {

    }, []);

    //注销
    function cancel() {
        $.confirm('确定注销？', () => {
            axios.get('/api/common/cancel').then(msg => {
                navigate('/login')
            });
        })
    }

    return (
        <div className="flex-column h-100">
            {/*头部*/}
            <div className="flex" style={{ height: '50px', backgroundColor: '#fff' }} >
                {state.isLeftShow && <div className="logo">WindAdmin</div>}
                <div className="flex-1 flex" style={{ borderBottom: '1px solid #DEE2E6' }} >
                    <div className="flex-1 flex-center">
                        <span className="head-nav-item mlr-15 font-16" onClick={() => setState({ isLeftShow: !state.isLeftShow })}><i className="icon-reorder"></i></span>
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
                        <span className="head-nav-item mlr-10"><i className="icon-user"></i>{common.get('user') && common.get('user').UserName}</span>
                        <span className="head-nav-item mlr-10" onClick={() => cancel()}><i className="icon-off"></i>注销</span>
                    </div>
                </div>
            </div>
            {/*******/}
            <div className="flex-1 flex">
                {/*左边栏目*/}
                {state.isLeftShow && <div className="left-body">
                    <div className="flex">
                        <input type="text" className="flex-1 search-text" placeholder="搜索" />
                        <div className="search-btn"><i className="icon-search"></i></div>
                    </div>
                    <ul className="nav">
                        <li className="tree tree-active">
                            <div className="nav-switch icon-folder_open tree-switch">系统管理</div>
                            <ol className="tree-box">
                                <li><a className={"nav-btn icon-circle_blank " + (location.pathname == '/admin' && 'nav-btn-active')} onClick={() => navigate("")}>首页</a></li>
                                <li><a className={"nav-btn icon-circle_blank " + (location.pathname == '/admin/test_main' && 'nav-btn-active')} onClick={() => navigate("test_main")}>测试页</a></li>
                                <li><a className="nav-btn icon-circle_blank" href="#">编辑页</a></li>
                            </ol>
                        </li>
                        <li className="tree">
                            <div className="nav-switch icon-folder_open tree-switch">个人中心</div>
                            <ol className="tree-box">
                                <li><a className="nav-btn icon-circle_blank" href="#">编辑页</a></li>
                                <li><a className="nav-btn icon-circle_blank" href="#">编辑页</a></li>
                                <li><a className="nav-btn icon-circle_blank" href="#">编辑页</a></li>
                            </ol>
                        </li>
                        <li className="tree">
                            <div className="nav-switch icon-folder_open tree-switch">消息中心</div>
                            <ol className="tree-box">
                                <li><a className="nav-btn icon-circle_blank" href="#">编辑页</a></li>
                                <li><a className="nav-btn icon-circle_blank" href="#">编辑页</a></li>
                                <li><a className="nav-btn icon-circle_blank" href="#">编辑页</a></li>
                            </ol>
                        </li>
                        <li>
                            <div className="nav-btn icon-folder_open">一级菜单</div>
                        </li>
                    </ul>
                </div>}
                {/*右边块*/}
                <div className="flex-1 p-7.5">
                    <Suspense fallback="">
                        <Routes>
                            <Route path="" exact element={<Index />}></Route>{/*设置主页*/}
                            <Route path="Test_Main" element={<Test_Main />}></Route>
                            <Route path="*" element={<div>404--页面不存在</div>}></Route>
                        </Routes>
                    </Suspense>
                </div>
            </div>
        </div>
    );
}
