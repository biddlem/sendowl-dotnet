using SendOwl.Endpoints;
using SendOwl.Model;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SendOwl.Test
{
    public class BundleIntegrationTest : IClassFixture<APIClientFixture>
    {
        private const string TestBundleName = "my-test-bundle";
        private readonly List<int> CreatedBundleIds;
        private readonly Lazy<List<long>> ExistingProductIds;
        private readonly BundleEndpoint endpoint;

        public BundleIntegrationTest(APIClientFixture fixture)
        {
            endpoint = fixture.SendOwlAPIClient.Bundle;
            CreatedBundleIds = fixture.CreatedBundleIds;
            ExistingProductIds = fixture.ExistingProductIds;
        }

        [Fact]
        public async Task GetAllAsync()
        {
            if(!CreatedBundleIds.Any()) await CreateBundle();
            var bundles = await endpoint.GetAllAsync();
            bundles.ShouldNotBeEmpty();
        }

        [Fact]
        public async Task GetAsync()
        {
            var existing = await CreateBundle();
            var bundle = await endpoint.GetAsync(existing.Id);
            bundle.ShouldNotBeNull();
            bundle.Id.ShouldBe(existing.Id);
        }

        [Fact]
        public async Task SearchAsync()
        {
            if (!CreatedBundleIds.Any()) await CreateBundle();
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
                    Product_ids = ExistingProductIds.Value
                }
            };
            var result = await endpoint.CreateAsync(bundle);
            CreatedBundleIds.Add(result.Id);
            result.Id.ShouldBeGreaterThan(1);
            result.Name.ShouldBe(bundle.Name);
            result.Price.ShouldBe(bundle.Price);
            result.Components.Product_ids.OrderBy(p => p).ToList()
                .ShouldBe(bundle.Components.Product_ids.OrderBy(p => p).ToList());
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
                        ExistingProductIds.Value.First()
                    }
                }
            };

            var created = await endpoint.CreateAsync(bundle);
            await Task.Delay(5000); //API returns 500 if updating too fast after creation
            CreatedBundleIds.Add(created.Id);
            created.Price.ShouldBe(bundle.Price);
            created.Name.ShouldBe(bundle.Name);

            created.Price = "5.00";
            created.Components.Product_ids.Add(ExistingProductIds.Value.Last());

            var updated = await endpoint.UpdateAsync(created);
            updated.Price.ShouldBe(created.Price);
            updated.Components.Product_ids.OrderBy(p => p).ToList()
                .ShouldBe(created.Components.Product_ids.OrderBy(p => p).ToList());

        }

        [Fact]
        public async Task DeleteAsync()
        {
            var bundle = await CreateBundle();

            var result = await endpoint.CreateAsync(bundle);
            CreatedBundleIds.Add(result.Id);
            result.ShouldNotBeNull();
            await Task.Delay(5000); //API returns 500 if deleting too fast after creation
            await endpoint.DeleteAsync(result.Id);
        }

        private async Task<SendOwlBundle> CreateBundle()
        {
            var bundle = await endpoint.CreateAsync(new SendOwlBundle
            {
                Name = TestBundleName,
                Price = "99.99",
                Components = new Components
                {
                    Product_ids = ExistingProductIds.Value
                }
            });

            CreatedBundleIds.Add(bundle.Id);
            return bundle;
        }
    }
}
