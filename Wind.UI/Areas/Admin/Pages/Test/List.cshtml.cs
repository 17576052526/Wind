using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DbOrm;
using DbOrm.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Wind.UI.Areas.Admin.Pages.Test
{
    public class ListModel : BasePageModel
    {
        //查询条件
        private string _where;
        private string Where
        {
            get
            {
                if (_where == null)
                {
                    _where += !String.IsNullOrEmpty(Request.Query["MainName"]) ? " and Test_Main.MainName like @MainName" : null;
                    _where += !String.IsNullOrEmpty(Request.Query["Test_Type_ID"]) ? " and Test_Main.Test_Type_ID = @Test_Type_ID" : null;
                    _where += !String.IsNullOrEmpty(Request.Query["CreateTime1"]) ? " and Test_Main.CreateTime >= @CreateTime1" : null;
                    _where += !String.IsNullOrEmpty(Request.Query["CreateTime2"]) ? " and Test_Main.CreateTime <= @CreateTime2" : null;

                    _where = Regex.Replace(_where, @"^\s*and", "", RegexOptions.IgnoreCase);
                }
                return _where;
            }
        }
        //查询条件参数
        private object _param;
        private object Param
        {
            get
            {
                if (_param == null)
                {
                    _param = new
                    {
                        MainName = "%" + Request.Query["MainName"].ToString() + "%",
                        Test_Type_ID = Request.Query["Test_Type_ID"].ToString(),
                        CreateTime1 = Request.Query["CreateTime1"].ToString(),
                        CreateTime2 = Request.Query["CreateTime2"].ToString(),
                    };
                }
                return _param;
            }
        }
        public List<Test_Main> List;
        public void OnGet(int page = 1)
        {
            this.PageIndex = page;//获取页码
            //取数
            using (DB db = new DB())
            {
                var sql = db.Selects<Test_Main>("count(*)")
                    .LeftJoin<Test_Type>("Test_Main.Test_Type_ID=Test_Type.ID")
                    .Where(Where);
                this.DataCount = sql.QueryScalar<int>(Param);

                this.List = sql.Select("Test_Main.*,Test_Type.TypeName")
                    .Query((PageIndex - 1) * PageSize, PageSize, Param);
            }
        }
    }
}
