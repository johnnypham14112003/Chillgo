using Chillgo.BusinessService.Interfaces;
using Chillgo.BusinessService.Services;
using Chillgo.Repository;
using Chillgo.Repository.Interfaces;
using Chillgo.Repository.Repositories;
using Chillgo.BusinessService.Extensions;

using Mapster;
using System.Text;

using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Security.Cryptography.X509Certificates;
using System.Security.Claims;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.Extensions.Options;


namespace Chillgo.Api
{
    public static class ServiceRegistration
    {
        public static IServiceCollection DependencyInjectionServices(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
        {
            //System Services
            services.InjectDbContext(configuration);
            services.InjectBusinessServices();
            services.InjectRepository();
            services.ConfigCORS();
            services.ConfigKebabCase();

            //Third Party Services
            services.ConfigFluentEmail(configuration);
            services.AddRazorTemplating();
            services.ConfigFirebase(env);
            services.IntergrateJwtFirebase(configuration);

            return services;
        }

        //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
        //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
        private static IServiceCollection InjectDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("SQLServerChillgoDB");
            services.AddDbContext<ChillgoDbContext>(options => options.UseSqlServer(connectionString));
            return services;
        }

        private static IServiceCollection InjectBusinessServices(this IServiceCollection services)
        {
            services.AddScoped<IServiceFactory, ServiceFactory>();
            //---------------------------------------------------
            services.AddScoped<IAccountService, AccountService>();

            //Add other BusinessServices here...

            return services;
        }

        private static IServiceCollection InjectRepository(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            //---------------------------------------------------------------------------
            services.AddScoped<IAccountRepository, AccountRepository>();

            //Add other repository here...

            return services;
        }

        //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
        //XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX
        private static IServiceCollection ConfigKebabCase(this IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.Conventions.Add(new RouteTokenTransformerConvention(new KebabRouteTransform()));
            }).AddNewtonsoftJson(options =>
            {//If using NewtonSoft in project then must orride default Naming rule of System.text
                options.SerializerSettings.ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new KebabCaseNamingStrategy()
                };
            });

            services.AddSwaggerGen(c => { c.SchemaFilter<KebabSwaggerSchema>(); });
            return services;
        }

        private static IServiceCollection ConfigMapster(this IServiceCollection services)
        {
            //services.AddMapster();
            //TypeAdapterConfig<AccountRequested, Account>.NewConfig().IgnoreNullValues(true);
            //TypeAdapterConfig<OrderDetail_InfoDto, OrderDetail>.NewConfig().IgnoreNullValues(true)
            //    .Map(destination => destination.Id, startFrom => startFrom.OrderDetailId);
            return services;
        }

        private static IServiceCollection ConfigFluentEmail(this IServiceCollection services, IConfiguration configuration)
        {
            string defaultFromEmail = configuration["FluentEmail:Email"]!;
            string host = configuration["FluentEmail:Host"]!;
            int port = int.Parse(configuration["FluentEmail:Port"]!);
            string username = configuration["FluentEmail:Email"]!;
            string password = configuration["FluentEmail:Password"]!;

            services.AddFluentEmail(defaultFromEmail)
                    .AddSmtpSender(host, port, username, password);
            return services;
        }

        private static IServiceCollection ConfigCORS(this IServiceCollection services)
        {
            services.AddCors(options => options.AddPolicy("AllowAll", b => b.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod()));
            return services;
        }

        private static IServiceCollection IntergrateJwtFirebase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHttpClient<IAuthenticationService, AuthenticationService>(httpClient =>
            {
                httpClient.BaseAddress = new Uri(configuration["Authentication:TokenUri"]!);
            });

            services.AddAuthentication().AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, jwtOptions =>
            {
                jwtOptions.Authority = configuration["Authentication:ValidIssuer"];
                jwtOptions.Audience = configuration["Authentication:Audience"];
                jwtOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = configuration["Authentication:ValidIssuer"],
                    ValidateAudience = true,
                    ValidAudience = configuration["Authentication:Audience"],
                    ValidateLifetime = true,
                    IssuerSigningKeyResolver = (token, securityToken, kid, validationParameters) =>
                    {
                        // Download public key Google
                        var client = new HttpClient();
                        var keys = client.GetStringAsync("https://www.googleapis.com/robot/v1/metadata/x509/securetoken@system.gserviceaccount.com").Result;
                        var keyDictionary = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(keys);

                        // Search kid key with public key
                        if (keyDictionary.TryGetValue(kid, out var key))
                        {
                            var cert = new X509Certificate2(Encoding.UTF8.GetBytes(key));
                            var publicKey = cert.GetRSAPublicKey();
                            return new[] { new RsaSecurityKey(publicKey) };
                        }

                        throw new SecurityTokenSignatureKeyNotFoundException($"Unable to find key with kid: {kid}");
                    }
                };
            });
            return services;
        }
        private static IServiceCollection ConfigFirebase(this IServiceCollection services, IWebHostEnvironment env)
        {
            if (FirebaseApp.DefaultInstance == null)
            {
                string solutionDirectory = Path.GetFullPath(Path.Combine(env.ContentRootPath, ".."));

                string filePath = Path.Combine(solutionDirectory, "chillgo-firebase.json");

                FirebaseApp.Create(new AppOptions
                {
                    Credential = GoogleCredential.FromFile(filePath)
                });
            }
            return services;
        }
    }
}
