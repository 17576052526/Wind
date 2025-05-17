import React, { useEffect, useRef, Suspense, lazy } from 'react'
import { Routes, Route, useNavigate, useLocation, Link } from "react-router-dom"
import axios from 'axios'
import base, { useStates } from '../_utils/base'
import '../_plugins//atomCss/atom.css'
import './assets/font/css/fontello.css'
import '../_plugins//AdminUI2/UI.css'
import alert from '../_plugins/alert/alert';
import confirm from '../_plugins/confirm/confirm'
import { findTree, findTreeBatUpdate } from '../_utils/findTree'
import './assets/admin.css'

let TestMain = lazy(() => import('./test/testMain'))
let Index = lazy(() => import('./index'))

export default function () {
    let navigate = useNavigate();
    let location = useLocation();
    let searchTxt = useRef();
    let [state, setState] = useStates({
        sourceNavData: null,//导航源数据
        navData: null,//导航数据
        isLeftShow: 1,//左边块是否显示 ，存在值则显示
    });
    useEffect(() => {
        if (!base.get('user')) { navigate('/login'); return; }//没登录就跳转到登录页面

        axios.create({ baseURL: null }).get('./navigation.json').then(response => {
            state.sourceNavData = response.data;
            setState({ navData: response.data });
        }).catch(error => alert(error));
    }, [])

    //注销
    function cancel() {
        confirm('确定注销？', () => {
            base.set('user', null);
            navigate('/login')
        })
    }

    //导航搜索
    function findNav() {
        if (searchTxt.current.value.length > 0) {
            let data = findTree(state.sourceNavData, (m) => m.name.indexOf(searchTxt.current.value) != -1);
            findTreeBatUpdate(data, 'isOpen', 1);//全部展开
            setState({ navData: data });
        } else {
            setState({ navData: state.sourceNavData });
        }
    }

    return (
        <div className="admin">
            {/*头部*/}
            <div className="admin-head">
                {state.isLeftShow && <div className="admin-logo">WindAdmin</div>}
                <div className="admin-head-right">
                    <div className="flex-1 flex-items-center">
                        <span className="admin-head-nav mlr-4" onClick={() => setState({ isLeftShow: !state.isLeftShow })}><i className="icon-reorder font-16"></i></span>
                        <span className="admin-head-nav mlr-4">首页</span>
                        <span className="admin-head-nav mlr-4">联系人</span>
                    </div>
                    <div className="flex-items-center">
                        <span className="admin-head-nav mlr-2.5">
                            <i className="icon-envelope"></i>
                            <span className="admin-mark">12</span>
                        </span>
                        <span className="admin-head-nav mlr-2.5">
                            <i className="icon-bell"></i>
                            <span className="admin-mark">8</span>
                        </span>
                        <span className="admin-head-nav mlr-2.5"><i className="icon-user"></i>管理员</span>
                        <span className="admin-head-nav mlr-2.5" onClick={() => cancel()}><i className="icon-off"></i>注销</span>
                    </div>
                </div>
            </div>
            {/*******/}
            <div className="flex-1 flex">
                {/*左边栏目*/}
                {state.isLeftShow && <div className="admin-left">
                    <div className="flex">
                        <input type="text" className="admin-search-text" placeholder="搜索" ref={searchTxt} onKeyUp={(e) => e.key == 'Enter' && findNav()} />
                        <div className="admin-search-btn" onClick={() => findNav()}><i className="icon-search"></i></div>
                    </div>
                    <ul className="admin-nav">
                        {state.navData && state.navData.map((m, i) =>
                            <li className={m.isOpen ? "admin-tree-active" : ""} key={i}>
                                <div className={"admin-nav-switch " + m.icon} onClick={() => { m.isOpen = !m.isOpen; setState({ navData: state.navData }) }}>{m.name}</div>
                                <ol className="admin-tree-box">
                                    {m.children && m.children.map((c, j) =>
                                        <li key={j}><Link className={"admin-nav-btn " + c.icon + (location.pathname == c.src ? ' admin-nav-btn-active' : '')} to={c.src}>{c.name}</Link></li>
                                    )}
                                </ol>
                            </li>
                        )}

                        {/*
                        <li className="admin-tree-active">
                            <div className="admin-nav-switch icon-folder_open">一级菜单</div>
                            <ol className="admin-tree-box">
                                <li><a className="admin-nav-btn icon-circle_blank" href="#">编辑页</a></li>
                                <li><a className="admin-nav-btn icon-circle_blank" href="#">编辑页</a></li>
                                <li><a className="admin-nav-btn-active admin-nav-btn icon-circle_blank" href="#">编辑页</a></li>
                            </ol>
                        </li>
                        <li>
                            <div className="admin-nav-btn-active admin-nav-btn icon-folder_open">一级菜单</div>
                        </li>
                        */}
                    </ul>
                </div>}
                {/*右边块*/}
                <div className="flex-1 ptb-2 plr-2">
                    <Suspense fallback="">
                        <Routes>
                            <Route path="" exact element={<Index />}></Route>{/*设置主页*/}
                            <Route path="TestMain" element={<TestMain />}></Route>
                            <Route path="*" element={<div>404--页面不存在</div>}></Route>
                        </Routes>
                    </Suspense>
                </div>
            </div>
        </div>
    )
}
