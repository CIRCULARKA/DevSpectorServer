using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using InvMan.Server.Database;
using InvMan.Server.Domain.Models;
using InvMan.Server.UI.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;

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
            services.AddDbContext<ApplicationDbContext>(
                options =>
                    options.UseSqlServer(
                        Configuration["ConnectionString"]
                    )
            );
            services.AddControllers().AddFluentValidation();
            services.AddTransient<IValidator<Device>, DeviceValidator>();

            services.AddApplicationServices();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            var context = app.ApplicationServices.CreateScope().ServiceProvider.GetService<ApplicationDbContext>();
            context.Database.Migrate();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
