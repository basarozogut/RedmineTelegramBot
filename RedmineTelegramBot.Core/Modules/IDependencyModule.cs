using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedmineTelegramBot.Core.Modules
{
    public interface IDependencyModule
    {
        void Load(IServiceCollection services);
    }
}
