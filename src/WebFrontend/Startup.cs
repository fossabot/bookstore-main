﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebFrontend.Data;

namespace WebFrontend
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
            services.AddControllersWithViews();
            services.AddRazorPages();


            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                options.SlidingExpiration = true;
            });

            services.AddDbContext<WebFrontendContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("WebFrontendContext")));

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
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseSecurityHeaders(policies =>
            {
                policies
                    .AddFrameOptionsDeny()
                    .AddXssProtectionBlock()
                    .AddContentTypeOptionsNoSniff()
                    .AddReferrerPolicyNoReferrer()
                    .RemoveServerHeader()
                    .AddContentSecurityPolicy(configure =>
                    {
                        configure.AddBaseUri().Self();
                        configure.AddObjectSrc().None();
                        configure.AddFormAction().Self();
                        configure.AddFrameAncestors().None();
                        configure.AddUpgradeInsecureRequests();
                    })
                    .AddFeaturePolicy(configure =>
                    {
                        configure.AddAccelerometer().None();
                        configure.AddAmbientLightSensor().None();
                        configure.AddAutoplay().None();
                        configure.AddCustomFeature("battery").None();
                        configure.AddCamera().None();
                        configure.AddCustomFeature("display-capture").None();
                        configure.AddCustomFeature("document-domain").None();
                        configure.AddEncryptedMedia().None();
                        configure.AddCustomFeature("execution-while-not-rendered").None();
                        configure.AddCustomFeature("execution-while-out-of-viewport").None();
                        configure.AddFullscreen().None();
                        configure.AddGeolocation().None();
                        configure.AddGyroscope().None();
                        configure.AddCustomFeature("layout-animations").None();
                        configure.AddCustomFeature("legacy-image-formats").None();
                        configure.AddMagnetometer().None();
                        configure.AddMicrophone().None();
                        configure.AddMidi().None();
                        configure.AddCustomFeature("oversize-images").None();
                        configure.AddPayment().None();
                        configure.AddPictureInPicture().None();
                        configure.AddCustomFeature("publickey-credentials-get").None();
                        configure.AddSyncXHR().None();
                        configure.AddCustomFeature("unoptimized-images").None();
                        configure.AddCustomFeature("unsized-media").None();
                        configure.AddUsb().None();
                        configure.AddCustomFeature("wake-lock").None();
                        configure.AddCustomFeature("xr-spatial-tracking").None();
                    });
            });

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapRazorPages();
            });
        }
    }
}
