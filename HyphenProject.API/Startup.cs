using AutoMapper;
using HyphenProject.Business.Abstract;
using HyphenProject.Business.Concrete.Manager;
using HyphenProject.Business.ElasticSearchOptions.Abstract;
using HyphenProject.Business.ElasticSearchOptions.Conrete;
using HyphenProject.Business.Mappings;
using HyphenProject.DataAccess.Abstract;
using HyphenProject.DataAccess.Concrete;
using HyphenProject.DataAccess.Concrete.EntityFramework;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


namespace HyphenProject.API
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
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductDal, ProductDal>();
            services.AddDbContext<HyphenProjectDbContext>(options => options.UseSqlServer(Configuration["ConnectionStrings:Connection"]));
            services.AddScoped<IElasticSearchService, ElasticSearchService>();
            services.AddScoped<IElasticSearchConfigration, ElasticSearchConfigration>();
            
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new CustomMappingProfile());
            });

            services.AddCors(options =>
            {
                options.AddPolicy(
                    "CorsPolicy",
                    x => x
                        .SetIsOriginAllowed((_) => true)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });

            services.AddSingleton(mappingConfig.CreateMapper());
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseSwagger();
                //app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Softtech.Hotel.Api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}