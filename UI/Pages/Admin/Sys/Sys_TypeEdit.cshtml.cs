using System;
using System.Linq;
using DbOrm;
using DbOrm.Model;
using Microsoft.AspNetCore.Mvc;

namespace UI.Pages.Admin
{
    public class Sys_TypeEditModel : EditPageModel
    {
        public Sys_Type Model;
        public void OnGet(string id)
        {
            if (id != null)
            {
                Model = DB.Selects<Sys_Type>().Where("ID=@ID", new { ID = id }).QueryFirstRow();
            }
        }

        //新增或修改
        public IActionResult OnPost(Sys_Type model, string id)
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
