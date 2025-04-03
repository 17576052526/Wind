import React, { useEffect, useRef } from 'react'
import axios from 'axios'
import { useStates, formToJSON, alert, confirm, usePager, useChecked } from '../importShare'
import { apiUrl, pageSize } from '../../config'
import {
    Box, BoxHead, BoxBody, BoxFoot, Tab, TabItem, FormBox, FormItem, FixedBox,
    BtnDefault, BtnPrimary, BtnSuccess, BtnInfo, BtnDanger, InputText, Checkbox, TextArea, Select, Date, DateTime, UploadImage, UploadFile,
    Table, Thead, Tbody, Tr, Th, Td
} from '../importJsx'

export default function ({ close }) {
    //状态
    let [state, setState] = useStates({

    })

    let form = useRef();

    useEffect(() => {
        //查询类型数据
        axios.post("/api/sys_type/SelectAll").then(msg => {
            setState({ typeData: msg.data });
        });

    }, [])

    //提交
    function submit() {
        axios.post("/api/test_main/insert", {
            ...{

            }, ...formToJSON(form.current)
        }).then(msg => {
            close();
        });
    }


    return (
        <>
            <FixedBox title="新建" close={close} foot={
                <>
                    <BtnPrimary onClick={() => submit()}>提交</BtnPrimary>
                    <BtnDefault onClick={() => close()}>取消</BtnDefault>
                </>
            }>
                <FormBox ref={form}>
                    <FormItem title="编号"><InputText name="MainID"></InputText></FormItem>
                    <FormItem title="名称"><InputText name="MainName"></InputText></FormItem>
                    <FormItem title="类型"><Select data={state.typeData && state.typeData.map(s => ({ value: s.ID, text: s.Name }))} name="TypeID"></Select></FormItem>
                    <FormItem title="数量"><InputText name="Quantity"></InputText></FormItem>
                    <FormItem title="金额"><InputText name="Amount"></InputText></FormItem>
                    <FormItem title="是否离职"><Checkbox name="IsShow"></Checkbox></FormItem>
                    <FormItem title="图片"><UploadImage name="Img"></UploadImage></FormItem>
                    <FormItem title="文件"><UploadFile name="Files"></UploadFile></FormItem>
                    <FormItem title="备注"><TextArea name="Remark"></TextArea></FormItem>
                    <FormItem title="日期"><Date name="CreateTime"></Date></FormItem>
                </FormBox>
            </FixedBox>
        </>
    )
}