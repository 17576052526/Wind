using System;
using System.Linq;
using DbOrm;
using DbOrm.Model;
using Microsoft.AspNetCore.Mvc;

namespace UI.Pages.Admin
{
    public class Sys_Sundry_BannerEditModel : EditPageModel
    {
        public Sys_Sundry Model;
        public void OnGet(string id)
        {
            if (id != null)
            {
                Model = DB.Selects<Sys_Sundry>().Where("ID=@ID", new { ID = id }).QueryFirstRow();
            }
        }

        //新增或修改
        public IActionResult OnPost(Sys_Sundry model, string id)
        {
            if (id == null)
            {
                object sort = DB.ExecuteScalars("select MAX(Sort) from Sys_Sundry where TypeID=@TypeID", new { TypeID = model.TypeID });
                model.Sort = (sort == DBNull.Value ? 0 : Convert.ToInt32(sort)) + 1;
                DB.Inserts(model);
            }
            else
            {
                model.Content = Request.Form["Model.Content"];
                DB.Updates(model, "ID=@ID", new { ID = id });
            }
            return Redirect(Request.Query["url"]);
        }
    }
}
