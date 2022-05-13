using System;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DevSpector.Database;
using DevSpector.Database.DTO;
using DevSpector.Domain.Models;
using DevSpector.UI.Filters;
using DevSpector.UI.Validators;

namespace DevSpector.UI
{
    public class Startup
    {
        private const string _ConnectionStringEnvVariableName =
            "DEVSPECTOR_SERVER_CONNSTR";

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

            services.AddDbContext<ApplicationContextBase, ApplicationDbContext>(
                options => {
                    if (Environment.IsDevelopment())
                    {
                        options.UseSqlite(Configuration["ConnectionString"]);
                    }
                    else
                    {
                        var connectionString = Configuration["ConnectionString"];
                        // For automated CI cases:
                        // I don't know how to deal with hidden appsettings.json: I can't load it to github actions
                        // because it checkouts my repo without that file as it is added to .gitignore (there is production connection string)
                        // My approach with docker where I use 'dotnet ef database update --connection <con-string>' doesn't working
                        // That is why I am getting connection string from variable and not from appsettings.json using Configuration
                        if (!string.IsNullOrWhiteSpace(connectionString))
                            connectionString = System.Environment.GetEnvironmentVariable(_ConnectionStringEnvVariableName);

                        if (string.IsNullOrWhiteSpace(connectionString))
                            throw new InvalidOperationException(
                                "Не удалось найти строку подключения. Перередайте её" +
                                $"через переменную среды \"{_ConnectionStringEnvVariableName}\"" +
                                " или задайте её в файле \"appsettings.json\" в корне программы"
                            );

                        options.UseSqlServer(connectionString);
                    }
                }
            );

            services.AddControllers();

            services.Configure<JsonOptions>(options => {
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                options.JsonSerializerOptions.Encoder =
                    JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic);
            });

            services.Configure<ApiBehaviorOptions>(options => {
                options.InvalidModelStateResponseFactory = context => {
                    return new BadRequestObjectResult(new BadRequestError {
                        Error = "Ошибка валидации",
                        Description = context.ModelState.Values.
                            Where(v => v.ValidationState == ModelValidationState.Invalid).
                            FirstOrDefault().Errors.Select(e => e.ErrorMessage)
                    });
                };
            });

            services.AddIdentity<User, IdentityRole>().
                AddRoles<IdentityRole>().
                AddEntityFrameworkStores<ApplicationContextBase>();

            services.Configure<IdentityOptions>(options => {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 3;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
            });

            services.AddTransient<IValidator<DeviceToAdd>, DeviceValidator>();
            services.AddTransient<AuthorizationFilter>();

            services.AddApplicationServices();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            MigrateDatabase(app);

            app.AddUserGroup("Техник");
            app.AddUserGroup("Администратор");
            app.AddUserGroup("Суперпользователь");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.AddSuperUserAsync("root", "123Abc!").GetAwaiter().GetResult();
                app.FillDbWithTemporaryDataAsync();
            }
            else
                app.AddSuperUserAsync("root", System.Environment.GetEnvironmentVariable("ROOT_PWD")).GetAwaiter().GetResult();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void MigrateDatabase(IApplicationBuilder builder)
        {
            var context = GetService<ApplicationContextBase>(builder);
            if (context.Database.GetPendingMigrations().Any())
                context.Database.Migrate();
        }

        private T GetService<T>(IApplicationBuilder builder) =>
            builder.ApplicationServices.CreateScope().
                ServiceProvider.
                    GetService<T>();
    }
}
