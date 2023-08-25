using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DbOrm;
using DbOrm.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Wind.UI.Pages.Admin.Test
{
    public class Test_MainEditModel : EditPageModel
    {
        public Test_Main Model;
        public void OnGet(string id)
        {
            if (id != null)
            {
                Model = DB.Selects<Test_Main>().Where("ID=@ID", new { ID = id }).QueryFirstRow();
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
                //为 null 不参与修改，所以一定要有值才会参与修改
                if (model.MainID == null) { model.MainID = ""; }
                if (model.MainName == null) { model.MainName = ""; }
                if (model.Sys_Type_ID == null) { model.Sys_Type_ID = 0; }
                if (model.Quantity == null) { model.Quantity = 0; }
                if (model.Amount == null) { model.Amount = 0; }
                if (model.IsShow == null) { model.IsShow = false; }
                if (model.Img == null) { model.Img = ""; }
                if (model.Files == null) { model.Files = ""; }
                if (model.Remark == null) { model.Remark = ""; }
                if (model.CreateTime == null) { model.CreateTime = Convert.ToDateTime("1/1/1753 12:00:00"); }

                DB.Updates(model, "ID=@ID", new { ID = id });
            }
            return Redirect(Request.Query["url"]);
        }
    }
}
