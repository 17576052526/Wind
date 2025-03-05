import React, { useState, useRef } from 'react';

export default function (_size) {
    let [pageIndex, setPageIndex] = useState(1);//当前页码索引
    let [pageCount, setPageCount] = useState(0);//总页数
    let pageSize = useRef(_size);//每页显示多少条
    let pageBtnNum = useRef(7);//设置最多显示多少个分页按钮

    //设置页码
    function setIndex(index) {
        index = parseInt(index) || 1;
        setPageIndex((index < 1 || pageCount == 0) ? 1 : index > pageCount ? pageCount : index)
    }

    //设置总数据量
    function setDataCount(dataCount) {
        if (pageSize.current == null) { alert('分页控件 setDataCount调用前，请先设置 pageSize'); return; }
        let count = dataCount % pageSize.current == 0 ? dataCount / pageSize.current : parseInt(dataCount / pageSize.current) + 1;//总的页码数量
        setPageCount(count);//重新渲染
        //当前页码重新计算，不要这样写setPageIndex(count == 0 ? 1 : pageIndex > count ? count : pageIndex);
        if (count == 0) { setPageIndex(1) }
        else if (pageIndex > count) { setPageIndex(count) }
    }


    function Pager({ style, className, noneClass, activeClass, lastPage, nextPage }) {
        let btnNum = pageCount < pageBtnNum.current ? pageCount : pageBtnNum.current;

        return (
            <div style={style} className={className}>
                <a className={pageIndex == 1 ? noneClass : undefined} onClick={() => setIndex(pageIndex - 1)}>{lastPage || '上一页'}</a>
                <a className={pageIndex == 1 ? activeClass : undefined} onClick={() => setPageIndex(1)}>1</a>

                {(() => {
                    //构建中间页码
                    let start = pageIndex - parseInt(btnNum / 2) + 1 < 2 ? 2 : pageIndex - parseInt(btnNum / 2) + 1;//根据当前页码构建起始索引，如果起始索引小于2，起始索引就是2
                    start = start + btnNum - 3 >= pageCount ? pageCount - btnNum + 2 : start;//结束索引超过了总页数，起始索引往前推
                    let arr = [];
                    for (let i = start; i <= start + btnNum - 3; i++) {
                        arr.push(
                            <a className={pageIndex == i ? activeClass : undefined} onClick={() => setPageIndex(i)}>{(i == start && i > 2) || (i == start + btnNum - 3 && i < pageCount - 1) ? '...' : i}</a>
                        );
                    }
                    return arr;
                })()}

                {pageCount > 1 && <a className={pageIndex == pageCount ? activeClass : undefined} onClick={() => setPageIndex(pageCount)}>{pageCount}</a>}
                <a className={(pageIndex == pageCount || pageCount == 0) ? noneClass : undefined} onClick={() => setIndex(pageIndex + 1)}>{nextPage || '下一页'}</a>
            </div>
        );
    }

    return {
        Pager,  //分页控件
        pageIndex,  //页码
        setPageIndex: setIndex, //设置页码
        setDataCount, //设置总数据量
        setPageSize: (s) => pageSize.current = parseInt(s),   //设置每页显示多少条
        setPageBtnNum: (s) => pageBtnNum.current = s,//设置最多显示多少个分页按钮
        pageCount: pageCount,   //总页数
    };
}