import React, { useEffect, useRef } from 'react';
import axios from 'axios'


export default function () {
    


    return (
        <>
            <div className="flex-wrap">
                <div className="box-info flex-1" style={{ minWidth: '300px' }} >
                    <div className="box-head">登录信息</div>
                    <div className="box-body">
                        <table className="table">
                            <tbody>
                                <tr>
                                    <td>上一次登录名</td>
                                    <td>7654561231.Q</td>
                                </tr>
                                <tr>
                                    <td>上一次登陆的时间</td>
                                    <td>2016-06-08</td>
                                </tr>
                                <tr>
                                    <td>上一次登陆的IP</td>
                                    <td>192.168.0.132</td>
                                </tr>
                                <tr>
                                    <td>上一次登陆的地区</td>
                                    <td>192.168.0.132</td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
                <div className="box-success flex-1 ml-2.5" style={{ minWidth: '300px' }} >
                    <div className="box-head">技术支持</div>
                    <div className="box-body">
                        <table className="table">
                            <tbody>
                                <tr>
                                    <td>QQ</td>
                                    <td>123456</td>
                                </tr>
                                <tr>
                                    <td>联系人</td>
                                    <td>王工</td>
                                </tr>
                                <tr>
                                    <td>手机</td>
                                    <td>1341111111</td>
                                </tr>
                                <tr>
                                    <td>微信</td>
                                    <td>2312321443</td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
            <div className="box-danger mt-2.5">
                <div className="box-head">系统信息</div>
                <div className="box-body">
                    <table className="table">
                        <tbody>
                            <tr>
                                <td>数据库</td>
                                <td>Sql Server 2008 R2</td>
                            </tr>
                            <tr>
                                <td>编程语言</td>
                                <td>.net core 5.0</td>
                            </tr>
                            <tr>
                                <td>服务器地址</td>
                                <td>192.168.0.132</td>
                            </tr>
                            <tr>
                                <td>服务器操作系统</td>
                                <td>Windows Server 2008企业版</td>
                            </tr>
                        </tbody>
                    </table>
                </div>
            </div>
        </>
    )
}
