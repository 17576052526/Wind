/*
         参数1：pageSize：每页显示多少条
         参数2：dataCount：总共多少条数据
         参数3：fun：点击分页按钮触发的方法
         参数4（非必填）：pageNum：最多显示多少个分页按钮
                类样式：
                    .pager-none 上一页下一页按钮不能点时的类样式
                    .pager-active 被激活的页码按钮
         */
$.fn.pager = function (pageSize, dataCount, fun, pageNum) {
    var box = this;
    var pageCount = dataCount % pageSize == 0 ? dataCount / pageSize : parseInt(dataCount / pageSize) + 1;//总的页码数量
    pageNum = pageNum || 7;
    pageNum = pageCount < pageNum ? pageCount : pageNum;
    var index = 1;//当前的页码
    //构造按钮
    function structBtn(pageIndex, text, className) {
        box.append('<a href="javascript:void(0)" pageindex="' + pageIndex + '" class="' + className + '">' + text + '</a>');
    }
    //构造分页按钮
    function structPage() {
        box.empty();//先清空子元素
        //上一页、第一页
        if (index == 1) {
            structBtn(0, "上一页", "pager-none");
            structBtn(1, "1", "pager-active");
        } else {
            structBtn(index - 1, "上一页", "");
            structBtn(1, "1", "");
        }
        //中间页码
        var minIndex = index > parseInt(pageNum / 2) ? index - parseInt(pageNum / 2) : 1;  //计算最小页码索引
        minIndex = minIndex + pageNum > pageCount ? pageCount - pageNum + 1 : minIndex;  //计算最大的最小页码索引
        for (var i = 1; i < pageNum - 1; i++) {
            if (minIndex + i == index)   //当前选中页
            {
                structBtn(minIndex + i, minIndex + i, 'pager-active');
            }
            else if (i == 1 && minIndex > 1)  //前面的 ...
            {
                structBtn(minIndex + i, '...', '');
            }
            else if (i == pageNum - 2 && minIndex + pageNum - 1 < pageCount)  //后面的 ...
            {
                structBtn(minIndex + i, '...', '');
            }
            else {
                structBtn(minIndex + i, minIndex + i, '');
            }
        }
        //最后一页
        if (pageCount > 1) {
            if (index == pageCount) {
                structBtn(index, index, 'pager-active');
            }
            else {
                structBtn(pageCount, pageCount, '');
            }
        }
        //下一页
        if (index == pageCount || pageCount == 0) {
            structBtn(index + 1, '下一页', 'pager-none');
        }
        else {
            structBtn(index + 1, '下一页', '');
        }
    }
    structPage();//构造分页按钮
    //分页点击事件（绑定在父级容器上）
    this.off('click');//移除事件
    this.on('click', 'a', function () {
        var pageIndex = parseInt($(this).attr('pageindex'));//获取页码
        if (pageIndex != index && pageIndex > 0 && pageIndex <= pageCount) {
            index = pageIndex;
            structPage();//重新构造分页按钮
            fun(index);//执行外部方法
        };
    });
}