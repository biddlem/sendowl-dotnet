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
    public class DiscountIntegrationTest : IClassFixture<APIClientFixture>
    {
        private readonly Lazy<List<long>> ExistingProductIds;
        private const string TestDiscountName = "my-test-discount";
        private readonly List<int> CreatedDiscountIds;
        private readonly DiscountEndpoint endpoint;

        public DiscountIntegrationTest(APIClientFixture fixture)
        {
            endpoint = fixture.SendOwlAPIClient.Discount;
            CreatedDiscountIds = fixture.CreatedDiscountIds;
            ExistingProductIds = fixture.ExistingProductIds;
        }

        [Fact]
        public async Task GetAllAsync()
        {
            if (!CreatedDiscountIds.Any()) await CreateDiscount();
            var discounts = await endpoint.GetAllAsync();
            discounts.ShouldNotBeEmpty();
        }

        [Fact]
        public async Task GetAsync()
        {
            var existing = await CreateDiscount();
            var discount = await endpoint.GetAsync(existing.Id);
            discount.ShouldNotBeNull();
            discount.Id.ShouldBe(existing.Id);
        }

        [Fact(Skip = "broken in upstream API?")]
        public async Task SearchAsync()
        {
            if (!CreatedDiscountIds.Any()) await CreateDiscount();
            var discounts = await endpoint.SearchAsync("test");
            discounts.ShouldNotBeEmpty();
        }

        [Fact]
        public async Task CreateAsync_Single_Code()
        {
            var discount = new SendOwlDiscount
            {
                Name = TestDiscountName + " single_code",
                Minimum_cart_value = "50.00",
                Code = "my-code",
                Usage_limit = 18,
                Fixed_discount = "14.99",
            };

            var result = await endpoint.CreateAsync(discount);
            CreatedDiscountIds.Add(result.Id);

            result.Name.ShouldBe(discount.Name);
            result.Minimum_cart_value.ShouldBe(discount.Minimum_cart_value);
            result.Usage_limit.ShouldBe(discount.Usage_limit);
            result.Fixed_discount.ShouldBe(discount.Fixed_discount);
            result.Codes.Count.ShouldBe(1);
            result.Code.ShouldContain(discount.Code);
            result.Created_at.Date.ShouldBe(DateTime.UtcNow.Date);
            result.Updated_at.Date.ShouldBe(DateTime.UtcNow.Date);
        }

        [Fact]
        public async Task CreateAsync_Many_Codes()
        {
            var discount = new SendOwlDiscount
            {
                Name = TestDiscountName + " many_codes",
                Codes = new List<string> { "code1", "code2" },
                Percentage_discount = "25.0",
                Use_limited_type = LimitingType.Many_codes_one_use
            };

            var result = await endpoint.CreateAsync(discount);
            CreatedDiscountIds.Add(result.Id);

            //It takes a few seconds for SendOwl API to create multiple codes. Wait and fetch resource again
            await Task.Delay(5000);
            result = await endpoint.GetAsync(result.Id);
            result.Name.ShouldBe(discount.Name);
            result.Percentage_discount.ShouldBe(discount.Percentage_discount);
            result.Codes.Count.ShouldBe(discount.Codes.Count);
            result.Codes.ShouldContain(discount.Codes.First());
            result.Codes.ShouldContain(discount.Codes.Last());
            result.Created_at.Date.ShouldBe(DateTime.UtcNow.Date);
            result.Updated_at.Date.ShouldBe(DateTime.UtcNow.Date);
        }

        [Fact]
        public async Task UpdateAsync()
        {
            var discount = new SendOwlDiscount
            {
                Name = TestDiscountName + "[Update]",
                Percentage_discount = "15.5"
            };

            var created = await endpoint.CreateAsync(discount);
            CreatedDiscountIds.Add(created.Id);
            created.Percentage_discount.ShouldBe(discount.Percentage_discount);
            created.Name.ShouldBe(discount.Name);

            created.Percentage_discount = "5.0";
            created.Usage_limit = 2;

            var updatedDiscount = await endpoint.UpdateAsync(created);
            updatedDiscount.Percentage_discount.ShouldBe(created.Percentage_discount);
            updatedDiscount.Usage_limit.ShouldBe(created.Usage_limit);
        }

        [Fact]
        public async Task DeleteAsync()
        {
            var discount = new SendOwlDiscount
            {
                Name = TestDiscountName + "[Delete]",
                Fixed_discount = "25.0"
            };

            var result = await endpoint.CreateAsync(discount);
            CreatedDiscountIds.Add(result.Id);
            result.ShouldNotBeNull();
            await Task.Delay(5000); //API returns 500 if deleting too fast after creation
            await endpoint.DeleteAsync(result.Id);
        }

        private async Task<SendOwlDiscount> CreateDiscount()
        {
            var discount = await endpoint.CreateAsync(new SendOwlDiscount
            {
                Name = TestDiscountName,
                Percentage_discount = "30.00"
            });

            CreatedDiscountIds.Add(discount.Id);

            return discount;
        }
    }
}
