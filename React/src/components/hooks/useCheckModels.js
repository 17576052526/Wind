/*
 数据列表的选中组件
 用法：
 1.let [checks, setChecks] = useCheckModels();
 2. onChange={(e) => e.target.checked ? setChecks(data) : setChecks([])}
 3. checked={checks.find(s => s == m)} onChange={() => setChecks(m)}

 说明：
 checks 是个数组里面包含当前已经选中的对象
 set(数组) 设置选中的有哪些，如果是 set([]) 则是全不选
 set(当前对象) 当前没有选中，则让其选中，已经选中，则取消选中
 set(当前对象, '+') 显示的设置当前的选中，如果已经选择了不会重复添加
 set(当前对象, '-') 显示的设置当前的取消选中
 */
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

    return [checks, set];
}
