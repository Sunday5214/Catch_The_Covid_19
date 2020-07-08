using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatchTheCovid19_UWPClient.Model
{
    public class CheckTemperature
    {

        [JsonProperty("Temp")]
        public double Temp
        {
            get;
            set;
        }

        [JsonProperty("code")]
        public int code
        {
            get;
            set;
        }

        [JsonProperty("memberIdx")]
        public int memberIdx
        {
            get;
            set;
        }
    }
}
