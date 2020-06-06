using CatchTheCovid19.RestClient.Option;
using System;
using System.Collections.Generic;
using System.Text;

namespace CatchTheCovid19.RestClient.Option
{
    public class NetworkOptions
    {
        public static string serverUrl { get; set; } = " https://adad60f5605b.ngrok.io";
        public static TimeEnum nowTime { get; set; }
        public static int timeOut { get; set; } = 30000;
    }
}
