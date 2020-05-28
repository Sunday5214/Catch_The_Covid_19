using CatchTheCovid10.Member;
using CatchTheCovid19.RestClient.Model;
using CatchTheCovid19.RestManager;
using CatchTheCovid19.Temperature;
using CatchTheCovid19_UWPClient.Model;
using Prism.Mvvm;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CatchTheCovid19_UWPClient.ViewModel
{
    public class CheckTemperatureViewModel : BindableBase
    {
        TemperatureManager temperatureManager = new TemperatureManager();
        RestManager restManager = new RestManager();

        public delegate void TeamperatureReadComplete(bool success);
        public event TeamperatureReadComplete TeamperatureReadCompleteEvent;



        private Member _member = new Member();
        public Member Member
        {
            get => _member;
            set => SetProperty(ref _member, value);
        }

        private float _temperature;
        public float Temperature
        {
            get => _temperature;
            set => SetProperty(ref _temperature, value);
        }

        public CheckTemperatureViewModel()
        {
            temperatureManager.ReadCompleteEvent += TemperatureManager_ReadCompleteEvent;
            temperatureManager.ConnectTemperatureArudu();
        }

        private async void TemperatureManager_ReadCompleteEvent(string data)
        {
            Temperature = float.Parse(data);
            await AddData(float.Parse(data));
        }

        public async Task AddData(float data)
        {
            QueryParam[] queryParam = new QueryParam[2];
            queryParam[0] = new QueryParam("Idx", Member.Idx);
            queryParam[1] = new QueryParam("temp", data);
            (_, var respStatus) = await restManager.GetResponse<Nothing>("/insertRecord", Method.GET, null, queryParam);
            if (respStatus == HttpStatusCode.OK)
            {
                TeamperatureReadCompleteEvent?.Invoke(true);
            }
            else
            {
                TeamperatureReadCompleteEvent?.Invoke(false);
            }
        }

        public async void StartReadTemperature()
        {
            Debug.WriteLine("온도 측정시작 : 1보냄");
            //await temperatureManager.SendSerialData("1");
        }

        public void SetMemberData(Member checkMember)
        {
            Member = checkMember;
        }
    }
}
