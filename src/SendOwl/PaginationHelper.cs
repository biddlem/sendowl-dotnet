using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SendOwl
{
    public class PaginationHelper
    {
        private const int ItemsPerPage = 50;
        public static async Task<List<TObject>> GetAllAsync<TObject, TCollection>(IHttpSerializerClient client, string relativeUrl, Func<TCollection, TObject> selector)
        {
            var items = new List<TObject>();
            var delimiter = relativeUrl.Contains("?") ? "&" : "?";
            var page = 1;
            while (true)
            {
                var query = $"{delimiter}per_page={ItemsPerPage}&page={page}";
                var response = await client.GetAsync<IEnumerable<TCollection>>(relativeUrl + query).ConfigureAwait(false);
                if (!response.Any()) break;
                items.AddRange(response.Select(selector));
                page++;
            }
            return items;
        }
    }
}
