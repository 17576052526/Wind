using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.IO;
using System.Text.RegularExpressions;

/// <summary>
/// 分页控件
/// </summary>
public class Pager
{
    /// <summary>
    /// 每页显示的数量
    /// </summary>
    public int PageSize { set; get; }
    /// <summary>
    /// 总数据量
    /// </summary>
    public int DataCount { set; get; }
    /// <summary>
    /// 总页码数量
    /// </summary>
    internal int PageCount { set; get; }
    /// <summary>
    /// 当前页的索引
    /// </summary>
    internal int PageIndex { set; get; }
    /// <summary>
    /// 显示页码按钮的个数
    /// </summary>
    public int PageNum { set { _PageNum = value; } get { return _PageNum; } }
    private int _PageNum = 7;
    /// <summary>
    /// 上一页显示的文本
    /// </summary>
    public string TopPageText { set { _TopPageText = value; } get { return _TopPageText; } }
    private string _TopPageText = "上一页";
    /// <summary>
    /// 下一页显示的文本
    /// </summary>
    public string BottomPageText { set { _BottomPageText = value; } get { return _BottomPageText; } }
    private string _BottomPageText = "下一页";
    /// <summary>
    /// 最外层标签的类样式
    /// </summary>
    public string CssClass { set; get; }
    /// <summary>
    /// 上一页和下一页的类样式
    /// </summary>
    public string TopBottomClass { set; get; }
    /// <summary>
    /// 页码按钮的类样式
    /// </summary>
    public string PagingClass { set; get; }
    /// <summary>
    /// 当前页的页码按钮类样式
    /// </summary>
    public string IndexClass { set; get; }

    /// <summary>
    /// 上一页、下一页不能点击时的按钮的样式
    /// </summary>
    public string NoCheFLBTClass { set; get; }
    internal string UrlFirst { set; get; }
    internal string UrlLast { set; get; }
    //用于输出字符到页面
    internal TextWriter Writer { set; get; }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="dataCount">总数据量（必填）</param>
    /// <param name="pageSize">每页显示多少条（必填）</param>
    /// <param name="pageIndex">当前页码（必填）</param>
    public Pager(int dataCount,int pageSize)
    {
        this.DataCount = dataCount;
        this.PageSize = pageSize;
        //初始化
        PageCount = DataCount % PageSize == 0 ? DataCount / PageSize : DataCount / PageSize + 1;   //总的页码数量
        PageNum = PageNum > PageCount ? PageCount : PageNum;   //要显示的页面按钮个数
    }

    public void Start()
    {
        //开始输出标签
        Writer.Write("<div class='" + CssClass + "'>");
        TagWrite();
        Writer.Write("</div>");

    }
    //模式    上一页  1 ... 4 5 6 7 8 ... 10  下一页
    internal void TagWrite()
    {
        //上一页、第一页
        if (PageIndex == 1)
        {
            A("javascript:void(0)", TopBottomClass + " " + NoCheFLBTClass, TopPageText);  //上一页
            A("javascript:void(0)", PagingClass + " " + IndexClass, "1");  //第一页
        }
        else
        {
            A(UrlFirst + (PageIndex - 1) + UrlLast, TopBottomClass, TopPageText);  //上一页
            A(UrlFirst + 1 + UrlLast, PagingClass, "1");  //第一页
        }
        //中间页码
        int MinIndex = PageIndex > PageNum / 2 ? PageIndex - PageNum / 2 : 1;  //计算最小页码索引
        MinIndex = MinIndex + PageNum > PageCount ? PageCount - PageNum + 1 : MinIndex;  //计算最大的最小页码索引
        for (int i = 1; i < PageNum - 1; i++)
        {
            if (MinIndex + i == PageIndex)   //当前选中页
            {
                A("javascript:void(0)", PagingClass + " " + IndexClass, (MinIndex + i).ToString());
            }
            else if (i == 1 && MinIndex > 1)  //前面的 ...
            {
                A(UrlFirst + (MinIndex + i) + UrlLast, PagingClass, "...");
            }
            else if (i == PageNum - 2 && MinIndex + PageNum - 1 < PageCount)  //后面的 ...
            {
                A(UrlFirst + (MinIndex + i) + UrlLast, PagingClass, "...");
            }
            else
            {
                A(UrlFirst + (MinIndex + i) + UrlLast, PagingClass, (MinIndex + i).ToString());
            }
        }
        //最后一页
        if (PageCount > 1)
        {
            if (PageIndex == PageCount)
            {
                A("javascript:void(0)", PagingClass + " " + IndexClass, PageCount.ToString());
            }
            else
            {
                A(UrlFirst + PageCount + UrlLast, PagingClass, PageCount.ToString());   //最后一页
            }
        }
        //下一页
        if (PageIndex == PageCount)
        {
            A("javascript:void(0)", TopBottomClass + " " + NoCheFLBTClass, BottomPageText);  //下一页
        }
        else
        {
            A(UrlFirst + (PageIndex + 1) + UrlLast, TopBottomClass, BottomPageText);  //下一页
        }
    }
    //输出a标签
    private void A(string href, string classStyle, string text)
    {
        Writer.Write($"<a href='{href}' class='{classStyle}'>{text}</a>");
    }
}
public static class PagerStatic
{
    /// <summary>
    /// Url分页
    /// </summary>
    /// <param name="model">分页实体类</param>
    public static object Pager(this IHtmlHelper helper, Pager model)
    {
        var request = helper.ViewContext.HttpContext.Request;
        string query = request.QueryString.Value;
        int pageIndex = Convert.ToInt32(request.Query["pn"]);//page 参数名 在.net core 中不能 OnGet(int page) 接收，所以此处用pn做为参数名
        model.PageIndex = pageIndex < 1 ? 1 : pageIndex > model.PageCount ? model.PageCount : pageIndex;
        model.Writer = helper.ViewContext.Writer;
        //计算 UrlFirst、UrlLast
        var arr = Regex.Split(query, "(?<=[?&]pn=)[^&]*", RegexOptions.IgnoreCase);//拆分成page参数前后
        if (arr.Length > 1)//url中存在page参数
        {
            model.UrlFirst = arr[0];
            model.UrlLast = arr[1];
        }
        else//url中不存在page参数
        {
            model.UrlFirst = (query.Length > 0 ? query + '&' : '?') + "pn=";
        }
        model.Start();
        return null;
    }

    /// <summary>
    /// 路由分页
    /// </summary>
    /// <param name="href">连接地址，不包括url参数</param>
    /// <param name="pageIndex">当前页码</param>
    /// <param name="model">分页实体类</param>
    public static object Pager(this IHtmlHelper helper, string href, int pageIndex, Pager model)
    {
        model.UrlFirst = href + '/';
        model.UrlLast = helper.ViewContext.HttpContext.Request.QueryString.Value;
        model.PageIndex = pageIndex > model.PageCount ? model.PageCount : pageIndex;
        model.Writer = helper.ViewContext.Writer;
        model.Start();
        return null;
    }
}