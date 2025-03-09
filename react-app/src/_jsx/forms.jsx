import React, { useState, useEffect } from 'react'

export function BtnDefault(props) {
    return (<button type="button" {...props} className={'btn-default ' + (props.className || '')}></button>)
}

export function BtnPrimary(props) {
    return (<button type="button" {...props} className={'btn-primary ' + (props.className || '')}></button>)
}

export function BtnSuccess(props) {
    return (<button type="button" {...props} className={'btn-success ' + (props.className || '')}></button>)
}

export function BtnInfo(props) {
    return (<button type="button" {...props} className={'btn-info ' + (props.className || '')}></button>)
}

export function BtnDanger(props) {
    return (<button type="button" {...props} className={'btn-danger ' + (props.className || '')}></button>)
}

export function InputText(props) {
    let [value, setValue] = useState(props.value)
    useEffect(() => setValue(props.value), [props.value])
    return (<input onChange={(e) => setValue(e.target.value)} type="text" {...props} value={value || ''} className={'input-text ' + (props.className || '')} />)
}

export function Checkbox(props) {
    let [checked, setChecked] = useState(props.checked)
    useEffect(() => setChecked(props.checked), [props.checked])
    return (<input onChange={(e) => !props.readOnly && setChecked(e.target.checked)} {...props} type="checkbox" checked={checked || false} value={props.value || true} />)
}

export function TextArea(props) {
    let [value, setValue] = useState(props.value)
    useEffect(() => setValue(props.value), [props.value])
    return (<textarea onChange={(e) => setValue(e.target.value)} {...props} value={value || ''} className={'input-text ' + (props.className || '')}></textarea>)
}

//参数：data 下拉的数据
export function Select(props) {
    let [value, setValue] = useState(props.value)
    useEffect(() => setValue(props.value), [props.value])
    return (
        <select onChange={(e) => !props.readOnly && setValue(e.target.value)} {...props} value={value || ''} className={'input-text ' + (props.className || '')} data="">
            <option value="">请选择</option>
            {props.data && props.data.map((s, i) => <option key={i} value={s.value}>{s.text}</option>)}
        </select>
    )
}

export function Date(props) {
    let [value, setValue] = useState(props.value)
    useEffect(() => setValue(props.value && props.value.substring(0, 10)), [props.value])
    return (<input onChange={(e) => setValue(e.target.value)} {...props} type="date" value={value || ''} className={'input-text ' + (props.className || '')} />)
}

export function DateTime(props) {
    let [value, setValue] = useState(props.value)
    useEffect(() => setValue(props.value), [props.value])
    return (<input onChange={(e) => setValue(e.target.value)} {...props} type="datetime-local" value={value || ''} className={'input-text ' + (props.className || '')} />)
}
