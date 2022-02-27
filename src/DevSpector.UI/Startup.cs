using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using FluentValidation.AspNetCore;
using DevSpector.Database;
using DevSpector.Domain.Models;
using DevSpector.Application;
using DevSpector.UI.Filters;
using DevSpector.UI.Validators;

namespace DevSpector.UI
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Environment = env;
        }

        public IConfiguration Configuration { get; }

        public IWebHostEnvironment Environment { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<ILogger<Startup>, Logger<Startup>>();

            services.AddDbContext<ApplicationDbContext>(
                options => {
                    if (Environment.IsDevelopment())
                        options.UseSqlite(Configuration["ConnectionString"]);
                    else {
                        // I don't know how to deal with hidden appsettings.json: I can't load it to github actions
                        // because it checkouts my repo without that file as it is added to .gitignore (there is production connection string)
                        // My approach with docker where I use dotnet ef database update --connection <con-string> just ignored
                        var connectionString = System.Environment.GetEnvironmentVariable("CON_STR");
                        options.UseSqlServer(connectionString);
                    }
                }
            );

            services.AddControllers().AddFluentValidation();

            services.AddIdentity<ClientUser, IdentityRole>().
                AddRoles<IdentityRole>().
                AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddTransient<IValidator<Device>, DeviceValidator>();
            services.AddTransient<AuthorizationFilter>();

            services.AddApplicationServices();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            var context = GetService<ApplicationDbContext>(app);
            var usersManager = GetService<ClientUsersManager>(app);

            app.AddUserGroups(context);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.FillDbWithTemporaryData(context, usersManager);
            }
            else
                app.AddRootUser(usersManager);

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private T GetService<T>(IApplicationBuilder builder) =>
            builder.ApplicationServices.CreateScope().
                ServiceProvider.
                    GetService<T>();
    }
}
