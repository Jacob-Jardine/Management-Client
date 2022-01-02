using Auth0.AspNetCore.Authentication;
using Management_Web_Application.Services.Auth0Service;
using Management_Web_Application.Services.GetPurchaseRequestService;
using Management_Web_Application.Services.ProductService;
using Management_Web_Application.Services.PurchaseService;
using Management_Web_Application.Services.StaffService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Polly;
using Polly.Extensions.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

        private static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                .WaitAndRetryAsync(5, retryAttempt =>
                    TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
        }

        private static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddAuth0WebAppAuthentication(options => {
                    options.Domain = Configuration["Auth0:Domain"];
                    options.ClientId = Configuration["Auth0:ClientId"];
                    options.ClientSecret = Configuration["Auth0:ClientSecret"];
                }).WithAccessToken(options =>
                {
                    options.Audience = Configuration["Auth0:Audience"];
                });
            services.AddControllersWithViews();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddRazorPages().AddRazorRuntimeCompilation();

            if (_env.IsDevelopment()) 
            {
                //services.AddSingleton<IStaffService, FakeStaffService>();
                services.AddHttpClient<IStaffService, StaffService>();
                services.AddHttpClient<IAuth0Service, Auth0Service>();
                services.AddHttpContextAccessor();
                services.AddHttpClient<IGetPurchaseRequestService, GetPurchaseRequestService>();
                services.AddSingleton<ISendPurchaseRequestService, FakeSendPurchaseRequestService>();
                services.AddSingleton<IProductService, ProductService>();
            }
            else if(_env.IsStaging()|| _env.IsProduction())
            {
                services.AddHttpClient<IStaffService, StaffService>()
                    .AddPolicyHandler(GetRetryPolicy())
                    .AddPolicyHandler(GetCircuitBreakerPolicy());

                services.AddHttpClient<ISendPurchaseRequestService, SendPurchaseRequestService>()
                    .AddPolicyHandler(GetRetryPolicy())
                    .AddPolicyHandler(GetCircuitBreakerPolicy());

                services.AddHttpClient<IGetPurchaseRequestService, GetPurchaseRequestService>()
                    .AddPolicyHandler(GetRetryPolicy())
                    .AddPolicyHandler(GetCircuitBreakerPolicy());
            }     
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

            app.UseAuthentication();
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
