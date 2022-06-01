using ChurchAdmin.Context;
using MailKit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OpenXmlPowerTools;
using SjxLogistics.Components;
using SjxLogistics.Controllers.AuthenticationComponent;
using SjxLogistics.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Schedular
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
            AccessConfig authConfig = new();
            Configuration.Bind("Authentication", authConfig);
            services.AddSingleton(authConfig);

            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });

            services.AddDbContext<CustomerDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddMvc();
            services.AddMemoryCache();
            services.AddSession();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IpasswordHasher, BcryptPassHasher>();
            services.AddSingleton<AccessToken>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<TokkenGeneratorClass>();
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Schedular", Version = "v1" });
            });
        }

        ////HandleJWT
        //Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(O =>
        //    {
        //    o.TokenValidationParameters = new TokenValidationParameters()
        //    {
        //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authConfig.AccessKey)),
        //        ValidIssuer = authConfig.Issuer,
        //        ValidAudience = authConfig.Audiance,
        //        ValidateIssuer = true,
        //        ValidateAudience = true
        //    };
        //});
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Schedular v1"));
            }

            app.UseHttpsRedirection();

              app.UseRouting();
             app.UseAuthentication();
               app.UseSession();

            app.UseAuthorization();
            app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
