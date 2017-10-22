using SendOwl.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SendOwl.Endpoints
{
    public abstract class HTTPEndpoint<T, Y, Z> : Endpoint where Y : IListItem<T>, new() where T: SendOwlObject<Z>
    {
        public HTTPEndpoint(IHttpSerializerClient httpClient)
            : base(httpClient)
        { }

        /// <summary>
        /// Get item by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<T> GetAsync(object id)
        {
            return (await httpClient.GetAsync<Y>($"{Path}/{id}").ConfigureAwait(false)).Value;
        }

        /// <summary>
        /// Get all items
        /// </summary>
        /// <returns></returns>
        public async Task<List<T>> GetAllAsync()
        {
            return await PaginationHelper.GetAllAsync<T, Y>(httpClient, Path, s => s.Value).ConfigureAwait(false);
        }

        /// <summary>
        /// Search for items by name
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        public async Task<List<T>> SearchAsync(string term)
        {
            return await PaginationHelper.GetAllAsync<T, Y>(httpClient, $"{Path}/search?term={term}", s => s.Value).ConfigureAwait(false);
        }

        /// <summary>
        /// Create item
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<T> CreateAsync(T obj)
        {
            return (await httpClient.PostAsync(Path, new Y { Value = obj }).ConfigureAwait(false)).Value;
        }

        /// <summary>
        /// Update item
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<T> UpdateAsync(T obj)
        {
            await httpClient.PutAsync($"{Path}/{obj.Id}", new Y { Value = obj }).ConfigureAwait(false);
            return await GetAsync(obj.Id).ConfigureAwait(false);
        }

        /// <summary>
        /// Delete item
        /// </summary>
        /// <param name="item id"></param>
        /// <returns></returns>
        public async Task DeleteAsync(Z id)
        {
            await httpClient.DeleteAsync($"{Path}/{id}").ConfigureAwait(false);
        }
    }
}
