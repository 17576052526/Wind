using System;
using System.Linq;
using System.Text.RegularExpressions;
using DbOrm;
using DbOrm.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;

namespace UI.Pages.Admin
{
    public class TypesModel : ListPageModel
    {
        public List<Sys_Type> List;
        public void OnGet()
        {
            using (DB db = new DB())
            {
                var sql = db.Select<Sys_Type>();
                //��������
                sql.WhereAnd("TypeID = @TypeID", new { TypeID = 0 });
                this.List = sql.Query();

                //��ѯ�Ӽ�
                foreach(var m in List)
                {
                    m.Children = db.Select<Sys_Type>().Where("TypeID=@TypeID", new { TypeID = m.ID }).Query();

                    //��ѯ�Ӽ����Ӽ�
                    foreach (var c in m.Children)
                    {
                        c.Children = db.Select<Sys_Type>().Where("TypeID=@TypeID", new { TypeID = c.ID }).Query();
                    }
                }
            }
        }

        //ɾ������ҳ��ͷ������ @page "{handler}"��/list/Delete �Ϳɷ��ʣ�����Ҫ /list?handler=Delete���ܷ���
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

    }
}
