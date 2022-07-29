/*
 说明： 选中项组件
 checks 选中项列表（数组）
 setChecks(Array)设置选中的项有哪些，setChecks([])清空选中项
 setChecks(object)选中项中存在则删除，不存在则添加
 setChecks(object,'+')添加选中项
 setChecks(object,'-')删除选中项
 sync(data,(s,m)=>s.ID==m.ID)同步选中项，在每次重新加载数据时调用
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
        sync(msg, (s, m) => s.ID == m.ID);
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
