using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace UI.Pages.Admin
{
    [Authorize]//��֤�Ƿ��е�¼
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
