using System.IO;
using JobWebsiteMVC.Data;
using JobWebsiteMVC.Interfaces;
using JobWebsiteMVC.Models;
using JobWebsiteMVC.Profiles;
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

namespace JobWebsiteMVC
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
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(
                    Configuration.GetConnectionString("DefaultConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddDefaultUI()
                    .AddDefaultTokenProviders();

            services.AddAutoMapper(typeof(JobProfile));

            services.AddControllersWithViews();
            services.AddRazorPages();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("JobSeekerOnly", policy => policy.RequireClaim("JobType.Description", "Job Seeker"));
                options.AddPolicy("JobOwnerOnly", policy => policy.RequireClaim("JobType.Description", "Job Owner"));
                options.AddPolicy("AdminOnly", policy => policy.RequireClaim("JobType.Description", "Admin"));
            });

            // Register DI
            services.AddTransient<IEmailSender, EmailService>();
            services.AddScoped<IJobService, JobService>();
            services.AddScoped<IJobTypesService, JobTypesService>();
            services.AddScoped<IJobBenefitsService, JobBenefitsService>();
            services.AddScoped<IUserTypesService, UserTypesService>();
            services.AddScoped<IAttachmentService, AttachmentService>();

            services.AddTransient<DataSeeder>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, DataSeeder seeder)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                seeder.SeedDatabase();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
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
            });
        }
    }
}