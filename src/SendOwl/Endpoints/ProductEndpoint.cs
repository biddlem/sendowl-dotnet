﻿using SendOwl.Model;
using System.Collections.Generic;
using System.IO;
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
            return (await httpClient.PostMultipartAsync<SendOwlProductListItem, SendOwlProduct>("products.json", product, "product", stream, fileName)).Product;
        }

        /// <summary>
        /// Update product
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public async Task<SendOwlProduct> UpdateAsync(SendOwlProduct product)
        {
            await httpClient.PutAsync($"{Path}/{product.Id}", new SendOwlProductListItem { Product = product });
            return await GetAsync(product.Id);
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
