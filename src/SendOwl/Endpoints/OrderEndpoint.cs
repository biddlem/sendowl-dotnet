using SendOwl.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SendOwl.Endpoints
{
    public class OrderEndpoint
    {
        public string Path => "orders";
        internal readonly IHttpSerializerClient httpClient;

        public OrderEndpoint(IHttpSerializerClient client)
        {
            this.httpClient = client;
        }

        /// <summary>
        /// Get item by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<SendOwlOrder> GetAsync(int id)
        {
            return (await httpClient.GetAsync<SendOwlOrderListItem>($"{Path}/{id}").ConfigureAwait(false)).Value;
        }

        /// <summary>
        /// Get all items
        /// </summary>
        /// <returns></returns>
        public async Task<List<SendOwlOrder>> GetAllAsync()
        {
            return await PaginationHelper.GetAllAsync<SendOwlOrder, SendOwlOrderListItem>(httpClient, Path, s => s.Value).ConfigureAwait(false);
        }

        /// <summary>
        /// Search for items by name
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        public async Task<List<SendOwlOrder>> SearchAsync(string term)
        {
            return await PaginationHelper.GetAllAsync<SendOwlOrder, SendOwlOrderListItem>(httpClient, $"{Path}/search?term={term}", s => s.Value).ConfigureAwait(false);
        }

        /// <summary>
        /// Update order. Only select fields can be altered
        /// See https://www.sendowl.com/developers/api/orders#orders-update
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<SendOwlOrder> UpdateAsync(int orderId, SendOwlOrderUpdate obj)
        {
            await httpClient.PutAsync($"{Path}/{orderId}", obj).ConfigureAwait(false);
            return await GetAsync(orderId).ConfigureAwait(false);
        }

        /// <summary>
        /// Refunds an order, optionally attempting to cancel a subscription if applicable.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task RefundAsync(SendOwlOrder obj)
        {
            await httpClient.PostNoResponseAsync($"{Path}/{obj.Id}/refund");
        }

        /// <summary>
        /// Cancels a subscription order.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task CancelSubscriptionAsync(SendOwlOrder obj)
        {
            await httpClient.PostNoResponseAsync($"{Path}/{obj.Id}/cancel_subscription");
        }

        /// <summary>
        /// esends emails relating to this order. The emails sent are are dependent on the resend option passed in.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="option"></param>
        /// <returns></returns>
        public async Task ResendEmailAsync(SendOwlOrder obj, ResendEmailOption option)
        {
            await httpClient.PostNoResponseAsync($"{Path}/{obj.Id}/cancel_subscription", new { type = option });
        }
    }
}
