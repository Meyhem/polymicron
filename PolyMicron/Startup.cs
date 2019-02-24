using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Pm.Common.Exceptions;
using Pm.Core;
using Pm.Core.Interfaces;
using Pm.Data;
using Pm.Data.Interfaces;
using ProjectPlaguemangler.Filters;
using Serilog;
using System.IO;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;

namespace ProjectPlaguemangler
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
             var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);

            Configuration = builder.Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .CreateLogger();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .AddSessionStateTempDataProvider();

            services.Configure<FormOptions>(opts =>
            {
                opts.MultipartBodyLengthLimit = 104857600; // 100 megs for post edit form
                opts.ValueLengthLimit = 104857600; // 100 megs for post edit form
            });

            services.AddResponseCompression();
            services.AddAuthentication("CookieScheme")
                .AddCookie("CookieScheme", opts =>
                {
                    opts.LoginPath = "/cms";
                });

            services.AddSession();
            services.AddDbContext<PmEntities>(options => options.UseNpgsql(Configuration.GetConnectionString("PmEntities")));
            services.AddScoped<ExceptionHandler>();

            services.AddDataProtection()
                .PersistKeysToFileSystem(new DirectoryInfo("./keys"))
                .SetDefaultKeyLifetime(TimeSpan.FromDays(7))
                .UseCryptographicAlgorithms(new AuthenticatedEncryptorConfiguration
                {
                    EncryptionAlgorithm = EncryptionAlgorithm.AES_256_CBC
                });


            services.AddTransient(typeof(IRepository<>), typeof(PmRepository<>));
            services.AddTransient<IPasswordDerivationService, PasswordDerivationService>();
            services.AddTransient<UserService>();
            services.AddTransient<PostService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
            app.UseStaticFiles();
            app.UseSession();
            app.UsePmNotFoundHandler();
            app.UsePmVisitorId();
            app.UseResponseCompression();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            Log.Warning($"Starting server in {env.EnvironmentName} environment.");
            Initialize(app.ApplicationServices).GetAwaiter().GetResult();
        }

        public async Task Initialize(IServiceProvider serviceProvider)
        {
            var secSection = new SecurityConfigurationSection();
            Configuration.Bind("Security", secSection);
            if (secSection == null || 
                string.IsNullOrEmpty(secSection.RootUsername) || 
                string.IsNullOrEmpty(secSection.RootPassword) ||
                string.IsNullOrEmpty(secSection.RootDisplayName))
            {
                throw new PmValidationException("Config file must have specified Security section with RootUsername, RootPassword and RootDisplayName");
            }
            using (var scope = serviceProvider.CreateScope())
            {
                var users = scope.ServiceProvider.GetRequiredService<UserService>();
                await users.SeedRootUser(secSection.RootUsername, secSection.RootPassword, secSection.RootDisplayName);

                var dbCtx = scope.ServiceProvider.GetRequiredService<PmEntities>();

                Log.Warning("Migrating db");
                dbCtx.Database.Migrate();
            }
        }
    }
}
