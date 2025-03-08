import { useReducer, createContext } from 'react';

/*
 说明：状态钩子，用于代替 useState()
 优势：1.多个状态不用声明多次，2.同时改变多个状态值只渲染一次
 用法：和class的this.setState()一样的：
    let [state, setState] = useStates({
        name: '测试name',//赋初始值
    });
    {state.name} 绑定值
    {state.title} 绑定值
    onClick={()=>setState({name:'XXX'})}

 */
export function useStates(obj) {
    if (obj && obj.constructor != Object) { throw new window.Error('useStates 的初始值只能是对象') }
    return useReducer((oldState, newState) => ({ ...oldState, ...newState }), obj || {});
}

//useContext父子组件传值，可以跨组件传值
export const context = createContext(null);

//form表单转json，checkbox不选就去取data-defaultValue属性值，如果用FormData则连字段都没有
export function formToJSON(form) {
    const obj = {};
    for (let element of form.elements) {
        const name = element.name;
        const value = element.value;

        if (name) { // 确保有name属性
            if (element.type === "checkbox" || element.type === "radio") {
                if (element.checked) {
                    obj[name] = value;
                } else {
                    obj[name] = element.getAttribute('data-defaultValue') || '';;
                }
            } else {
                obj[name] = value;
            }
        }
    }
    return obj;
}

export default {
    //全局变量，name 变量名，value 值，存入SessionStorage能防止刷新页面之后值就不存在的问题
    set: function (name, value) {
        this['__' + name] = value;
        this.setSessionStorage('base.__' + name, value);
    },
    get: function (name) {
        if (this['__' + name] === undefined) {//无论有没有取到值，第二次都不会去取值
            this['__' + name] = this.getSessionStorage('base.__' + name);
        }
        return this['__' + name];
    },

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
