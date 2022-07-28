import React, { useState, useEffect, useContext, useRef } from 'react';
import axios from 'axios'
import $ from 'jquery'
import usePager from './usePager'
import './pager.css'
//import './../../../css/font/css/fontello.css'

export default function () {
    /*
     Pager 分页控件
            style：设置内联样式
            className：最外层 div的类样式
            previousPage：设置上一页显示的文本
            nextPage：设置下一页显示的文本
            noneClass：上一页、下一页不能点时的类样式
            activeClass：当前页的类样式
     pageIndex 当前页码索引
     setPageIndex 设置当前页码索引
     setDataCount 设置总数据量
     setPageSize 设置每页显示多少条，也可以在 usePager(pageSize) 设置
     setPageNum 设置最多显示多少个页码按钮，默认是7个
     pageCount 获取总页数

     pager.css ：分页按钮样式，此样式不是必须要引入的，这只是个示例
     */


    let pageSize = 10;
    let { Pager, pageIndex, setPageIndex, setDataCount, setPageSize, setPageNum, pageCount } = usePager(pageSize);


    //获取数据，获取总数据量
    useEffect(() => {
        setDataCount(197);
    }, []);


    useEffect(() => {
        //ajax取数据

    }, [pageIndex]);


    return (
        <>
            {/*<Pager className="pager" previousPage={<i className="icon-chevron_left"></i>} nextPage={<i className="icon-chevron_right"></i>} noneClass="pager-none" activeClass="pager-active"></Pager>*/}
            <Pager className="pager" noneClass="pager-none" activeClass="pager-active"></Pager>
        </>
    );
}