//import $ from 'jquery';

export default {
    //服务器的请求地址，因为用了代理请求，所以是 /api
    baseURL: process.env.NODE_ENV == 'development' ? '/api' : '',

    //设置 localStorage
    setLocalStorage: (name, value) => {
        if (value instanceof Object) {
            window.localStorage.setItem(name, JSON.stringify(value));
        } else {
            window.localStorage.setItem(name, value);
        }
    },
    //获取 localStorage
    getLocalStorage: (name) => {
        let val = window.localStorage.getItem(name);
        try {
            return JSON.parse(val);
        }
        catch {
            return val;
        }
    },

    //设置 sessionStorage
    setSessionStorage: (name, value) => {
        if (value instanceof Object) {
            window.sessionStorage.setItem(name, JSON.stringify(value));
        } else {
            window.sessionStorage.setItem(name, value);
        }
    },
    //获取 sessionStorage
    getSessionStorage: (name) => {
        let val = window.sessionStorage.getItem(name);
        try {
            return JSON.parse(val);
        }
        catch {
            return val;
        }
    },
}
