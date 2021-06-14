using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using HerosCompanyApi.Models;
using HerosCompanyApi.Services.TokenGenerators;
using Microsoft.EntityFrameworkCore;
using HerosCompanyApi.Services.Encrypters;

namespace HerosCompanyApi
{
    public class Startup
    {
        private const string _connectionStringName = "HerosCompany";
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddDbContext<HerosCompanyDBContext>(o =>
            {
                string connectionString = _configuration.GetConnectionString(_connectionStringName);
                o.UseSqlServer(connectionString);
            });

            AuthenticationConfigurations authenticationConfigurations = new AuthenticationConfigurations();
            _configuration.Bind("Authentication", authenticationConfigurations);

            services.AddSingleton<AuthenticationConfigurations>(authenticationConfigurations);

            services.AddSingleton<AccessTokenGenerator>();
            services.AddSingleton<IPasswordHasherService, RFCSHAPasswordHasherService>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(o =>
            {

                o.TokenValidationParameters = new TokenValidationParameters()
                {
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authenticationConfigurations.AccessTokenSecret)),
                    ValidIssuer = authenticationConfigurations.Issuer,
                    ValidAudience = authenticationConfigurations.Audience,
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                };
            }
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
