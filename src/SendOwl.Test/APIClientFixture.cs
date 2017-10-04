using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SendOwl.Test
{
    public class APIClientFixture : IDisposable
    {
        public List<long> ExistingProductIds { get; }
        public int ExistingBundleId { get; }
        public SendOwlAPIClient SendOwlAPIClient { get;}
        public List<long> CreatedProductIds { get; }
        public List<int> CreatedBundleIds { get; }

        public APIClientFixture()
        {
            var key = GetVariable("sendowl_key");
            var secret = GetVariable("sendowl_secret");
            ExistingProductIds = GetVariable("sendowl_productids").Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).Select(x => long.Parse(x)).ToList();
            ExistingBundleId = int.Parse(GetVariable("sendowl_bundleid"));
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

        public string GetVariable(string name)
        {
            var variable = Environment.GetEnvironmentVariable(name);

#if NETSTANDARD2_0
            if(string.IsNullOrWhiteSpace(variable)) variable = Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.User)
#endif
            return variable;
        }
    }
}
