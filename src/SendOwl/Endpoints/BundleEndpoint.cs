using SendOwl.Model;

namespace SendOwl.Endpoints
{
    public class BundleEndpoint : HTTPEndpoint<SendOwlBundle, SendOwlBundleListItem, int>
    {
        public override string Path => "packages";

        public BundleEndpoint(IHttpSerializerClient client)
            : base(client)
        { }
    }
}
