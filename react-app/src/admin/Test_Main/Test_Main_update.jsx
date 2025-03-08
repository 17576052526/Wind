import React, { useEffect, useRef, useLayoutEffect } from 'react'
import axios from 'axios'
import { useStates, formToJSON, alert, confirm, usePager, useChecked } from '../importShare'
import { apiUrl, pageSize } from '../../config'
import {
    Box, BoxHead, BoxBody, BoxFoot, Tab, TabItem, FormBox, FormItem, FixedBox,
    BtnDefault, BtnPrimary, BtnSuccess, BtnInfo, BtnDanger, InputText, Checkbox, TextArea, Select, Date, DateTime, UploadImage, UploadFile,
    Table, Thead, Tbody, Tr, Th, Td
} from '../importJsx'

export default function ({ close, checked }) {
    //状态
    let [state, setState] = useStates({

    })

    let form = useRef();

    //页面加载，useLayoutEffect有防抖动效果，例如页面加载事件中关闭当前组件
    useLayoutEffect(() => {
        if (checked.length == 0) { alert('请先勾选'); close(); return; }
        if (checked.length > 1) { alert('一次只能修改一条'); close(); return; }

        setState({ data: checked[0] });

        //查询类型数据
        axios.post("/api/sys_type/SelectAll").then(msg => {
            setState({ typeData: msg.data });
        });
    }, []);

    //提交
    function submit() {
        axios.post("/api/test_main/update", {
            ...{
                ID: state.data.ID,

            },...formToJSON(form.current)
        }).then(msg => {
            close();
        });
    }


    return (
        <>
            <FixedBox title="修改" close={close} foot={
                <>
                    <BtnPrimary onClick={() => submit()}>提交</BtnPrimary>
                    <BtnDefault onClick={() => close()}>取消</BtnDefault>
                </>
            }>
                <FormBox ref={form}>
                    <FormItem title="编号"><InputText name="MainID" value={state.data && state.data.MainID}></InputText></FormItem>
                    <FormItem title="名称"><InputText name="MainName" value={state.data && state.data.MainName}></InputText></FormItem>
                    <FormItem title="类型"><Select data={state.typeData && state.typeData.map(s => ({ value: s.ID, text: s.Name }))} value={state.data && state.data.TypeID} name="TypeID"></Select></FormItem>
                    <FormItem title="数量"><InputText name="Quantity" value={state.data && state.data.Quantity}></InputText></FormItem>
                    <FormItem title="金额"><InputText name="Amount" value={state.data && state.data.Amount}></InputText></FormItem>
                    <FormItem title="是否离职"><Checkbox name="IsShow" checked={state.data && state.data.IsShow}></Checkbox></FormItem>
                    <FormItem title="图片"><UploadImage name="Img" value={state.data && state.data.Img}></UploadImage></FormItem>
                    <FormItem title="文件"><UploadFile name="Files" value={state.data && state.data.Files}></UploadFile></FormItem>
                    <FormItem title="备注"><TextArea name="Remark" value={state.data && state.data.Remark}></TextArea></FormItem>
                    <FormItem title="日期"><Date name="CreateTime" value={state.data && state.data.CreateTime}></Date></FormItem>
                </FormBox>
            </FixedBox>
        </>
    )
}