using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Wind.UI.Areas.Admin.Pages
{
    /// <summary>
    /// PageModel 基类，后台其他页面的 PageModel类继承该类（公共部分写到此类当中，例如：权限控制，判断是否有登录）
    /// </summary>
    [Authorize]//验证是否有登录
    //[ValidateInput(false)]  //用于不验证富文本控件提交过来的 html字符串
    public class BasePageModel: PageModel
    {
        /// <summary>
        /// 分页-每页显示的数据量
        /// </summary>
        public static readonly int PageSize = 15;
        /// <summary>
        /// 分页-总数据量
        /// </summary>
        public int DataCount { set; get; }
        /// <summary>
        /// 分页-当前页码
        /// </summary>
        public int PageIndex { set; get; }

        /// <summary>
        /// 共用的代码写在构造函数中
        /// </summary>
        public BasePageModel()
        {

        }
    }
}
