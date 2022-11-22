using BlogProject.Helpers;
using BlogProject.Services;
using BlogProject.Data;

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace BlogProject
{
    public class Program
    {
        public static async Task Main(string[] args)
        {

            

            //CreateHostBuilder(args).Build().Run();
            var host = CreateHostBuilder(args).Build();

            //Pull out my registered DataService
            var dataService = host.Services
                                    .CreateScope()
                                   .ServiceProvider
                                   .GetRequiredService<DataService>();

            await dataService.ManageDateAsync();

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
        
    }
}
