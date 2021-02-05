using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DbOrm;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Wind.UI.Controllers
{
    public class CommonController : Controller
    {
        //_hostEnvironment.WebRootPath;//wwwroot文件夹的绝对路径
        //_hostEnvironment.ContentRootPath;//当前项目的绝对路径
        private readonly IWebHostEnvironment _hostEnvironment;
        public CommonController(IWebHostEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }

        //文件上传，可以接收多个上传的文件
        public async Task<IActionResult> Upload()
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
            return Ok(filePaths.ToString().TrimEnd('|'));
        }

        //下载文件，若要浏览器打开文件可以用<a>标签连接文件地址（但是有的浏览器还是会下载而不是打开）
        public IActionResult Download(string url)
        {
            return File(new FileStream(_hostEnvironment.WebRootPath + url, FileMode.Open), "application/octet-stream", url.Substring(url.LastIndexOf('/') + 1));
        }

        //验证码
        public IActionResult VerifyCode()
        {
            string code = Common.VerifyCode.RandomStr(4);//获取随机数
            HttpContext.Session.SetString("VerifyCode", code);//验证码保存到session
            return File(Common.VerifyCode.CreateImage(code), "image/Jpeg");//生成图片并输出到客户端
        }

        //登录（不用于WebApi，WebApi登录另写）
        public IActionResult Login(string userName, string userPwd, string verifyCode)
        {
            //判断验证码
            string key = Request.HttpContext.Connection.RemoteIpAddress + "_LoginCount";
            int count = Convert.ToInt32(Base.GetCache(key)) + 1;
            Base.SetCache(key, count);
            if (count > 5)//第六次开始验证验证码
            {
                if (verifyCode.ToUpper() != HttpContext.Session.GetString("VerifyCode"))
                {
                    return Content("-2");
                }
            }
            //判断用户名密码
            List<dynamic> list = DB.Query("select * from Sys_Admin where UserName=@UserName and UserPwd=@UserPwd", new { UserName = userName, UserPwd = Base.Encry(userPwd) });
            if (list.Count > 0)
            {
                dynamic model = list[0];
                //存储用户信息（存储到cookie）
                var claims = new List<Claim>(){
                    new Claim(ClaimTypes.Name, model.UserName),
                    //new Claim("FullName", "bbb"),
                    //new Claim(ClaimTypes.Role, "Administrator"),
                };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                //发放登录凭证
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                //往 HttpContext.User.Claims 里面添加值
                //HttpContext.User.AddIdentity(new ClaimsIdentity(new List<Claim>() { new Claim("qwe", "ffffffff") }));
                //获取用户信息
                //string name = HttpContext.User.Claims.First(s => s.Type == "qwe").Value;

                Base.RemoveCache(key);
                return Content("1");
            }
            else
            {
                return Content(count > 4 ? "-3" : "-1");
            }
        }
    }
}
