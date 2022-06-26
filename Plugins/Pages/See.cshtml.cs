using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Plugins.Pages
{
    public class Data
    {
        public string Name { set; get; }
        public string Preview { set; get; }
        public List<string> See { set; get; }


        public static string WebRootPath;//wwwroot的路径
        private static Dictionary<string, Data> list;//配置信息，key 数据的唯一标识
        public static Dictionary<string, Data> List
        {
            get
            {
                if (list == null)
                {
                    list = new Dictionary<string, Data>(StringComparer.OrdinalIgnoreCase);
                    //解析xml
                    XmlDocument doc = new XmlDocument();
                    doc.Load(WebRootPath + "/DataBase.xml");
                    foreach (XmlNode m in doc.DocumentElement.ChildNodes)
                    {
                        var nodeSee = m.SelectNodes("see");
                        var preview = m.SelectSingleNode("preview");
                        list.Add(m.SelectSingleNode("id").InnerText, new Data()
                        {
                            Name = m.SelectSingleNode("name").InnerText,
                            Preview = preview == null ? null : preview.InnerText,
                            See = Enumerable.Range(0, nodeSee.Count).Select(s => nodeSee[s].InnerText).ToList()
                        });
                    }
                }
                return list;
            }
        }
    }
    public class SeeModel : PageModel
    {
        //_hostEnvironment.WebRootPath;//wwwroot文件夹的绝对路径
        //_hostEnvironment.ContentRootPath;//当前项目的绝对路径
        public SeeModel(IWebHostEnvironment hostEnvironment)
        {
            if (Data.WebRootPath == null)
            {
                Data.WebRootPath = hostEnvironment.WebRootPath;
            }
        }


        public Data DataModel;
        public void OnGet(string id)
        {
            DataModel = Data.List[id];
            ViewData["Title"] = DataModel.Name;
        }
    }
}
