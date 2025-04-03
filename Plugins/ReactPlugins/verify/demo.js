import verify from './verify';

/*
验证插件：
verify(box)   验证方法，box不一定要是form，也可以是div，只要是父级就行
verify="notnull"   非空验证
verify="^\d+$"   正则表达式验证
msg="请输入名称"   验证不通过时的提示信息
 */
export default function () {
    return (
        <>
            <div id="box">
                <input type="text" verify="notnull" msg="请输入名称" />
                <input type="text" verify="^\d+$" msg="数量输入格式不正确，例如：10" />
                <input type="submit" value="提交" onClick={() => !verify(document.getElementById('box')) || alert('验证通过')} />
            </div>
        </>
    );
}
