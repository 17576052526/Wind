/*
 说明：选中项组件
 checked 选中的项
 setChecked(data,isAdd) 设置选中的项
    data：选中或取消选中的项，可以是单个对象，也可以是数组
    isAdd：true选中，false取消选中
 注意：jsx列表修改或删除数据后应清空选中项 setChecked(checked, false);
 */
import React, { useState, useEffect } from 'react';
import useChecked from './useChecked'

export default function () {
    let [checked, setChecked] = useChecked();

    let [data, setData] = useState();
    useEffect(() => {
        //ajax获取数据
        setData([
            { ID: 1, name: 't1' },
            { ID: 2, name: 't2' },
            { ID: 3, name: 't3' },
        ]);
    }, [])

    return (
        <div>
            <input type="checkbox" onChange={(e) => setChecked(data, e.target.checked)} checked={data && data.every(m => checked.some(s => JSON.stringify(s) == JSON.stringify(m)))} />全选，当前选中了 {checked.length}个
            {data && data.map((m, i) =>
                <div><input type="checkbox" checked={checked.some(s => JSON.stringify(s) == JSON.stringify(m))} onChange={(e) => setChecked(m, e.target.checked)} /> {m.name}</div>
            )}
        </div>
    )
}
