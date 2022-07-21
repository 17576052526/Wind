using DbOrm;

namespace UI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //��ȡ appsettings.json �����ļ�
            DB.ConnString = builder.Configuration.GetValue<string>("ConnString");

            //AddMvcCore(); //������ĺ��ķ���
            //AddControllers(); //WebApiģʽ����������� AddMvcCore()
            //AddControllersWithViews(); //��׼MVCģʽ����������� AddControllers()
            //AddRazorPages(); //Pageҳ������������� AddMvcCore()
            //AddMvc();//����MVC����Pageҳ������������� AddControllersWithViews() �� AddRazorPages() ���ܡ�������ȫ
            builder.Services.AddControllersWithViews();//+ Mvc ����ע��
            builder.Services.AddControllers()//+ WebApi ����ע��
                .AddJsonOptions(p => { p.JsonSerializerOptions.PropertyNamingPolicy = null; });//webApi ����jsonԭ���ֶη��أ�Ĭ��������ĸСд
            builder.Services.AddRazorPages()//+ Razorҳ�� ����ע��
                .AddRazorPagesOptions(s =>
                {
                    //���� Razorҳ����Post�ύ�������ûᱨ400����MVC��WebApi����Ҫ������
                    s.Conventions.ConfigureFilter(new Microsoft.AspNetCore.Mvc.IgnoreAntiforgeryTokenAttribute());
                    //���� Razor·��
                    //s.Conventions.AddPageRoute("/admin/index", "/");//����һ��ʵ�ʵ�ַ�������������������ĵ�ַ  �����ݲ������ĵ�ַȥƥ�����һ��·��������һ��֧��ͨ�����������֧��ͨ���
                });

            //����������֤
            builder.Services.AddAuthentication(Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(config =>
            {
                //config.Cookie.HttpOnly = true; //����֮��js����ͨ���ű���ȡcookie
                //config.ExpireTimeSpan = TimeSpan.FromMinutes(20);//���ù���ʱ��
                config.Cookie.Name = "AdminUser";
                config.LoginPath = "/Admin/Login.html";
            });

            builder.Services.AddSession();//+ Session ����ע��

            var app = builder.Build();

            //���ô���ҳ�棬�����������²���Ч����������Ч��
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error/500.html");//500����ҳ������
                app.UseStatusCodePagesWithReExecute("/Error/404.html");//404����ҳ������
            }

            app.UseRouting();//+ �˾��������δ֪��������ʾ���ж���

            app.UseStaticFiles();//+ ���� wwwroot��̬��ԴĿ¼
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

            app.UseAuthentication();//+ ��֤
            app.UseAuthorization();//+ ��Ȩ

            app.UseSession();//+ ���ÿ���ʹ��Session
            app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");//+ Mvc ·��ע��
            app.MapControllers();//+ webApi ·��ע��
            app.MapRazorPages();//+ Razorҳ�� ·��ע��

            //�ض������磺www.aa.com/ �ض��� www.aa.com/admin
            //app.MapGet("/", async context =>
            //{
            //    context.Response.Redirect("/admin");
            //    await context.Response.WriteAsync(String.Empty);
            //});

            //app.MapFallbackToFile("index.html");//����.net core react��Ŀʱ��������ʼҳΪ react����� index.html

            app.Run();
        }
    }
}