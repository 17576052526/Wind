import './loading1.css'

/*
加载超过一定时间出现 loading图
    startLoading(dom, interval)：经过 interval（毫秒）时间后，在dom节点内出现loading图
    stopLoading(dom)：停止 dom里面的 loading图显示
*/

var _loading_item = [];
//开始loading
export function startLoading(dom, interval = 1500) {
    var obj = {};
    obj.time = setTimeout(function () {
        dom.insertAdjacentHTML("beforeend", '<div class="loading-box" style="background-color: rgba(255,255,255,.3);position: absolute;left: 0px;top: 0px;z-index: 100;width: 100%;height: 100%;display: flex;justify-content: center;align-items: center;"><div class="loading1"><span></span><span></span><span></span><span></span><span></span></div></div>');
        if (window.getComputedStyle(dom).position == 'static') { dom.style.position = 'relative'; }
        obj.isStart = true;//标识计时器已执行
    }, interval);
    _loading_item.push(obj);
}
//停止loading
export function stopLoading(dom) {
    if (_loading_item.length > 0) {
        clearTimeout(_loading_item[0].time);
        if (_loading_item[0].isStart) { dom.querySelector('.loading-box').remove(); }
        _loading_item.splice(0, 1);
    }
}