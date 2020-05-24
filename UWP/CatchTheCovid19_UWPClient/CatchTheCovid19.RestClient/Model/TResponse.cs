using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CatchTheCovid19.IRestClient.Model
{
    public class TResponse<T>
    {
        [JsonProperty("status")]
        public int Status { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("data")]
        public T Data { get; set; }
    }
    public sealed class Nothing
    {
        public static Nothing AtAll => null;
    }
}
