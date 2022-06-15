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
        private bool _doesDbNeedsPopulation = true;

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

                    Console.WriteLine($"Environment name is: {Environment.EnvironmentName}");
                    if (Environment.IsDevelopment())
                    {
                        Console.WriteLine("Entering development evnironment.");
                        Console.WriteLine("Connection string: " + Configuration["LocalStorageConnectionString"]);
                        options.UseSqlite(Configuration["LocalStorageConnectionString"]);
                    }
                    else
                    {
                        Console.WriteLine("Entering production evnironment.");
                        // For automated CI cases:
                        // I don't know how to deal with hidden appsettings.json: I can't load it to github actions
                        // because it checkouts my repo without that file as it is added to .gitignore (there is production connection string)
                        // My approach with docker where I use 'dotnet ef database update --connection <con-string>' doesn't working
                        // That is why I am getting connection string from variable and not from appsettings.json using Configuration
                        string connectionString = System.Environment.GetEnvironmentVariable(_ConnectionStringEnvVariableName);

                        if (string.IsNullOrWhiteSpace(connectionString))
                            throw new InvalidOperationException(
                                "Не удалось найти строку подключения. Перередайте её" +
                                $" через переменную среды \"{_ConnectionStringEnvVariableName}\"" +
                                " или используйте локальное хранилище"
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
            // MigrateDatabase(app);
            var context = GetService<ApplicationContextBase>(app);
            context.Database.Migrate();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                if (_doesDbNeedsPopulation)
                    app.InitializeData("root", "123Abc!");
            }
            else
            {
                if (_doesDbNeedsPopulation)
                    app.InitializeData("root", System.Environment.GetEnvironmentVariable("ROOT_PWD"));
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void MigrateDatabase(IApplicationBuilder builder)
        {
            _doesDbNeedsPopulation = false;

            var context = GetService<ApplicationContextBase>(builder);
            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
                _doesDbNeedsPopulation = true;
            }
        }

        private T GetService<T>(IApplicationBuilder builder) =>
            builder.ApplicationServices.CreateScope().
                ServiceProvider.
                    GetService<T>();
    }
}
