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
        public Result Insert(Test_Main model)
        {
            DB.Inserts(model);
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
        public Result Update(Test_Main model)
        {
            DB.Updates(model, "ID=@ID", new { ID = model.ID });
            return Result.OK();
        }

        [HttpPost]
        public Result Select(dynamic obj)
        {
            using (DB db = new DB())
            {
                var sql = db.Select<Test_Main>("count(*)");
                //构造条件，注意参数赋值要转类型
                if (obj.MainName != "" && obj.MainName != null) { sql.WhereAnd("MainName like @MainName", new { MainName = '%' + obj.MainName.ToString() + '%' }); }


                //查询总数据量
                int total = (int)sql.QueryScalar();
                //设置查询列
                sql = sql.Select("Test_Main.*,Test_Type.TypeName");
                //表连接查询
                sql.LeftJoin<Test_Type>("Test_Main.Test_Type_ID=Test_Type.ID");
                //排序
                //if (obj.orderBy != null) { sql.OrderBy(obj.orderBy.ToString()); }
                //分页
                return Result.OK(new { total = total, list = sql.Query(((int)obj.pageIndex - 1) * (int)obj.pageSize, (int)obj.pageSize) });
            }
        }
    }
}
