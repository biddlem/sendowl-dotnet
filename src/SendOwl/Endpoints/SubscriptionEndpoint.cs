using SendOwl.Model;

namespace SendOwl.Endpoints
{
    public class SubscriptionEndpoint : HTTPEndpoint<SendOwlSubscription, SendOwlSubscriptionListItem, int>
    {
        public override string Path => "subscriptions";

        public SubscriptionEndpoint(IHttpSerializerClient client)
            : base(client)
        { }
    }
}
