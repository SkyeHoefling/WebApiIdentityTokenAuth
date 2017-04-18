using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using WebApiIdentityTokenAuth.Services;

namespace WebApiIdentityTokenAuth
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // configure database
            var server = "localhost"; 
            var connection = $@"Server={server};Database=purchasing_dev;user=sa;password=Password01!;";
            services.AddDbContext<IdentityDbContext>(options => options.UseSqlServer(
                connection,
                optionsBuilder => optionsBuilder.MigrationsAssembly("WebApiIdentityTokenAuth")));

            // configure token generation
            services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<IdentityDbContext>()
                .AddDefaultTokenProviders();
            
            // configure identity options
            services.Configure<IdentityOptions>(o => {
                o.SignIn.RequireConfirmedEmail = true;
            });

            services.AddTransient<IMessageService, FileMessageService>();

            services.AddMvc();
            services.AddCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseCors(b => b.WithOrigins("http://dev.localhost.com:4000")
                .AllowAnyOrigin()
                .AllowCredentials()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseIdentity();
            app.UseMvc();
        }
    }
}
