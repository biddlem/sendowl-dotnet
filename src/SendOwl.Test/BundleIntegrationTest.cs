using SendOwl.Endpoints;
using SendOwl.Model;
using Shouldly;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SendOwl.Test
{
    public class BundleIntegrationTest : IClassFixture<APIClientFixture>
    {
        private readonly int bundleId;
        private readonly List<long> productIds;
        private const string TestBundleName = "my-test-bundle";
        private readonly List<int> CreatedBundleIds;
        private readonly BundleEndpoint endpoint;

        public BundleIntegrationTest(APIClientFixture fixture)
        {
            endpoint = fixture.SendOwlAPIClient.Bundle;
            CreatedBundleIds = fixture.CreatedBundleIds;
            bundleId = fixture.ExistingBundleId;
            productIds = fixture.ExistingProductIds;
        }

        [Fact]
        public async Task GetAllAsync()
        {
            var bundles = await endpoint.GetAllAsync();
            bundles.ShouldNotBeEmpty();
        }

        [Fact]
        public async Task GetAsync()
        {
            var bundle = await endpoint.GetAsync(bundleId);
            bundle.ShouldNotBeNull();
        }

        [Fact]
        public async Task SearchAsync()
        {
            var bundles = await endpoint.SearchAsync("test");
            bundles.ShouldNotBeEmpty();
        }

        [Fact]
        public async Task CreateAsync()
        {
            var bundle = new SendOwlBundle
            {
                Name = TestBundleName,
                Price = "99.99",
                Components = new Components
                {
                    Product_ids = productIds
                }
            };
            var result = await endpoint.CreateAsync(bundle);
            CreatedBundleIds.Add(result.Id);
            result.Id.ShouldBeGreaterThan(1);
            result.Name.ShouldBe(bundle.Name);
            result.Price.ShouldBe(bundle.Price);
            result.Components.Product_ids.ShouldBe(bundle.Components.Product_ids);
        }

        [Fact]
        public async Task UpdateAsync()
        {
            var bundle = new SendOwlBundle
            {
                Name = TestBundleName + "[Update]",
                Price = "140.00",
                Components = new Components
                {
                    Product_ids = new List<long>
                    {
                        productIds.First()
                    }
                }
            };

            var created = await endpoint.CreateAsync(bundle);
            CreatedBundleIds.Add(created.Id);
            created.Price.ShouldBe(bundle.Price);
            created.Name.ShouldBe(bundle.Name);

            created.Price = "5.00";
            created.Components.Product_ids.Add(productIds.Last());

            var updated = await endpoint.UpdateAsync(created);
            updated.Price.ShouldBe(created.Price);
            updated.Components.Product_ids.ShouldBe(created.Components.Product_ids);
        }

        [Fact]
        public async Task DeleteAsync()
        {
            var bundle = new SendOwlBundle
            {
                Name = TestBundleName + "[Delete]",
                Components = new Components
                {
                    Product_ids = new List<long>()
                    {
                        productIds.First()
                    }
                }
            };

            var result = await endpoint.CreateAsync(bundle);
            CreatedBundleIds.Add(result.Id);
            result.ShouldNotBeNull();
            await Task.Delay(5000); //API returns 500 if deleting too fast after creation
            await endpoint.DeleteAsync(result.Id);
        }
    }
}
