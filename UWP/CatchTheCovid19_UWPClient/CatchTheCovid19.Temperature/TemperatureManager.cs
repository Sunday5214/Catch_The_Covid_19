using CatchTheCovid19.Serial;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace CatchTheCovid19.Temperature
{
    public class TemperatureManager
    {
        SerialCommunicator serialCommunicator = new SerialCommunicator("COM4");

        public delegate void ReadComplete(string data);
        public event ReadComplete ReadCompleteEvent;

        public TemperatureManager()
        {
            //ConnectTemperatureArudu();
        }
        public async void ConnectTemperatureArudu()
        {
            //정확한 디바이스 이름 파악되면 바꿀것   
            if ((await serialCommunicator.FindPortsDevice()) == true)
            {
                Debug.WriteLine("포트찾음");
                if ((await serialCommunicator.ConnectSerial(115200)) == true)
                {
                    Debug.WriteLine("성공적으로 연결됨");
                    serialCommunicator.BUFFSIZE = 10;
                    serialCommunicator.ListenCompleteEvent += SerialCommunicator_ListenCompleteEvent;
                    GetSerialData();

                }
                else
                {
                    Debug.WriteLine("연결실패");
                }
            } 
            else
            {
                Debug.WriteLine("포트검색불가");
            }
        }

        private void SerialCommunicator_ListenCompleteEvent(string data)
        {
            Debug.WriteLine(data + "받음");
            ReadCompleteEvent?.Invoke(data);
        }

        public void GetSerialData()
        {
            serialCommunicator.Listen();
        }

        public async Task SendSerialData(string data)
        {
            await serialCommunicator.SendSerial(data);
        }
    }
}
