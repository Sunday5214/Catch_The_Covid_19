using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatchTheCovid19_UWPClient.Model
{
    public class CheckMemberCard
    {
        [JsonProperty("id")]
        public int Id
        {
            get;
            set;
        }

        [JsonProperty("student")]
        public bool? IsStudent
        {
            get;
            set;
        }

        [JsonProperty("grade")]
        public int? Grade
        {
            get;
            set;
        }

        [JsonProperty("cardId")]
        public string CardId
        {
            get;
            set;
        }

        [JsonProperty("name")]
        public string Name
        {
            get;
            set;
        }

        [JsonProperty("class")]
        public int? Class
        {
            get;
            set;
        }

        [JsonProperty("classNumber")]
        public int? ClassNumber
        {
            get;
            set;
        }
    }
}
