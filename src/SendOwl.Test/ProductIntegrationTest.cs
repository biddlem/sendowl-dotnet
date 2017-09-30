using SendOwl;
using Shouldly;
using System.Threading.Tasks;
using Xunit;

namespace SendOwl.Test
{
    public class ProductIntegrationTest
    {
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
    }
}
