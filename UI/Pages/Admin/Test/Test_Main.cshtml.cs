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
                //��������
                if (Request.Query["MainID"].ToString().Length > 0) { sql.WhereAnd("MainID like @MainID", new { MainID = '%' + Request.Query["MainID"] + '%' }); }

                //�����Ӳ�ѯ
                sql.LeftJoin<Sys_Type>("Test_Main.Sys_Type_ID=Sys_Type.ID");
                //��ѯ��������
                this.DataCount = Convert.ToInt32(sql.QueryScalar());
                //���ò�ѯ��
                sql.Select("Test_Main.*,Sys_Type.Name");
                //����
                sql.OrderBy("Test_Main.ID desc");
                this.List = sql.Query((PageIndex - 1) * PageSize, PageSize);
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
