using DbOrm;
using DbOrm.Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace UI.Controllers.Api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [JWTAuthorize]
    public class Test_MainController : ControllerBase
    {
        [HttpPost]
        public Result Insert(Test_Main param)
        {
            DB.Inserts(param);
            return Result.OK();
        }

        [HttpPost]
        public Result Delete(List<int> param)
        {
            using (DB db = new DB())
            {
                db.BeginTransaction();
                try
                {
                    foreach (var m in param)
                    {
                        db.Delete<Test_Main>("ID=@ID", new { ID = m });
                    }
                    db.CommitTransaction();
                }
                catch { db.RollbackTransaction(); throw; }
            }
            return Result.OK();
        }

        [HttpPost]
        public Result Update(Test_Main param)
        {
            DB.Updates(param, "ID=@ID", new { ID = param.ID });
            return Result.OK();
        }

        [HttpPost]
        public Result Select(dynamic param)
        {
            using (DB db = new DB())
            {
                var sql = db.Select<Test_Main>("count(*)");
                //构造条件，注意参数赋值要转类型
                if (param.MainName != "" && param.MainName != null) { sql.WhereAnd("MainName like @MainName", new { MainName = '%' + param.MainName.ToString() + '%' }); }

                //表连接查询
                sql.LeftJoin<Sys_Type>("Test_Main.TypeID=Sys_Type.ID");
                //查询总数据量
                int total = Convert.ToInt32(sql.QueryScalar());
                //设置查询列
                sql.Select("Test_Main.*,Sys_Type.Name");
                //排序
                sql.OrderBy("Test_Main.ID desc");
                //分页
                return Result.OK(new { total = total, list = sql.Query(((int)param.pageIndex - 1) * (int)param.pageSize, (int)param.pageSize) });
            }
        }
    }
}
