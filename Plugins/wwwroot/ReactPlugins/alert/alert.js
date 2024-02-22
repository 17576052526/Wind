import './alert.css'

export default function (msg) {
    var box = document.getElementById('__alert435');
    if (box == null) {
        box = document.createElement('div');
        box.className = "alert-box";
        box.id = "__alert435";
        document.body.appendChild(box);
    }
    //创建alert 框
    var al = document.createElement('div');
    al.className = "alert";
    box.appendChild(al);
    //创建内容框
    var content = document.createElement('div');
    content.style.overflow = "auto";
    content.style.height = "100%";
    content.innerText = msg;
    al.appendChild(content);
    //创建删除按钮
    var removeBtn = document.createElement('span');
    removeBtn.className = "alert-remove";
    removeBtn.innerHTML = '+';
    removeBtn.onclick = () => {
        al.classList.add('alert-hide');
        setTimeout(() => al.remove(), 2000);
    }
    al.appendChild(removeBtn);
    //创建计时器
    var time;
    function setTime() {
        time = setTimeout(() => removeBtn.click(), 5000);
    }
    setTime();
    //悬浮其上停止计时器，离开重新启动计时器
    al.addEventListener('mouseenter', () => clearTimeout(time));
    al.addEventListener('mouseleave', () => setTime());
}