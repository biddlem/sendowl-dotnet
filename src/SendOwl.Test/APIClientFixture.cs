using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SendOwl.Test
{
    public class APIClientFixture : IDisposable
    {
        public SendOwlAPIClient SendOwlAPIClient { get;}
        public List<long> CreatedProductIds { get; }

        public APIClientFixture()
        {
            SendOwlAPIClient = new SendOwlAPIClient("key", "secret");
            CreatedProductIds = new List<long>();
        }

        public void Dispose()
        {
            if (SendOwlAPIClient != null)
            {
                try
                {
                    Task.WhenAll(CreatedProductIds.Select(x => SendOwlAPIClient.Product.DeleteAsync(x)))
                        .GetAwaiter().GetResult();
                }
                catch
                {
                    //ignored
                }
            }
        }
    }
}
