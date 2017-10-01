using SendOwl.Endpoints;
using SendOwl.Model;
using Shouldly;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace SendOwl.Test
{
    public class ProductIntegrationTest : IClassFixture<APIClientFixture>
    {
        private const string TestProductName = "my-test-product";
        private readonly List<long> CreatedProductIds;
        private readonly ProductEndpoint endpoint;

        public ProductIntegrationTest(APIClientFixture fixture)
        {
            endpoint = fixture.SendOwlAPIClient.Product;
            CreatedProductIds = fixture.CreatedProductIds;
        }

        [Fact]
        public async Task GetAllAsync()
        {
            var products = await endpoint.GetAllAsync();
            products.ShouldNotBeEmpty();
            var count = products.Count;
        }

        [Fact]
        public async Task GetAsync()
        {
            var product = await endpoint.GetAsync(123456);
            product.ShouldNotBeNull();
        }

        [Fact]
        public async Task SearchAsync()
        {
            var products = await endpoint.SearchAsync("coinbase");
            products.ShouldNotBeEmpty();
        }

        [Fact]
        public async Task CreateAsync()
        {
            var product = new SendOwlProduct
            {
                Name = TestProductName,
                Price = "18.99",
                Product_type = ProductType.Software
            };

            var result = await endpoint.CreateAsync(product);
            CreatedProductIds.Add(result.Id);
            result.Name.ShouldBe(product.Name);
            result.Price.ShouldBe(product.Price);
            result.Product_type.ShouldBe(product.Product_type);
            result.Id.ShouldBeGreaterThan(1);
        }

        [Fact(Skip = "not working because SendOwl bug - (Cloudflare - are you human?) response)")]
        public async Task CreateAsync_With_File_Upload()
        {
            var product = new SendOwlProduct
            {
                Name = TestProductName + "[Cat]",
                Price = "1.99",
                Product_type = ProductType.Digital,
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
            CreatedProductIds.Add(created.Id);
            created.Price.ShouldBe(product.Price);
            created.Name.ShouldBe(product.Name);

            created.Price = "5.00";
            created.Product_type = ProductType.Digital;

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
