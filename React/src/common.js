//import $ from 'jquery';

let common = {
    //获取浏览器缓存
    getCache: function (name) {
        return window.localStorage.getItem(name);
    },
    //写入浏览器缓存
    setCache: function (name, value) {
        return window.localStorage.setItem(name, value);
    }
}
export default common;
