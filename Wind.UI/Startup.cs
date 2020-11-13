using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

namespace Wind.UI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            //读取 appsettings.json 配置文件
            DbHelper.ConnString = configuration.GetValue<string>("ConnString");
        }

        public void ConfigureServices(IServiceCollection services)
        {
            //Razor页面、Mvc视图需要此配置
            services.AddRazorPages();
            //配置 Razor页面配置能Post提交（不配置会报400），MVC和WebApi不需要配置
            services.AddMvc().AddRazorPagesOptions(s =>
            {
                s.Conventions.ConfigureFilter(new Microsoft.AspNetCore.Mvc.IgnoreAntiforgeryTokenAttribute());
            });
            //配置控制器 WebApi需要此配置
            services.AddControllers();
            //webApi 返回json默认是首字母小写，加此代码返回原样字段
            services.AddControllersWithViews().AddJsonOptions(p => { p.JsonSerializerOptions.PropertyNamingPolicy = null; });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //------------默认配置-------------//
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            //------------自定义配置-------------//
            //wwwroot 目录配置（静态资源目录）
            app.UseStaticFiles();
            //根目录下所有静态资源都可以访问
            //app.UseStaticFiles(new StaticFileOptions()
            //{
            //    FileProvider = new PhysicalFileProvider(Directory.GetCurrentDirectory())
            //});
            //配置Areas 目录下所有静态资源都可以访问
            //app.UseStaticFiles(new StaticFileOptions()
            //{
            //    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(),"Areas")),
            //    RequestPath=new PathString(value:"/Areas")
            //});
            //配置Areas 目录下所有静态资源都可以访问，且不需要输入/Areas，例如：浏览器输入/admin/css/main.css 会定位到项目中的 /Areas/admin/css/main.css
            //app.UseStaticFiles(new StaticFileOptions()
            //{
            //    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Areas/admin")),
            //    RequestPath = new PathString(value: "/admin")
            //});
            //配置 MVC、Razor页面、WebApi
            app.UseEndpoints(endpoints =>
            {
                //配置Razor页面路由
                endpoints.MapRazorPages();
                //配置 MVC路由
                endpoints.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
                //配置 webApi路由
                endpoints.MapControllers();
            });
        }
    }
}
