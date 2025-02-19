(function () {
    var scrollY = document.documentElement.scrollTop || document.body.scrollTop;
    function scroll() {
        var scrollTop = document.documentElement.scrollTop || document.body.scrollTop;
        var height = window.innerHeight;
        for (let th of document.querySelectorAll('[scroll-in],[scroll-out]')) {
            let inClass = th.getAttribute('scroll-in');
            let outClass = th.getAttribute('scroll-out');
            if (scrollY > scrollTop && th.getBoundingClientRect().top + scrollTop + th.offsetHeight > scrollTop + height) {
                outClass && outClass.split(' ').forEach(m => th.classList.add(m));
                inClass && inClass.split(' ').forEach(m => th.classList.remove(m));
            } else if (th.getBoundingClientRect().top + scrollTop < scrollTop + height) {
                inClass && inClass.split(' ').forEach(m => th.classList.add(m));
                outClass && outClass.split(' ').forEach(m => th.classList.remove(m));
            }
        }
        scrollY = scrollTop;
    }
    document.addEventListener('load', scroll);
    //���ٴ���ִ��Ƶ�ʣ�����������Ҳ��ִ��
    var __scroll_is;
    document.addEventListener('scroll', function () {
        if (!__scroll_is) {
            __scroll_is = true;
            setTimeout(() => { __scroll_is = false; scroll(); }, 50);
        }
    });
})();