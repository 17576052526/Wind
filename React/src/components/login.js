import React, { useState, useEffect, useContext, useRef } from 'react';
import axios from 'axios'
import $ from 'jquery'
import '../css/global.css'
import '../css/font/css/fontello.css'

export default function () {


    useEffect(() => {

    }, []);



    return (
        <div className="plr-15 flex-center" style={{ height: '100vh', backgroundColor: '#E9ECEF' }}>
            <div className="max-w-100 bg-white plr-30 ptb-30 mlr-auto" style={{ width: '360px', boxShadow: '0 0 1px rgb(0 0 0 / 13%), 0 1px 3px rgb(0 0 0 / 20%)' }}>
                <div className="font-bold font-16 text-center" style={{ color :'#666'}} >后台管理中心-登录</div>
                <label className="br-4 plr-10 mt-20 flex-center" style={{ height: '38px', border: '1px solid #CED4DA' }}><input type="text" className="border-none flex-1" placeholder="登录名" /><i className="icon-user font-16"></i></label>
                <label className="br-4 plr-10 mt-20 flex-center" style={{ height: '38px', border: '1px solid #CED4DA' }}><input type="text" className="border-none flex-1" placeholder="密码" /><i className="icon-lock font-16"></i></label>
                <div className="br-4 plr-10 mt-20 flex-center" style={{ height: '38px', border: '1px solid #CED4DA' }}><input type="text" className="border-none flex-1 h-100" placeholder="验证码" /><img src="" title="看不清，换一张" /></div>
                <label className="mt-20 inline-block"><input type="checkbox" className="vertical-middle" /><span className="vertical-middle">记住密码</span></label>
                <button className="w-100 mt-20 ptb-10 border-none br-4 color-white" style={{ backgroundColor: '#007BFF' }}>登录</button>
            </div>
        </div>
    );
}
