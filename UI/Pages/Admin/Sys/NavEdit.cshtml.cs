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

        //新增或修改
        public IActionResult OnPost(Sys_Type model, string id)
        {
            if (id == null)
            {
                object sort = DB.ExecuteScalars("select MAX(Sort) from Sys_Type where TypeID=@TypeID", new { TypeID = model.TypeID });
                model.Sort = (sort == DBNull.Value ? 0 : Convert.ToInt32(sort)) + 1;
                DB.Inserts(model);
            }
            else
            {
                model.Temp1 = Request.Form["Model.Temp1"];//获取原始值，OnPost() 接收的值会把空字符串转换为null，从而导致不能清空值，因为DbOrm 为null的不参与修改
                model.Temp2 = Request.Form["Model.Temp2"];
                DB.Updates(model, "ID=@ID", new { ID = id });
            }
            return Redirect(Request.Query["url"]);
        }
    }
}
