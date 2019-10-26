using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Channel_Nine.network
{
    class GetWebPage
    {
        public async static Task<string> GetWebPageData(string url, string headers = null)
        {
            // Here we make the network GET call for fetching the page HTML.
            var http = new HttpClient();
            //http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", App.bearerToken);
            var response = await http.GetAsync(url);
            var webResult = await response.Content.ReadAsStringAsync();
            return webResult;

        }
    }
}
