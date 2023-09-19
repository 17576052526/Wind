using System;
using System.Linq;
using System.Text.RegularExpressions;
using DbOrm;
using DbOrm.Model;
using Microsoft.AspNetCore.Mvc;

namespace UI.Pages.Admin
{
    public class Sys_Sundry_BannerModel : ListPageModel
    {
        public List<Sys_Sundry> List;
        public void OnGet()
        {
            using (DB db = new DB())
            {
                var sql = db.Select<Sys_Sundry>("count(*)");
                //��������
                if (Request.Query["Name"].ToString().Length > 0) { sql.WhereAnd("Name like @Name", new { Name = '%' + Request.Query["Name"] + '%' }); }
                sql.WhereAnd("TypeID = @TypeID", new { TypeID = Request.Query["typeID"].ToString() });

                //��ѯ��������
                this.DataCount = Convert.ToInt32(sql.QueryScalar());
                //���ò�ѯ��
                sql.Select("Sys_Sundry.*");
                //����
                sql.OrderBy("Sys_Sundry.Sort asc");
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
                        db.Delete<Sys_Sundry>("ID=@ID", new { ID = m });
                    }
                    db.CommitTransaction();
                }
                catch { db.RollbackTransaction(); throw; }
            }
            return Redirect(Request.Path.Value.Remove(Request.Path.Value.LastIndexOf('/')) + Request.QueryString);
        }

        //���ƣ�����
        public IActionResult OnGetMove(int id, int isTop)
        {
            using (DB db = new DB())
            {
                db.BeginTransaction();
                try
                {
                    //��ѯ��ǰ����
                    var model = db.Select<Sys_Sundry>().Where("ID=@ID", new { ID = id }).QueryFirstRow();
                    Sys_Sundry modelpro = null;
                    if (isTop == 1)//����
                    {
                        //��ѯ��һ��
                        modelpro = db.Select<Sys_Sundry>().Where("TypeID=@TypeID and Sort<@Sort", new { TypeID = model.TypeID, Sort = model.Sort }).OrderBy("Sort desc").QueryFirstRow();
                    }
                    else
                    {
                        //��ѯ��һ��
                        modelpro = db.Select<Sys_Sundry>().Where("TypeID=@TypeID and Sort>@Sort", new { TypeID = model.TypeID, Sort = model.Sort }).OrderBy("Sort asc").QueryFirstRow();

                    }
                    if (modelpro != null)
                    {
                        //�Ի�sort
                        db.ExecuteNonQuery("update Sys_Sundry set Sort=@Sort where ID=@ID", new { ID = model.ID, Sort = modelpro.Sort });
                        db.ExecuteNonQuery("update Sys_Sundry set Sort=@Sort where ID=@ID", new { ID = modelpro.ID, Sort = model.Sort });
                    }
                    db.CommitTransaction();
                }
                catch { db.RollbackTransaction(); throw; }
            }
            return Redirect(Request.Path.Value.Remove(Request.Path.Value.LastIndexOf('/')) + "?typeID=" + Request.Query["typeID"]);
        }

    }
}
