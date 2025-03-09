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

export function Th(props) {
    return (<th {...props}><div className="table-resize-item" style={props.style && props.style.width ? { width: props.style.width } : {}}>{props.children}</div></th>)
}

export function Td(props) {
    return (<td {...props}><div className="table-resize-item" style={props.style && props.style.width ? { width: props.style.width } : {}}>{props.children}</div></td>)
}
