using System;
using System.Linq;
using DbOrm;
using DbOrm.Model;
using Microsoft.AspNetCore.Mvc;

namespace UI.Pages.Admin
{
    public class Test_MainEditModel : EditPageModel
    {
        public Test_Main Model;
        public void OnGet(string id)
        {
            if (id != null)
            {
                Model = DB.Selects<Test_Main>().Where("ID=@ID", new { ID = id }).QuerySingle();
            }
        }

        //新增或修改
        public IActionResult OnPost(Test_Main model, string id)
        {
            if (id == null)
            {
                DB.Inserts(model);
            }
            else
            {
                DB.Updates(model, "ID=@ID", new { ID = id });
            }
            return Redirect(Request.Query["url"]);
        }
    }
}
