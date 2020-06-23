using CatchTheCovid10.InitData;
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
using Windows.Data.Json;
using Windows.Storage;
using Windows.UI.Core;
using Newtonsoft.Json;

namespace CatchTheCovid19_UWPClient.ViewModel
{
    public class CheckTemperatureViewModel : BindableBase
    {
        RestManager restManager = new RestManager();

       

        public delegate void TeamperatureReadComplete(bool success);
        public event TeamperatureReadComplete TeamperatureReadCompleteEvent;

        SerialCommunicator serial = new SerialCommunicator();
        private List<CheckTemperature> checkTemperatures = new List<CheckTemperature>();

        private string json = "";

        public CheckTemperatureViewModel()
        {
            serial.ListenCompleteEvent += Serial_ListenCompleteEvent;
            serial.BUFFSIZE = 5;
            Connect();
        }
        private async void Connect()
        {
            await serial.FindDevicebyName("COM3");
            await serial.ConnectSerial(9600);
            serial.Listen();
        }

        private async void Serial_ListenCompleteEvent(string data)
        {
            if (data.Contains("."))
            {
                AddDataServer(double.Parse(data));
                TeamperatureReadCompleteEvent?.Invoke(true);
            }
            else
            {
                DistanceData = map(int.Parse(data), 0, 150, 100, 1);
                Debug.WriteLine(DistanceData);
                if (DistanceData >= 40 && DistanceData <= 50)
                {
                    await StopDistanceData();
                    GetTemperatureData();
                }
                //값이 바뀔때만 보내주도록
            }
            
        }

        private int _distanceData;
        public int DistanceData
        {
            get => _distanceData;
            set => SetProperty(ref _distanceData, value);
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

        private void SaveSetting(string settingName, string data)
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            localSettings.Values[settingName] = data;
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
                checkTemperatures.Add(new CheckTemperature { code = (int)NetworkOptions.nowTime, memberIdx = Member.Idx, Temp = data });
                string json = JsonConvert.SerializeObject(checkTemperatures);
                SaveSetting("ReqSaveData", json);
                TeamperatureReadCompleteEvent?.Invoke(true);
            }
            //http://10.80.162.7:8080/insertRecord?Idx=0&code=0&temp=36.50
        }

        public async Task AddDataNotSaved(object temperatures) 
        {
            if (temperatures == null) return;
            List<CheckTemperature> checkTemperatures = JsonConvert.DeserializeObject<List<CheckTemperature>>(temperatures.ToString());
            foreach(var checkTemperature in checkTemperatures)
            {
                QueryParam[] queryParam = new QueryParam[3];
                queryParam[0] = new QueryParam("Idx", checkTemperature.memberIdx);
                queryParam[1] = new QueryParam("code", checkTemperature.code);
                queryParam[2] = new QueryParam("temp", checkTemperature.Temp);
                (_, var respStatus) = await restManager.GetResponse<Default>("/insertRecord", Method.GET, null, queryParam);
            }
        }

       

        private int map(int x, int in_min, int in_max, int out_min, int out_max)
        {
            return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
        }

        private int constrain(int amt, int low, int high)
        {
            return amt < low ? low : (amt > high ? high : amt);
        }

        public async void GetTemperatureData()
        {
            await serial.SendSerial("1");
        }

        public async void GetDistanceData()
        {
            await serial.SendSerial("2");
        }

        public async Task StopDistanceData()
        {
            await serial.SendSerial("3");
        }

        public async void PendingBarcode()
        {
            await serial.SendSerial("5");
        }

        public async void StopBarcode()
        {
            await serial.SendSerial("4");
        }

        public void SetMemberData(Member checkMember)
        {
            Member = checkMember;

        }
    }
}
