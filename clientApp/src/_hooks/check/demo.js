/*
 说明： 选中项组件
 checks 选中项列表（数组）
 setChecks([])清空所有选中项
 setChecks(Array)选中项替换为当前传入的Array
 setChecks(Array,'+')选中项后面追加Array
  setChecks(Array,'-')选中项里面删除Array包含的项
 setChecks(object)选中项中存在则删除，不存在则添加
 setChecks(object,'+')添加选中项
 setChecks(object,'-')删除选中项
 sync(data,(s,m)=>s.ID==m.ID)同步选中项，参数一：匹配的数据源,  参数二：未匹配到的是否删除，参数三：匹配的方法，不传就是JSON字符串比较
 */
import React, { useState, useEffect, useContext, useRef } from 'react';
import axios from 'axios'
import useCheck from './useCheck'

export default function () {
    let [checks, setChecks, sync] = useCheck();

    let [data, setData] = useState();
    useEffect(() => {
        //ajax获取数据
        let msg = [
            { ID: 1, name: 't1' },
            { ID: 2, name: 't2' },
            { ID: 3, name: 't3' },
        ];
        setData(msg);
        //同步 checks数据
        sync(msg, true, (s, m) => s.ID == m.ID);
    }, [])

    return (
        <div>
            <input type="checkbox" onChange={(e) => e.target.checked ? setChecks(data) : setChecks([])} />全选，当前选中了 {checks.length}个
            {data && data.map((m, i) =>
                <div><input type="checkbox" checked={checks.some(s => s == m)} onChange={() => setChecks(m)} /> {m.name}</div>
            )}
        </div>
    );
}
