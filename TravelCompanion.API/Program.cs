
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using System.Diagnostics;
using TravelCompanion.API.Middleware;
using TravelCompanion.Domain.Mapping;
using TravelCompanion.Domain.Models;
using TravelCompanion.Domain.Services;

namespace TravelCompanion.API
{
    public class Program
    {
        private static IConfiguration Configuration;

        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            Configuration = builder.Configuration;

            // Add services to the container.

            builder.Services.AddControllers();

            builder.Services.AddMemoryCache();

            builder.Services.AddLogging(options =>
            {
                options.AddRinLogger();
                options.AddApplicationInsights(configureTelemetryConfiguration: (config) =>
                        config.ConnectionString = builder.Configuration["TravelCompanionAppInsights"],
                    configureApplicationInsightsLoggerOptions: (options) => { });
            });

            builder.Services.AddAuthentication(o =>
                {
                    o.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                })
                .AddCookie()
                .AddApple(a =>
                {
                    a.ClientId = Configuration["AppleClientId"];
                    a.KeyId = Configuration["AppleKeyId"];
                    a.TeamId = Configuration["AppleTeamId"];
                    a.UsePrivateKey(keyId
                        =>
                    {
                        return builder.Environment.ContentRootFileProvider.GetFileInfo($"AuthKey_{keyId}.p8");
                    });
                    a.SaveTokens = true;
                })
                .AddGoogle(g =>
                {
                    g.ClientId = Configuration["TravelCompanionGoogleClientId"];
                    g.ClientSecret = Configuration["TravelCompanionGoogleClientSecret"];
                    g.SaveTokens = true;
                });
            //.AddApple(a =>
            //{
            //    a.ClientId = Configuration["AppleClientId"];
            //    a.KeyId = Configuration["AppleKeyId"];
            //    a.TeamId = Configuration["AppleTeamId"];
            //    a.UsePrivateKey(keyId
            //        => WebHostEnvironment.ContentRootFileProvider.GetFileInfo($"AuthKey_{keyId}.p8"));
            //    a.SaveTokens = true;
            //});

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.MapType<DateTime>(() => new OpenApiSchema { Type = "string", Format = "date" });

                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Travel Companion",
                    Version = "1.0.0",
                    Description = "An AI-driven travel companion.",
                });

                c.AddSecurityDefinition("API Key", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.ApiKey,
                    In = ParameterLocation.Header,
                    Description = "Paste your API Key.",
                    Name = "X-API-Key"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "API Key"
                            }
                        },
                        Array.Empty<string>()
                    }
                });

                c.CustomOperationIds(e => $"{e.ActionDescriptor.RouteValues["controller"]}{e.ActionDescriptor.RouteValues["action"]}");

                c.SwaggerGeneratorOptions.Servers = new List<OpenApiServer>
                {
                    new()
                    {
                        Url = Debugger.IsAttached
                            ? "https://localhost:7278"
                            : @"https://YOUR_API.azurewebsites.net/" // make sure to add your travel companion api base address
                    }
                };
            });

            builder.Services.AddDbContext<TravelCompanionContext>(options =>
            {
                options.UseSqlServer(builder.Configuration["TravelCompanionContext"]);
            }, ServiceLifetime.Transient);

            builder.Services.AddScoped<AppUserService>();
            builder.Services.AddScoped<TripService>();
            builder.Services.AddScoped<TripChatService>();
            builder.Services.AddScoped<TripEventService>();

            builder.Services.AddAutoMapper(typeof(MappingProfile));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseApiKeyMiddleware();

            app.MapControllers();

            app.Run();
        }
    }
}
