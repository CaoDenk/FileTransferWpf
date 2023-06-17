using p2pchat.Code;
using p2pchat.Crud;
using p2pchat.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace p2pchat.CrudRequest
{

    struct LoginResp
    {
        public int statusCode { get; set; }
        public string uuid { get; set; }

    }
    internal class Login
    {

         string Name { get; set; }
         string Password { get; set; }
         string Zone { get; set; }
         
        //public string uuid { get; set; }
        public Login(string Name,string Password,string Zone) { 
            this.Password=Password;
            this.Name = Name;
            this.Zone = Zone;
        }


        /// <summary>
        /// 应该返回对象
        /// </summary>
        /// <returns></returns>
        public async Task<LoginResp> ToLogin()
        {

            var data = new Dictionary<string, string>
            {
                { "username", Name },
                { "password", Password }
            };
            var content = new FormUrlEncodedContent(data);
            HttpClient httpClient = new HttpClient();

           
            HttpResponseMessage httpMsg = await httpClient.PostAsync(RequestApi.LOGIN_URL, content);

            HttpContent resp = httpMsg.Content;

            return await resp.ReadFromJsonAsync<LoginResp>();
        }








    }
}
