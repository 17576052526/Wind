import './xy-resize';


export default function () {
    return (
        <>
            <div style={{ display: 'flex' }} >
                <div style={{ flex: '1', backgroundColor: '#000000' }} >111</div>
                <div style={{ width: '8px', cursor: 'col-resize' }} class="x-resize">00</div>
                <div style={{ flex: '1', backgroundColor: '#eee' }} >222</div>
            </div>
            <div style={{ display: 'flex', height: '300px', flexDirection: 'column' }} >
                <div style={{ flex: '1', backgroundColor: 'blue' }}>111</div>
                <div style={{ height: '15px', cursor: 'row-resize' }} class="y-resize">00</div>
                <div style={{ flex: '1', backgroundColor: '#eee' }} >222</div>
            </div>
        </>
    );
}
