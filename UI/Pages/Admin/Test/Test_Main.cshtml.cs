using System;
using System.Linq;
using System.Text.RegularExpressions;
using DbOrm;
using DbOrm.Model;
using Microsoft.AspNetCore.Mvc;

namespace UI.Pages.Admin
{
    public class Test_MainModel : ListPageModel
    {
        public List<Test_Main> List;
        public void OnGet()
        {
            using (DB db = new DB())
            {
                var sql = db.Select<Test_Main>("count(*)");
                //构造条件
                if (Request.Query["MainID"].ToString().Length > 0) { sql.WhereAnd("MainID like @MainID", new { MainID = '%' + Request.Query["MainID"] + '%' }); }

                //表连接查询
                sql.LeftJoin<Sys_Type>("Test_Main.Sys_Type_ID=Sys_Type.ID");
                //查询总数据量
                this.DataCount = Convert.ToInt32(sql.QueryScalar());
                //设置查询列
                sql.Select("Test_Main.*,Sys_Type.Name");
                //排序
                sql.OrderBy("Test_Main.ID desc");
                this.List = sql.Query((PageIndex - 1) * PageSize, PageSize);
            }
        }

        //删除，在页面头部加上 @page "{handler}"，/list/Delete 就可访问，否则要 /list?handler=Delete才能访问
        public IActionResult OnPostDelete()
        {
            using (DB db = new DB())
            {
                db.BeginTransaction();
                try
                {
                    foreach (var m in Request.Form["checkID"])
                    {
                        db.Delete<Test_Main>("ID=@ID", new { ID = m });
                    }
                    db.CommitTransaction();
                }
                catch { db.RollbackTransaction(); throw; }
            }
            return Redirect(Request.Path.Value.Remove(Request.Path.Value.LastIndexOf('/')) + Request.QueryString);
        }

    }
}
