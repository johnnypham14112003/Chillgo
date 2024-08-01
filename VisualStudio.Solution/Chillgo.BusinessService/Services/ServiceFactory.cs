using Chillgo.BusinessService.Interfaces;
using Chillgo.Repository.Interfaces;
using FluentEmail.Core;
using Microsoft.Extensions.Configuration;
using Razor.Templating.Core;

namespace Chillgo.BusinessService.Services
{
    public class ServiceFactory : IServiceFactory
    {
        private readonly Lazy<IAccountService> _accountService;
        public ServiceFactory(
            IConfiguration configuration,
            IUnitOfWork unitOfWork,
            IFluentEmailFactory fluentEmailFactory,
            IRazorTemplateEngine razorTemplateEngine)
        {
            _accountService = new Lazy<IAccountService>(() => new AccountService(unitOfWork));
        }

    public IAccountService GetAccountService()
    {
        return _accountService.Value;
    }
}
}
