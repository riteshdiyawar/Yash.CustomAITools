using AutoMapper;
 
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.IO;
using System.Text;

namespace Identity.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to thD:\Yash\Projects\IBC CFM\IBC.CFM\Services\Identity\Identity.API\Startup.cse container.
        public void ConfigureServices(IServiceCollection services)
        {
            var builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            var config = builder.Build();

            var key = Encoding.ASCII.GetBytes(config["Key"]);

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(config["BearerKey"], x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = config["userkey"],
                    ValidateAudience = false
                };
            });

            services.AddControllers();
            services.AddAutoMapper(typeof(Startup));

            //services.Add(new ServiceDescriptor(typeof(IUsersBL), typeof(UsersBL), ServiceLifetime.Transient));
            //services.Add(new ServiceDescriptor(typeof(IUsersDALRepository), typeof(UsersDALRepository), ServiceLifetime.Transient));
            //services.Add(new ServiceDescriptor(typeof(ILoggerService), typeof(LoggerService), ServiceLifetime.Transient));
            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CFM Identity API", Version = "v1" });
            });

            services.Configure<AppSettings>(Configuration.GetSection("ConfigSettings"));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // if (env.IsDevelopment())
            //{
            //   app.UseDeveloperExceptionPage();
            //}

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("../swagger/v1/swagger.json", "CFM API V1");
            });

            app.UseAuthentication();

            //  app.UseHttpsRedirection();
            app.UseCors("AllowOrigin");
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
