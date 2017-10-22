using SendOwl.Endpoints;
using Shouldly;
using System.Threading.Tasks;
using Xunit;

namespace SendOwl.Test
{
    public class OrderIntegrationTest : IClassFixture<APIClientFixture>
    {
        private readonly OrderEndpoint endpoint;

        public OrderIntegrationTest(APIClientFixture fixture)
        {
            endpoint = fixture.SendOwlAPIClient.Order;
        }

        [Fact]
        public async Task GetAllAsync()
        {
            var orders = await endpoint.GetAllAsync();
            orders.ShouldNotBeEmpty();
        }
    }
}
