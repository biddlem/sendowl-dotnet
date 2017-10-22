using SendOwl.Endpoints;
using System;

namespace SendOwl
{
    public class SendOwlAPIClient : IDisposable
    {
        private const string BaseUrl = "https://www.sendowl.com/api/v1/";
        internal readonly IHttpSerializerClient client;
        public ProductEndpoint Product => new ProductEndpoint(client);
        public BundleEndpoint Bundle => new BundleEndpoint(client);
        public SubscriptionEndpoint Subscription => new SubscriptionEndpoint(client);
        public DiscountEndpoint Discount => new DiscountEndpoint(client);
        public OrderEndpoint Order => new OrderEndpoint(client);

        public SendOwlAPIClient(string apiKey, string apiSecret)
            :this(new HttpSerializerClient(BaseUrl, apiKey, apiSecret))
        { }

        internal SendOwlAPIClient(IHttpSerializerClient client)
        {
            this.client = client;
        }

        public void Dispose()
        {
            client?.Dispose();
        }
    }
}
