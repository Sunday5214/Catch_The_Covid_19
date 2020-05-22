using CatchTheCovid19.IRestClient.Model;
using CatchTheCovid19.IRestClient.Option;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Threading.Tasks;

namespace CatchTheCovid19.IRestClient
{
    public class RestManager
    {
        public class UrlSegment
        {
            public string Name { get; set; }
            public object Value { get; set; }

            public UrlSegment(string name, object value)
            {
                Name = name;
                Value = value;
            }
        }
        public class QueryParam
        {
            public string Name { get; set; }
            public object Value { get; set; }

            public QueryParam(string name, object value)
            {
                Name = name;
                Value = value;
            }
        }
        public class Header
        {
            public string Name { get; set; }
            public string Value { get; set; }

            public Header(string name, string value)
            {
                Name = name;
                Value = value;
            }
        }
        public class RestManager
        {
            private static RestSharp.IRestClient CreateClient()
            {
                var restClient = new IRestClient(NetworkOptions.serverUrl) { Timeout = NetworkOptions.timeOut };
                return restClient;
            }
            public async Task<TResponse<T>> GetResponse<T>(string resource, Method method, string parameterJson = null, QueryParam[] queryParams = null, UrlSegment[] urlSegments = null, Header[] headers = null)
            {
                TResponse<T> resp = null;
                try
                {
                    var client = CreateClient();
                    var restRequest = CreateRequest(resource, method, parameterJson, queryParams, urlSegments, headers);
                    var response = await client.ExecuteTaskAsync(restRequest);

                    resp = JsonConvert.DeserializeObject<TResponse<T>>(response.Content);
                }
                catch (Exception e)
                {
                    resp = new TResponse<T> { Message = e.ToString(), Status = 404, Data = default };
                }


                return resp;
            }

            private RestRequest CreateRequest(string resource, Method method, string parameterJson, QueryParam[] queryParams, UrlSegment[] urlSegments, Header[] headers)
            {
                var restRequest = new RestRequest(resource, method) { Timeout = NetworkOptions.timeOut };
                restRequest = AddToRequest(restRequest, null, parameterJson, queryParams, urlSegments, headers);

                return restRequest;
            }

            private RestRequest AddToRequest(RestRequest restRequest, object token, string parameterJson, QueryParam[] queryParams, UrlSegment[] urlSegments, Header[] headers)
            {
                if (urlSegments != null)
                {
                    foreach (var urlSegment in urlSegments)
                    {
                        restRequest.AddUrlSegment(urlSegment.Name, urlSegment.Value);
                    }
                }

                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        restRequest.AddHeader(header.Name, header.Value);
                    }
                }

                if (queryParams != null)
                {
                    foreach (var queryParam in queryParams)
                    {
                        restRequest.AddParameter(queryParam.Name, queryParam.Value);
                    }
                }

                if (!string.IsNullOrEmpty(parameterJson))
                {
                    restRequest.AddHeader("Content-Type", "application/json");
                    restRequest.AddParameter("application/json", parameterJson, ParameterType.RequestBody);
                }
                return restRequest;
            }
        }
    }
}
