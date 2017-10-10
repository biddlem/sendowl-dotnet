using Microsoft.Extensions.Configuration;
using SendOwl.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SendOwl.Test
{
    public class APIClientFixture : IDisposable
    {
        private readonly Guid Id = Guid.NewGuid();
        private static TestSettings Settings { get; } = new TestSettings();
        public Lazy<List<long>> ExistingProductIds { get; }
        public SendOwlAPIClient SendOwlAPIClient { get; }
        public List<long> CreatedProductIds { get; } = new List<long>(8);
        public List<int> CreatedBundleIds { get; } = new List<int>(8);
        public List<int> CreatedSubscriptionIds { get; } = new List<int>(8);

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
            SendOwlAPIClient = new SendOwlAPIClient(Settings.SendOwl_Key, Settings.SendOwl_Secret);

            ExistingProductIds = new Lazy<List<long>>(() =>
            {
                var tasks = new List<Task<long>>();
                for (int i = 0; i < 2; i++)
                {
                    var task = SendOwlAPIClient.Product.CreateAsync(new SendOwlProduct
                    {
                        Name = $"Bundle product {i} [test]",
                        Price = $"1{i}.99",
                        Product_type = ProductType.digital
                    }).ContinueWith(p => { CreatedProductIds.Add(p.Result.Id); return p.Result.Id; });
                    tasks.Add(task);
                }
                return Task.WhenAll(tasks).GetAwaiter().GetResult().ToList();
            });
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
                    tasks.AddRange(CreatedSubscriptionIds.Select(x => SendOwlAPIClient.Subscription.DeleteAsync(x)));
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
