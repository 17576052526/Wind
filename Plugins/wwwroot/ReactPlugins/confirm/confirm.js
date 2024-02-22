/*
确认弹出框
参数一：提示内容
参数二：确定调用的方法
参数三：取消调用的方法
参数四：提示标题
*/
export default function (content, yesFun, noFun, title = '确认框') {
    var box = document.createElement('div');
    box.setAttribute('style', 'position:fixed;left:0px;top:0px;z-index:10000;width:100%;height:100%;')
    document.body.appendChild(box)
    box.innerHTML = `
    <div class="box box-move" style="width:350px;min-height:150px;position:fixed;top:10%;left:50%;z-index:10000;transform:translateX(-50%);box-shadow:0px 0px 10px #aaa;">
        <div class="box-head box-move-switch" style="display:flex;justify-content:space-between;"><div>${title}</div><span style="transform:rotate(45deg);height:18px;font-size:18px;font-weight:bold;color:#aaa;cursor:pointer;">+</span></div>
        <div class="box-body">${content}</div>
        <div class="box-foot">
            <input type="button" value="确定" class="btn-primary" />&nbsp;&nbsp;&nbsp;&nbsp;
            <input type="button" value="取消" class="btn-default" />
        </div>
    </div>
            `;
    box.querySelector('span').addEventListener('click', () => { noFun && noFun(); box.remove(); });
    box.querySelectorAll('input')[0].addEventListener('click', () => { yesFun && yesFun(); box.remove(); });
    box.querySelectorAll('input')[1].addEventListener('click', () => { noFun && noFun(); box.remove(); });
    box.querySelectorAll('input')[0].focus();//给其焦点，回车触发点击事件
}