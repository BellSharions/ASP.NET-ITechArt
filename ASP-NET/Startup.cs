using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using Serilog;
using System;
using DAL;
using DAL.Entities;
using DAL.Entities.Roles;
using Business;
using ASP_NET.Services;

namespace ASP_NET
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<SmtpOptions>(
            Configuration.GetSection(nameof(SmtpOptions)));
            services.Configure<CloudinaryOptions>(
            Configuration.GetSection(nameof(CloudinaryOptions)));
            services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);
            services.AddSwaggerGen(c =>
            {
                c.EnableAnnotations();
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "ASP.NET",
                    Description = "Example of SwaggerUI",
                    Contact = new OpenApiContact
                    {
                        Name = "Yury Chertko",
                        Email = "bellsharions@gmail.com",
                        Url = new Uri("https://github.com/BellSharions"),
                    }
                });
                
            });
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection"),
                    x => x.MigrationsAssembly("DAL")));
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true).AddRoles<Role>()
                .AddRoleManager<RoleManager>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.ConfigureApplicationCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromDays(30);
            });
            Services.IServiceCollectionExtensions.RegisterServices(services);
            services.AddHealthChecks()
                .AddCheck(
                    "OrderingDB-check",
                    new SqlConnectionHealthCheck(Configuration.GetConnectionString("DefaultConnection")),
                    HealthStatus.Unhealthy,
                    new string[] { "orderingdb" });
            services.AddRazorPages().AddNewtonsoftJson();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }
            else
            {
               app.UseExceptionHandler("/Error");
               app.UseHsts();
            }
            app.UseSerilogRequestLogging();
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            
            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapRazorPages();
                endpoints.MapHealthChecks("/hc");
                endpoints.MapControllerRoute(
                        name: "default",
                        pattern: "{controller=Home}/{action=Index}");
            });
        }

    }
}
