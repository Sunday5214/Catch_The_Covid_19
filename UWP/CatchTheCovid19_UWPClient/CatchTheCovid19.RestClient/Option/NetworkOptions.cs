using CatchTheCovid19.RestClient.Option;
using System;
using System.Collections.Generic;
using System.Text;

namespace CatchTheCovid19.RestClient.Option
{
    public class NetworkOptions
    {
        public static string serverUrl { get; set; } = "http://10.80.162.7:8080";
        public static TimeEnum nowTime { get; set; }
        public static int timeOut { get; set; } = 30000;
    }
}
