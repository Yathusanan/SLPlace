using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SLPlaceAPI.Data;
using SLPlaceAPI.Mapping;
using SLPlaceAPI.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace SLPlaceAPI
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
            services.AddControllers();
            services.AddDbContext<ApplicationDBContext>(options => 
                                  options.UseSqlServer(Configuration.GetConnectionString("SLPConnection")));
            services.AddScoped<IPlaceRepository, PlaceRespository>();
            services.AddAutoMapper(typeof(SLPMapper));
            services.AddSwaggerGen( option =>
                                    {
                                        option.SwaggerDoc("SLPlaceOpenAPI", new Microsoft.OpenApi.Models.OpenApiInfo()
                                        {
                                            Title = "SL Places - API",
                                            Version = "1.0",
                                            Description = "This API Provides the Informations About Best Places For Tourists In Srilanka",

                                            Contact = new Microsoft.OpenApi.Models.OpenApiContact()
                                            {
                                                Email = "yathu@yathu.com",
                                                Name = "Y.Yathusanan",
                                                Url = new Uri("https://wwww.yathu.com")
                                            },

                                            License = new Microsoft.OpenApi.Models.OpenApiLicense()
                                            {
                                                Name = "MIT License",
                                                Url = new Uri("https://wwww.yathu.com")
                                            }
                                        });;

                                        var xmlCommentFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                                        var cmtCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentFile);
                                        option.IncludeXmlComments(cmtCommentsFullPath);
                                    });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/SLPlaceOpenAPI/swagger.json", "Place Api");
            });

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
