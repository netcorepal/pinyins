using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.Extensions.PlatformAbstractions;
using System.IO;
using Microsoft.EntityFrameworkCore;
using NetCorePal.Pinyins.Service;
using SchoolPal.Toolkit.Caching;
using SchoolPal.Toolkit.Caching.Redis;
using NetCorePal.Pinyins.Service.Repository;
using Exceptionless;

namespace NetCorePal.Pinyins.Api
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            //#if DEBUG
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "NetCorePal.Pinyins.Api"
                });

                //Determine base path for the application.  
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;
                //Set the comments path for the swagger json and ui.  
                var xmlPath = Path.Combine(basePath, "NetCorePal.Pinyins.Api.xml");
                options.IncludeXmlComments(xmlPath);
            });

            //#endif
            //DbContext
            var connection = Configuration.GetConnectionString("DefaultConnection");
            //redis
            string pre = Configuration.GetSection("RedisPrefix").Value;
            string redisCon = Configuration.GetSection("ConnectionStrings:RedisConnectionStrings").Value;

            services.AddDbContext<MyContext>(options => options.UseMySQL(connection));
            services.AddSingleton(typeof(ICache), new RedisCache(redisCon, pre));

            services.AddScoped<IPinYinService, PinYinService>();
            services.AddScoped<IDuoYinZiDictionaryRepository, DuoYinZiDictionaryRepository>();
            services.AddScoped<IBaiJiaXingRepository, BaiJiaXingRepository>();

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
            //#if DEBUG
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "NetCorePal.Pinyins.Api");
            });
            //#endif
            app.UseHttpsRedirection();
            app.UseMvc();
            app.UseExceptionless(Configuration);
        }
    }
}
