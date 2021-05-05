using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Upico.Persistence;
using Microsoft.EntityFrameworkCore;
using Upico.Core.Domain;
using Microsoft.AspNetCore.Identity;
using Upico.Core.Repositories;
using Upico.Persistence.Repositories;
using Upico.Core;

namespace Upico
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
            //For identity.entityframworkcore.
            services.AddDbContext<UpicODbContext>(options => options.UseSqlServer(Configuration.GetConnectionString("default")));

            
            services.AddIdentity<AppUser, IdentityRole>(opt => {
                opt.Password.RequireNonAlphanumeric = false;
            })
                .AddEntityFrameworkStores<UpicODbContext>()
                .AddDefaultTokenProviders();

            

            services.AddAuthentication();

            //injecting interface
            services.AddScoped<IUnitOfWork, UnitOfWork>();


            //default
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Upico", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Upico v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
