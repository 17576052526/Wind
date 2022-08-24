import React, { useEffect, useRef } from 'react';
import axios from 'axios'
import $ from 'jquery'
import { useStates } from '../../common'
import '../importShare'
import usePager from '../../_hooks/pager/usePager'
import useCheck from '../../_hooks/check/useCheck'
import Test_Main_insert from './Test_Main_insert'
import Test_Main_update from './Test_Main_update'


export default function () {
    let [state, setState] = useStates({

    })

    let pageSize = 10;
    let { Pager, pageIndex, setPageIndex, setDataCount, setPageSize, setPageBtnNum, pageCount } = usePager(pageSize);

    let [checks, setChecks, sync] = useCheck();

    let MainName = useRef();

    async function load(isMatchDel) {
        let param = {
            pageIndex: pageIndex,
            pageSize: pageSize,
            like: {
                MainName: MainName.current.value,
            }
        }
        let msg = await axios.post("/api/test_main/select", param);
        if (msg.code == 1) {
            sync(msg.data.list, isMatchDel);
            setDataCount(msg.data.total);
            setState({ data: msg.data.list });
        }
    }

    useEffect(() => load(true), [pageIndex]);

    //删除
    function remove() {
        if (checks.length == 0) { $.alert('请先勾选'); return; }
        $.confirm('确定删除？', async () => {
            let param = checks.map(s => s.ID);
            let msg = await axios.post("/api/test_main/delete", param);
            if (msg.code == 1) {
                load();
            }
        })
    }

    return (
        <>
            <div className="box h-100">
                <div className="box-head flex">
                    <div className="flex-1">流程列表（测试）</div>
                    <div className="-mlr-5">
                        <input type="button" className="btn-primary h-pre mlr-5" value="新建" onClick={() => setState({ isTest_Main_insert: true })} />
                        <input type="button" className="btn-danger h-pre mlr-5" value="删除" onClick={() => remove()} />
                        <input type="button" className="btn-primary h-pre mlr-5" value="修改" onClick={() => setState({ isTest_Main_update: true })} />
                    </div>
                </div>
                <div className="box-body flex-column">
                    <div className="mb-10">
                        <label className="mlr-5">名称：<input type="text" className="input-text" ref={MainName} /></label>
                        <button className="btn-primary mlr-5" onClick={() => load()}><i className="icon-search"></i>搜索</button>
                    </div>
                    <div className="table-fixed flex-1">
                        <table className="table">
                            <thead>
                                <tr>
                                    <th className="table-resize"><div className="table-resize-item"><input type="checkbox" onChange={(e) => setChecks(state.data, e.target.checked ? '+' : '-')} checked={state.data && state.data.every(s => checks.some(c => s == c))} /></div></th>
                                    <th className="table-resize"><div className="table-resize-item">序号</div></th>
                                    <th className="table-resize"><div className="table-resize-item">编号</div></th>
                                    <th className="table-resize"><div className="table-resize-item">名称</div></th>
                                    <th className="table-resize"><div className="table-resize-item">类型名称</div></th>
                                    <th className="table-resize"><div className="table-resize-item">数量</div></th>
                                    <th className="table-resize"><div className="table-resize-item">价格</div></th>
                                    <th className="table-resize"><div className="table-resize-item">是否</div></th>
                                    <th className="table-resize"><div className="table-resize-item">图片</div></th>
                                    <th className="table-resize"><div className="table-resize-item">文件</div></th>
                                    <th className="table-resize"><div className="table-resize-item">备注</div></th>
                                    <th className="table-resize"><div className="table-resize-item">创建时间</div></th>
                                </tr>
                            </thead>
                            <tbody>
                                {state.data && state.data.map((m, i) =>
                                    <tr>
                                        <td><div className="table-resize-item"><input type="checkbox" checked={checks.some(s => s == m)} onChange={() => setChecks(m)} /></div></td>
                                        <td><div className="table-resize-item">{pageSize * (pageIndex - 1) + i + 1}</div></td>
                                        <td><div className="table-resize-item"><a className="cursor-pointer">{m.MainID}</a></div></td>
                                        <td><div className="table-resize-item">{m.MainName}</div></td>
                                        <td><div className="table-resize-item">{m.Type.TypeName}</div></td>
                                        <td><div className="table-resize-item">{m.Quantity}</div></td>
                                        <td><div className="table-resize-item">{m.Amount}</div></td>
                                        <td><div className="table-resize-item"><input type="checkbox" checked={m.IsShow} /></div></td>
                                        <td><div className="table-resize-item"><a target="_blank" href={m.Img && m.Img.split('|')[0]}><img src={m.Img && m.Img.split('|')[0]} /></a></div></td>
                                        <td><div className="table-resize-item"><a href={m.Files} download>下载文件</a></div></td>
                                        <td><div className="table-resize-item">{m.Remark}</div></td>
                                        <td><div className="table-resize-item">{m.CreateTime && m.CreateTime.replace('T', ' ')}</div></td>
                                    </tr>
                                )}
                            </tbody>
                        </table>
                    </div>
                    <Pager className="pager mt-5" previousPage={<i className="icon-chevron_left"></i>} nextPage={<i className="icon-chevron_right"></i>} noneClass="pager-none" activeClass="pager-active"></Pager>
                </div>
            </div>
            {/*新建*/}
            {state.isTest_Main_insert && <Test_Main_insert close={() => setState({ isTest_Main_insert: null })}></Test_Main_insert>}
            {/*修改*/}
            {state.isTest_Main_update && <Test_Main_update close={() => setState({ isTest_Main_update: null })}></Test_Main_update>}
        </>
    );
}
