using CatchTheCovid10.Member;
using CatchTheCovid19.RestClient.Model;
using CatchTheCovid19.RestClient.Option;
using CatchTheCovid19.RestManager;
using CatchTheCovid19.Serial;
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
using Windows.UI.Core;

namespace CatchTheCovid19_UWPClient.ViewModel
{
    public class CheckTemperatureViewModel : BindableBase
    {
        RestManager restManager = new RestManager();

        private object _lock = new object();

        public delegate void TeamperatureReadComplete(bool success);
        public event TeamperatureReadComplete TeamperatureReadCompleteEvent;

        SerialRTU serialRTU = new SerialRTU("COM6", 19200, "Decimal", "8", "1", "0", "1000");

        public CheckTemperatureViewModel()
        {
            serialRTU.TeamperatureReadCompleteEvent += SerialRTU_TeamperatureReadCompleteEvent;

        }

        private async void SerialRTU_TeamperatureReadCompleteEvent(List<double> datas)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
            {
                AddDataServer(datas[4]);
                serialRTU.StopPoll();
            });

        }

        private async void AddDataServer(double data)
        {
            await AddData(data);
        }

        private Member _member = new Member();
        public Member Member
        {
            get => _member;
            set => SetProperty(ref _member, value);
        }

        private double _temperature;
        public double Temperature
        {
            get => _temperature;
            set => SetProperty(ref _temperature, value);
        }

        public async Task AddData(double data)
        {
            Temperature = data;
            QueryParam[] queryParam = new QueryParam[3];
            queryParam[0] = new QueryParam("Idx", Member.Idx);
            queryParam[1] = new QueryParam("code", (int)NetworkOptions.nowTime);
            queryParam[2] = new QueryParam("temp", data);
            (_, var respStatus) = await restManager.GetResponse<Default>("/insertRecord", Method.GET, null, queryParam);
            if (respStatus == HttpStatusCode.OK)
            {
                TeamperatureReadCompleteEvent?.Invoke(true);
            }
            else
            {
                TeamperatureReadCompleteEvent?.Invoke(false);
            }
            //http://10.80.162.7:8080/insertRecord?Idx=0&code=0&temp=36.50
        }

        public void GetTemperatureData()
        {
            serialRTU.StartPoll();
        }



        public void SetMemberData(Member checkMember)
        {
            Member = checkMember;

        }
    }
}
