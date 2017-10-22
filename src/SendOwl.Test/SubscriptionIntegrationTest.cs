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
    public class SubscriptionIntegrationTest : IClassFixture<APIClientFixture>
    {
        private const string TestSubscriptionName = "my-test-subscription";
        private readonly List<int> CreatedSubcriptionIds;
        private readonly Lazy<List<long>> ExistingProductIds;
        private readonly SubscriptionEndpoint endpoint;

        public SubscriptionIntegrationTest(APIClientFixture fixture)
        {
            endpoint = fixture.SendOwlAPIClient.Subscription;
            CreatedSubcriptionIds = fixture.CreatedSubscriptionIds;
            ExistingProductIds = fixture.ExistingProductIds;
        }

        [Fact]
        public async Task GetAllAsync()
        {
            if(!CreatedSubcriptionIds.Any()) await CreateSubscription();
            var subscriptions = await endpoint.GetAllAsync();
            subscriptions.ShouldNotBeEmpty();
        }

        [Fact]
        public async Task GetAsync()
        {
            var existing = await CreateSubscription();
            var subscription = await endpoint.GetAsync(existing.Id);
            subscription.ShouldNotBeNull();
            subscription.Id.ShouldBe(existing.Id);
        }

        [Fact]
        public async Task SearchAsync()
        {
            if (!CreatedSubcriptionIds.Any()) await CreateSubscription();
            await Task.Delay(3000); //takes a few sec for SendOwl to index...
            var subscriptions = await endpoint.SearchAsync("test");
            subscriptions.ShouldNotBeEmpty();
        }

        [Fact]
        public async Task CreateAsync()
        {
            var subscription = new SendOwlSubscription
            {
                Name = "My new Subscription",
                Trial_price = "0.99",
                Trial_frequency = Frequency.Daily,
                Trial_no_of_occurrences = 7,
                Recurring_price = "9.99",
                Frequency_value = 2,
                Frequency_interval = FrequencyInterval.Day,
                Recurring_type = PaymentType.Ongoing,
                Access_all_products = true
            };
            var result = await endpoint.CreateAsync(subscription);
            CreatedSubcriptionIds.Add(result.Id);

            result.Id.ShouldBeGreaterThan(1);
            result.Name.ShouldBe(subscription.Name);
            result.Trial_price.ShouldBe(subscription.Trial_price);
            result.Trial_frequency.ShouldBe(subscription.Trial_frequency);
            result.Trial_no_of_occurrences.ShouldBe(subscription.Trial_no_of_occurrences);
            result.Recurring_price.ShouldBe(subscription.Recurring_price);
            result.Frequency_interval.ShouldBe(subscription.Frequency_interval);
            result.Frequency_value.ShouldBe(subscription.Frequency_value);
            result.Recurring_type.ShouldBe(subscription.Recurring_type);
            result.Access_all_products.ShouldBe(subscription.Access_all_products);
            result.Created_at.Date.ShouldBe(DateTime.UtcNow.Date);
            result.Updated_at.Date.ShouldBe(DateTime.UtcNow.Date);
        }

        [Fact]
        public async Task UpdateAsync()
        {
            var subscription = new SendOwlSubscription
            {
                Name = TestSubscriptionName + "[Update]",
                Recurring_price = "34.99",
                Frequency_interval = FrequencyInterval.Year,
                Frequency_value = 1,
                Access_all_products = true
            };

            var created = await endpoint.CreateAsync(subscription);
            CreatedSubcriptionIds.Add(created.Id);
            created.Recurring_price.ShouldBe(subscription.Recurring_price);
            created.Name.ShouldBe(subscription.Name);

            created.Recurring_price = "5.00";

            var updated = await endpoint.UpdateAsync(created);
            updated.Recurring_price.ShouldBe(created.Recurring_price);
        }

        [Fact]
        public async Task DeleteAsync()
        {
            var subscription = await CreateSubscription();

            var result = await endpoint.CreateAsync(subscription);
            CreatedSubcriptionIds.Add(result.Id);
            result.ShouldNotBeNull();
            await Task.Delay(5000); //API returns 500 if deleting too fast after creation
            await endpoint.DeleteAsync(result.Id);
        }

        private async Task<SendOwlSubscription> CreateSubscription()
        {
            var subscription = await endpoint.CreateAsync(new SendOwlSubscription
            {
                Name = TestSubscriptionName,
                Recurring_price = "99.99",
                Recurring_type = PaymentType.Ongoing,
                Frequency_interval = FrequencyInterval.Month,
                Frequency_value = 1,
                Access_all_products = true
            });

            CreatedSubcriptionIds.Add(subscription.Id);
            return subscription;
        }
    }
}
