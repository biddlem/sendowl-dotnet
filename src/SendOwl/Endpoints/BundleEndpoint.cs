using SendOwl.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SendOwl.Endpoints
{
    public class BundleEndpoint : Endpoint
    {
        public override string Path => "packages";

        public BundleEndpoint(IHttpSerializerClient client)
            : base(client)
        { }

        /// <summary>
        /// Get bundle by id
        /// </summary>
        /// <param name="bundleId"></param>
        /// <returns></returns>
        public async Task<SendOwlBundle> GetAsync(int bundleId)
        {
            return (await httpClient.GetAsync<SendOwlBundleListItem>($"{Path}/{bundleId}")).Package;
        }

        /// <summary>
        /// Get all bundles
        /// </summary>
        /// <returns></returns>
        public async Task<List<SendOwlBundle>> GetAllAsync()
        {
            return await PaginationHelper.GetAllAsync<SendOwlBundle, SendOwlBundleListItem>(httpClient, Path, x => x.Package);
        }

        /// <summary>
        /// Search for bundle by name
        /// </summary>
        /// <param term="term"></param>
        /// <returns></returns>
        public async Task<List<SendOwlBundle>> SearchAsync(string term)
        {
            return await PaginationHelper.GetAllAsync<SendOwlBundle, SendOwlBundleListItem>(httpClient, $"{Path}/search?term={term}", p => p.Package);
        }

        /// <summary>
        /// Create bundle
        /// </summary>
        /// <returns></returns>
        public async Task<SendOwlBundle> CreateAsync(SendOwlBundle bundle)
        {
            return (await httpClient.PostAsync(Path, new SendOwlBundleListItem { Package = bundle })).Package;
        }

        /// <summary>
        /// Delete bundle
        /// </summary>
        /// <param name="bundleId"></param>
        /// <returns></returns>
        public async Task DeleteAsync(int bundleId)
        {
            await httpClient.DeleteAsync($"{Path}/{bundleId}");
        }
    }
}
