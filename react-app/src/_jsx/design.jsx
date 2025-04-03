import React from 'react'
import { apiUrl } from '../config'

export function Box(props) {
    return (<div {...props} className={'box ' + (props.className || '')}></div>)
}

//������title ����   children �ұ���ʾ�Ľڵ�
export function BoxHead(props) {
    return (
        <div {...props} className={"box-head " + (props.className || '')} title="">
            {props.title && <div className="flex-1">{props.title}</div>}
            <div className="-mlr-1 mlr-1-c">
                {props.children}
            </div>
        </div>
    )
}

export function BoxBody(props) {
    return (<div {...props} className={'box-body ' + (props.className || '')}></div>)
}

export function BoxFoot(props) {
    return (<div {...props} className={'box-foot ' + (props.className || '')}></div>)
}

export function Tab(props) {
    return (<div {...props} className={'tab ' + (props.className || '')}></div>)
}

export function TabItem(props) {
    return (<div {...props} className={'tab-item ' + (props.className || '')}></div>)
}

export function FormBox(props) {
    return (<form {...props} className={'form-box ' + (props.className || '')}></form>)
}

export function FormItem(props) {
    return (
        <div {...props} className={'form-item ' + (props.className || '')}>
            <span>{props.title}</span>
            {props.children}
        </div>
    )
}

//������
export function FixedBox({ className, style, title, foot, close, children }) {
    return (
        <Box className={"box-move fixed-center " + (className || '')} style={{ ...{ width: '800px', height: '500px' }, ...style }}>
            <BoxHead className="box-move-switch" title={title}>
                <i className="icon-remove cursor-pointer" onClick={() => close()}></i>
            </BoxHead>
            <BoxBody>
                {children}
            </BoxBody>
            {foot && <BoxFoot className="mlr-3-c">
                {foot}
            </BoxFoot>}
        </Box>
    )
}

//�������������ʾ���ݿ��е����ݶ�Ӧ��ͼƬ�������ƷͼƬ������������ʾҳ�沼��ͼƬ��
export function Image(props) {
    return (
        props.src && <img {...props} src={apiUrl + props.src} className={"img-show " + (props.className || '')} />
    )
}

export function Download(props) {
    return (
        props.href && <a {...props} href={apiUrl + props.href} download><i className="icon-download_alt"></i></a>
    )
}
