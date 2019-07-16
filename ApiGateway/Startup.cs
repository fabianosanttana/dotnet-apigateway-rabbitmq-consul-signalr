using System;
using System.Text;
using ApiGateway.Aggregators;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace ApiGateway
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new Microsoft.Extensions.Configuration.ConfigurationBuilder();
            builder.SetBasePath(env.ContentRootPath)
                   .AddJsonFile("ocelot.json", optional: false, reloadOnChange: true)
                   .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                   .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var audienceConfig = Configuration.GetSection("Audience");

            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(audienceConfig["Chave"]));
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                ValidateIssuer = true,
                ValidIssuer = audienceConfig["Iss"],
                ValidateAudience = true,
                ValidAudience = audienceConfig["Author"],
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                RequireExpirationTime = true,
            };

            services.AddAuthentication()
                    .AddJwtBearer("TestKey", x =>
                     {
                         x.RequireHttpsMetadata = false;
                         x.TokenValidationParameters = tokenValidationParameters;
                     });


            services.AddOcelot(Configuration)
                    .AddTransientDefinedAggregator<CustomerWithProductAggregator>();
        }


        public async void Configure(IApplicationBuilder app, IHostingEnvironment env)
        =>
            await app.UseOcelot();

    }
}
