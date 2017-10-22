using SendOwl.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SendOwl.Endpoints
{
    public abstract class HTTPEndpoint<TObject, TCollection, TId> where TCollection : IListItem<TObject>, new() where TObject: SendOwlObject<TId>
    {
        public abstract string Path { get; }
        internal readonly IHttpSerializerClient httpClient;

        public HTTPEndpoint(IHttpSerializerClient httpClient)
        {
            this.httpClient = httpClient;
        }

        /// <summary>
        /// Get item by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TObject> GetAsync(object id)
        {
            return (await httpClient.GetAsync<TCollection>($"{Path}/{id}").ConfigureAwait(false)).Value;
        }

        /// <summary>
        /// Get all items
        /// </summary>
        /// <returns></returns>
        public async Task<List<TObject>> GetAllAsync()
        {
            return await PaginationHelper.GetAllAsync<TObject, TCollection>(httpClient, Path, s => s.Value).ConfigureAwait(false);
        }

        /// <summary>
        /// Search for items by name
        /// </summary>
        /// <param name="term"></param>
        /// <returns></returns>
        public async Task<List<TObject>> SearchAsync(string term)
        {
            return await PaginationHelper.GetAllAsync<TObject, TCollection>(httpClient, $"{Path}/search?term={term}", s => s.Value).ConfigureAwait(false);
        }

        /// <summary>
        /// Create item
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<TObject> CreateAsync(TObject obj)
        {
            return (await httpClient.PostAsync(Path, new TCollection { Value = obj }).ConfigureAwait(false)).Value;
        }

        /// <summary>
        /// Update item
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<TObject> UpdateAsync(TObject obj)
        {
            await httpClient.PutAsync($"{Path}/{obj.Id}", new TCollection { Value = obj }).ConfigureAwait(false);
            return await GetAsync(obj.Id).ConfigureAwait(false);
        }

        /// <summary>
        /// Delete item
        /// </summary>
        /// <param name="item id"></param>
        /// <returns></returns>
        public async Task DeleteAsync(TId id)
        {
            await httpClient.DeleteAsync($"{Path}/{id}").ConfigureAwait(false);
        }
    }
}
