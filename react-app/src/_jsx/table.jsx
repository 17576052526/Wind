import React from 'react'

export function Table(props) {
    return (<div {...props} className={'table-fixed ' + (props.className || '')}><table className="table table-resize">{props.children}</table></div>)
}

export function Thead(props) {
    return (<thead {...props}></thead>)
}

export function Tbody(props) {
    return (<tbody {...props}></tbody>)
}

export function Tr(props) {
    return (<tr {...props}></tr>)
}
//div层设 class和style ，其他的设给td th
export function Th(props) {
    return (<th {...props} style={props.style && props.style.width ? { width: props.style.width } : {}}><div className={'table-resize-item ' + (props.childclass || '')} style={props.style}>{props.children}</div></th>)
}

export function Td(props) {
    return (<td {...props} style={props.style && props.style.width ? { width: props.style.width } : {}}><div className={'table-resize-item ' + (props.childclass || '')} style={props.style}>{props.children}</div></td>)
}
