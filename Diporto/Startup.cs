using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SpaServices.Webpack;
using System.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Diporto.Database;
using Diporto.Models;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Amazon;
using Amazon.S3;
using AspNetCoreRateLimit;
using Serilog.Extensions.Logging;

namespace Diporto
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IConfiguration>(Configuration);
            
            // Add framework services.
            services.AddMvc().AddJsonOptions(options => {
                options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            });

            var connectionString = Configuration["DbContextSettings:ConnectionString"];
            var databaseURL = Environment.GetEnvironmentVariable("DATABASE_URL");
            if ((connectionString == null || connectionString.Length == 0) && databaseURL.Length > 0) {
                // Attempt to use Env Var DATABASE_URL
                var uri = new Uri(databaseURL);
                var creds = uri.UserInfo.Split(':');
                connectionString = $"Username={creds[0]};Password={creds[1]};Host={uri.Host};Port={uri.Port};Database={uri.PathAndQuery.Substring(1)};Pooling=true;Use SSL Stream=True;SSL Mode=Require;TrustServerCertificate=True;";
            }
            services.AddDbContext<DatabaseContext>(opts => opts.UseNpgsql(connectionString));

            services.AddIdentity<User, IdentityRole<int>>(options => {
                options.Cookies.ApplicationCookie.AutomaticChallenge = false;
            })
                .AddEntityFrameworkStores<DatabaseContext, int>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options => {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 16;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;

                options.Cookies.ApplicationCookie.Events = new CookieAuthenticationEvents {
                    OnRedirectToLogin = ctx => {
                        if (ctx.Request.Path.StartsWithSegments("/api") && ctx.Response.StatusCode == (int) HttpStatusCode.OK) {
                            ctx.Response.StatusCode = (int) HttpStatusCode.Unauthorized;
                        } else {
                            ctx.Response.Redirect(ctx.RedirectUri);
                        }
                        return Task.FromResult(0);
                    }
                };
            });

            services.AddDefaultAWSOptions(Configuration.GetAWSOptions());
            services.AddAWSService<IAmazonS3>();

            // needed to load configuration from appsettings.json
            services.AddOptions();

            // needed to store rate limit counters and ip rules
            services.AddMemoryCache();

            //load general configuration from appsettings.json
            services.Configure<IpRateLimitOptions>(Configuration.GetSection("IpRateLimiting"));

            //load ip rules from appsettings.json
            services.Configure<IpRateLimitPolicies>(Configuration.GetSection("IpRateLimitPolicies"));

            // inject counter and rules stores
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            loggerFactory.AddFile("Logs/Diporto-{Date}.txt");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseWebpackDevMiddleware(new WebpackDevMiddlewareOptions {
                    HotModuleReplacement = true,
                    ReactHotModuleReplacement = true
                });
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                Console.WriteLine("Auto seeding database");
                serviceScope.ServiceProvider.GetService<DatabaseContext>().Database.Migrate();
                serviceScope.ServiceProvider.GetService<DatabaseContext>().EnsureSeedData();
            }

            app.UseStaticFiles();

            app.UseIdentity();

            app.UseJwtBearerAuthentication(new JwtBearerOptions {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                TokenValidationParameters = new TokenValidationParameters {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetSection("AppConfiguration:Key").Value)),
                    ValidAudience = Configuration.GetSection("AppConfiguration:SiteUrl").Value,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidIssuer = Configuration.GetSection("AppConfiguration:SiteUrl").Value
                }
            });

            app.UseIpRateLimiting();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "Home", action = "Index" });
            });
        }
    }
}
