using SendOwl.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SendOwl.Endpoints
{
    public class ProductEndpoint : Endpoint
    {
        public override string Path => "products";

        public ProductEndpoint(IHttpSerializerClient client)
            : base(client)
        { }

        /// <summary>
        /// Get product by id
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public async Task<SendOwlProduct> GetAsync(long productId)
        {
            return (await httpClient.GetAsync<SendOwlProductListItem>($"{Path}/{productId}")).Product;
        }

        /// <summary>
        /// Search for product by name
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        public async Task<List<SendOwlProduct>> SearchAsync(string term)
        {
            return await PaginationHelper.GetAllAsync<SendOwlProduct, SendOwlProductListItem>(httpClient, $"{Path}/search?term={term}", p => p.Product);
        }

        /// <summary>
        /// Get all products
        /// </summary>
        /// <returns></returns>
        public async Task<List<SendOwlProduct>> GetAllAsync()
        {
            return await PaginationHelper.GetAllAsync<SendOwlProduct, SendOwlProductListItem>(httpClient, Path, p => p.Product);
        }

        /// <summary>
        /// Create product
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public async Task<SendOwlProduct> CreateAsync(SendOwlProduct product)
        {
            return (await httpClient.PostMultipartAsync<SendOwlProductListItem, SendOwlProduct>(Path, product, "product")).Product;
        }

        /// <summary>
        /// Delete product
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public async Task DeleteAsync(long productId)
        {
            await httpClient.DeleteAsync($"{Path}/{productId}");
        }
    }
}
