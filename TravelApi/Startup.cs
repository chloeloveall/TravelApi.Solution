using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using TravelApi.Models;
using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.Mvc.Versioning;
// using Microsoft.AspNetCore.Mvc.ApplicationModels;
// using System.Collections.Generic;  
// using System.Linq;  

namespace TravelApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // public void ConfigureServices(IServiceCollection services)
        // {

        //     services.AddDbContext<TravelApiContext>(opt =>
        //         opt.UseMySql(Configuration["ConnectionStrings:DefaultConnection"], ServerVersion.AutoDetect(Configuration["ConnectionStrings:DefaultConnection"])));
        //     services.AddControllers();
        // }

        public void ConfigureServices(IServiceCollection services)
        {
          services.AddControllers();
          services.AddMvc(); 
          services.AddDbContext<TravelApiContext>(opt =>
            opt.UseMySql(Configuration["ConnectionStrings:DefaultConnection"], ServerVersion.AutoDetect(Configuration["ConnectionStrings:DefaultConnection"])));
          services.AddApiVersioning(o => {
            o.ReportApiVersions = true;
            o.AssumeDefaultVersionWhenUnspecified = true;
            o.DefaultApiVersion = new ApiVersion(1, 0);
          });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}