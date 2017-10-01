using SendOwl.Model;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace SendOwl.Test
{
    public class ProductIntegrationTest : IDisposable
    {
        private const string TestProductName = "my-test-product";
        private static List<long> CreatedProductIds = new List<long>();
        SendOwlAPIClient sendOwl = new SendOwlAPIClient("key", "secret");

        [Fact]
        public async Task GetAllAsync()
        {
            var products = await sendOwl.Product.GetAllAsync();
            products.ShouldNotBeEmpty();
            var count = products.Count;
        }

        [Fact]
        public async Task GetAsync()
        {
            var product = await sendOwl.Product.GetAsync(123456);
            product.ShouldNotBeNull();
        }

        [Fact]
        public async Task SearchAsync()
        {
            var products = await sendOwl.Product.SearchAsync("coinbase");
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

            var result = await sendOwl.Product.CreateAsync(product);
            CreatedProductIds.Add(result.Id);
            result.Name.ShouldBe(product.Name);
            result.Price.ShouldBe(product.Price);
            result.Product_type.ShouldBe(product.Product_type);
            result.Id.ShouldBeGreaterThan(1);
        }

        [Fact]
        public async Task DeleteAsync()
        {
            var product = new SendOwlProduct
            {
                Name = TestProductName + "[Delete]"
            };

            var result = await sendOwl.Product.CreateAsync(product);
            CreatedProductIds.Add(result.Id);
            result.ShouldNotBeNull();
            await sendOwl.Product.DeleteAsync(result.Id);
        }

        public void Dispose()
        {
            if(sendOwl != null)
            {
                try
                {
                    Task.WhenAll(CreatedProductIds.Select(x => sendOwl.Product.DeleteAsync(x)))
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
