/*
 用来控制树结构的显示隐藏
 let [isTreeShow, setShowHide, copyShowHide] = useTree('child');
 isTreeShow(model) 用来判断当前这个对象下的子级是显示还是隐藏
 setShowHide(param, isShow, isChild) param数组或对象，isShow 显示还是隐藏，isChild 子级是否也跟着显示隐藏
 copyShowHide(oldArr, newArr)  旧树结构里面的显示隐藏，复制到新对象里面来
 */
import React, { useState, useEffect, useContext, useRef } from 'react';
import axios from 'axios'
import useTree from './useTree'

export default function () {
    let [isTreeShow, setShowHide, copyShowHide] = useTree('child');

    let [data, setData] = useState();
    useEffect(() => {
        let msg = [
            {
                title: 'a1',
                child: [
                    { title: 'a11' },
                    {
                        title: 'a12',
                        child: [
                            { title: 'a121', },
                            { title: 'a122', },
                            { title: 'a123', }
                        ]
                    },
                    { title: 'a13' },
                ]
            },
            {
                title: 'b1',
                child: [
                    { title: 'b11' },
                    { title: 'b12' },
                ]
            },
            { title: 'c1' },
        ];
        setData(msg)
        //copyShowHide(data, msg)
    }, [])

    function loadTree(data, isShow) {
        return data && data.map((m, i) =>
            <>
                <tr style={{ display: !isShow && 'none' }}>
                    <td><div className="table-resize-item"><input type="button" value="显示隐藏" onClick={() => setShowHide(m)} />XXX</div></td>
                    <td><div className="table-resize-item">{m.title}</div></td>
                </tr>
                {loadTree(m.child, isTreeShow(m))}
            </>
        )
    }

    return (
        <div>
            {data && loadTree(data, true)}
        </div>
    );
}
