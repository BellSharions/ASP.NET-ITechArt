using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.AspNetCore;
using System;
using DAL;
using DAL.Entities;
using DAL.Entities.Roles;
using Business;
using System.Threading.Tasks;
using ASP_NET.Controllers.AuthControllers;
using ASP_NET.Profiles;

namespace ASP_NET
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
            services.Configure<SmtpOptions>(
            Configuration.GetSection(nameof(SmtpOptions)));
            services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);
            services.AddSwaggerGen(c =>
            {
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
                    x => x.MigrationsAssembly("DAL"))); //Use "DAL" instead, to ensure correct migration assembly
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true).AddRoles<Role>()//IdentityUser<Guid>
                .AddRoleManager<RoleManager>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            services.ConfigureApplicationCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromDays(30);
            });
            services.AddTransient<SampleData>();
            services.AddHealthChecks()
                .AddCheck(
                    "OrderingDB-check",
                    new SqlConnectionHealthCheck(Configuration.GetConnectionString("DefaultConnection")),
                    HealthStatus.Unhealthy,
                    new string[] { "orderingdb" });
            services.AddRazorPages().AddNewtonsoftJson();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, SampleData seeder, IWebHostEnvironment env)
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
            seeder.SeedRoles();
            seeder.SeedProducts();
            app.UseSerilogRequestLogging();
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ASP.NET API");
            });

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
