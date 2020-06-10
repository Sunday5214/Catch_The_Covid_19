using CatchTheCovid10.InitData;
using CatchTheCovid19.I2C;
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

       

        public delegate void TeamperatureReadComplete(bool success);
        public event TeamperatureReadComplete TeamperatureReadCompleteEvent;

        SerialCommunicator serial = new SerialCommunicator();

 
        public CheckTemperatureViewModel()
        {
            serial.ListenCompleteEvent += Serial_ListenCompleteEvent;
            serial.BUFFSIZE = 5;
            Connect();
        }
        private async void Connect()
        {
            await serial.FindDevicebyName("Serial");
            await serial.ConnectSerial(9600);
            serial.Listen();
        }

        private async void Serial_ListenCompleteEvent(string data)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
            {
                AddDataServer(double.Parse(data));
                //serial.StopPoll();
            });
        }

        private int _distanceData;
        public int DistanceData
        {
            get => _distanceData;
            set => SetProperty(ref _distanceData, value);
        }

        public async Task StartI2C()
        {
            await GetData();
        }

        private int map(int x, int in_min, int in_max, int out_min, int out_max)
        {
            return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
        }

        private int constrain(int amt, int low, int high)
        {
            return amt < low ? low : (amt > high ? high : amt);
        }

        public async Task GetData()
        {
            VL53L0XSensor sensor = new VL53L0XSensor();
            await sensor.InitializeAsync();
            bool IsSixCm = false;
            int data = -1;
            while (!IsSixCm)
            {
                data = sensor.ReadDistance();
                DistanceData = map(constrain(data, 12, 100), 100, 12, 1, 100);
              
                if (DistanceData <= 90 && DistanceData >= 80)
                {
                    IsSixCm = true;
                    GetTemperatureData();
                }
                await Task.Delay(200);
            }
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

        public async void GetTemperatureData()
        {
            //serialRTU.StartPoll();
            await serial.SendSerial("1");
        }



        public void SetMemberData(Member checkMember)
        {
            Member = checkMember;

        }
    }
}
