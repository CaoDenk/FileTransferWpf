using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace p2pchat.Crud
{
    internal class RequestApi
    {
        //const string URL = "http://172.16.111.67";
        const string URL = "http://150.158.94.145";
        public const string REGISTE_URL= $"{URL}/p2pchat/register.html";
        public const string UPDATE_URL = $"{URL}/pub/quora-3-2-20.apk";
        public const string ABOUT_URL = $"{URL}/p2pchat/about.html";
        public const string LOGIN_URL = $"{URL}:8080/p2pchat/login";
        public const string QUERY_URL = $"{URL}:8080/p2pchat/queryUser";
        public const string PUT_IPV6_URL = $"{URL}:8080/p2pchat/putIpv6";
        public const string GET_IPV6_URL = $"{URL}:8080/p2pchat/getIpv6";
    }
}
