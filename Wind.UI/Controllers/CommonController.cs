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

        #region 验证码
        /// <summary>
        /// 根据字符串生成验证码图片（字节）
        /// </summary>
        private byte[] CreateImage(string TestStr)
        {
            Bitmap image = new Bitmap(TestStr.Length * 18, 25);  //设置生成图片的高度和宽度
            Graphics g = Graphics.FromImage(image);
            g.Clear(Color.White);   //白色填充
            //定义颜色
            Color[] c = { Color.Black, Color.Red, Color.DarkBlue, Color.Green, Color.Orange, Color.Brown, Color.DarkCyan, Color.Purple };
            //定义字体 
            string[] font = { "Verdana", "Microsoft Sans Serif", "Comic Sans MS", "Arial", "宋体" };
            Random rand = new Random();
            //随机输出噪点
            //for (int i = 0; i < 50; i++)
            //{
            //    int x = rand.Next(image.Width);
            //    int y = rand.Next(image.Height);
            //    g.DrawRectangle(new Pen(Color.LightGray, 0), x, y, 1, 1);
            //}
            //画噪线
            for (int i = 0; i < 4; i++)    //每次循环画一条线   x轴和Y轴指定一个点，两个点指定一条线
            {
                //随机生成噪线的起点和终点
                int x1 = rand.Next(100);
                int y1 = rand.Next(30);
                int x2 = rand.Next(100);
                int y2 = rand.Next(30);
                Color col = c[rand.Next(c.Length)];   //获取颜色
                g.DrawLine(new Pen(col), x1, y1, x2, y2);
            }
            //输出不同字体和颜色的验证码字符
            for (int i = 0; i < TestStr.Length; i++)
            {
                string fon = font[rand.Next(font.Length)];
                Color col = c[rand.Next(c.Length)];

                Font f = new Font(fon, 13, FontStyle.Italic);  //定义字体，大写，加粗
                Brush b = new SolidBrush(col);  //字体颜色
                int x = 0 + (i * 15);
                int y = i % 2 == 0 ? 2 : 0;
                g.DrawString(TestStr.Substring(i, 1), f, b, x, y);  //指定的字符绘制到指定的位置
            }

            MemoryStream ms = new MemoryStream();
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            g.Dispose();
            image.Dispose();

            return ms.ToArray();
        }

        /// <summary>
        /// 生成随机字符串， size：字符串的数量
        /// </summary>
        private string RndStr(int size)
        {
            char[] charArr = { '2', '3', '4', '5', '6', '8', '9', 'A', 'B', 'E', 'F', 'G', 'H', 'J', 'K', 'L', 'N', 'P', 'R', 'S', 'T', 'W', 'X', 'Y' };
            Random rand = new Random();
            string str = "";
            for (int i = 0; i < size; i++) { str += charArr[rand.Next(charArr.Length)]; }
            return str;
        }
        //验证码
        public IActionResult VerifyCode()
        {
            string code = RndStr(4);//获取随机数
            HttpContext.Session.SetString("VerifyCode", code);//验证码保存到session
            return File(CreateImage(code), "image/Jpeg");//生成图片并输出到客户端
        }
        #endregion

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
