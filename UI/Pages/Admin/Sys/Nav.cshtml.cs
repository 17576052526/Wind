using System;
using System.Linq;
using System.Text.RegularExpressions;
using DbOrm;
using DbOrm.Model;
using Microsoft.AspNetCore.Mvc;

namespace UI.Pages.Admin
{
    public class NavModel : ListPageModel
    {
        public List<Sys_Type> List;
        public void OnGet()
        {
            using (DB db = new DB())
            {
                var sql = db.Select<Sys_Type>();
                //构造条件
                sql.WhereAnd("TypeID = @TypeID", new { TypeID = 0 });
                //排序
                sql.OrderBy("Sort asc");
                this.List = sql.Query();
                //查询子集
                foreach (var m in List)
                {
                    m.Children = db.Select<Sys_Type>().Where("TypeID=@TypeID", new { TypeID = m.ID }).OrderBy("Sort asc").Query();
                }
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
                        db.Delete<Sys_Type>("ID=@ID", new { ID = m });
                    }
                    db.CommitTransaction();
                }
                catch { db.RollbackTransaction(); throw; }
            }
            return Redirect(Request.Path.Value.Remove(Request.Path.Value.LastIndexOf('/')) + Request.QueryString);
        }
        //上移，下移
        public IActionResult OnGetMove(int id, int isTop)
        {
            using (DB db = new DB())
            {
                db.BeginTransaction();
                try
                {
                    //查询当前数据
                    var model = db.Select<Sys_Type>().Where("ID=@ID", new { ID = id }).QueryFirstRow();
                    Sys_Type modelpro = null;
                    if (isTop == 1)//上移
                    {
                        //查询上一条
                        modelpro = db.Select<Sys_Type>().Where("TypeID=@TypeID and Sort<@Sort", new { TypeID = model.TypeID, Sort = model.Sort }).OrderBy("Sort desc").QueryFirstRow();
                    }
                    else
                    {
                        //查询下一条
                        modelpro = db.Select<Sys_Type>().Where("TypeID=@TypeID and Sort>@Sort", new { TypeID = model.TypeID, Sort = model.Sort }).OrderBy("Sort asc").QueryFirstRow();

                    }
                    if (modelpro != null)
                    {
                        //对换sort
                        db.ExecuteNonQuery("update Sys_Type set Sort=@Sort where ID=@ID", new { ID = model.ID, Sort = modelpro.Sort });
                        db.ExecuteNonQuery("update Sys_Type set Sort=@Sort where ID=@ID", new { ID = modelpro.ID, Sort = model.Sort });
                    }
                    db.CommitTransaction();
                }
                catch { db.RollbackTransaction(); throw; }
            }
            return Redirect(Request.Path.Value.Remove(Request.Path.Value.LastIndexOf('/')));
        }
    }
}
