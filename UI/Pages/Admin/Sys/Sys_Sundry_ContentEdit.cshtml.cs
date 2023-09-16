using System;
using System.Linq;
using DbOrm;
using DbOrm.Model;
using Microsoft.AspNetCore.Mvc;

namespace UI.Pages.Admin
{
    public class Sys_Sundry_ContentEditModel : EditPageModel
    {
        public Sys_Sundry Model;
        public void OnGet(string typeID)
        {
            Model = DB.Selects<Sys_Sundry>().Where("typeID=@typeID", new { typeID = typeID }).QueryFirstRow();
        }

        //新增或修改
        public IActionResult OnPost(Sys_Sundry model)
        {
            if (model.ID == null)
            {
                DB.Inserts(model);
            }
            else
            {
                DB.Updates(model, "ID=@ID", new { ID = model.ID });
            }
            return Redirect("/admin/default");
        }
    }
}
