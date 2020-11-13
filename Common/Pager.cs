using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.IO;

/// <summary>
/// 分页视图实体类，该类和分页控件无关，仅用于控制器传递数据到视图
/// </summary>
public class PageModel
{
    public int DataCount { set; get; }
    public int PageSize { set; get; }
    public int PageIndex { set; get; }
    public object DataList { set; get; }//此处不用List<T> 是用于兼容 DataTable 类型
}

/// <summary>
/// 分页控件 .net core版
/// </summary>
public static class PagerClass
{
    /// <summary>
    /// 分页控件，父级类样式：.pager，不能点的类样式：.pager-none，当前页类样式：.pager-active
    /// </summary>
    /// <param name="href">分页请求的地址</param>
    /// <param name="pageSize">每页显示多少条</param>
    /// <param name="dataCount">总数据量</param>
    /// <param name="pageIndex">当前页码</param>
    /// <param name="pageNum">显示多少个分页按钮</param>
    /// <returns></returns>
    public static object Pager(this IHtmlHelper helper, string href, int pageSize, int dataCount, int pageIndex, int pageNum = 7)
    {
        int pageCount = dataCount % pageSize == 0 ? dataCount / pageSize : dataCount / pageSize + 1;   //总的页码数量
        pageNum = pageNum > pageCount ? pageCount : pageNum;   //要显示的页面按钮个数
        string parameter = helper.ViewContext.HttpContext.Request.QueryString.Value;//url参数
        TextWriter writer = helper.ViewContext.Writer;
        //开始输出标签，模式：上一页  1 ... 4 5 6 7 8 ... 10  下一页
        writer.Write("<div class='pager'>");
        //----------
        //上一页、第一页
        if (pageIndex == 1)
        {
            WriteA(writer, "javascript:void(0)", "pager-none", "上一页");  //上一页
            WriteA(writer, "javascript:void(0)", "pager-active", "1");  //第一页
        }
        else
        {
            WriteA(writer, href + "/" + (pageIndex - 1) + parameter, "", "上一页");  //上一页
            WriteA(writer, href + "/1" + parameter, "", "1");  //第一页
        }
        //中间页码
        int minIndex = pageIndex > pageNum / 2 ? pageIndex - pageNum / 2 : 1;  //计算最小页码索引
        minIndex = minIndex + pageNum > pageCount ? pageCount - pageNum + 1 : minIndex;  //计算最大的最小页码索引
        for (int i = 1; i < pageNum - 1; i++)
        {
            if (minIndex + i == pageIndex)   //当前选中页
            {
                WriteA(writer, "javascript:void(0)", "pager-active", minIndex + i);
            }
            else if (i == 1 && minIndex > 1)  //前面的 ...
            {
                WriteA(writer, href + "/" + (minIndex + i) + parameter, "", "...");
            }
            else if (i == pageNum - 2 && minIndex + pageNum - 1 < pageCount)  //后面的 ...
            {
                WriteA(writer, href + "/" + (minIndex + i) + parameter, "", "...");
            }
            else
            {
                WriteA(writer, href + "/" + (minIndex + i) + parameter, "", minIndex + i);
            }
        }
        //最后一页
        if (pageCount > 1)
        {
            if (pageIndex == pageCount)
            {
                WriteA(writer, "javascript:void(0)", "pager-active", pageCount);
            }
            else
            {
                WriteA(writer, href + "/" + pageCount + parameter, "", pageCount);
            }
        }
        //下一页
        if (pageIndex == pageCount || pageCount == 0)
        {
            WriteA(writer, "javascript:void(0)", "pager-none", "下一页");
        }
        else
        {
            WriteA(writer, href + "/" + (pageIndex + 1) + parameter, "", "下一页");
        }
        //----------
        writer.Write("</div>");
        return null;
    }
    //输出<a>标签
    private static void WriteA(TextWriter writer, string href, string @class, object text)
    {
        writer.Write("<a href='" + href + "' class='" + @class + "'>" + text + "</a>");
    }
}