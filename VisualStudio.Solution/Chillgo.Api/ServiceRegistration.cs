using Chillgo.BusinessService.Interfaces;
using Chillgo.BusinessService.Services;
using Chillgo.Repository;
using Chillgo.Repository.Interfaces;
using Chillgo.Repository.Repositories;
using Chillgo.BusinessService.Extensions;

using System.Text;
using System.Text.Json;
using System.Security.Cryptography.X509Certificates;

using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Newtonsoft.Json.Serialization;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;


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
            services.ConfigFirebase(env);
            services.IntergrateJwtFirebase(env);

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
            services.AddScoped<ILocationService, LocationService>();
            services.AddScoped<IConversationService, ConversationService>();
            services.AddScoped<IMessageService, MessageService>();
            services.AddScoped<IPackageService, PackageService>();
            services.AddScoped<IPackageTransactionService, PackageTransactionService>();
            
            services.AddScoped<IFirebaseStorageService, FirebaseStorageService>();
            //Add other BusinessServices here...

            return services;
        }

        private static IServiceCollection InjectRepository(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            //---------------------------------------------------------------------------
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<ILocationRepository, LocationRepository>();
            services.AddScoped<IConversationRepository, ConversationRepository>();
            services.AddScoped<IMessageRepository, MessageRepository>();
            services.AddScoped<IPackageRepository, PackageRepository>();
            services.AddScoped<IPackageTransactionRepository, PackageTransactionRepository>();
            
            services.AddScoped<IImageRepository, ImageRepository>();
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

        private static IServiceCollection ConfigCORS(this IServiceCollection services)
        {
            services.AddCors(options => options.AddPolicy("AllowAll", b => b.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod()));
            return services;
        }

        private static IServiceCollection IntergrateJwtFirebase(this IServiceCollection services, IWebHostEnvironment env)
        {
            //Read attributes from Firebase secret file
            string solutionDirectory = Directory.GetParent(env.ContentRootPath).FullName;
            string firebaseConfigPath = Path.Combine(solutionDirectory, "chillgo-firebase.json");

            var firebaseConfig = System.Text.Json.JsonSerializer.Deserialize<JsonElement>(System.IO.File.ReadAllText(firebaseConfigPath));
            var firebaseSecret = firebaseConfig.GetProperty("firebase_secret");

            string apiKey = firebaseSecret.GetProperty("api_key").GetString();
            string projectId = firebaseSecret.GetProperty("project_id").GetString();

            services.AddHttpClient<IAuthenticationService, AuthenticationService>(httpClient =>
            {
                //httpClient.BaseAddress = new Uri(configuration["Authentication:TokenUri"]!);
                httpClient.BaseAddress = new Uri($"https://identitytoolkit.googleapis.com/v1/accounts:signInWithCustomToken?key={apiKey}");
            });

            services.AddAuthentication().AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, jwtOptions =>
            {
                jwtOptions.Authority = $"https://securetoken.google.com/{projectId}";
                jwtOptions.Audience = projectId;
                jwtOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = $"https://securetoken.google.com/{projectId}",
                    ValidateAudience = true,
                    ValidAudience = projectId,
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
            string solutionDirectory = Directory.GetParent(env.ContentRootPath).FullName;
            string firebaseConfigPath = Path.Combine(solutionDirectory, "chillgo-firebase.json");

            var firebaseConfig = System.Text.Json.JsonSerializer.Deserialize<JsonElement>(System.IO.File.ReadAllText(firebaseConfigPath));
            var firebaseSecret = firebaseConfig.GetProperty("firebase_secret");

            if (FirebaseApp.DefaultInstance == null)
            {
                var credentialJson = System.Text.Json.JsonSerializer.Serialize(firebaseSecret);

                //Old config for authen
                //FirebaseApp.Create(new AppOptions
                //{
                //    Credential = GoogleCredential.FromJson(credentialJson)
                //});

                //New Config for Firebase storage
                var credential = GoogleCredential.FromJson(credentialJson)
                    .CreateScoped(new[]
                    {
                        "https://www.googleapis.com/auth/firebase.storage",
                        "https://www.googleapis.com/auth/cloud-platform"
                    });

                FirebaseApp.Create(new AppOptions
                {
                    Credential = credential
                });
                //---------------------------------
            }

            //For Firebase storage
            var bucketName = firebaseSecret.GetProperty("bucket").GetString();
            services.AddSingleton(bucketName!);
            //--------------------
            return services;
        }
    }
}
