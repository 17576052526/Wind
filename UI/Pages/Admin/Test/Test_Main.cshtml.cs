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
    public class Test_MainModel : ListPageModel
    {
        //��ѯ����
        private string _where;
        private string Where
        {
            get
            {
                if (_where == null)
                {
                    _where += !String.IsNullOrEmpty(Request.Query["MainID"]) ? " and Test_Main.MainID like @MainID" : null;
                    _where += !String.IsNullOrEmpty(Request.Query["Test_Type_ID"]) ? " and Test_Main.Test_Type_ID = @Test_Type_ID" : null;
                    _where += !String.IsNullOrEmpty(Request.Query["CreateTime1"]) ? " and Test_Main.CreateTime >= @CreateTime1" : null;
                    _where += !String.IsNullOrEmpty(Request.Query["CreateTime2"]) ? " and Test_Main.CreateTime <= @CreateTime2" : null;

                    _where = Regex.Replace(_where, @"^\s*and", "", RegexOptions.IgnoreCase);
                }
                return _where;
            }
        }
        //��ѯ��������
        private object _param;
        private object Param
        {
            get
            {
                if (_param == null)
                {
                    _param = new
                    {
                        MainID = "%" + Request.Query["MainID"].ToString() + "%",
                        Test_Type_ID = Request.Query["Test_Type_ID"].ToString(),
                        CreateTime1 = Request.Query["CreateTime1"].ToString(),
                        CreateTime2 = Request.Query["CreateTime2"].ToString(),
                    };
                }
                return _param;
            }
        }
        public List<Test_Main> List;
        public void OnGet()
        {
            //ȡ��
            using (DB db = new DB())
            {
                var sql = db.Select<Test_Main>("count(*)")
                    .LeftJoin<Test_Type>("Test_Main.Test_Type_ID=Test_Type.ID")
                    .Where(Where, Param);
                this.DataCount = (int)sql.QueryScalar();

                if (Request.Query["orderby"].Count > 0)
                {
                    sql = sql.OrderBy(Request.Query["orderby"]);
                }
                this.List = sql.Select("Test_Main.*,Test_Type.TypeName")
                    .Query((PageIndex - 1) * PageSize, PageSize);
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
                catch { db.RollbackTransaction();throw; }
            }
            return Redirect(Request.Path.Value.Remove(Request.Path.Value.LastIndexOf('/')) + Request.QueryString);
        }

    }
}
