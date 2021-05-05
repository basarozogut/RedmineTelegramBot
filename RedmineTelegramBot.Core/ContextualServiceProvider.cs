using RedmineTelegramBot.Core;
using System;

namespace RedmineTelegramBot.Core
{
    public class ContextualServiceProvider : IServiceProvider
    {
        private readonly IWorkContext _workContext;
        private readonly IServiceProvider _wrappedServiceProvider;

        public ContextualServiceProvider(IWorkContext workContext, IServiceProvider wrappedServiceProvider)
        {
            _workContext = workContext;
            _wrappedServiceProvider = wrappedServiceProvider;
        }

        public object GetService(Type serviceType)
        {
            if (serviceType == typeof(IWorkContext))
            {
                return _workContext;
            }
            else
            {
                return _wrappedServiceProvider.GetService(serviceType);
            }
        }
    }
}
