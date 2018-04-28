using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MUDService.Models;
using Microsoft.EntityFrameworkCore;
using MUDService.Helpers;
using MUDService.Hubs;
using MUDService.DataAccess;
using Microsoft.AspNetCore.SignalR;
using Serilog;

namespace MUDService
{
    public class Startup
    {
        public Startup(IHostingEnvironment env, IConfiguration configuration)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
            Configuration = configuration;
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging(loggingBuilder =>
          loggingBuilder.AddSerilog(dispose: true));

            services.AddDbContextPool<MUDContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("MUDContext")));

            services.AddSignalR();
            // Add framework services.
            services.AddMvc();
            services.AddRouting();

            services.AddScoped<IReplicationHelper, ReplicationHelper>();
            services.AddScoped<IMUDDataAccess, MUDDataAccess>();
            services.AddScoped<IMUDHelper, MUDHelper>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddSerilog();
            
            if (env.IsDevelopment())
            {
                loggerFactory.AddDebug();

                loggerFactory.AddFile("log.txt");
            }
            else
            {
                loggerFactory.AddDebug();

                loggerFactory.AddFile("log.txt");
            }

            //if (env.IsDevelopment())
            //{

            //}

            app.UseSignalR(routes =>
            {
                routes.MapHub<MUDHub>("MUDHub");
            });

            app.UseMvc();
        }
    }
}
