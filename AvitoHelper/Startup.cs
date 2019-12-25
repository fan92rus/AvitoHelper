using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AvitoHelper.Builders;
using AvitoHelper.DataBase;
using AvitoHelper.Helpers;
using AvitoHelper.Middlwares;
using AvitoHelper.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AvitoHelper
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            this.env = env;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment env { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DatabaseContext>(options => options.UseLazyLoadingProxies().UseNpgsql("Host=postgres;Port=5432;Database=AvitoParser;Username=postgres;Password=1"));
            services.AddHostedService<AvitoParser>();
            services.AddSingleton<EmailSender>();
            services.AddSingleton<OrderModelBuilder>();
            services.AddSingleton<OrderBuilder>();
            services.AddSingleton<EmailHelper>();
            services.Configure<ConsoleLifetimeOptions>(opts => opts.SuppressStatusMessages = true);
            services.AddSwaggerDocument();
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors(
                        options => options.SetIsOriginAllowedToAllowWildcardSubdomains().
                        WithOrigins("http://localhost:8080", "http://*.localhost:8080", "http://localhost:8081", "http://localhost:8082")
                        .AllowAnyHeader().AllowCredentials().AllowCredentials());
            }
            else
            {
                app.UseCors(
                            options => options.SetIsOriginAllowedToAllowWildcardSubdomains().
                            WithOrigins(Environment.GetEnvironmentVariable("FRONTEND_DOMAIN"))
                            .AllowAnyHeader().AllowCredentials().AllowCredentials());
            }


            app.UseHttpsRedirection();
            app.UseDeveloperExceptionPage();
            app.UseRouting();
            app.UseAuthorization();
            app.UseOpenApi(); // serve documents (same as app.UseSwagger())
            app.UseSwaggerUi3(); // serve Swagger UI
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
