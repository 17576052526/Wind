import './table-resize';

/**
拖拽调整表格列宽
使用说明：
1.<table>上加class="table-resize"，
2.第一行的 th或td 上加 class="table-resize-disable" 标记该列不拖拽
 */
export default function () {
    return (
        <>
            <table class="table-resize">
                <thead>
                    <tr>
                        <th style={{ width: '20px' }}>111</th>
                        <th class="table-resize-disable">111</th>
                        <th>111</th>
                    </tr>
                </thead>
                <tbody>
                    <tr>
                        <td>你说什么你说什么</td>
                        <td>222</td>
                        <td>333</td>
                    </tr>
                </tbody>
            </table>
        </>
    );
}
