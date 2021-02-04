using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(JobWebsiteMVC.Areas.Identity.IdentityHostingStartup))]

namespace JobWebsiteMVC.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}