import React, { useEffect, useRef } from 'react'
import axios from 'axios'
import { useStates, formToJSON, alert, confirm, usePager, useChecked } from '../importShare'
import { apiUrl, pageSize } from '../../config'
import {
    Box, BoxHead, BoxBody, BoxFoot, Tab, TabItem, FormBox, FormItem, FixedBox, Image, Download,
    BtnDefault, BtnPrimary, BtnSuccess, BtnInfo, BtnDanger, InputText, Checkbox, TextArea, Select, Date, DateTime, UploadImage, UploadFile,
    Table, Thead, Tbody, Tr, Th, Td
} from '../importJsx'
import Test_Main_insert from './Test_Main_insert'
import Test_Main_update from './Test_Main_update'

export default function () {
    //状态
    let [state, setState] = useStates({

    })
    //分页hooks
    let { Pager, pageIndex, setPageIndex, setDataCount, setPageSize, setPageBtnNum, pageCount } = usePager(pageSize);
    //选中，全选
    let [checked, setChecked] = useChecked();

    let form = useRef();

    //加载数据
    function load() {
        axios.post("/api/test_main/select", {
            ...{
                pageIndex,
                pageSize
            }, ...state.searchObj
        }).then(msg => {
            setDataCount(msg.data.total);
            setState({ data: msg.data.list });
        });
    }
    //搜索
    function search() {
        state.searchObj = formToJSON(form.current);
        load();
    }
    //页码改变查询数据
    useEffect(() => load(), [pageIndex]);

    //删除
    function remove(obj) {
        if (obj) { checked = [obj]; setChecked(checked); }
        if (checked.length == 0) { alert('请先勾选'); return; }
        confirm('当前选中' + checked.length + '条，确定删除？', () => {
            axios.post("/api/test_main/delete", checked.map(s => s.ID)).then(msg => {
                load();
            });
            setChecked([]);//清空选中项
        }, () => obj && setChecked([]))
    }

    return (
        <>
            <Box className="h-full">
                <BoxHead title="用户列表">
                    <BtnPrimary onClick={() => setState({ isTest_Main_insert: true })}>新建</BtnPrimary>
                    <BtnDanger onClick={() => remove()}>删除</BtnDanger>
                </BoxHead>
                <BoxBody className="flex-column">
                    <form className="mb-2.5" ref={form}>
                        <InputText placeholder="名称" name="MainName"></InputText>
                        <BtnPrimary onClick={() => search()}>搜索</BtnPrimary>
                    </form>
                    <Table className="flex-1">
                        <Thead>
                            <Tr>
                                <Th><Checkbox onChange={(e) => setChecked(state.data, e.target.checked)} checked={(state.data || []).every(m => checked.some(s => JSON.stringify(s) == JSON.stringify(m)))}></Checkbox></Th>
                                <Th>序号</Th>
                                <Th>编号</Th>
                                <Th>名称</Th>
                                <Th>类型名称</Th>
                                <Th>数量</Th>
                                <Th>价格</Th>
                                <Th>是否</Th>
                                <Th>图片</Th>
                                <Th>文件</Th>
                                <Th>备注</Th>
                                <Th>创建时间</Th>
                                <Th>操作</Th>
                            </Tr>
                        </Thead>
                        <Tbody>
                            {state.data && state.data.map((m, i) =>
                                <Tr key={i}>
                                    <Td><Checkbox checked={checked.some(s => JSON.stringify(s) == JSON.stringify(m))} onChange={(e) => setChecked(m, e.target.checked)}></Checkbox></Td>
                                    <Td>{pageSize * (pageIndex - 1) + i + 1}</Td>
                                    <Td>{m.MainID}</Td>
                                    <Td>{m.MainName}</Td>
                                    <Td>{m.Type.Name}</Td>
                                    <Td>{m.Quantity}</Td>
                                    <Td>{m.Amount}</Td>
                                    <Td><Checkbox checked={m.IsShow} readOnly={true}></Checkbox></Td>
                                    <Td><Image src={m.Img && m.Img.split('|')[0]}></Image></Td>
                                    <Td><Download href={m.Files}></Download></Td>
                                    <Td>{m.Remark}</Td>
                                    <Td>{m.CreateTime}</Td>
                                    <Td className="mlr-1-c h-pre-c" style={{ overflow: 'visible' }}>
                                        <BtnPrimary className="btn-xs" onClick={() => { setChecked(m); setState({ isTest_Main_update: true }) }}>修改</BtnPrimary>
                                        <BtnDanger className="btn-xs" onClick={() => remove(m)}>删除</BtnDanger>
                                    </Td>
                                </Tr>
                            )}
                            {(state.data == null || state.data.length == 0) && <Tr><Td colSpan="100" className="text-center font-bold">未查询到数据</Td></Tr>}
                        </Tbody>
                    </Table>
                    <Pager className="pager mt-1.5" lastPage={<i className="icon-chevron_left"></i>} nextPage={<i className="icon-chevron_right"></i>} noneClass="pager-none" activeClass="pager-active"></Pager>
                </BoxBody>
            </Box>
            {state.isTest_Main_insert && <Test_Main_insert close={() => { setState({ isTest_Main_insert: null }); load() }}></Test_Main_insert>}
            {state.isTest_Main_update && <Test_Main_update close={() => { setState({ isTest_Main_update: null }); load(); setChecked([]) }} checked={checked}></Test_Main_update>}
        </>
    )
}
