import React, { useState } from 'react';

export default function () {
    let [checks, setChecks] = useState([]);

    function set(param, type) {
        function addOrDel(model) {
            let index = checks.indexOf(model);
            if (type == null) {
                if (index == -1) {
                    checks.push(model);
                } else {
                    checks.splice(index, 1);
                }
            } else {
                if (type == '+') {
                    index == -1 && checks.push(model);
                } else if (type == '-') {
                    index != -1 && checks.splice(index, 1);
                }
            }
        }
        if (Array.isArray(param)) {
            if (type) {
                for (let i = param.length - 1; i >= 0; i--) {
                    addOrDel(param[i]);
                }
            } else {
                checks = param;
            }
        } else {
            addOrDel(param)
        }
        setChecks([...checks]);
    }

    //ͬ����data��ƥ�������Դ,  isDel��δƥ�䵽���Ƿ�ɾ����match��ƥ��ķ�������������JSON�ַ����Ƚ�
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
