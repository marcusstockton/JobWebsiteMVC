using JobWebsiteMVC.Data;
using JobWebsiteMVC.Hubs;
using JobWebsiteMVC.Interfaces;
using JobWebsiteMVC.Models;
using JobWebsiteMVC.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;

namespace JobWebsiteMVC
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            services.AddDatabaseDeveloperPageExceptionFilter();

            services.AddIdentity<ApplicationUser, IdentityRole>()
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddDefaultUI()
                    .AddDefaultTokenProviders();

            services.AddAutoMapper(typeof(Startup));

            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("JobSeekerOnly", policy => policy.RequireClaim("JobType.Description", "Job Seeker"));
                options.AddPolicy("JobOwnerOnly", policy => policy.RequireClaim("JobType.Description", "Job Owner"));
                options.AddPolicy("AdminOnly", policy => policy.RequireClaim("JobType.Description", "Admin"));
            });

            // Register DI
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IJobService, JobService>();
            services.AddScoped<IJobTypesService, JobTypesService>();
            services.AddScoped<IJobBenefitsService, JobBenefitsService>();
            services.AddScoped<IAttachmentService, AttachmentService>();

            services.AddTransient<IEmailSender, EmailService>();
            services.AddTransient<DataSeeder>();

            services.AddSignalR();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DataSeeder seeder)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                seeder.SeedDatabase(false).Wait();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Uploads")),
                RequestPath = "/Uploads"
            });

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();

                endpoints.MapHub<NotificationsHub>("/notificationHub");
            });
        }
    }
}