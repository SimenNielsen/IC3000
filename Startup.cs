using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using IC3000.Context;
using Microsoft.EntityFrameworkCore;
using System;
using IC3000.Service;

namespace IC3000
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// Register context services to be available as dependency injection in Controllers.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ClaimContext>(options => options.UseCosmos(
                Environment.GetEnvironmentVariable("CUSTOMCONNSTR_COSMOS_DB"),
                Environment.GetEnvironmentVariable("COSMOS_DATABASE_NAME")
            ));
            services.Add(new ServiceDescriptor(typeof(QueueService), new QueueService(Environment.GetEnvironmentVariable("CUSTOMCONNSTR_SERVICE_BUS"))));
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
