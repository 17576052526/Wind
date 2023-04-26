using DbOrm;
using Microsoft.AspNetCore.Rewrite;

namespace UI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //读取 appsettings.json 配置文件
            DB.ConnString = builder.Configuration.GetValue<string>("ConnString");

            //AddMvcCore(); //最基础的核心服务
            //AddControllers(); //WebApi模式用这个，包含 AddMvcCore()
            //AddControllersWithViews(); //标准MVC模式用这个，包含 AddControllers()
            //AddRazorPages(); //Page页面用这个，包含 AddMvcCore()
            //AddMvc();//既有MVC又有Page页面用这个，包含 AddControllersWithViews() 及 AddRazorPages() 功能。功能最全
            builder.Services.AddControllersWithViews();//+ Mvc 服务注册
            builder.Services.AddControllers()//+ WebApi 服务注册
                .AddNewtonsoftJson(s =>//需要安装 Microsoft.AspNetCore.Mvc.NewtonsoftJson  6+版本
                {
                    s.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();//webApi 返回json原样字段返回，默认是首字母小写
                    s.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";//设置 DateTime输出格式 
                });
            builder.Services.AddRazorPages()//+ Razor页面 服务注册
                .AddRazorPagesOptions(s =>
                {
                    //配置 Razor页面能Post提交（不配置会报400），MVC和WebApi不需要此配置
                    s.Conventions.ConfigureFilter(new Microsoft.AspNetCore.Mvc.IgnoreAntiforgeryTokenAttribute());
                    //配置 Razor路由
                    //s.Conventions.AddPageRoute("/admin/index", "/");//参数一：实际地址，参数二：浏览器输入的地址  ，根据参数二的地址去匹配参数一的路径，参数一不支持通配符，参数二支持通配符
                });

            //配置身份认证
            builder.Services.AddAuthentication(Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(config =>
            {
                //config.Cookie.HttpOnly = true; //设置之后js不能通过脚本读取cookie
                //config.ExpireTimeSpan = TimeSpan.FromMinutes(20);//设置过期时间
                config.Cookie.Name = "AdminUser";
                config.LoginPath = "/Admin/Login.html";
            });

            builder.Services.AddSession();//+ Session 服务注册

            var app = builder.Build();

            //配置错误页面，（开发环境下不起效，发布后生效）
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler(configure =>
                {
                    configure.Run(async context =>
                    {
                        var exHeader = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerPathFeature>();
                        var ex = exHeader.Error;
                        if (ex != default)
                        {
                            //写入错误日志
                            File.AppendAllText(System.AppDomain.CurrentDomain.BaseDirectory + "Error.txt", DateTime.Now.ToString("yyyy-MM-dd HH:mm ") + ex.ToString() + "\r\n\r\n");
                            //跳转到错误页面
                            //context.Response.Redirect("/Error/500.html");//此句可以注释
                            await context.Response.WriteAsJsonAsync(new { code = "500", msg = "服务器内部错误，请查阅错误日志定位错误" });//此句一定要
                        }
                    });
                });
                app.UseStatusCodePagesWithReExecute("/Error/404.html");//404错误页面配置
                //app.UseRewriter(new RewriteOptions().AddRedirectToHttpsPermanent());//http重定向到https
            }

            app.UseRouting();//+ 此句代码作用未知，但其他示例中都有

            app.UseStaticFiles();//+ 配置 wwwroot静态资源目录
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

            app.UseAuthentication();//+ 认证
            app.UseAuthorization();//+ 授权

            app.UseSession();//+ 配置可以使用Session
            app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");//+ Mvc 路由注册
            app.MapControllers();//+ webApi 路由注册
            app.MapRazorPages();//+ Razor页面 路由注册

            //重定向，例如：www.aa.com/ 重定向到 www.aa.com/admin
            //app.MapGet("/", async context =>
            //{
            //    context.Response.Redirect("/admin");
            //    await context.Response.WriteAsync(String.Empty);
            //});

            app.MapFallbackToFile("index.html");//设置起始页为 wwwroot里面的 index.html

            app.Run();
        }
    }
}