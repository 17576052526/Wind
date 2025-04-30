import React, { useEffect, useRef } from 'react'
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

    useEffect(() => {
        setState({ data: checked[0] });

        //查询类型数据
        axios.post("/api/TestType/select").then(msg => {
            setState({ typeData: msg.data });
        });
    }, []);

    //提交
    function submit() {
        axios.post("/api/TestMain/update", {
            ...{
                id: state.data.id,

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
                    <FormItem title="编号"><InputText name="mainId" value={state.data && state.data.mainId}></InputText></FormItem>
                    <FormItem title="名称"><InputText name="mainName" value={state.data && state.data.mainName}></InputText></FormItem>
                    <FormItem title="类型"><Select data={state.typeData && state.typeData.map(s => ({ value: s.id, text: s.name }))} value={state.data && state.data.testTypeId} name="testTypeId"></Select></FormItem>
                    <FormItem title="数量"><InputText name="quantity" value={state.data && state.data.quantity}></InputText></FormItem>
                    <FormItem title="金额"><InputText name="amount" value={state.data && state.data.amount}></InputText></FormItem>
                    <FormItem title="是否离职"><Checkbox name="isShow" checked={state.data && state.data.isShow}></Checkbox></FormItem>
                    <FormItem title="图片"><UploadImage name="img" value={state.data && state.data.img}></UploadImage></FormItem>
                    <FormItem title="文件"><UploadFile name="files" value={state.data && state.data.files}></UploadFile></FormItem>
                    <FormItem title="备注"><TextArea name="remark" value={state.data && state.data.remark}></TextArea></FormItem>
                    <FormItem title="日期"><Date name="createTime" value={state.data && state.data.createTime}></Date></FormItem>
                </FormBox>
            </FixedBox>
        </>
    )
}