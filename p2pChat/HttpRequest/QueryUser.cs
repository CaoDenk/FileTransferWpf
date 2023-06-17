using p2pchat.Crud;
using p2pchat.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace p2pchat.HttpRequest
{
    class QueryUserResult
    {
        public string uid { set; get; }
        public string username { set; get; }
        public string ipv6 { set; get; }
        public int statusCode { set; get; }

    }
    class QueryUser
    {

     
        /// <summary>
        /// 异步根据用户名查找用户
        /// </summary>
        /// <param name="username">用户名</param>
        /// <returns></returns>
        public async Task<QueryUserResult> Query(string username)
        {
    
            HttpClient httpClient = new HttpClient();

            string queryParam = $"{RequestApi.QUERY_URL}?username={username}";

            HttpResponseMessage msg= await httpClient.GetAsync(queryParam);

            return await msg.Content.ReadFromJsonAsync<QueryUserResult>();

        }




    }
}
