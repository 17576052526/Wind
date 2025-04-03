import React from 'react'
import '../_plugins/atomCss/atom.css'   //非必要引用
import '../_plugins/AdminUI2/UI.css'    //非必要引用
import { Box, BoxHead, BoxBody, BoxFoot, Tab, TabItem, FormBox, FormItem } from './design'
import { BtnDefault, BtnPrimary, BtnSuccess, BtnInfo, BtnDanger, InputText, Checkbox, TextArea, Select, Date, DateTime } from './forms'
import { UploadImage, UploadFile } from './upload'
import { Table, Thead, Tbody, Tr, Th, Td } from './table'

/**
JSX 布局组件：
优点：
1.只是定义了一套写法规范，不依赖于布局样式，不依赖于项目，至于内部实现是怎样的，可以根据项目实际需求去写
2.易于维护，例如：换一整套UI，只需要修改组件，而不需要每个页面都去改
3.页面干净整洁
缺点：
1.因为封装了内部实现，所以遇到问题不好排查，以及不好理解内部实现是怎样的
 */

export default function () {
    return (
        <>
            <Box>
                <BoxHead title="用户列表">
                    <BtnPrimary>新增</BtnPrimary>
                </BoxHead>
                <BoxBody>
                    aaaaaaaaaaaaaaaaaa<br />
                    aaaaaaaaaaaaaaaaaa<br />
                    aaaaaaaaaaaaaaaaaa<br />
                </BoxBody>
                <BoxFoot>
                    <BtnPrimary>新增</BtnPrimary>
                </BoxFoot>
            </Box>

            <Box>
                <Tab>
                    <TabItem className="tab-active">类型1</TabItem>
                    <TabItem>类型2</TabItem>
                    <TabItem>类型3</TabItem>
                </Tab>
                <BoxBody>
                    aaaaaaaaaaaaaaaaaa<br />
                    aaaaaaaaaaaaaaaaaa<br />
                    aaaaaaaaaaaaaaaaaa<br />
                </BoxBody>
            </Box>

            <FormBox>
                <FormItem title="姓名"><InputText></InputText></FormItem>
                <FormItem title="姓名"><InputText></InputText></FormItem>
                <FormItem title="姓名"><InputText></InputText></FormItem>
                <FormItem title="姓名"><InputText></InputText></FormItem>
                <FormItem title="姓名"><InputText></InputText></FormItem>
            </FormBox>

            <BtnDefault>新增</BtnDefault>
            <BtnPrimary>新增</BtnPrimary>
            <BtnSuccess>新增</BtnSuccess>
            <BtnInfo>新增</BtnInfo>
            <BtnDanger>新增</BtnDanger>

            <InputText></InputText>

            <Checkbox></Checkbox>

            <TextArea></TextArea>

            <Select data={[{ value: '1', text: '类型一' }, { value: '2', text: '类型二' }, { value: '3', text: '类型三' }]} value="2"></Select>

            <Date></Date>

            <DateTime></DateTime>

            <UploadImage></UploadImage>

            <UploadFile></UploadFile>

            <Table>
                <Thead>
                    <Tr>
                        <Th>111</Th>
                        <Th>222</Th>
                        <Th>333</Th>
                    </Tr>
                </Thead>
                <Tbody>
                    <Tr>
                        <Td>111</Td>
                        <Td>222</Td>
                        <Td>333</Td>
                    </Tr>
                    <Tr>
                        <Td>111</Td>
                        <Td>222</Td>
                        <Td>333</Td>
                    </Tr>
                </Tbody>
            </Table>
        </>
    )
}
