using SendOwl.Model;
using System.Threading.Tasks;

namespace SendOwl.Endpoints
{
    public class DiscountEndpoint : HTTPEndpoint<SendOwlDiscount, SendOwlDiscountListItem, int>
    {
        public override string Path => "discounts";

        public DiscountEndpoint(IHttpSerializerClient client)
            : base(client)
        { }

        /// <summary>
        /// Note: When creating multiple discount codes, there may be a short delay between the request being made and the new codes being visible in future discount show/index calls..
        /// See https://www.sendowl.com/developers/api/discount_codes#discount-multiple-codes
        /// </summary>
        /// <param name="obj">discount</param>
        /// <returns></returns>
        public new async Task<SendOwlDiscount> CreateAsync(SendOwlDiscount obj)
        {
            return await base.CreateAsync(obj).ConfigureAwait(false);
        }
    }
}
