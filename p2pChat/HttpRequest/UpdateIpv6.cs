using p2pchat.Code;
using p2pchat.Crud;
using p2pchat.CrudRequest;
using p2pchat.ExeTask;
using p2pchat.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace p2pchat.HttpRequest
{
    class UpdateIpv6
    {

        public static void HttpPutIpv6()
        {
            Task.Run(() =>
            {
                var data = new Dictionary<string, string>
                {
                    { "uid", GlobalVar.uuid },
                    { "ipv6", Utils.Utils.GetIpv6WithPortStr() }
                };
                var content = new FormUrlEncodedContent(data);
                HttpClient httpClient = new HttpClient();
                httpClient.PutAsync(RequestApi.PUT_IPV6_URL, content);

            });
          
        }

    }
}
