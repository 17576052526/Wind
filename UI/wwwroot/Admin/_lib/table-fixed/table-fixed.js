﻿/* eslint-disable *///此处不是注释，react当中不校验代码，不加此句react当中会报错
/*
 表格固定行和固定列
    1.<table>外面加 <div class="table-fixed" style="max-height:300px">
    2.<th>是要被固定的行
    3.th、td 上加.table-fixed-col类样式是要被固定的列
 注意事项：
    1.如果第一行被固定，则不能设置<table>的 border-top，同理，第一列被固定，就不能设置<table>的 border-left，最后一列被固定就不能设置 border-right
    2.th（固定的行）背景色必须设置在th上，不能设置在tr上，同理，固定的列背景色也必须设置在td上
 */
$(document).on('mouseenter', '.table-fixed', function () {//悬浮其上在绑定scroll，react 中table可能后出现
    var scrollTop = 0, scrollLeft = 0;
    if ($(this).is('[tableFixed]')) { return; }//绑定过就不在绑定
    $(this).attr('tableFixed', '');//标记该节点已经绑定过此插件
    $(this).on('scroll', function () {//scroll绑定在document上无效
        var cur = $(this);
        var top = cur.scrollTop();
        if (scrollTop != top) {//滚动的是y轴
            scrollTop = top;
            cur.find('th').css('transform', 'translateY(' + scrollTop + 'px)');//translateY 不要设置在tr或 thead上
        } else {//滚动的是x轴
            scrollLeft = cur.scrollLeft();
            cur.find('.table-fixed-col').css('transform', 'translateX(' + scrollLeft + 'px)');
        }
        cur.find('th.table-fixed-col').css('transform', 'translate(' + scrollLeft + 'px,' + scrollTop + 'px)');
    });
});