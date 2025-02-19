(function () {
    var scrollY = document.documentElement.scrollTop || document.body.scrollTop;
    function scroll() {
        var scrollTop = document.documentElement.scrollTop || document.body.scrollTop;
        var height = window.innerHeight;
        for (let th of document.querySelectorAll('[scroll-in],[scroll-out]')) {
            let inClass = th.getAttribute('scroll-in');
            let outClass = th.getAttribute('scroll-out');
            if (scrollY > scrollTop && th.getBoundingClientRect().top + scrollTop + th.offsetHeight > scrollTop + height) {
                if (outClass) { th.className = th.className + ' ' + outClass; }
                if (inClass) { th.className = th.className.replace(' ' + inClass, ''); }
            } else if (th.getBoundingClientRect().top + scrollTop < scrollTop + height) {
                if (inClass) { th.className = th.className + ' ' + inClass; }
                if (outClass) { th.className = th.className.replace(' ' + outClass, ''); }
            }
        }
        scrollY = scrollTop;
    }
    document.addEventListener('load', scroll);
    //减少代码执行频率，滑动过程中也会执行
    var __scroll_is;
    document.addEventListener('scroll', function () {
        if (!__scroll_is) {
            __scroll_is = true;
            setTimeout(() => { __scroll_is = false; scroll(); }, 50);
        }
    });
})();