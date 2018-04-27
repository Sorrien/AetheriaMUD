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

namespace MUDService
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
            loggerFactory.AddDebug();

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
