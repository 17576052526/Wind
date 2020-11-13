using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
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
    }
}
