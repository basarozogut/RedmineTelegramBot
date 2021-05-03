using RestSharp;

namespace RedmineTelegramBot.Core
{
    public interface IRestClientFactory
    {
        public IRestClient CreateRestClient();
    }
}
