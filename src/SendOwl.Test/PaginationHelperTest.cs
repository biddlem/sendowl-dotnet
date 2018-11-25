using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using SendOwl.Model;
using Shouldly;
using Xunit;

namespace SendOwl.Test
{
    public class PaginationHelperTest
    {
        private const int ItemsPerPage = 50;

        public static IEnumerable<object[]> OnePageOfResults
        {
            get
            {
                for (var i = 1; i <= ItemsPerPage - 1; i++)
                {
                    yield return new object[] { i };
                }
            }
        }

        public static IEnumerable<object[]> UpToTwoFullPagesOfResults
        {
            get
            {
                for (var i = 1; i <= ItemsPerPage * 2; i++)
                {
                    yield return new object[] { i };
                }
            }
        }

        [Fact]
        public async Task GetAllAsync_No_Results_Only_Makes_One_Api_Call()
        {
            var mockClient = new Mock<IHttpSerializerClient>();
            var mockProducts = new List<SendOwlProductListItem>();

            mockClient.Setup(x => x.GetAsync<IEnumerable<SendOwlProductListItem>>(It.Is<string>(i => i == $"products?per_page={ItemsPerPage}&page=1")))
                .Returns(Task.FromResult<IEnumerable<SendOwlProductListItem>>(mockProducts));

            var result = await PaginationHelper.GetAllAsync<SendOwlProduct, SendOwlProductListItem>(mockClient.Object, "products", s => s.Value).ConfigureAwait(false);

            result.Count.ShouldBe(0);

            mockClient.Verify(x => x.GetAsync<IEnumerable<SendOwlProductListItem>>(It.IsAny<string>()), Times.Once);
        }

        [Theory]
        [MemberData(nameof(OnePageOfResults))]
        public async Task GetAllAsync_Less_Than_50_Results_Only_Makes_One_Api_Call(int numberOfResults)
        {
            var mockClient = new Mock<IHttpSerializerClient>();
            var mockProducts = new List<SendOwlProductListItem>();

            for (var i = 1; i <= numberOfResults; i++)
            {
                mockProducts.Add(new SendOwlProductListItem { Value = new SendOwlProduct() });
            }

            mockClient.Setup(x => x.GetAsync<IEnumerable<SendOwlProductListItem>>(It.Is<string>(i => i == $"products?per_page={ItemsPerPage}&page=1")))
                .Returns(Task.FromResult<IEnumerable<SendOwlProductListItem>>(mockProducts));

            var result = await PaginationHelper.GetAllAsync<SendOwlProduct, SendOwlProductListItem>(mockClient.Object, "products", s => s.Value).ConfigureAwait(false);

            result.Count.ShouldBe(numberOfResults);

            mockClient.Verify(x => x.GetAsync<IEnumerable<SendOwlProductListItem>>(It.IsAny<string>()), Times.Once);
        }

        [Theory]
        [MemberData(nameof(UpToTwoFullPagesOfResults))]
        public async Task GetAllAsync_Makes_Correct_Number_Of_Api_Calls(int numberOfResults)
        {
            var mockClient = new Mock<IHttpSerializerClient>();
            var mockProducts = new List<SendOwlProductListItem>();

            for (var i = 1; i <= numberOfResults; i++)
            {
                mockProducts.Add(new SendOwlProductListItem { Value = new SendOwlProduct() });
            }

            mockClient.Setup(x => x.GetAsync<IEnumerable<SendOwlProductListItem>>(It.Is<string>(i => i == $"products?per_page={ItemsPerPage}&page=1")))
                .Returns(Task.FromResult(mockProducts.Take(ItemsPerPage)));

            mockClient.Setup(x => x.GetAsync<IEnumerable<SendOwlProductListItem>>(It.Is<string>(i => i == $"products?per_page={ItemsPerPage}&page=2")))
                .Returns(Task.FromResult(mockProducts.Skip(ItemsPerPage).Take(ItemsPerPage)));

            mockClient.Setup(x => x.GetAsync<IEnumerable<SendOwlProductListItem>>(It.Is<string>(i => i == $"products?per_page={ItemsPerPage}&page=3")))
                .Returns(Task.FromResult(mockProducts.Take(0)));

            var result = await PaginationHelper.GetAllAsync<SendOwlProduct, SendOwlProductListItem>(mockClient.Object, "products", s => s.Value).ConfigureAwait(false);

            result.Count.ShouldBe(numberOfResults);

            int expectedNumberOfApiCalls = numberOfResults % ItemsPerPage == 0 ? (mockProducts.Count - 1) / ItemsPerPage + 2 : (mockProducts.Count - 1) / ItemsPerPage + 1;

            mockClient.Verify(x => x.GetAsync<IEnumerable<SendOwlProductListItem>>(It.IsAny<string>()), Times.Exactly(expectedNumberOfApiCalls));
        }
    }
}
