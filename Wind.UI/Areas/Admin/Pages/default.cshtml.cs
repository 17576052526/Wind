using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Wind.UI.Areas.Admin.Pages
{
    [Authorize]
    public class defaultModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
