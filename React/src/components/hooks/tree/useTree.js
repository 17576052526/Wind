import React, { useState } from 'react';

export default function (childName) {
    if (!childName) { throw new Error('useTree，未初始化'); }

    let [xr, setXr] = useState();//用于触发重新渲染

    //判断子级是否展开
    function isTreeShow(model) { return model.__isChildShow; }

    //子级，子级的子级，显示隐藏
    function showHide(data, isShow) {
        if (!data) { return; }
        for (let m of data) {
            m.__isChildShow = isShow;
            showHide(m[childName])
        }
    }

    //设置当前对象的显示和隐藏
    function setModel(model, isShow, isChild) {
        //设置当前的子级显示或隐藏
        model.__isChildShow = isShow != null ? isShow : !model.__isChildShow;

        //当前子级隐藏，子级的子级也要隐藏
        if (!model.__isChildShow) {
            showHide(model[childName], false);
        } else if (isChild) {
            showHide(model[childName], true);
        }
    }

    //设置展开或收缩，model：当前对象，isShow：true 展开 false 收缩，isChild：子级以及子级的子级是否做同样的操作
    function setShowHide(param, isShow, isChild) {
        if (param instanceof Array) {
            for (let m of param) {
                setModel(m, isShow, isChild)
            }
        } else {
            setModel(param, isShow, isChild)
        }

        //触发重新渲染
        setXr(!xr);
    }

    //__isChildShow复制到新对象当中去
    function copyShowHide(oldArr, newArr) {
        function dg(oldObj, newObj) {
            if ((!oldObj) || (!newObj)) { return; }

            for (let i = 0; i < oldObj.length; i++) {
                if (newObj.length > i) {
                    newObj[i].__isChildShow = oldObj[i].__isChildShow;
                    dg(oldObj[i][childName], newObj[i][childName]);
                }
            }
        }
        dg(oldArr, newArr);
    }

    return [isTreeShow, setShowHide, copyShowHide]
}
