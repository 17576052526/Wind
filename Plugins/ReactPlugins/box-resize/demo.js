import './box-resize';


export default function () {
    return (
        <>
            <div class="box-resize" style={{ position: 'fixed', left: '50%', top: '50%', transform: 'translate(-50%, -50%)', border: '1px solid #ccc', width: '300px', height: '200px' }} >
                调整浮动框大小
            </div>
        </>
    );
}
