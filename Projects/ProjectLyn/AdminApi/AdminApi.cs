using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AdminApi
{
    public class AdminApi
    {
        private readonly HttpClient _client;
        public AdminApi(string address, HttpClient client)
        {
            _client = client ?? new HttpClient();
            _client.BaseAddress = new Uri(address);
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
    }
}
