using System;
using System.Collections.Generic;
using System.Text;

namespace CatchTheCovid19.IRestClient.Option
{
    public class NetworkOptions
    {
        public static string serverUrl { get; set; } = "http://192.168.6.1:8080";
        public static int timeOut { get; set; } = 30000;
    }
}
