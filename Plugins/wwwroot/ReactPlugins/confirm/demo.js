import confirm from './confirm';


export default function () {
    return (
        <>
            <input type="button" value="确认弹出框" onClick={() => confirm('确认删除', () => { alert('已执行删除代码') })} />
        </>
    );
}
