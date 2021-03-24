using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plugins
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            //��ȡ appsettings.json �����ļ�
            //DB.ConnString = configuration.GetValue<string>("ConnString");
        }

        public void ConfigureServices(IServiceCollection services)
        {
            //AddMvcCore(); //������ĺ��ķ���
            //AddControllers(); //WebApiģʽ����������� AddMvcCore()
            //AddControllersWithViews(); //��׼MVCģʽ����������� AddControllers()
            //AddRazorPages(); //Pageҳ������������� AddMvcCore()
            //AddMvc();//����MVC����Pageҳ������������� AddControllersWithViews() �� AddRazorPages() ���ܡ�������ȫ

            services.AddMvc()
            //���� Razorҳ����Post�ύ�������ûᱨ400����MVC��WebApi����Ҫ������
            .AddRazorPagesOptions(s =>
            {
                s.Conventions.ConfigureFilter(new Microsoft.AspNetCore.Mvc.IgnoreAntiforgeryTokenAttribute());
            })
            //webApi ����jsonԭ���ֶη��أ�Ĭ��������ĸСд
            .AddJsonOptions(p => { p.JsonSerializerOptions.PropertyNamingPolicy = null; });

            //���������֤
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(config =>
            {
                //config.Cookie.HttpOnly = true; //����֮��js����ͨ���ű���ȡcookie
                //config.ExpireTimeSpan = TimeSpan.FromMinutes(20);//���ù���ʱ��
                config.Cookie.Name = "AdminUser";
                config.LoginPath = "/Admin/Login.html";
            });

            //������ʹ�� Session
            services.AddSession();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //����ҳ������
            if (env.IsDevelopment())//if (env.IsDevelopment()) �����ж��ǿ����������Ƿ�������
            {
                app.UseDeveloperExceptionPage();//ҳ��ֱ����ʾ�쳣��Ϣ
            }
            else
            {
                app.UseExceptionHandler("/Error/500.html");//500����ҳ������
                app.UseStatusCodePagesWithReExecute("/Error/404.html");//404����ҳ������

                //app.UseRewriter(new RewriteOptions().AddRedirectToHttpsPermanent());//http�ض���https
            }

            app.UseRouting();

            //wwwroot Ŀ¼���ã���̬��ԴĿ¼��
            app.UseStaticFiles();
            //��Ŀ¼�����о�̬��Դ�����Է���
            //app.UseStaticFiles(new StaticFileOptions()
            //{
            //    FileProvider = new PhysicalFileProvider(Directory.GetCurrentDirectory())
            //});
            //����Areas Ŀ¼�����о�̬��Դ�����Է���
            //app.UseStaticFiles(new StaticFileOptions()
            //{
            //    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(),"Areas")),
            //    RequestPath=new PathString(value:"/Areas")
            //});
            //����Areas Ŀ¼�����о�̬��Դ�����Է��ʣ��Ҳ���Ҫ����/Areas�����磺���������/admin/css/main.css �ᶨλ����Ŀ�е� /Areas/admin/css/main.css
            //app.UseStaticFiles(new StaticFileOptions()
            //{
            //    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Areas/admin")),
            //    RequestPath = new PathString(value: "/admin")
            //});

            //���������֤������Ҫд��app.UseRouting();֮��app.UseEndpoints() ֮ǰ
            app.UseAuthentication();
            app.UseAuthorization();
            //������ʹ�� Session������Ҫд��app.UseEndpoints() ֮ǰ
            app.UseSession();
            //���� ·��
            app.UseEndpoints(endpoints =>
            {
                //����Razorҳ��·��
                endpoints.MapRazorPages();
                //���� MVC·��
                endpoints.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");
                //���� webApi·��
                endpoints.MapControllers();
            });
        }
    }
}
