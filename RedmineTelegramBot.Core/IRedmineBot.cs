using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedmineTelegramBot.Core
{
    public interface IRedmineBot
    {
        Task Start();
        Task Stop();
    }
}
