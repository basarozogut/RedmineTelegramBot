using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedmineTelegramBot.Core.Modules
{
    public static class ModuleRegistrar
    {
        public static IServiceCollection RegisterModule(this IServiceCollection serviceCollection, IDependencyModule module)
        {
            module.Load(serviceCollection);

            return serviceCollection;
        }
    }
}
