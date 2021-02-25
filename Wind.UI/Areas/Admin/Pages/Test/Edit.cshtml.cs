using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbOrm;
using DbOrm.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Wind.UI.Areas.Admin.Pages.Test
{
    public class EditModel : EditPageModel
    {
        public Test_Main Model;
        public void OnGet(string id)
        {
            if (id != null)
            {
                Model = DB.Select<Test_Main>().Where("ID=@ID").QueryFirstRow(new { ID = id });
            }
        }

        //新增或修改
        public void OnPost(Test_Main model)
        {
            if (model.ID == null)
            {
                DB.Insert(model);
            }
            else
            {
                DB.Update(model, "ID=@ID");
            }
            Response.Redirect("List");
        }
    }
}
