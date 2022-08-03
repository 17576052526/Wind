//import $ from 'jquery';

let common = {
    //获取浏览器缓存
    getCache: function (name) {
        return JSON.parse(window.localStorage.getItem(name));
    },
    //写入浏览器缓存
    setCache: function (name, value) {
        if (!(value instanceof Object)) { alert('common.setCache() 只能存入对象或数组，不能存基础数据类型'); }//因为取出的时候，判断不了是基础数据类型还是引用数据类型
        return window.localStorage.setItem(name, JSON.stringify(value));
    }
}
export default common;
