using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using FluentValidation.AspNetCore;
using InvMan.Server.Database;
using InvMan.Server.Domain.Models;
using InvMan.Server.Application;
using InvMan.Server.UI.Filters;
using InvMan.Server.UI.Validators;

namespace InvMan.Server.UI
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
            services.AddSwaggerGen();

            services.AddDbContext<ApplicationDbContext>(
                options =>
                    options.UseSqlServer(
                        Configuration["ConnectionString"]
                    )
            );

            services.AddControllers().AddFluentValidation();

            services.AddIdentity<ClientUser, IdentityRole>().
                AddRoles<IdentityRole>().
                AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddTransient<IValidator<Device>, DeviceValidator>();
            services.AddTransient<AuthorizationFilter>();

            services.AddApplicationServices();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var context = GetService<ApplicationDbContext>(app);
            var usersManager = GetService<ClientUsersManager>(app);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(
                    options => {
                        options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                        options.RoutePrefix = string.Empty;
                    }
                );


                app.FillDbWithTemporaryData(context, usersManager);
            }

            app.AddUserGroups(context);

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
