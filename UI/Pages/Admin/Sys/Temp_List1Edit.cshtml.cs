using System;
using System.Linq;
using DbOrm;
using DbOrm.Model;
using Microsoft.AspNetCore.Mvc;

namespace UI.Pages.Admin
{
    public class Temp_List1EditModel : EditPageModel
    {
        public Temp_List1 Model;
        public void OnGet(string id)
        {
            if (id != null)
            {
                Model = DB.Selects<Temp_List1>().Where("ID=@ID", new { ID = id }).QueryFirstRow();
            }
        }

        //新增或修改
        public IActionResult OnPost(Temp_List1 model, string id)
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
