import './loading1.css'

/*
���س���һ��ʱ����� loadingͼ
    startLoading(dom, interval)������ interval�����룩ʱ�����dom�ڵ��ڳ���loadingͼ
    stopLoading(dom)��ֹͣ dom����� loadingͼ��ʾ
*/

var _loading_item = [];
//��ʼloading
export function startLoading(dom, interval = 1500) {
    var obj = {};
    obj.time = setTimeout(function () {
        dom.insertAdjacentHTML("beforeend", '<div class="loading-box" style="background-color: rgba(255,255,255,.3);position: absolute;left: 0px;top: 0px;z-index: 100;width: 100%;height: 100%;display: flex;justify-content: center;align-items: center;"><div class="loading1"><span></span><span></span><span></span><span></span><span></span></div></div>');
        if (window.getComputedStyle(dom).position == 'static') { dom.style.position = 'relative'; }
        obj.isStart = true;//��ʶ��ʱ����ִ��
    }, interval);
    _loading_item.push(obj);
}
//ֹͣloading
export function stopLoading(dom) {
    if (_loading_item.length > 0) {
        clearTimeout(_loading_item[0].time);
        if (_loading_item[0].isStart) { dom.querySelector('.loading-box').remove(); }
        _loading_item.splice(0, 1);
    }
}