using System.IO;
using Server.Services;
using Server.Persistence;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Server.Models;

namespace source {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            var keysDirectoryName = "Keys";
            var keysDirectoryPath = Path.Combine(@"C:\Users\kaden\Desktop\local_cookies", keysDirectoryName);
            if (!Directory.Exists(keysDirectoryPath)) {
                Directory.CreateDirectory(keysDirectoryPath);
            };

            services.AddDataProtection()
                .PersistKeysToFileSystem(new DirectoryInfo(keysDirectoryPath))
                .SetApplicationName("auth_cookie");

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options => {
                options.Cookie.HttpOnly = false;
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.Name = "auth_cookie";
                options.Events = new CookieAuthenticationEvents {
                    OnRedirectToLogin = redirectContext => {
                        redirectContext.HttpContext.Response.StatusCode = 401;
                        return Task.CompletedTask;
                    }
                };
            });

            services.AddAuthorization(options => {
                options.AddPolicy(Role.USER, policy => policy.RequireRole(Role.USER));
                options.AddPolicy(Role.SUPPORT, policy => policy.RequireRole(Role.USER, Role.SUPPORT));
                options.AddPolicy(Role.MANAGEMENT, policy => policy.RequireRole(Role.USER, Role.MANAGEMENT));
            });

            services.AddScoped<IPersistenceContext, DataContext>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddControllers();
            services.AddCors();
            services.AddControllersWithViews().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore); ;
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "source", Version = "v1" }); });

            // Add HTTP Context to keep track of login status on the client.
            services.AddHttpContextAccessor();

            services.AddDbContext<DataContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DevelopSqlServer")));

            services.AddScoped<LoginService>();
            services.AddScoped<UserService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            var cookiePolicyOptions = new CookiePolicyOptions {
                MinimumSameSitePolicy = SameSiteMode.None,
            };

            app.UseCookiePolicy(cookiePolicyOptions);
            app.UseCors(options => options.WithOrigins("http://localhost:4200", "https://localhost:4200", "localhost:4200").AllowAnyMethod().AllowAnyHeader().AllowCredentials());
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => {
                endpoints.MapControllers();
            });
        }
    }
}
