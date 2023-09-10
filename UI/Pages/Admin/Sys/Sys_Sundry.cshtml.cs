using System;
using System.Linq;
using System.Text.RegularExpressions;
using DbOrm;
using DbOrm.Model;
using Microsoft.AspNetCore.Mvc;

namespace UI.Pages.Admin
{
    public class Sys_SundryModel : ListPageModel
    {
        public List<Sys_Sundry> List;
        public void OnGet()
        {
            using (DB db = new DB())
            {
                var sql = db.Select<Sys_Sundry>("count(*)");
                //构造条件
                if (Request.Query["Name"].ToString().Length > 0) { sql.WhereAnd("Name like @Name", new { Name = '%' + Request.Query["Name"] + '%' }); }
                sql.WhereAnd("TypeID = @TypeID", new { TypeID = Request.Query["typeID"].ToString() });

                //查询总数据量
                this.DataCount = Convert.ToInt32(sql.QueryScalar());
                //设置查询列
                sql.Select("Sys_Sundry.*");
                //排序
                sql.OrderBy("Sys_Sundry.ID desc");
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
                        db.Delete<Sys_Sundry>("ID=@ID", new { ID = m });
                    }
                    db.CommitTransaction();
                }
                catch { db.RollbackTransaction(); throw; }
            }
            return Redirect(Request.Path.Value.Remove(Request.Path.Value.LastIndexOf('/')) + Request.QueryString);
        }

    }
}
