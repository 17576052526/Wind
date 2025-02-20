(function () {
    var scrollY = document.documentElement.scrollTop || document.body.scrollTop;
    function scroll() {
        var scrollTop = document.documentElement.scrollTop || document.body.scrollTop;
        var height = window.innerHeight;
        for (let th of document.querySelectorAll('[scroll-in],[scroll-out]')) {
            let inClass = th.getAttribute('scroll-in');
            let outClass = th.getAttribute('scroll-out');
            if (scrollY > scrollTop && th.getBoundingClientRect().top + th.offsetHeight > height) {
                outClass && outClass.trim().split(/\s+/).forEach(m => th.classList.add(m));
                inClass && inClass.trim().split(/\s+/).forEach(m => th.classList.remove(m));
            } else if (th.getBoundingClientRect().top < height) {
                inClass && inClass.trim().split(/\s+/).forEach(m => th.classList.add(m));
                outClass && outClass.trim().split(/\s+/).forEach(m => th.classList.remove(m));
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