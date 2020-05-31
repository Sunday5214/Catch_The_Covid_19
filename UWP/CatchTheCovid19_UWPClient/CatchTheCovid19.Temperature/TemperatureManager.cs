using CatchTheCovid19.Serial;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace CatchTheCovid19.Temperature
{
    public class TemperatureManager
    {
        SerialCommunicator serialCommunicator = new SerialCommunicator("COM6");

        Timer timer = new Timer();

        public delegate void ReadComplete(string data);
        public event ReadComplete ReadCompleteEvent;

        int count = 0;
        byte[] sendDatas = new byte[]
        {
            0x01, 0x03, 0x00, 0x02, 0x00, 0x02, 0x65, 0xCB
        };
        public TemperatureManager()
        {
            timer.Elapsed += Timer_Elapsed;
            timer.Interval = 1000;
            timer.AutoReset = true;
            //timer.Enabled = true;
            
            //ConnectTemperatureArudu();
        }

        public void StartTimer()
        {
            timer.Start();
        }

        public void StopTimer()
        {
            count = 0;
            timer.Stop();
            timer.Dispose();
        }

        private async void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            await serialCommunicator.SendSerial8bit(sendDatas);
            //count++;
            //if (count == 8)
            //{
            //    count = 0;
            //    timer.Stop();
            //}
        }

        public async void ConnectTemperatureSensor()
        {
            //정확한 디바이스 이름 파악되면 바꿀것   
            if ((await serialCommunicator.FindDevicebyName("Silicon")) == true)
            {
                Debug.WriteLine("포트찾음");
                if ((await serialCommunicator.ConnectSerial(19200)) == true)
                {
                    Debug.WriteLine("성공적으로 연결됨");
                    serialCommunicator.BUFFSIZE = 4;
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
        
        private void SerialCommunicator_ListenCompleteEvent(object data)
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();
            bf.Serialize(ms, data);

            var bye = ms.ToArray();
            
            Debug.WriteLine(data + "받음");
            //ReadCompleteEvent?.Invoke(data);
        }

        public void GetSerialData()
        {
            serialCommunicator.Listen();
        }

        public async Task SendSerialData()
        {
            await serialCommunicator.SendSerial8bit(sendDatas);
        }
    }
}
