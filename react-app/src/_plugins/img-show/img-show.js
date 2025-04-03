/*
注意：react中插入节点时一次只允许一个顶级节点（空格和换行也会被当成顶级节点），``里面的第一个字符串和最后一个字符串不能是空格和换行
*/
document.addEventListener('click', function (e) {
    if (!e.target.classList.contains('img-show')) { return }
    document.body.insertAdjacentHTML("beforeend", 
    `<div style="position:fixed;left:0;top:0;z-index:99999;height:100%;width:100%;background-color:rgba(0,0,0,0.5);display:flex;justify-content:center;align-items:center;overflow:auto;">
        <div style="cursor:pointer;position:absolute;right:10px;top:10px;width:20px;height:20px;" onclick="this.parentNode.remove()">
            <div style="height:4px;width:28px;transform-origin: top left;transform: rotate(45deg);background-color:#f5f5f5;border-radius:999px;"></div>
            <div style="height:4px;width:28px;transform-origin: bottom left;transform: translateY(14px) rotate(-45deg);background-color:#f5f5f5;border-radius:999px;"></div>
        </div>
        <img src="${e.target.src}" />
    </div>`);
})