import React from 'react'

export function Table(props) {
    return (<div {...props} className={'table-fixed ' + (props.className || '')}><table className="table">{props.children}</table></div>)
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
    return (<th {...props}></th>)
}

export function Td(props) {
    return (<td {...props}></td>)
}
