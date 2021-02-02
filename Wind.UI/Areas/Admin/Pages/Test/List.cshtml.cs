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
                    _where += !String.IsNullOrEmpty(Request.Query["Title"]) ? " and Test.Title like @Title" : null;
                    _where += !String.IsNullOrEmpty(Request.Query["TypesID"]) ? " and Test.TypesID = @TypesID" : null;
                    _where += !String.IsNullOrEmpty(Request.Query["Dates1"]) ? " and Test.Dates >= @Dates1" : null;
                    _where += !String.IsNullOrEmpty(Request.Query["Dates2"]) ? " and Test.Dates <= @Dates2" : null;

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
                        Title = "%" + Request.Query["Title"] + "%",
                        TypesID = Request.Query["TypesID"],
                        Dates1 = Request.Query["Dates1"],
                        Dates2 = Request.Query["Dates2"],
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
            using (DB db=new DB())
            {
                this.DataCount=(int)db.Selects("count(*)").From("Test_Main").Where(Where).QueryScalar(Param);
                this.List = db.Selects().From("Test_Main").Query<Test_Main>(Param);
            }
        }
    }
}
