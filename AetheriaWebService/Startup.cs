using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using AetheriaWebService.Models;
using Microsoft.EntityFrameworkCore;
using AetheriaWebService.Helpers;
using AetheriaWebService.Hubs;
using AetheriaWebService.DataAccess;
using Microsoft.AspNetCore.SignalR;

namespace AetheriaWebService
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
            services.AddDbContextPool<AetheriaContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("AetheriaContext")));

            services.AddSignalR();
            // Add framework services.
            services.AddMvc();


            services.AddScoped<IReplicationHelper, ReplicationHelper>();
            services.AddScoped<IAetheriaDataAccess, AetheriaDataAccess>();
            services.AddScoped<IAetheriaHelper, AetheriaHelper>();
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
                routes.MapHub<AetheriaHub>("AetheriaHub");
            });

            app.UseMvc();
        }
    }
}
