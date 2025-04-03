using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UI.Pages.Admin
{
    /// <summary>
    /// 编辑页基类（公共部分写到此类当中，例如：权限控制，判断是否有登录）
    /// </summary>
    [Authorize]//验证是否有登录
    public class EditPageModel: PageModel
    {

        /// <summary>
        /// 共用的代码写在构造函数中
        /// </summary>
        public EditPageModel()
        {

        }
    }
}
