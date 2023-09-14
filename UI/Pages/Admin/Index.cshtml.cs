using DbOrm.Model;
using DbOrm;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace UI.Pages.Admin
{
    [Authorize]//验证是否有登录
    public class IndexModel : PageModel
    {
        public List<Sys_Type> List;
        public void OnGet()
        {
            using (DB db = new DB())
            {
                var sql = db.Select<Sys_Type>();
                //构造条件
                sql.WhereAnd("TypeID = @TypeID", new { TypeID = 0 });
                this.List = sql.OrderBy("Sort asc").Query();

                //查询子集
                foreach (var m in List)
                {
                    m.Children = db.Select<Sys_Type>().Where("TypeID=@TypeID", new { TypeID = m.ID }).OrderBy("Sort asc").Query();

                }
            }
        }
    }
}
