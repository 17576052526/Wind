using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wind.UI.Pages.Admin
{
    /// <summary>
    /// 列表页基类（公共部分写到此类当中，例如：权限控制，判断是否有登录）
    /// </summary>
    [Authorize]//验证是否有登录
    public class ListPageModel: PageModel
    {
        public static readonly int pageSize = 15;
        /// <summary>
        /// 分页-每页显示的数据量
        /// </summary>
        public int PageSize { get => pageSize; }
        /// <summary>
        /// 分页-总数据量
        /// </summary>
        public int DataCount { set; get; }

        /// <summary>
        /// 分页-当前页码
        /// </summary>
        public int PageIndex { get { string page = Request.Query["page"]; return page != null ? Convert.ToInt32(page) : 1; } }//注意 page参数名只能用 Request.Query["page"]取值，OnGet(string page) 取不到值
        /// <summary>
        /// 共用的代码写在构造函数中
        /// </summary>
        public ListPageModel()
        {

        }
    }
}
