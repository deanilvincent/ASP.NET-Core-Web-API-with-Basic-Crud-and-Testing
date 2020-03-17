using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WebApi.Contracts;
using WebApi.Data;
using WebApi.Repositories;

namespace WebApi
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
            services.AddControllers();
            // postgresql
            // services.AddDbContext<AppDbContext>(options => options.UseNpgsql(@"Server=127.0.0.1;Database=BasicCrudAppDb;User id=postgres;Password=H3yyyY0(_)1234;Integrated Security=false;"));
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(@"Server=tcp:mvsandboxserveraccenture.database.windows.net,1433;Initial Catalog=testdb01;Persist Security Info=False;User ID=ssa;Password=JejeM0nster;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"));

            // repos
            services.AddTransient<ICustomerRepo, CustomerRepo>();

            // allow cors
            services.AddCors(options => options.AddPolicy("CorsPolicy", policy =>
            {
                policy.AllowCredentials().AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:8080");
            }));
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

            app.UseCors("CorsPolicy");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
