using DbOrm.Model;
using DbOrm;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace UI.Pages.Admin
{
    [Authorize]//��֤�Ƿ��е�¼
    public class IndexModel : PageModel
    {
        public List<Sys_Type> List;
        public void OnGet()
        {
            using (DB db = new DB())
            {
                var sql = db.Select<Sys_Type>();
                //��������
                sql.WhereAnd("TypeID = @TypeID", new { TypeID = 0 });
                this.List = sql.OrderBy("Sort asc").Query();

                //��ѯ�Ӽ�
                foreach (var m in List)
                {
                    m.Children = db.Select<Sys_Type>().Where("TypeID=@TypeID", new { TypeID = m.ID }).OrderBy("Sort asc").Query();

                }
            }
        }
    }
}
