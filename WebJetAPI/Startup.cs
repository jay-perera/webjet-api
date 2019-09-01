using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebJetAPI.Services;
using WebJetAPI.Services.ExternalService;
using WebJetAPI.Utility;

namespace WebJetAPI
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

            // Add Cors
            services.AddCors(o => o.AddPolicy("WebjetPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));

            AddOtherServices(services);
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        private void AddOtherServices(IServiceCollection services)
        {
            services.AddTransient<IApiUtilityService, ApiUtilityService>();
            services.AddTransient<IMovieService, MovieService>();
            services.AddTransient<IExternalMovieService, ExternalMovieService>();
            services.AddTransient<IApiHelperService, ApiHelperService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseCors("WebjetPolicy");

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
