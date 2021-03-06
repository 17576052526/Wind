using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DbOrm;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite;
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
            DB.ConnString = configuration.GetValue<string>("ConnString");
        }

        public void ConfigureServices(IServiceCollection services)
        {
            //AddMvcCore(); //最基础的核心服务
            //AddControllers(); //WebApi模式用这个，包含 AddMvcCore()
            //AddControllersWithViews(); //标准MVC模式用这个，包含 AddControllers()
            //AddRazorPages(); //Page页面用这个，包含 AddMvcCore()
            //AddMvc();//既有MVC又有Page页面用这个，包含 AddControllersWithViews() 及 AddRazorPages() 功能。功能最全

            services.AddMvc()
            .AddRazorPagesOptions(s =>
            {
                //配置 Razor页面能Post提交（不配置会报400），MVC和WebApi不需要此配置
                s.Conventions.ConfigureFilter(new Microsoft.AspNetCore.Mvc.IgnoreAntiforgeryTokenAttribute());
                //配置 Razor路由
                //s.Conventions.AddPageRoute("/admin/index", "/");//参数一：实际地址，参数二：浏览器输入的地址  ，根据参数二的地址去匹配参数一的路径，参数一不支持通配符，参数二支持通配符
            })
            //webApi 返回json原样字段返回，默认是首字母小写
            .AddJsonOptions(p => { p.JsonSerializerOptions.PropertyNamingPolicy = null; });

            //配置身份认证
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(config =>
            {
                //config.Cookie.HttpOnly = true; //设置之后js不能通过脚本读取cookie
                //config.ExpireTimeSpan = TimeSpan.FromMinutes(20);//设置过期时间
                config.Cookie.Name = "AdminUser";
                config.LoginPath = "/Admin/Login.html";
            });

            //配置能使用 Session
            services.AddSession();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //错误页面配置
            if (env.IsDevelopment())//if (env.IsDevelopment()) 用来判断是开发环境还是发布环境
            {
                app.UseDeveloperExceptionPage();//页面直接显示异常信息
            }
            else
            {
                app.UseExceptionHandler("/Error/500.html");//500错误页面配置
                app.UseStatusCodePagesWithReExecute("/Error/404.html");//404错误页面配置

                //app.UseRewriter(new RewriteOptions().AddRedirectToHttpsPermanent());//http重定向到https
            }

            app.UseRouting();

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

            //配置身份认证，必须要写在app.UseRouting();之后，app.UseEndpoints() 之前
            app.UseAuthentication();
            app.UseAuthorization();
            //配置能使用 Session，必须要写在app.UseEndpoints() 之前
            app.UseSession();
            //配置 路由
            app.UseEndpoints(endpoints =>
            {
                //配置Razor页面路由
                endpoints.MapRazorPages();
                //配置 MVC路由
                endpoints.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
                //配置 webApi路由
                endpoints.MapControllers();
                //设置地址重定向，例如：浏览器输入 aaa.com 重定向到 aaa.com/admin
                endpoints.MapGet("/", async context =>
                {
                    context.Response.Redirect("/admin");
                    await context.Response.WriteAsync(String.Empty);
                });
            });
        }
    }
}
