using Chillgo.BusinessService.Interfaces;
using Chillgo.BusinessService.Services;
using Chillgo.Repository;
using Chillgo.Repository.Interfaces;
using Chillgo.Repository.Models;
using Chillgo.Repository.Repositories;
using Mapster;
using Microsoft.EntityFrameworkCore;


namespace Chillgo.Api
{
    public static class ServiceRegistration
    {
        public static IServiceCollection DependencyInjectionServices (this IServiceCollection services, IConfiguration configuration)
        {
            //System Services
            services.InjectDbContext(configuration);
            services.InjectBusinessServices();
            services.InjectRepository();
            services.ConfigCORS();

            //Third Party Services
            services.ConfigFluentEmail(configuration);
            services.AddRazorTemplating();
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
            services.AddScoped<ILocationService, LocationService>();
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
            //Add other repository here...

            return services;
        }
        //----------------------------------------------------------------------------------
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
    }
}
