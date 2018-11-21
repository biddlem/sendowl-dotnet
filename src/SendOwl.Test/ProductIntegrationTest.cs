using SendOwl.Endpoints;
using SendOwl.Model;
using Shouldly;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SendOwl.Test
{
    public class ProductIntegrationTest : IClassFixture<APIClientFixture>
    {
        private readonly Lazy<List<long>> ExistingProductIds;
        private const string TestProductName = "my-test-product";
        private readonly List<long> CreatedProductIds;
        private readonly ProductEndpoint endpoint;

        public ProductIntegrationTest(APIClientFixture fixture)
        {
            endpoint = fixture.SendOwlAPIClient.Product;
            CreatedProductIds = fixture.CreatedProductIds;
            ExistingProductIds = fixture.ExistingProductIds;
        }

        [Fact]
        public async Task GetAllAsync()
        {
            var existing = ExistingProductIds.Value;
            var products = await endpoint.GetAllAsync();
            products.Count().ShouldBeGreaterThanOrEqualTo(existing.Count);
        }

        [Fact]
        public async Task GetAsync()
        {
            var product = await endpoint.GetAsync(ExistingProductIds.Value.First());
            product.ShouldNotBeNull();
        }

        [Fact]
        public async Task SearchAsync()
        {
            var existing = ExistingProductIds.Value;
            await Task.Delay(3000); //takes a few sec for SendOwl to index...
            var products = await endpoint.SearchAsync("test");
            products.Count().ShouldBeGreaterThanOrEqualTo(existing.Count);
        }

        [Fact]
        public async Task ShopifyLookupAsync()
        {
            var shopifyVariantId = long.MaxValue;

            var shopifyProduct = new SendOwlProduct
            {
                Name = $"{TestProductName} [shopify test]",
                Price = "1.99",
                Product_type = ProductType.digital,
                Self_hosted_url = "http://test.com/file",
                Shopify_variant_id = shopifyVariantId
            };

            var created = await endpoint.CreateAsync(shopifyProduct);

            CreatedProductIds.Add(created.Id);

            await Task.Delay(3000); //takes a few sec for SendOwl to index...

            var result = await endpoint.ShopifyLookupAsync(shopifyVariantId);

            result.ShouldNotBeNull();
            result.Count.ShouldBe(1);

            var expectedResult = result.First();

            expectedResult.Name.ShouldBe(shopifyProduct.Name);
            expectedResult.Price.ShouldBe(shopifyProduct.Price);
            expectedResult.Product_type.ShouldBe(shopifyProduct.Product_type);
            expectedResult.Self_hosted_url.ShouldBe(shopifyProduct.Self_hosted_url);
            expectedResult.Shopify_variant_id.ShouldBe(shopifyProduct.Shopify_variant_id);
        }

        [Fact]
        public async Task CreateAsync()
        {
            var fileURL = "http://file.com/file";
            var product = new SendOwlProduct
            {
                Name = TestProductName,
                Price = "18.99",
                Product_type = ProductType.software,
                License_type = "MIT",
                Self_hosted_url = fileURL
            };

            var result = await endpoint.CreateAsync(product);
            result.Self_hosted_url.ShouldBe(fileURL);
            CreatedProductIds.Add(result.Id);
            result.Name.ShouldBe(product.Name);
            result.Price.ShouldBe(product.Price);
            result.Product_type.ShouldBe(product.Product_type);
            result.Id.ShouldBeGreaterThan(1);
            result.Add_to_cart_url.ShouldNotBeNull();
            result.Instant_buy_url.ShouldNotBeNull();
            result.Created_at.Date.ShouldBe(DateTime.UtcNow.Date);
            result.Updated_at.Date.ShouldBe(DateTime.UtcNow.Date);
        }

        [Fact]
        public async Task CreateAsync_With_File_Upload()
        {
            var product = new SendOwlProduct
            {
                Name = TestProductName + "[Cat]",
                Price = "1.99",
                Product_type = ProductType.digital,
            };

            using (var stream = File.OpenRead("cat.jpg"))
            {
                var result = await endpoint.CreateAsync(product, stream, "cat.jpg");
                CreatedProductIds.Add(result.Id);
                result.Name.ShouldBe(product.Name);
                result.Price.ShouldBe(product.Price);
                result.Product_type.ShouldBe(product.Product_type);
                result.Attachment.Filename.ShouldBe("cat.jpg");
                result.Id.ShouldBeGreaterThan(1);
            }
        }

        [Fact]
        public async Task UpdateAsync()
        {
            var product = new SendOwlProduct
            {
                Name = TestProductName + "[Update]",
                Price = "10.00"
            };

            var created = await endpoint.CreateAsync(product);
            await Task.Delay(5000); //API returns 500 if updating too fast after creation
            CreatedProductIds.Add(created.Id);
            created.Price.ShouldBe(product.Price);
            created.Name.ShouldBe(product.Name);

            created.Price = "5.00";
            created.Product_type = ProductType.service;

            var updatedProduct = await endpoint.UpdateAsync(created);
            updatedProduct.Price.ShouldBe(created.Price);
            updatedProduct.Product_type.ShouldBe(created.Product_type);
        }

        [Fact]
        public async Task DeleteAsync()
        {
            var product = new SendOwlProduct
            {
                Name = TestProductName + "[Delete]"
            };

            var result = await endpoint.CreateAsync(product);
            CreatedProductIds.Add(result.Id);
            result.ShouldNotBeNull();
            await Task.Delay(5000); //API returns 500 if deleting too fast after creation
            await endpoint.DeleteAsync(result.Id);
        }
    }
}
