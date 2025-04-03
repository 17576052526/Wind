using DbOrm;
using DbOrm.Model;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace UI.Controllers.Api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [JWTAuthorize]
    public class Sys_TypeController : ControllerBase
    {
        [HttpPost]
        public Result SelectAll()
        {
            using (DB db = new DB())
            {
                var sql = db.Select<Sys_Type>("*");
                return Result.OK(sql.Query());
            }
        }
    }
}
