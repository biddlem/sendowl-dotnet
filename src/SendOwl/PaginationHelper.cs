using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SendOwl
{
    public class PaginationHelper
    {
        private const int ItemsPerPage = 50;
        public static async Task<List<TResult>> GetAllAsync<TResult, Y>(IHttpSerializerClient client, string url, Func<Y, TResult> selector)
        {
            var items = new List<TResult>();
            var delimiter = url.Contains("?") ? "&" : "?";
            var page = 1;
            while (true)
            {
                var query = $"{delimiter}per_page={ItemsPerPage}&page={page}";
                var response = await client.GetAsync<IEnumerable<Y>>(url + query);
                if (!response.Any()) break;
                items.AddRange(response.Select(selector));
                page++;
            }
            return items;
        }
    }
}
