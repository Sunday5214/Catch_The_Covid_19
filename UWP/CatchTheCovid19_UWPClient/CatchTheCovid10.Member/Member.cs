using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CatchTheCovid10.Member
{
    public class Member
    {
        [JsonProperty("student")]
        public bool IsStudent
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
        [JsonProperty("id")]
        public int Idx
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

        [JsonProperty("classNumber")]
        public int ClassNumber
        {
            get;
            set;
        }
    }
}
