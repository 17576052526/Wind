import React, { useState } from 'react';

export default function () {
    let [checks, setChecks] = useState([]);

    function set(param, type) {
        if (Array.isArray(param)) {
            checks = param;
        } else {
            let index = checks.indexOf(param);
            if (type == null) {
                if (index == -1) {
                    checks.push(param);
                } else {
                    checks.splice(index, 1);
                }
            } else {
                if (type == '+') {
                    index == -1 && checks.push(param);
                } else if (type == '-') {
                    index != -1 && checks.splice(index, 1);
                }
            }
        }
        setChecks([...checks]);
    }

    //同步，data：匹配的数据源,  isDel：未匹配到的是否删除，match：匹配的方法，不传就是JSON字符串比较
    function sync(data, isDel, match) {
        for (let i = checks.length - 1; i >= 0; i--) {
            let model = data.find(s => match ? match(checks[i], s) : JSON.stringify(checks[i]) == JSON.stringify(s));
            if (model) {
                checks[i] = model;
            } else if (isDel) {
                checks.splice(i, 1);
            }
        }
        setChecks([...checks]);
    }

    return [checks, set, sync];
}
