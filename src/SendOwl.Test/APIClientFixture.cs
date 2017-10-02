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
        public List<int> CreatedBundleIds { get; }

        public APIClientFixture()
        {
            var key = System.Environment.GetEnvironmentVariable("sendowl_key");
            var secret = System.Environment.GetEnvironmentVariable("sendowl_secret");
            SendOwlAPIClient = new SendOwlAPIClient(key, secret);
            CreatedProductIds = new List<long>();
            CreatedBundleIds = new List<int>();
        }

        public void Dispose()
        {
            if (SendOwlAPIClient != null)
            {
                try
                {
                    var tasks = new List<Task>();
                    tasks.AddRange(CreatedProductIds.Select(x => SendOwlAPIClient.Product.DeleteAsync(x)));
                    tasks.AddRange(CreatedBundleIds.Select(x => SendOwlAPIClient.Bundle.DeleteAsync(x)));
                    Task.WhenAll(tasks).GetAwaiter().GetResult();
                }
                catch
                {
                    //ignored
                }
            }
        }
    }
}
