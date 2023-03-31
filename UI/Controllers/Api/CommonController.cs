using DbOrm;
using DbOrm.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Text;

namespace UI.Controllers.Api
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        //_hostEnvironment.WebRootPath;//wwwroot文件夹的绝对路径
        //_hostEnvironment.ContentRootPath;//当前项目的绝对路径
        private readonly IWebHostEnvironment _hostEnvironment;
        public CommonController(IWebHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }

        //文件上传，可以接收多个上传的文件
        public async Task<Result> Upload()
        {
            string path = _hostEnvironment.WebRootPath;//wwwroot文件夹的绝对路径
            //构造保存目录
            string direct = "/upload/" + DateTime.Now.ToString("yyyyMMdd/");
            if (!System.IO.Directory.Exists(path + direct)) { System.IO.Directory.CreateDirectory(path + direct); } //不存在目录就创建目录
            //遍历文件
            StringBuilder filePaths = new StringBuilder();
            foreach (var item in Request.Form.Files)
            {
                string fileExt = item.FileName.Substring(item.FileName.LastIndexOf('.')); //文件后缀名
                string newFileName = Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds).ToString() + (new Random().Next(100000, 1000000)) + fileExt;  //构造文件名，规则：时间戳+随机数6位
                using (var stream = new FileStream(path + direct + newFileName, FileMode.Create))
                {
                    await item.CopyToAsync(stream);
                }
                filePaths.Append(direct + newFileName + '|');
            }
            return Result.OK(filePaths.ToString().TrimEnd('|'));
        }

        [HttpPost]
        public Result Login(object param)
        {
            dynamic jObj = JObject.Parse(param.ToString());
            string userName = jObj.userName;
            string userPwd = jObj.userPwd;
            string verifyCode = jObj.verifyCode;
            //验证验证码
            int errorCount = Convert.ToInt32(Base.GetCache("LoginCount" + userName));
            if (errorCount >= 3)
            {
                if (verifyCode == null || verifyCode.Length == 0 || verifyCode.ToUpper() != (string)Base.GetCache("VerifyCode" + HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString()))
                {
                    return Result.Fail("验证码错误", -2);
                }
            }
            //验证用户名密码
            List<Sys_Admin> list = DB.Querys<Sys_Admin>("select * from Sys_Admin where UserName=@UserName and UserPwd=@UserPwd", new { UserName = userName, UserPwd = userPwd == null ? "" : Base.Encry(userPwd) });
            if (list.Count > 0)
            {
                Sys_Admin admin = list[0];
                string token = JWT.Signature(new
                {
                    ID = admin.ID,
                    UserName = admin.UserName
                });
                return Result.OK(token);
            }
            else
            {
                errorCount = errorCount + 1;
                Base.SetCache("LoginCount" + userName, errorCount, 5, false);
                if (errorCount >= 3)
                {
                    return Result.Fail("用户名或密码错误", -2);
                }
                return Result.Fail("用户名或密码错误");
            }
        }

        public FileResult VerifyCode()
        {
            string code = Common.VerifyCode.RandomStr(4);//获取随机数
            Base.SetCache("VerifyCode" + HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString(), code, 5, false);//验证码存储起来
            return File(Common.VerifyCode.CreateImage(code), "image/Jpeg");//生成图片并输出到客户端
        }

        //注销
        public Result Cancel()
        {
            JWT.Cancel(Request.Headers["Authorization"]);
            return Result.OK();
        }
    }
}
