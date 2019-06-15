using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NGODP.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;
using NGODP.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using NGODP.Hubs;
using DinkToPdf;
using DinkToPdf.Contracts;

namespace NGODP
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // services.Configure<CookiePolicyOptions>(options =>
            // {
            //     // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            //     //options.CheckConsentNeeded = context => true;
            //     //options.MinimumSameSitePolicy = SameSiteMode.None;
            // });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.CookieName = "ngodp";
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout";
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            var connection = Configuration.GetConnectionString("Default");
            services.AddDbContext<ngodpContext>(options => options.UseSqlite(connection));

            services.AddSingleton<IGenerator,Generator>();

            services.AddSingleton<ISmsSender,SmsSender>();

            // Add converter to DI
            services.AddSingleton(typeof(IConverter), new BasicConverter(new PdfTools()));

            services.AddSignalR();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            //app.UseCookiePolicy();

            app.UseAuthentication();   

            app.UseSignalR(routes => {

                routes.MapHub<ChatHub>("/chatHub");
            });     

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Account}/{action=Login}/{id?}");

                routes.MapRoute(
                    name: "second",
                    template: "{controller=Account}/{action=Home}/{id?}");
            });
        }
    }
}