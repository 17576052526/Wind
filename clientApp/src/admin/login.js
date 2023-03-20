import React, { useState, useEffect, useContext, useRef } from 'react';
import { useNavigate } from "react-router-dom";
import axios from 'axios'
import $ from 'jquery'
import '../_plugins/globalCss/global.css'
import '../_plugins/font/css/fontello.css'
import common from '../common'

export default function () {
    let navigate = useNavigate();
    let userName = useRef();
    let userPwd = useRef();
    let verifyCode = useRef();
    let verifyImg = useRef();
    let remember = useRef();
    let [isVerifyCode, setIsVerifyCode] = useState();

    useEffect(() => {
        //记住密码，读取信息
        if (common.getLocalStorage("rememberUserName")) {
            userName.current.value = common.getLocalStorage("rememberUserName");
            userPwd.current.value = common.getLocalStorage("rememberUserPwd");
            remember.current.checked = true;
            userName.current.focus();
        }
    }, [])

    //登录
    function login() {
        if (userName.current.value.length == 0) { alert('请输入登录名'); return; }
        else if (verifyCode.current && verifyCode.current.value.length == 0) { alert('请输入验证码'); return; }
        let obj = {
            userName: userName.current.value,
            userPwd: userPwd.current.value,
            verifyCode: verifyCode.current ? verifyCode.current.value : '',
        }
        axios.post("/api/common/login", obj).then(msg => {
            if (msg.code == 1) {
                //记住密码
                if (remember.current && remember.current.checked) {
                    common.setLocalStorage("rememberUserName", userName.current.value);
                    common.setLocalStorage("rememberUserPwd", userPwd.current.value);
                } else {
                    common.setLocalStorage("rememberUserName", '');
                    common.setLocalStorage("rememberUserPwd", '');
                }
                //登录成功之后，解析 返回的 token成json，保存在本地
                let tokenArr = msg.data.split('.');
                let user = JSON.parse(window.atob(tokenArr[1]));
                user.token = msg.data;
                common.setUser(user);
                navigate('/admin');
            } else if (msg.code == -2) {
                alert(msg.msg);
                verifyImg.current && verifyImg.current.click();//刷新验证码
                setIsVerifyCode(true);
            }
        });
    }



    return (
        <div className="plr-15 flex-center" style={{ height: '100vh', backgroundColor: '#E9ECEF' }}>
            <div className="max-w-100 bg-white plr-30 ptb-30 mlr-auto" style={{ width: '360px', boxShadow: '0 0 1px rgb(0 0 0 / 13%), 0 1px 3px rgb(0 0 0 / 20%)' }}>
                <div className="font-bold font-16 text-center" style={{ color: '#666' }} >后台管理中心-登录</div>
                <label className="br-4 plr-10 mt-20 flex-center" style={{ height: '38px', border: '1px solid #CED4DA' }}><input type="text" className="border-none flex-1" placeholder="登录名" ref={userName} onKeyUp={(e) => e.keyCode == 13 && login()} /><i className="icon-user font-16"></i></label>
                <label className="br-4 plr-10 mt-20 flex-center" style={{ height: '38px', border: '1px solid #CED4DA' }}><input type="password" className="border-none flex-1" placeholder="密码" ref={userPwd} onKeyUp={(e) => e.keyCode == 13 && login()} /><i className="icon-lock font-16"></i></label>
                {isVerifyCode && <div className="br-4 plr-10 mt-20 flex-center" style={{ height: '38px', border: '1px solid #CED4DA' }}><input type="text" className="border-none flex-1 h-100" placeholder="验证码" ref={verifyCode} onKeyUp={(e) => e.keyCode == 13 && login()} /><img src={common.apiUrl + '/api/common/verifyCode?id=' + Math.random()} ref={verifyImg} onClick={(e) => e.target.src = common.apiUrl + '/api/common/verifyCode?id=' + Math.random()} title="看不清？换一张" /></div>}
                <label className="mt-20 inline-block"><input type="checkbox" className="vertical-middle" ref={remember} /><span className="vertical-middle">记住密码</span></label>
                <button className="w-100 mt-20 ptb-10 border-none br-4 color-white" style={{ backgroundColor: '#007BFF' }} onClick={() => login()}>登录</button>
            </div>
        </div>
    );
}
