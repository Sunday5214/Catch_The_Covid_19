using CatchTheCovid19.RestClient.Option;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CatchTheCovid10.InitData
{
    public class Info
    {
        [JsonProperty("Codes")]
        public List<string> Codes
        {
            get;
            set;
        }

        [JsonProperty("grade")]
        public int Grade
        {
            get;
            set;
        }

        [JsonProperty("check")]
        public int Check 
        {
            get;
            set;
        }

        [JsonProperty("class")]
        public int Class
        {
            get;
            set;
        }

        [JsonProperty("uncheck")]
        public int Uncheck
        {
            get;
            set;
        }
    }

}
