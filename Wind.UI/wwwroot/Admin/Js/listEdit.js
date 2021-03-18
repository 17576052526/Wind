//获取url参数
function queryString(name) {
    var reg = new RegExp('(^|&)' + name + '=([^&]*)(&|$)', 'i');
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}
//loading效果
$(window).on('beforeunload', function () {
    $(document.body).loading();
});
