using CatchTheCovid19.RestManager;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CatchTheCovid10.InitData
{
    public class InfoManager
    {
        RestManager restManager = new RestManager();
        public static Info Infos
        {
            get;
            set;
        }

        public async Task GetInfoData()
        {
            var resp = await restManager.GetResponse<Info>("/info", Method.GET);
            if (resp.respStatus == System.Net.HttpStatusCode.OK)
            {
                Infos = resp.respData;
            }
            else
            {

            }
        }
    }
}
