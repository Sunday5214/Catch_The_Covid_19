﻿using System;
using System.Collections.Generic;
using System.Text;

namespace CatchTheCovid19.IRestClient.Option
{
    public class NetworkOptions
    {
        public static string serverUrl { get; set; } = "http://e30fac91.ngrok.io";
        public static int timeOut { get; set; } = 30000;
    }
}
