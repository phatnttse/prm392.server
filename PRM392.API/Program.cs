﻿
using DotNetEnv;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using OpenIddict.Validation.AspNetCore;
using PRM392.API.Configurations;
using PRM392.API.Middlewares;
using Quartz;
using static OpenIddict.Abstractions.OpenIddictConstants;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json.Serialization;
using PRM392.Services.Mappers;
using PRM392.Repositories.Base;
using PRM392.Repositories.Interfaces;
using PRM392.Services.Interfaces;
using PRM392.Services;
using Microsoft.IdentityModel.Logging;
using PRM392.Repositories.DbContext;
using PRM392.Repositories.Entities;
using PRM392.Repositories;
using PRM392.Utils;
using Net.payOS;
using PRM392.Services.Hubs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace PRM392.API
{
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// The main method which starts the application.
        /// </summary>
        /// <param name="args">The command-line arguments.</param>
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Load environment variables from .env file
            Env.Load();

            // Load SMTP configuration from environment variables
            //var smtpConfig = SmtpConfig.LoadFromEnvironment();

            // Add services to the container.

            // Configure the DbContext with the connection string
            var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING") ??
                  throw new InvalidOperationException("Connection string 'DB_CONNECTION_STRING' not found.");

            var migrationsAssembly = typeof(Program).GetTypeInfo().Assembly.GetName().Name;

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(connectionString, b => b.MigrationsAssembly(migrationsAssembly));
                options.UseOpenIddict();
            }, ServiceLifetime.Scoped);

            // Add Identity
            builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();


            // Configure Identity options and password complexity here
            builder.Services.Configure<IdentityOptions>(options =>
            {
                // User settings
                options.User.RequireUniqueEmail = false;

                // Password settings
                /*
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = false;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;
                */

                // Configure Identity to use the same JWT claims as OpenIddict
                options.ClaimsIdentity.UserNameClaimType = Claims.Name;
                options.ClaimsIdentity.UserIdClaimType = Claims.Subject;
                options.ClaimsIdentity.RoleClaimType = Claims.Role;
                options.ClaimsIdentity.EmailClaimType = Claims.Email;
            });

            // Configure OpenIddict periodic pruning of orphaned authorizations/tokens from the database.
            builder.Services.AddQuartz(options =>
            {
                options.UseSimpleTypeLoader();
                options.UseInMemoryStore();
            });

            // Register the Quartz.NET service and configure it to block shutdown until jobs are complete.
            builder.Services.AddQuartzHostedService(options => options.WaitForJobsToComplete = true);

            builder.Services.AddOpenIddict()
                .AddCore(options =>
                {
                    options.UseEntityFrameworkCore()
                           .UseDbContext<ApplicationDbContext>();

                    options.UseQuartz();
                })
                .AddServer(options =>
                {
                    options.SetTokenEndpointUris("connect/token");

                    options.AllowPasswordFlow()
                           .AllowRefreshTokenFlow();

                    options.SetAccessTokenLifetime(TimeSpan.FromDays(7)); 
                    options.SetRefreshTokenLifetime(TimeSpan.FromDays(14)); 


                    options.RegisterScopes(
                        Scopes.Profile,
                        Scopes.Email,
                        Scopes.Address,
                        Scopes.Phone,
                        Scopes.Roles);

                    if (builder.Environment.IsDevelopment())
                    {
                        options.AddDevelopmentEncryptionCertificate()
                               .AddDevelopmentSigningCertificate();

                        options.UseAspNetCore().DisableTransportSecurityRequirement();

                    }
                    else
                    {
                        var oidcCertFileName = builder.Configuration["OIDC:Certificates:Path"];
                        var oidcCertFilePassword = builder.Configuration["OIDC:Certificates:Password"];

                        if (string.IsNullOrWhiteSpace(oidcCertFileName))
                        {
                            // You must configure persisted keys for Encryption and Signing.
                            // See https://documentation.openiddict.com/configuration/encryption-and-signing-credentials.html
                            options.AddEphemeralEncryptionKey()
                                   .AddEphemeralSigningKey();

                            options.UseAspNetCore().DisableTransportSecurityRequirement();
                        }
                        else
                        {
                            var oidcCertificate = new X509Certificate2(oidcCertFileName, oidcCertFilePassword);

                            options.AddEncryptionCertificate(oidcCertificate)
                                   .AddSigningCertificate(oidcCertificate);
                        }
                    }

                    options.UseAspNetCore()
                           .EnableTokenEndpointPassthrough();

                })
                .AddValidation(options =>
                {
                    options.UseLocalServer();
                    options.UseAspNetCore();
                });


            builder.Services.AddAuthentication(o =>
            {
                o.DefaultScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
                o.DefaultAuthenticateScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
                o.DefaultChallengeScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
            })
            .AddCookie()
            .AddOpenIdConnect(options =>
            {
                options.Authority = "https://prm392.bonheur.pro";
                options.ClientId = OidcServerConfig.PRM392ClientID;
                options.SaveTokens = true;
                options.ResponseType = "code";
                options.Scope.Add("openid");
                options.Scope.Add("profile");
                options.Scope.Add("email");
                options.Scope.Add("offline_access ");
                options.Scope.Add("roles");
                options.GetClaimsFromUserInfoEndpoint = true;

                // Lấy access_token từ query 
                options.Events.OnMessageReceived = context =>
                {
                    var accessToken = context.Request.Query["access_token"].ToString();
                    if (!string.IsNullOrEmpty(accessToken))
                    {
                        // Thêm token vào header Authorization
                        context.HttpContext.Request.Headers["Authorization"] = "Bearer " + accessToken;
                        context.Token = accessToken;
                    }
                    return Task.CompletedTask;
                };

                options.Events.OnTokenValidated = context =>
                {
                    var token = context.SecurityToken as JwtSecurityToken;
                    var userId = token?.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

                    if (userId != null)
                    {
                        var claimsIdentity = (ClaimsIdentity)context.Principal.Identity;
                        claimsIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, userId));
                    }

                    return Task.CompletedTask;
                };
            });

            builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
            {
                options.TokenLifespan = TimeSpan.FromDays(7); 
            });


            // Add cors
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecification", policy =>
                {
                    policy.WithOrigins("http://localhost:4300")
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials(); // Enable credentials (cookies, authorization headers)
                });
            });


            // Add controllers
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            // Add API versioning
            builder.Services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
                options.ReportApiVersions = true;
                options.ApiVersionReader = ApiVersionReader.Combine(
                    new QueryStringApiVersionReader("api-version"),
                    new HeaderApiVersionReader("x-version"),
                    new MediaTypeApiVersionReader("version")
                    );
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = OidcServerConfig.ServerName, Version = "v1" });
                c.OperationFilter<SwaggerAuthorizeOperationFilter>();
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        Password = new OpenApiOAuthFlow
                        {
                            TokenUrl = new Uri("/connect/token", UriKind.Relative)
                        }
                    }
                });
                // Đọc file XML Comment
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            //PayOs
            var payOsClientId = Environment.GetEnvironmentVariable("PAYOS_CLIENT_ID") ??
                 throw new InvalidOperationException("Environement string 'PAYOS_CLIENT_ID' not found.");

            var payOsApiKey = Environment.GetEnvironmentVariable("PAYOS_API_KEY") ??
                throw new InvalidOperationException("Environement string 'PAYOS_API_KEY' not found.");

            var payOsCheckSumKey = Environment.GetEnvironmentVariable("PAYOS_CHECKSUM_KEY") ??
                throw new InvalidOperationException("Environement string 'PAYOS_CHECKSUM_KEY' not found.");

            PayOS payOS = new PayOS(payOsClientId, payOsApiKey, payOsCheckSumKey);

            builder.Services.AddSingleton(payOS);

            // Mapper
            builder.Services.AddAutoMapper(typeof(MappingProfiles));

            // Exception Handler
            builder.Services.AddExceptionHandler<ExceptionHandler>();
            builder.Services.AddProblemDetails();

            //UnitOfWork
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            //Repositories
            builder.Services.AddScoped<UserAccountRepository>();
            builder.Services.AddScoped<UserRoleRepository>();
            builder.Services.AddScoped<ProductRepository>();
            builder.Services.AddScoped<CategoryRepository>();
            builder.Services.AddScoped<CartItemRepository>();
            builder.Services.AddScoped<OrderRepository>();
            builder.Services.AddScoped<OrderDetailRepository>();
            builder.Services.AddScoped<NotificationRepository>();
            builder.Services.AddScoped<ChatMessageRepository>();
            builder.Services.AddScoped<StoreLocationRepository>();
            builder.Services.AddScoped<IStoreLocation, StoreLocationRepository>();
            builder.Services.AddScoped<ProductImageRepository>();

            //Services
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IUserRoleService, UserRoleService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<ICartItemService, CartItemService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IPaymentService, PaymentService>();
            builder.Services.AddScoped<INotificationService, NotificationService>();
            builder.Services.AddScoped<IChatMessageService, ChatMessageService>();
            builder.Services.AddScoped<IStoreLocationService, StoreLocationService>();
            builder.Services.AddScoped<IUserAccountService, UserAccountService>();

            // HttpClient
            builder.Services.AddHttpClient();

            //SignalR
            builder.Services.AddSignalR();

            builder.WebHost.UseUrls("http://*:7267");

            var app = builder.Build();

            app.UseCors("AllowSpecification");

            app.UseCors(builder => builder
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod());

            app.UseExceptionHandler(opt => { });

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.DocumentTitle = "Swagger UI - PRM392";
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{OidcServerConfig.ServerName} V1");
                    c.OAuthClientId(OidcServerConfig.SwaggerClientID);
                });

                IdentityModelEventSource.ShowPII = true;
            }
            else
            {
                // The default HSTS value is 30 days.
                // You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // Configure the HTTP request pipeline.
            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseRouting(); // Đảm bảo gọi UseRouting trước

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.MapHub<ChatHubService>("/hubs/chat");

            // Configure HttpContextAccessor for Utilities
            var httpContextAccessor = app.Services.GetRequiredService<IHttpContextAccessor>();
            Utilities.Configure(httpContextAccessor);

            /************* SEED DATABASE *************/

            using var scope = app.Services.CreateScope();
            try
            {
                await OidcServerConfig.RegisterClientApplicationsAsync(scope.ServiceProvider);
            }
            catch (Exception ex)
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
                logger.LogCritical(ex, "An error occured whilst creating/seeding database");

                throw;
            }

            app.Run();
        }
    }
}
