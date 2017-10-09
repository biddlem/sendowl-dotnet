using SendOwl.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

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
