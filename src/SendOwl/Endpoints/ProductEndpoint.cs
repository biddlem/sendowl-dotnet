using System.Collections.Generic;
using SendOwl.Model;
using System.IO;
using System.Threading.Tasks;

namespace SendOwl.Endpoints
{
    public class ProductEndpoint : HTTPEndpoint<SendOwlProduct, SendOwlProductListItem, long>
    {
        public override string Path => "products";

        public ProductEndpoint(IHttpSerializerClient client)
            : base(client)
        { }

        /// <summary>
        /// Create product
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public new async Task<SendOwlProduct> CreateAsync(SendOwlProduct product)
        {
            return await CreateAsync(product, null, null);
        }

        /// <summary>
        /// Create product with uploaded file
        /// </summary>
        /// <param name="product"></param>
        /// <param name="file stream"></param>
        /// <param name="file name"></param>
        /// <returns></returns>
        public async Task<SendOwlProduct> CreateAsync(SendOwlProduct product, Stream stream, string fileName)
        {
            return (await httpClient.PostMultipartAsync<SendOwlProductListItem, SendOwlProduct>("products.json", product, "product", stream, fileName)
                .ConfigureAwait(false)).Value;
        }

        /// <summary>
        /// Get items by shopify variant id
        /// </summary>
        /// <param name="variantId"></param>
        /// <returns></returns>
        public async Task<List<SendOwlProduct>> ShopifyLookupAsync(object variantId)
        {
            return await PaginationHelper.GetAllAsync<SendOwlProduct, SendOwlProductListItem>(httpClient, $"products/shopify_lookup?variant_id={variantId}", s => s.Value).ConfigureAwait(false);
        }
    }
}
