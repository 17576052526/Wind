using DbOrm;
using DbOrm.Model;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace UI.Controllers.Api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpPost]
        public Result Insert(Test_Main param)
        {
            DB.Insert(param);
            return Result.OK();
        }

        [HttpPost]
        public Result Delete(List<string> param)
        {
            using (DB db = new DB())
            {
                db.BeginTransaction();
                try
                {
                    foreach (var m in param)
                    {
                        db.Deletes<Test_Main>("ID=@ID", new { ID = m });
                    }
                    db.Commit();
                }
                catch { db.Rollback(); throw; }
            }
            return Result.OK();
        }

        [HttpPost]
        public Result Update(Test_Main param)
        {
            DB.Update(param, "ID=@ID", new { ID = param.ID });
            return Result.OK();
        }
        /*
         {
	pageIndex:1,
	paeSize:20,
	orderBy:"Create desc",
	equal:{
		Name:'aaa'
	},
	like:{
		Name:'aaa'
	},
	lt:{
		CreateTime:'2020-05-10'
	},
	gt:{
		CreateTime:'2020-05-10'
	},
}
         */
        [HttpPost]
        public Result Select(object param)
        {
            dynamic obj = JsonConvert.DeserializeObject<dynamic>(param.ToString());
            var sql = DB.Select<Test_Main>();
            if (obj.orderBy != null)
            {
                sql = sql.OrderBy(obj.orderBy.ToString());
            }
            if (obj.equal != null)
            {
                JObject equal = obj.equal;
                foreach (var m in equal)
                {
                    sql.Where(m.Key + "='" + m.Value + "'");
                }
            }
            if (obj.like != null)
            {
                JObject like = obj.like;
                foreach (var m in like)
                {
                    sql.Where(m.Key + " like '%" + m.Value + "%'");
                }
            }
            if (obj.gt != null)
            {
                JObject gt = obj.gt;
                foreach (var m in gt)
                {
                    sql.Where(m.Key + "<='" + m.Value + "'");
                }
            }
            if (obj.lt != null)
            {
                JObject lt = obj.lt;
                foreach (var m in lt)
                {
                    sql.Where(m.Key + ">='" + m.Value + "'");
                }
            }
            List<Test_Main> list = null;
            if (obj.pageSize != null && obj.pageIndex != null)
            {
                list = sql.Query((obj.pageIndex - 1) * obj.pageSize, obj.pageSize);
            }
            else
            {
                list = sql.Query();
            }
            return Result.OK(list);
        }
    }
}
