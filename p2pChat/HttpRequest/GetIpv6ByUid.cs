using p2pchat.Crud;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace p2pchat.HttpRequest
{

    public class GetRessult
    {
        public int statusCode { get; set; }
        public string ipv6 { get; set; }
    }
    class GetIpv6ByUid
    {


        public static async Task<GetRessult> GetIpv6(string uid)
        {
            
            HttpClient httpClient = new HttpClient();

            string queryParam = $"{RequestApi.GET_IPV6_URL}?uid={uid}";
            HttpResponseMessage hrm=await Task.Run(
                async() => { return await httpClient.GetAsync(queryParam); }
                );

            return await hrm.Content.ReadFromJsonAsync<GetRessult>();

        }


    }
}
