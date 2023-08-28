using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace UI.Pages.Admin
{
    [Authorize]//验证是否有登录
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
