using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MUDService.DataAccess;
using MUDService.Helpers;
using MUDService.Hubs;
using MUDService.Logic;
using MUDService.Models;

namespace MUDService
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
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

            services.AddScoped<IReplicationLogic, ReplicationLogic>();
            services.AddScoped<ICellLogic, CellLogic>();
            services.AddScoped<IMUDDataAccess, MUDDataAccess>();
            services.AddScoped<IMUDHelper, MUDHelper>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<MUDHub>("/MUDHub");
                endpoints.MapControllers();
            });
        }
    }
}
