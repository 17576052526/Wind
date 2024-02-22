import { startLoading, stopLoading } from './loading';


export default function () {
    return (
        <>
            <input type="button" value="开始loading" onClick={() => startLoading(document.body)} />
            <input type="button" value="停止loading" style={{ position:'relative',zIndex:'1000000'}} onClick={() => stopLoading(document.body)} />
        </>
    );
}
