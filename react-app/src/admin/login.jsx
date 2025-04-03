import React, { useState, useEffect, useRef } from 'react';
import { useNavigate } from "react-router-dom";
import axios from 'axios'
import base from '../_utils/base'
import { apiUrl } from '../config'
import styles from './assets/login.module.css'

export default function () {
    let navigate = useNavigate();
    let userName = useRef();
    let userPwd = useRef();
    let verifyCode = useRef();
    let verifyImg = useRef();
    let remember = useRef();
    let loading = useRef();
    let [isVerifyCode, setIsVerifyCode] = useState();

    useEffect(() => {
        //记住密码，读取信息
        if (base.getLocalStorage("rememberUserName")) {
            userName.current.value = base.getLocalStorage("rememberUserName");
            userPwd.current.value = base.getLocalStorage("rememberUserPwd");
            remember.current.checked = true;
            userName.current.focus();
        }
    }, [])

    //登录
    function login() {
        if (userName.current.value.length == 0) { alert('请输入登录名'); return; }
        else if (verifyCode.current && verifyCode.current.value.length == 0) { alert('请输入验证码'); return; }
        loading.current.style.display = 'inline-block';//此处不能使用 useState 来让loading图片显示，因为会触发重新渲染，从而导致验证码图片刷新，就会导致刚刚输入的验证码和现在的验证码对不上
        let obj = {
            userName: userName.current.value,
            userPwd: userPwd.current.value,
            verifyCode: verifyCode.current ? verifyCode.current.value : '',
        }
        axios.post("/api/common/login", obj).then(msg => {
            loading.current.style.display = 'none';
            if (msg.code == 200) {
                //记住密码
                if (remember.current && remember.current.checked) {
                    base.setLocalStorage("rememberUserName", userName.current.value);
                    base.setLocalStorage("rememberUserPwd", userPwd.current.value);
                } else {
                    base.setLocalStorage("rememberUserName", '');
                    base.setLocalStorage("rememberUserPwd", '');
                }
                //登录成功之后，解析 返回的 token成json，保存在本地
                let tokenArr = msg.data.split('.');
                let user = JSON.parse(window.atob(tokenArr[1]));
                user.token = msg.data;
                base.set('user', user);
                navigate('/admin');
            } else {
                alert(msg.msg);
                userPwd.current.value = '';
                if (msg.code == -2) {
                    if (isVerifyCode) {
                        verifyImg.current.click();
                        verifyCode.current.value = '';
                    }
                    setIsVerifyCode(true);
                }
            }
        });
    }

    return (
        <>
            <div className={styles.login}>
                <div className={styles['login-box']}>
                    <div className={styles['login-title']}>数据管理中心-登录</div>
                    <label className={styles['login-input-box']}><input type="text" className={styles['login-input']} placeholder="登录名" ref={userName} onKeyUp={(e) => e.key == 'Enter' && login()} /><i className={styles['icon-user']}></i></label>
                    <label className={styles['login-input-box']}><input type="password" className={styles['login-input']} placeholder="密码" ref={userPwd} onKeyUp={(e) => e.key == 'Enter' && login()} /><i className={styles['icon-lock']}></i></label>
                    {isVerifyCode && <div className={styles['login-input-box']}><input type="text" className={styles['login-input']} placeholder="验证码" ref={verifyCode} onKeyUp={(e) => e.key == 'Enter' && login()} /><img src={apiUrl + '/api/common/verifyCode?id=' + Math.random()} ref={verifyImg} onClick={(e) => e.target.src = apiUrl + '/api/common/verifyCode?id=' + Math.random()} title="看不清，换一张" /></div>}
                    <label className={styles['login-remember-box']}><input type="checkbox" ref={remember} />记住密码</label>
                    <button className={styles['login-button']} onClick={() => login()}><i className={styles['icon-spin4'] + ' ' + styles['animate-spin']} ref={loading} style={{ display: 'none' }}></i>登录</button>
                </div>
            </div>
        </>
    )
}
