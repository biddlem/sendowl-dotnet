using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SendOwl.Test
{
    public class APIClientFixture : IDisposable
    {
        private static TestSettings Settings { get; } = new TestSettings();
        public List<long> ExistingProductIds { get; }
        public int ExistingBundleId { get; }
        public SendOwlAPIClient SendOwlAPIClient { get;}
        public List<long> CreatedProductIds { get; } = new List<long>();
        public List<int> CreatedBundleIds { get; } = new List<int>();

        static APIClientFixture()
        {
            var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("testsettings.json", optional: true)
            .AddEnvironmentVariables()
            .Build();
            configuration.Bind(Settings);
        }

        public APIClientFixture()
        {
            ExistingProductIds = Settings.SendOwl_ProductIds
                .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => long.Parse(x)).ToList();
            ExistingBundleId = Settings.SendOwl_BundleId;
            SendOwlAPIClient = new SendOwlAPIClient(Settings.SendOwl_Key, Settings.SendOwl_Secret);
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
