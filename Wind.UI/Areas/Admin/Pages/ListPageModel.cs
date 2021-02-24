using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wind.UI.Areas.Admin.Pages
{
    /// <summary>
    /// 列表页基类（公共部分写到此类当中，例如：权限控制，判断是否有登录）
    /// </summary>
    [Authorize]//验证是否有登录
    public class ListPageModel: PageModel
    {
        /// <summary>
        /// 分页-每页显示的数据量
        /// </summary>
        public static readonly int PageSize = 15;
        /// <summary>
        /// 分页-总数据量
        /// </summary>
        public int DataCount { set; get; }

        private int pageIndex = 1;
        /// <summary>
        /// 分页-当前页码
        /// </summary>
        [BindProperty(SupportsGet = true)]//url参数自动赋值给属性
        public int PageIndex { set => pageIndex = value; get => pageIndex; }
        /// <summary>
        /// 共用的代码写在构造函数中
        /// </summary>
        public ListPageModel()
        {

        }
    }
}
