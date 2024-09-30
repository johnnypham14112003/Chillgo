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


namespace Chillgo.Api
{
    public static class ServiceRegistration
    {
        public static IServiceCollection DependencyInjectionServices(this IServiceCollection services, IConfiguration configuration)
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
            services.ConfigFirebase(configuration);

            return services;
        }

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
        //----------------------------------------------------------------------------------
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

        private static IServiceCollection ConfigFirebase(this IServiceCollection services, IConfiguration configuration)
        {
            var firebaseConfig = new JObject();

            //Read from appsettings
            var appsettingsConfig = configuration.GetSection("FirebaseAdmin").Get<Dictionary<string, string>>();
            foreach (var item in appsettingsConfig)
            {
                firebaseConfig[item.Key] = item.Value;
            }

            //Convert Jobject to json
            string jsonConfig = JsonConvert.SerializeObject(firebaseConfig);
            if (FirebaseApp.DefaultInstance == null)
            {
                FirebaseApp.Create(new AppOptions
                {
                    Credential = GoogleCredential.FromJson(jsonConfig)
                });
            }

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = $"https://securetoken.google.com/{firebaseConfig["project_id"]}";
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = $"https://securetoken.google.com/{firebaseConfig["project_id"]}",
                        ValidateAudience = true,
                        ValidAudience = firebaseConfig["project_id"]!.ToString(),
                        ValidateLifetime = true,
                        IssuerSigningKeyResolver = (token, securityToken, kid, parameters) =>
                        {
                            // Download public keys from Firebase
                            var httpClient = new HttpClient();
                            var keys = httpClient.GetStringAsync("https://www.googleapis.com/robot/v1/metadata/x509/securetoken@system.gserviceaccount.com").Result;

                            // Parse response
                            var x509Keys = JObject.Parse(keys);

                            // find public key by kid (Key ID)
                            if (x509Keys.ContainsKey(kid))
                            {
                                var pubKey = x509Keys[kid].ToString();
                                var certificate = new X509Certificate2(Encoding.UTF8.GetBytes(pubKey));

                                // convert X.509 certificate to RSA public key
                                var rsa = certificate.GetRSAPublicKey();
                                return new[] { new RsaSecurityKey(rsa) };
                            }

                            return null;
                        }
                    };
                });
            return services;
        }
    }
}
