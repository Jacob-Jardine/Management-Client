using Management_Web_Application.Services.PurchaseService;
using Management_Web_Application.Services.StaffService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Management_Web_Application
{
    public class Startup
    {
        private IWebHostEnvironment _env = null;

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            //services.AddSingleton<IPurchaseRequestService, PurchaseRequestService>();
            services.AddSingleton<IStaffService, FakeStaffService>();
            
            //services.AddHttpClient<IStaffService, StaffService>();
            services.AddHttpClient<IPurchaseRequestService, PurchaseRequestService>();
            services.AddRazorPages().AddRazorRuntimeCompilation();
            //if (_env.IsDevelopment())
            //{
            //    services.AddSingleton<IStaffService, FakeStaffService>();
            //    services.AddRazorPages().AddRazorRuntimeCompilation();
            //    //services.AddHttpClient<IStaffService, StaffService>();
            //}
            //else
            //{
            //    services.AddHttpClient<IStaffService, StaffService>();
            //}
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
