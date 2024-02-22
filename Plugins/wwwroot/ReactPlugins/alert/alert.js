import './alert.css'

export default function (msg) {
    var box = document.getElementById('__alert435');
    if (box == null) {
        box = document.createElement('div');
        box.className = "alert-box";
        box.id = "__alert435";
        document.body.appendChild(box);
    }
    //����alert ��
    var al = document.createElement('div');
    al.className = "alert";
    box.appendChild(al);
    //�������ݿ�
    var content = document.createElement('div');
    content.style.overflow = "auto";
    content.style.height = "100%";
    content.innerText = msg;
    al.appendChild(content);
    //����ɾ����ť
    var removeBtn = document.createElement('span');
    removeBtn.className = "alert-remove";
    removeBtn.innerHTML = '+';
    removeBtn.onclick = () => {
        al.classList.add('alert-hide');
        setTimeout(() => al.remove(), 2000);
    }
    al.appendChild(removeBtn);
    //������ʱ��
    var time;
    function setTime() {
        time = setTimeout(() => removeBtn.click(), 5000);
    }
    setTime();
    //��������ֹͣ��ʱ�����뿪����������ʱ��
    al.addEventListener('mouseenter', () => clearTimeout(time));
    al.addEventListener('mouseleave', () => setTime());
}