using System.Collections.Generic;
using System.Globalization;
using Business.Abstract;
using Business.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.DependencyInjection;
using WebUI.Services;

namespace WebUI
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //Localization
            services.AddLocalization(options => options.ResourcesPath = "Resources");
            //MVC and ViewLocalization
            services.AddMvc()
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddViewLocalization(LanguageViewLocationExpanderFormat.SubFolder);
            services.AddTransient<CustomLocalizer>();
            //Session
            services.AddSession();
            services.AddDistributedMemoryCache();

            //Cookie
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
            //Layer of business
            services.AddScoped<IPaperNotifyService, PaperNotifyManager>();
            services.AddScoped<IMenuService, MenuManager>();
            services.AddScoped<IGenelService, GenelManager>();
            services.AddScoped<IMesajService, MesajManager>();
            services.AddScoped<IBildiriService, BildiriManager>();
            services.AddScoped<IEkService, EkManager>();
            services.AddScoped<IHakemBildiriAtamaService, HakemBildiriAtamaManager>();
            services.AddScoped<IKonuEtiketiService, KonuEtiketiManager>();
            services.AddScoped<ISehirService, SehirManager>();
            services.AddScoped<IUlkeService, UlkeManager>();
            services.AddScoped<IUyeService, UyeManager>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<ISessionService, SessionService>();
            //Layer of data access
            services.AddScoped<IPaperNotifyDal, EfPaperNotifyDal>();
            services.AddScoped<IMenuDal, EfMenuDal>();
            services.AddScoped<IGenelDal, EfGenelDal>();
            services.AddScoped<IMesajDal, EfMesajDal>();
            services.AddScoped<IBildiriDal, EfBildiriDal>();
            services.AddScoped<IEkDal, EfEkDal>();
            services.AddScoped<IHakemBildiriAtamaDal, EfHakemBildiriAtamaDal>();
            services.AddScoped<IKonuEtiketiDal, EfKonuEtiketiDal>();
            services.AddScoped<ISehirDal, EfSehirDal>();
            services.AddScoped<IUlkeDal, EfUlkeDal>();
            services.AddScoped<IUyeDal, EfUyeDal>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ISessionService session)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                //app.UseExceptionHandler("/Home/ErrorPage");
            }
            app.UseStaticFiles();
            //Localization
            var supportedCultures = new List<CultureInfo>
            {
                new CultureInfo("en"),
                new CultureInfo("tr")
            };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("tr"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });
            app.UseSession();
            app.UseAuthentication();
            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}");
                routes.MapRoute("areaRoute", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
