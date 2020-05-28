using CatchTheCovid19.RestClient.Option;
using System;
using System.Collections.Generic;
using System.Text;

namespace CatchTheCovid19.RestClient.Option
{
    public class NetworkOptions
    {
        public static string serverUrl { get; set; } = "http://e30fac91.ngrok.io";
        public static TimeEnum nowTime { get; set; }
        public static int timeOut { get; set; } = 30000;
    }
}
