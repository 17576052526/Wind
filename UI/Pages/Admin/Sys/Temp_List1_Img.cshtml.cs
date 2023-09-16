using System;
using System.Linq;
using System.Text.RegularExpressions;
using DbOrm;
using DbOrm.Model;
using Microsoft.AspNetCore.Mvc;

namespace UI.Pages.Admin
{
    public class Temp_List1_ImgModel : ListPageModel
    {
        public List<Temp_List1> List;
        public void OnGet()
        {
            using (DB db = new DB())
            {
                var sql = db.Select<Temp_List1>("count(*)");
                //��������
                if (Request.Query["Name"].ToString().Length > 0) { sql.WhereAnd("Name like @Name", new { Name = '%' + Request.Query["Name"] + '%' }); }
                sql.WhereAnd("TypeID = @TypeID", new { TypeID = Request.Query["typeID"].ToString() });

                //��ѯ��������
                this.DataCount = Convert.ToInt32(sql.QueryScalar());
                //���ò�ѯ��
                sql.Select("Temp_List1.*");
                //����
                sql.OrderBy("Temp_List1.ID desc");
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
                        db.Delete<Temp_List1>("ID=@ID", new { ID = m });
                    }
                    db.CommitTransaction();
                }
                catch { db.RollbackTransaction(); throw; }
            }
            return Redirect(Request.Path.Value.Remove(Request.Path.Value.LastIndexOf('/')) + Request.QueryString);
        }

    }
}
