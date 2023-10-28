using Microsoft.AspNetCore.Http.Features;
using asp_net_with_angular_client_template.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;
using FluentValidation;
using FluentValidation.Validators;
using Microsoft.AspNetCore.Identity;
using System.Configuration;
using FluentValidation.AspNetCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using asp_net_with_angular_client_template.Validators;
using asp_net_with_angular_client_template.Models.Dto;
using asp_net_with_angular_client_template.Services.Interface;
using asp_net_with_angular_client_template.Services.Implementation;
using asp_net_with_angular_client_template.Repository.Implementations;
using asp_net_with_angular_client_template.Repository.Interfaces;
using asp_net_with_angular_client_template.Helpers;
using System.Text;

namespace asp_net_with_angular_client_template
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        [Obsolete]
        public void ConfigureServices(IServiceCollection services)
        {
            // connection to MySql DB
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseMySQL(Configuration.GetConnectionString("DefaultConnection"));
            });

            SwaggerConfigure(services);
            JWTConfigure(services);
            LogConfiguration(services);

            services.AddCors();
            services.AddControllersWithViews();

            #region Validations
            services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore).
                AddFluentValidation(fv =>
                    fv.RegisterValidatorsFromAssemblyContaining<LoginValidator>());

            services.AddTransient<IValidator<LoginDto>, LoginValidator>();

            #endregion

            #region Services
            services.AddTransient<IHashService, HashService>();
            services.AddTransient<IIdentityService, IdentityService>();
            #endregion

            #region Repositories
            services.AddTransient<IUserRepository, UserRepository>();
            #endregion
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            Log.Information("Application starting.");

            app.UseCors();
            app.UseRouting();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller}/{action=Index}/{id?}");
                endpoints.MapFallbackToFile("index.html");
            });

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApp");
            });

            app.UseHttpsRedirection();

            Log.Information("Application started.");
        }

        #region Private methods

        private void LogConfiguration(IServiceCollection services)
        {
            var logger = new LoggerConfiguration()
                .ReadFrom.Configuration(Configuration)
                .WriteTo.Console()
                .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            services.AddSingleton<Serilog.ILogger>(logger);
        }

        private void SwaggerConfigure(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApp", Version = "v1" });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme {
                            Reference = new OpenApiReference {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                        }
                     },
                     Array.Empty<string>()
                    }
                });
            });
        }

        private static void JWTConfigure(IServiceCollection services)
        {
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(AuthOptions.KEY));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).
                    AddJwtBearer(options =>
                    {
                        options.RequireHttpsMetadata = false;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidIssuer = AuthOptions.ISSUER,

                            ValidateAudience = true,
                            ValidAudience = AuthOptions.AUDIENCE,
                            ValidateLifetime = true,

                            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                            ValidateIssuerSigningKey = true,
                        };

                        options.Events = new JwtBearerEvents
                        {
                            OnMessageReceived = context =>
                            {
                                var accessToken = context.Request.Query["access_token"];

                                var path = context.HttpContext.Request.Path;
                                if (!string.IsNullOrEmpty(accessToken))
                                {
                                    context.Token = accessToken;
                                }
                                return Task.CompletedTask;
                            }
                        };
                    }).AddCookie();
        }

        #endregion
    }
}
