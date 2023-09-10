using System;
using System.Linq;
using DbOrm;
using DbOrm.Model;
using Microsoft.AspNetCore.Mvc;

namespace UI.Pages.Admin
{
    public class NavEditModel : EditPageModel
    {
        public Sys_Type Model;
        public void OnGet(string id)
        {
            if (id != null)
            {
                Model = DB.Selects<Sys_Type>().Where("ID=@ID", new { ID = id }).QueryFirstRow();
            }
        }

        //�������޸�
        public IActionResult OnPost(Sys_Type model, string id)
        {
            model.Temp2 = model.Temp2 == null ? "" : model.Temp2;//NULL ת��""
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
