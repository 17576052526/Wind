import './table-fixed.css';

/**
 固定表格用法：
 <table> 外加一层 <div class="table-fixed"> ，且设置此div的宽高，<thead>、<tfoot> 会自动固定
 .table-fixed-left 设置单元格左固定
 .table-fixed-right 设置单元格右固定
 */
export default function () {
    return (
        <>
            <div class="table-fixed" style={{ width: '300px', height: '150px' }} >
                <table class="table" style={{ width:'400px'}} >
                    <thead>
                        <tr>
                            <th class="table-fixed-left">111</th>
                            <th>222</th>
                            <th class="table-fixed-right">333</th>
                        </tr>
                    </thead>
                    <tbody>
                        <tr>
                            <td>111</td>
                            <td>222</td>
                            <td>333</td>
                        </tr>
                        <tr>
                            <td>111</td>
                            <td>222</td>
                            <td>333</td>
                        </tr>
                        <tr>
                            <td>111</td>
                            <td>222</td>
                            <td>333</td>
                        </tr>
                        <tr>
                            <td>111</td>
                            <td>222</td>
                            <td>333</td>
                        </tr>
                        <tr>
                            <td>111</td>
                            <td>222</td>
                            <td>333</td>
                        </tr>
                    </tbody>
                    <tfoot>
                        <tr>
                            <td>底部111</td>
                            <td>底部222</td>
                            <td>底部333</td>
                        </tr>
                    </tfoot>
                </table>
            </div>
        </>
    );
}
